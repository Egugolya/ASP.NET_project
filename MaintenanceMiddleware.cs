using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lesson2;

public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_configuration.GetValue<bool>("Maintenance"))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(@"
                <!DOCTYPE html>
                <html lang='uk'>
                <head>
                    <meta charset='UTF-8'>
                    <title>Технічні роботи</title>
                    <style>
                        body { font-family: Arial, sans-serif; text-align: center; padding: 50px; background-color: #f4f4f4; color: #333; }
                        h1 { color: #d9534f; }
                    </style>
                </head>
                <body>
                    <h1>Сайт на технічному обслуговуванні</h1>
                    <p>Ми проводимо планові оновлення. Будь ласка, зазирніть пізніше!</p>
                </body>
                </html>");
            return;
        }

        await _next(context);
    }
}
