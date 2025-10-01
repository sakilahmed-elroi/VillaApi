using VillaApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // Enables middleware to serve generated Swagger as JSON endpoint
    app.UseSwaggerUI();     // Enables Swagger UI at /swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
