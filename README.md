# RestaurantApi

ASP.NET Core Web API (.NET 8+) backed by PostgreSQL (Supabase) via Entity Framework Core.

## Entities

- **Restaurant** — Id, Name, Address, ContactNumber, HoursOfOperation
- **Player** — Id, FirstName, LastName, Dob, PrimaryAddress, AlternateAddress, OfficeAddress, MobileNumber, Email, DriversLicense, Passport
- **Address** (owned by Player, three instances) — StreetNumber, Line1, Line2, City, State, Postal, Country
- **Membership** — PlayerId, RestaurantId
- **Favorite** — PlayerId, RestaurantId

## Setup

Requires **.NET SDK 8 or higher** (`dotnet --version`). Developed on .NET 10.

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
Once deployed (Azure App Service, Render, Fly.io, etc.), Swagger UI is served at the same path on the public URL: `https://<your-host>/swagger`. The `.http` file also works against the hosted URL — just change the `@host` variable at the top.

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
