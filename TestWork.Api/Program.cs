using Scalar.AspNetCore;
using TestWork.Data;
using TestWork.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();

builder.Services
    .AddRepositories("Server=localhost;Port=5432;User Id=admin;Password=admin;Database=testdb2")
    .AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();