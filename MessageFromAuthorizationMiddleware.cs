using System.Text.Json;
using Lesson2.Models.DTO;

namespace Lesson2;

public class MessageFromAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public MessageFromAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method)
            || !string.Equals(context.Request.Path.Value?.TrimEnd('/'), "/messages", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (context.Items["User"] is not string email)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Request.EnableBuffering();
        AddMessageRequest? request = null;
        try
        {
            request = await context.Request.ReadFromJsonAsync<AddMessageRequest>();
        }
        catch (JsonException)
        {
            request = null;
        }

        context.Request.Body.Position = 0;
        if (request != null
            && !string.IsNullOrWhiteSpace(request.From)
            && !string.Equals(email, request.From, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }
}
