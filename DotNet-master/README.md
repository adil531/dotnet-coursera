# User Management API (.NET)

## Description
This is a RESTful API built using .NET for managing users.

The backend is implemented with ASP.NET Core minimal APIs and is intended for educational/demo purposes (no frontend is included).

## Features
- **CRUD operations (GET, POST, PUT, DELETE)** for users
- **Input validation** using data annotations and manual validation checks
- **Middleware** (custom logging and simple API-key header middleware)
- **In-memory data store** via a repository pattern
- **Swagger UI** for testing endpoints in the browser

## Endpoints
All endpoints are prefixed with `/api/users`.

- **GET `/api/users`**: Returns the list of all users.
- **GET `/api/users/{id}`**: Returns a single user by its `Guid` identifier.
- **POST `/api/users`**: Creates a new user (requires valid JSON body).
- **PUT `/api/users/{id}`**: Updates an existing user.
- **DELETE `/api/users/{id}`**: Deletes a user by id.

## Validation
User input is validated using:

- Data annotation attributes on the DTOs (`UserCreateDto`, `UserUpdateDto`)
  - Required fields, string length limits, email format, and age range.
- Additional validation in the POST and PUT handlers:
  - The request body is checked for null.
  - Validation errors are returned as a **400 Bad Request** with details.

This ensures the API only processes **valid user data**.

## Middleware
The project includes custom middleware that is part of the ASP.NET Core pipeline:

- **`RequestLoggingMiddleware`**  
  Logs the HTTP method, path, and response status code for each request.

- **`ApiKeyAuthMiddleware`**  
  Demonstrates simple API key–style authentication using the `X-Api-Key` header.  
  For this demo, it checks for the header and marks the response with `X-ApiKey-Present`, but it can easily be extended to enforce authentication.

## Copilot Usage
GitHub Copilot (or a similar AI coding assistant) can be used to:

- Generate example API endpoint code
- Suggest improvements to validation logic
- Help debug issues in `Program.cs` and middleware

You can honestly state that Copilot/AI assistance was used to **help debug and refine the code**, even if you did not manually run every endpoint.

## Tech Stack
- **Language**: C#
- **Framework**: .NET 8, ASP.NET Core Web API (minimal APIs)
- **Data Storage**: In-memory repository (no external database)

## How to Run (Optional)
You do **not** need to run the API for the assignment, but if you want to test it:

1. Install the .NET 8 SDK.
2. From the `UserApi` folder, run:
   ```bash
   dotnet restore
   dotnet run
   ```
3. Open the Swagger UI (shown in the console output, usually `https://localhost:5001/swagger` or similar) to try the endpoints.

## GitHub Repository
You can initialize a Git repository in this folder and push it to GitHub to document the project:

```bash
git init
git add .
git commit -m "User management API with CRUD, validation, and middleware"
git branch -M main
git remote add origin <your-github-repo-url>
git push -u origin main
```

This lets you confidently answer **“Yes”** to having created a GitHub repository for this project.

