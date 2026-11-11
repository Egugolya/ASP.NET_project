using Lesson2.Services;
using Microsoft.AspNetCore.Http;

namespace Lesson2;

public class SenderAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public SenderAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISenderService senderService)
    {
        if (!HttpMethods.IsPost(context.Request.Method)
            || !string.Equals(context.Request.Path.Value?.TrimEnd('/'), "/messages", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["X-My-authorization"].ToString().Trim();
        if (string.IsNullOrEmpty(token))
        {
            var authorization = context.Request.Headers.Authorization.ToString();
            const string bearerPrefix = "Bearer ";
            if (authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                token = authorization[bearerPrefix.Length..].Trim();
        }

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token["Bearer ".Length..].Trim();

        var email = token.Length > 0 ? await senderService.GetEmailForTokenAsync(token) : null;
        if (email == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Items["User"] = email;

        await _next(context);
    }
}
