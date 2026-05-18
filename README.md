# RestaurantApi

ASP.NET Core Web API (.NET 8+) backed by PostgreSQL (Supabase) via Entity Framework Core.

## Entities

- **Restaurant** — Id, Name, Address, ContactNumber, HoursOfOperation
- **Player** — Id, FirstName, LastName, Dob, PrimaryAddress, AlternateAddress, OfficeAddress, MobileNumber, Email, DriversLicense, Passport
- **Address** (owned by Player) — StreetNumber, Line1, Line2, City, State, Postal, Country
- **Membership** — PlayerId, RestaurantId
- **Favorite** — PlayerId, RestaurantId

## Setup

Requires **.NET SDK 8 or higher** (`dotnet --version`). Developed on .NET 10.

```sh
# 1. set the Postgres connection string as a User Secret (not committed)
dotnet user-secrets init --project RestaurantApi
dotnet user-secrets set "ConnectionStrings:Default" \
  "Host=db.<your-ref>.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=<your-password>;SSL Mode=Require;Trust Server Certificate=true" \
  --project RestaurantApi

# 2. apply migrations
dotnet ef database update --project RestaurantApi

# 3. run
dotnet run --project RestaurantApi
```

## Testing the API

### Locally
- **Swagger UI** — `https://localhost:<port>/swagger` (port shown in console at startup)
- **`.http` file** — open `RestaurantApi.http` in VS Code / Rider and click "Send Request"

### After hosting
Once deployed (e.g. Azure App Service, Render, Fly.io), Swagger UI is served at the same path on the public URL: `https://<your-host>/swagger`. The `.http` file also works against the hosted URL — just change the `@host` variable at the top of the file.

## Endpoints

| Method | Path | Description |
|---|---|---|
| POST | `/api/restaurants` | Save restaurant |
| POST | `/api/players` | Save player |
| POST | `/api/memberships` | Save membership |
| POST | `/api/favorites` | Save favorite |
| GET  | `/api/restaurants?name=` | Partial search restaurants by name |
| GET  | `/api/players?firstName=&lastName=` | Partial search players |
| GET  | `/api/memberships?firstName=&lastName=` | Player memberships with `isFavoriteRestaurant` |
| GET  | `/api/favorites?firstName=&lastName=` | Player favorites with `linked` flag |
| GET  | `/api/restaurants/members?name=&age=` | Members of a restaurant aged ≥ X |
