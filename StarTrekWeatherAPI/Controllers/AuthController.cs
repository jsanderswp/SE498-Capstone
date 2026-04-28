using System.Net.Http.Headers;
using System.Text;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string Username = "admin";
    private const string Password = "password123";

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            Unauthorized(context);
            return;
        }

        try
        {
            var header = AuthenticationHeaderValue.Parse(authHeader!);
            if (header.Scheme != "Basic" || header.Parameter == null)
            {
                Unauthorized(context);
                return;
            }

            var credentials = Encoding.UTF8
                .GetString(Convert.FromBase64String(header.Parameter))
                .Split(':', 2);

            if (credentials[0] != Username || credentials[1] != Password)
            {
                Unauthorized(context);
                return;
            }
        }
        catch
        {
            Unauthorized(context);
            return;
        }

        await _next(context);
    }

    private static void Unauthorized(HttpContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"StarTrekWeatherAPI\"";
    }
}