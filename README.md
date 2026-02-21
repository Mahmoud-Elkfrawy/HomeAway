# HomeAway

[![Build Status](https://img.shields.io/github/actions/workflow/status/Mahmoud-Elkfrawy/HomeAway/ci.yml?branch=develop&label=build&logo=github)](https://github.com/Mahmoud-Elkfrawy/HomeAway/actions)
[![Coverage Status](https://img.shields.io/codecov/c/github/Mahmoud-Elkfrawy/HomeAway?label=coverage)](https://codecov.io/gh/Mahmoud-Elkfrawy/HomeAway)
[![License](https://img.shields.io/github/license/Mahmoud-Elkfrawy/HomeAway)](https://github.com/Mahmoud-Elkfrawy/HomeAway/blob/develop/LICENSE)

HomeAway is a multi-layered ASP.NET Core Web API for managing hotels, rooms, reservations and users. It follows a clean, domain-driven structure with separate `API`, `Application`, `Domain`, and `Infrastructure` projects to keep concerns separated and make the codebase easy to maintain and extend.

## Example API endpoints

Below are common endpoints and example `curl` requests. Replace `https://localhost:5001` with your configured base URL and include a valid JWT where noted.

- Register a user

  curl -X POST "https://localhost:5001/api/auth/register" \
    -H "Content-Type: application/json" \
    -d '{"userName":"jdoe","email":"jdoe@example.com","fullName":"John Doe","password":"P@ssw0rd"}'

- Login and receive JWT

  curl -X POST "https://localhost:5001/api/auth/login" \
    -H "Content-Type: application/json" \
    -d '{"userName":"jdoe","password":"P@ssw0rd"}'

  Response: { "token": "<JWT_TOKEN>" }

- Get all hotels

  curl "https://localhost:5001/api/hotels"

- Get hotel by id

  curl "https://localhost:5001/api/hotels/1"

- Create a hotel (authenticated, provider role)

  curl -X POST "https://localhost:5001/api/hotels" \
    -H "Authorization: Bearer <JWT_TOKEN>" \
    -H "Content-Type: application/json" \
    -d '{"name":"Ocean View","address":"123 Beach Ave","city":"Cairo","country":"Egypt"}'

- Get all rooms

  curl "https://localhost:5001/api/rooms"

- Create a room (authenticated, provider/admin role)

  curl -X POST "https://localhost:5001/api/rooms" \
    -H "Authorization: Bearer <JWT_TOKEN>" \
    -H "Content-Type: application/json" \
    -d '{"hotelId":1,"type":0,"pricePerNight":99.99,"capacity":2}'

- Book a reservation

  curl -X POST "https://localhost:5001/api/reservations" \
    -H "Authorization: Bearer <JWT_TOKEN>" \
    -H "Content-Type: application/json" \
    -d '{"roomId":1,"userId":"<USER_ID>","from":"2025-01-01","to":"2025-01-05"}'

Add or adapt these examples to match your DTO shapes if they differ.


## Responses & HTTP status codes

The table below shows typical responses and status codes for common operations.

- Register (POST `/api/auth/register`)
  - 200 OK: "User Registered Successfully"
  - 400 Bad Request: validation errors (e.g., username taken)
  - 500 Internal Server Error: { "message": "Internal server error", "error": "..." }

- Login (POST `/api/auth/login`)
  - 200 OK: { "token": "<JWT_TOKEN>" }
  - 401 Unauthorized: "Invalid username or password"
  - 500 Internal Server Error

- Get all hotels (GET `/api/hotels`)
  - 200 OK: [ { "id":1, "name":"Ocean View", "city":"Cairo", ... }, ... ]
  - 500 Internal Server Error

- Get hotel by id (GET `/api/hotels/{id}`)
  - 200 OK: { "id":1, "name":"Ocean View", ... }
  - 404 Not Found: when id does not exist

- Create hotel (POST `/api/hotels`)
  - 201 Created: Location header points to `/api/hotels/{id}` and body contains { "id": {id} }
  - 400 Bad Request: validation errors
  - 401/403: when the caller is not authenticated or authorized

- Create room (POST `/api/rooms`)
  - 201 Created: Location header points to `/api/rooms/{id}`
  - 400 Bad Request
  - 401/403 Unauthorized or forbidden

- Book reservation (POST `/api/reservations`)
  - 201 Created: reservation created
  - 400 Bad Request: invalid dates or unavailable room
  - 500 Internal Server Error

Example successful response for `GET /api/hotels/1`:

```
{
  "id": 1,
  "name": "Ocean View",
  "address": "123 Beach Ave",
  "city": "Cairo",
  "country": "Egypt",
  "rooms": [
    { "id": 1, "type": 0, "pricePerNight": 99.99, "capacity": 2 }
  ]
}
```

Example error response:

```
{ "message": "Internal server error", "error": "Detailed error message" }
```

## Postman collection

A minimal Postman collection is included at `postman/HomeAway.postman_collection.json`. Import it into Postman and set the `baseUrl` variable to your API base URL. The collection contains example requests for register, login and get hotels.

## OpenAPI / Swagger snippet

Minimal OpenAPI paths you can add to an existing `openapi.json` or use as a starting point with Swagger UI:

```yaml
openapi: 3.0.1
info:
  title: HomeAway API
  version: 1.0.0
servers:
  - url: https://localhost:5001
paths:
  /api/auth/register:
    post:
      summary: Register a new user
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
      responses:
        '200': { description: OK }
        '400': { description: Bad Request }
  /api/auth/login:
    post:
      summary: Login and obtain JWT
      requestBody:
        required: true
        content:
          application/json: {}
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: object
                properties:
                  token:
                    type: string
  /api/hotels:
    get:
      summary: Get all hotels
      responses:
        '200': { description: OK }
    post:
      summary: Create hotel
      security:
        - bearerAuth: []
      responses:
        '201': { description: Created }
components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
```

Paste this into your Swagger/OpenAPI editor or extend your existing `swagger.json` to document endpoints more completely.

## Which license should you use?

Recommended options:
- MIT — permissive, simple, widely used. Good if you want others to freely use and modify the project with attribution.
- Apache-2.0 — permissive and includes patent grant. Use if you want stronger legal protection.
- GPL-3.0 — copyleft; requires derivative works to be open-source under the same license.

Recommendation: use `MIT` for most open-source web API projects unless you need Apache's patent protection or prefer GPL's copyleft requirements.


## Key features

- Hotel, room and reservation management
- User authentication and authorization (JWT-based)
- Repository and service layers to encapsulate data access and business logic
- Entity Framework Core migrations for database schema management

## Solution layout

- `HomeAway.API` — Web API project and HTTP controllers
- `HomeAway.Application` — Application services, DTOs and interfaces
- `HomeAway.Domain` — Entities, value objects and domain interfaces
- `HomeAway.Infrastructure` — EF Core `DbContext`, repositories, identity and persistence

This layered organization keeps domain logic independent of frameworks and infrastructure concerns.

## Technology stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core (EF Core) for data access and migrations
- ASP.NET Core Identity + JWT for authentication
- SQL Server (development/production DB)

## Getting started (development)

Prerequisites:
- .NET 8 SDK
- SQL Server or LocalDB

Steps:
1. Clone the repository:
   `git clone <repo-url>`
2. Set the database connection string and JWT settings in `HomeAway.API/appsettings.json` or via environment variables.
3. From the solution root, restore and build:
   `dotnet restore` then `dotnet build`
4. Apply EF Core migrations (from the `HomeAway.Infrastructure` project or solution root):
   `dotnet ef database update --project HomeAway.Infrastructure --startup-project HomeAway.API`
5. Run the API (from `HomeAway.API`):
   `dotnet run --project HomeAway.API`

The API will be reachable at the configured URL (commonly `https://localhost:5001` or `http://localhost:5000`).

## Configuration

Primary configuration values live in `HomeAway.API/appsettings.json`. Important settings include:
- Connection string for the SQL Server database
- JWT configuration (issuer, audience, signing key, expiration)

For production, prefer environment variables or a secrets store for sensitive configuration.

## Database & migrations

Database schema is managed via EF Core migrations located in `HomeAway.Infrastructure/Migrations`. Use the `dotnet ef` CLI to create or apply migrations and to inspect the database state.

## Contributing

- Follow the branch strategy in the repository (feature branches -> pull requests -> `develop` branch)
- Write unit tests for new business logic where applicable
- Keep changes focused and include a short description in PRs

## License

MIT

## Contacts

For questions about the codebase or to contribute, open an issue or a pull request on the repository.
