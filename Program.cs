using Lesson2;
using Lesson2.Database;
using Lesson2.Mapping;
using Lesson2.Models;
using Lesson2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IRepository<Sender>, UserRepository>();
builder.Services.AddSingleton<SenderTokenStore>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(_ => { }, typeof(AddMessageRequestToMessageMappingProfile).Assembly);

builder.Services.AddScoped<IMessageProvider>(provider =>
    new JsonMessageProvider(builder.Configuration["FilePath"] ?? "messages.json"));
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISenderService, SenderService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<SenderAuthorizationMiddleware>();
app.UseMiddleware<MessageFromAuthorizationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Lesson2 API v1"));
}

app.MapControllers();

app.Run();