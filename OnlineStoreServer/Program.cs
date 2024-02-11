using Microsoft.EntityFrameworkCore;
using OnlineStoreApiBlazor.Data;
using OnlineStoreServer.Components;
using OnlineStoreServer.Repositories;
using OnlineStoreServer.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default") ??
    throw new InvalidOperationException("Connection string not found"));
});

// Register services at the server

builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();

// Register services at the client
builder.Services.AddScoped(httpClient => new HttpClient { BaseAddress = new Uri("https://localhost:7003") });
builder.Services.AddScoped<IProductService, ClientService>();
builder.Services.AddScoped<ICategoryService, ClientService>();

builder.Services.AddScoped<MessageDialogService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseSwagger();
app.UseSwaggerUI();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();


app.Run();
