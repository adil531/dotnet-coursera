using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.Middleware;
using UserApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection for user repository
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Custom middleware: logging and simple API-key check
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ApiKeyAuthMiddleware>();

var usersGroup = app.MapGroup("/api/users");

// GET: /api/users
usersGroup.MapGet("/", ([FromServices] IUserRepository repo) =>
{
    return Results.Ok(repo.GetAll());
})
.WithName("GetUsers")
.WithOpenApi();

// GET: /api/users/{id}
usersGroup.MapGet("/{id:guid}", ([FromServices] IUserRepository repo, Guid id) =>
{
    var user = repo.GetById(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
})
.WithName("GetUserById")
.WithOpenApi();

// POST: /api/users
usersGroup.MapPost("/", async (
    [FromServices] IUserRepository repo,
    HttpContext httpContext) =>
{
    var dto = await httpContext.Request.ReadFromJsonAsync<UserCreateDto>();
    if (dto is null)
    {
        return Results.BadRequest("Request body is required.");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(dto);
    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
    {
        return Results.ValidationProblem(validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(v => v.ErrorMessage ?? string.Empty).ToArray()
            ));
    }

    var created = repo.Create(dto);
    return Results.Created($"/api/users/{created.Id}", created);
})
.WithName("CreateUser")
.WithOpenApi();

// PUT: /api/users/{id}
usersGroup.MapPut("/{id:guid}", async (
    [FromServices] IUserRepository repo,
    Guid id,
    HttpContext httpContext) =>
{
    var dto = await httpContext.Request.ReadFromJsonAsync<UserUpdateDto>();
    if (dto is null)
    {
        return Results.BadRequest("Request body is required.");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(dto);
    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
    {
        return Results.ValidationProblem(validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(v => v.ErrorMessage ?? string.Empty).ToArray()
            ));
    }

    var updated = repo.Update(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
})
.WithName("UpdateUser")
.WithOpenApi();

// DELETE: /api/users/{id}
usersGroup.MapDelete("/{id:guid}", ([FromServices] IUserRepository repo, Guid id) =>
{
    var deleted = repo.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteUser")
.WithOpenApi();

app.Run();

