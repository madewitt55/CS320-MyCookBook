using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using MyCookBookApi.Models;
using MyCookBookApi.Services;
using MyCookBookApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IRecipeRepository, MockRecipeRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddEndpointsApiExplorer();
// Add controllers and ensure enums are properly serialized as strings
builder.Services.AddControllers().AddJsonOptions(options =>
{
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});



builder.Services.AddCors(options =>
{
options.AddPolicy("AllowAll", policy =>
policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
