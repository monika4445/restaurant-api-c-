# RestaurantApi

![CI](https://github.com/monika4445/restaurant-api-c-/actions/workflows/ci.yml/badge.svg)

ASP.NET Core Web API (.NET 10) backed by PostgreSQL (Supabase) via Entity Framework Core.

## Entities

- **Restaurant** — Id, Name, Address, ContactNumber, HoursOfOperation
- **Player** — Id, FirstName, LastName, Dob, PrimaryAddress, AlternateAddress, OfficeAddress, MobileNumber, Email, DriversLicense, Passport
- **Address** (owned by Player, three instances) — StreetNumber, Line1, Line2, City, State, Postal, Country
- **Membership** — PlayerId, RestaurantId
- **Favorite** — PlayerId, RestaurantId

## Setup

Requires **.NET SDK 10** (`dotnet --version`).

```sh
# Once per machine — install the EF Core CLI tool
dotnet tool install --global dotnet-ef

# 1. Store the Postgres connection string as a User Secret (not committed)
dotnet user-secrets init --project RestaurantApi
dotnet user-secrets set "ConnectionStrings:Default" \
  "Host=aws-1-<region>.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.<your-ref>;Password=<your-password>;SSL Mode=Require;Trust Server Certificate=true" \
  --project RestaurantApi
# Use the Supabase "Session pooler" string (IPv4). The "Direct" connection is IPv6-only.

# 2. Apply migrations to create tables + unique indexes
dotnet ef database update --project RestaurantApi

# 3. Run
dotnet run --project RestaurantApi
```

The console will print the URL the app is listening on (e.g. `http://localhost:5099`).

## Testing the API

### Locally
- **Swagger UI** — `http://localhost:<port>/swagger`
- **`.http` file** — open `RestaurantApi.http` in VS Code (REST Client extension) or JetBrains Rider and click *Send Request*. Requests are chained: creating a restaurant + player + membership flows the IDs into subsequent calls.

### After hosting
Once deployed (see Deployment below), Swagger UI is served at the same path on the public URL: `https://<your-host>/swagger`. The `.http` file also works against the hosted URL — just change the `@host` variable at the top.

### Run the test suite

```sh
dotnet test
```

Expected: **14 passed, 2 skipped**. The skipped tests use Postgres-specific `ILike`, which the in-memory test database doesn't implement — they are documented as such.

### Verification checklist (5-minute walkthrough)

Open Swagger UI and click through these in order. Each row is one edge case worth verifying.

| # | Endpoint | Input | Expected |
|---|---|---|---|
| 1  | POST `/api/restaurants` | valid body | **201** + `createdAt` populated |
| 2  | POST `/api/restaurants` | same body again | **409** with friendly message |
| 3  | POST `/api/restaurants` | UPPERCASE name, same address | **409** (case-insensitive duplicate) |
| 4  | POST `/api/restaurants` | empty strings | **400** with field-level errors |
| 5  | POST `/api/players` | valid body | **201** |
| 6  | POST `/api/players` | `dob: "2099-01-01"` | **400** "Date of birth cannot be in the future" |
| 7  | POST `/api/players` | same email different case | **409** |
| 8  | POST `/api/memberships` | `playerId` = empty Guid | **404** "Player not found" |
| 9  | POST `/api/memberships` | valid player + restaurant | **201** |
| 10 | POST `/api/memberships` | same pair again | **409** "already a member" |
| 11 | POST `/api/favorites` | same pair | **201** (favorites are independent of membership) |
| 12 | GET  `/api/restaurants?name=es` | partial substring | matching restaurants |
| 13 | GET  `/api/restaurants?name=` | empty | **400** "name is required" |
| 14 | GET  `/api/players?firstName=ad` | partial first name | matching players |
| 15 | GET  `/api/memberships?firstName=…&lastName=…` | exact match | restaurants with `isFavoriteRestaurant` flag |
| 16 | GET  `/api/favorites?firstName=…&lastName=…` | exact match | restaurants with `linked` flag |
| 17 | GET  `/api/restaurants/members?name=…&age=18` | partial + age | `playersAgedAtLeastCount` per restaurant |
| 18 | GET  `/api/restaurants/members?name=…&age=-1` | negative age | **400** |
| 19 | GET  `/health` | — | **200** "Healthy" |

## Deployment (Render, free tier)

1. Push the repo to GitHub (already done if you're reading this).
2. Sign up at https://render.com and connect your GitHub account.
3. **New → Web Service** → pick this repo.
4. Configure:
   - **Environment**: Docker
   - **Region**: closest to you
   - **Instance type**: Free
   - **Health check path**: `/health`
5. Under **Environment variables**, add:
   - `ConnectionStrings__Default` = your Supabase **session-pooler** connection string (the same one used locally — see Setup above).
6. Click **Create Web Service**. Render will build the Dockerfile and deploy. First build takes ~3–5 min.
7. Once live, your API is at `https://<service-name>.onrender.com`. Swagger UI: `https://<service-name>.onrender.com/swagger`.

**Notes**
- Render's free tier spins down after 15 min of inactivity. First request after spin-down takes ~30s (cold start). For an assessment demo this is fine.
- `PORT` is injected by Render; the app honors it (`Program.cs`).
- Migrations: Render won't run them automatically. Either run `dotnet ef database update` against the production DB once from your machine, or temporarily add `Database.Migrate()` in `Program.cs`.

## Endpoints

| Method | Path | Description |
|---|---|---|
| POST | `/api/restaurants` | Save restaurant |
| POST | `/api/players` | Save player |
| POST | `/api/memberships` | Save membership |
| POST | `/api/favorites` | Save favorite (independent of membership) |
| GET  | `/api/restaurants?name=` | Partial, case-insensitive search by name |
| GET  | `/api/players?firstName=&lastName=` | Partial search; either or both query params |
| GET  | `/api/memberships?firstName=&lastName=` | Player memberships; each restaurant has `isFavoriteRestaurant` |
| GET  | `/api/favorites?firstName=&lastName=` | Player favorites; each restaurant has `linked` (= also a member) |
| GET  | `/api/restaurants/members?name=&age=` | Restaurants matching name + count of members aged ≥ X |

## Design notes

- **Layering**: Controller → Service → DbContext. No DB calls in controllers.
- **DTOs separate from entities** to avoid leaking EF tracking refs and circular `Player.Memberships` graphs.
- **Owned Address** entities on Player (primary / alternate / office) — Address is a value object, not a separate aggregate.
- **Database-enforced uniqueness** on Restaurant `(Name, Address)`, Player `Email` / `Passport` / `DriversLicense`, and the two join tables `(PlayerId, RestaurantId)` — guarantees duplicate rules even under race conditions.
- **Duplicate detection is case-insensitive and trims whitespace**, matching how a human reader judges duplicates.
- **Exception middleware** translates domain exceptions (`NotFoundException`, `ConflictException`, `ValidationException`) and database-level unique violations into RFC 7807 `ProblemDetails` responses.
- **User Secrets** for the connection string — never in the repo.
