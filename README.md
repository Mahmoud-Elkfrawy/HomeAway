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

Specify the project license here (e.g., MIT) or replace this section with your chosen license.

## Contacts

For questions about the codebase or to contribute, open an issue or a pull request on the repository.
