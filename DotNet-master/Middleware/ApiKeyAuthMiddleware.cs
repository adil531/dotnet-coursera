namespace UserApi.Middleware;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApiKeyHeaderName = "X-Api-Key";

    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Demonstration-only: check header existence but do not block.
        if (!context.Request.Headers.ContainsKey(ApiKeyHeaderName))
        {
            // You could enforce 401 here, but for the assignment
            // we'll just add a note to the response headers.
            context.Response.Headers["X-ApiKey-Present"] = "false";
        }
        else
        {
            context.Response.Headers["X-ApiKey-Present"] = "true";
        }

        await _next(context);
    }
}

