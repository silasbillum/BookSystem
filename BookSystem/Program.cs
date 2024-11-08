using BookSystem.Components;
using BookSystem.Context;
using BookSystem.DomainModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50MB
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.MaxDepth = 64; // Adjust depth if needed
});
builder.Services.AddHttpClient("YourApiClient", client =>
{
client.BaseAddress = new Uri("https://localhost:7219/"); // Set to your API's base URL
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7219/SavedBooks") });





builder.Services.AddMudServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();
//app.MapPost("/api/books", (Book book) =>
//{
//    // Handle the received book data
//    Console.WriteLine($"Received book: {book.BookTitle}, Pages: {book.BookPages}, Summary: {book.BookSummary}");

//    // Example response
//    return Results.Ok();
//});


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapScalarApiReference(options =>
{
    options
        .WithDefaultHttpClient(ScalarTarget.C, ScalarClient.Libcurl)
        .OpenApiRoutePattern = "/swagger/v1/swagger.json";


});

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();





app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
