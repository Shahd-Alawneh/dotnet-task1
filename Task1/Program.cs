using Microsoft.EntityFrameworkCore;
using Task1.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("CustomerDb"));


// OpenAPI (generates the JSON endpoint)
builder.Services.AddOpenApi();

// Swagger UI (nice interactive UI at /swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // OpenAPI JSON endpoint (default path: /openapi/v1.json)
    app.MapOpenApi();

    // Swagger UI at /swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Connect Swagger UI to the OpenAPI JSON that MapOpenApi exposes
        options.SwaggerEndpoint("/openapi/v1.json", "Customer API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }