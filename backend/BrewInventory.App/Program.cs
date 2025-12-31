using BrewInventory.App.Data;
using BrewInventory.App.Endpoints;
using BrewInventory.App.Models;
using BrewInventory.App.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BrewInventoryContext>();

builder.Services.Configure<BrewfatherSettings>(
    builder.Configuration.GetSection("Brewfather"));

builder.Services.AddHttpClient<IBrewfatherClient, BrewfatherClient>();
builder.Services.AddScoped<IBrewfatherSyncService, BrewfatherSyncService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("AllowAngularDev");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var browserPath = Path.Combine(builder.Environment.WebRootPath, "browser");
var fileProvider = new PhysicalFileProvider(browserPath);

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = fileProvider,
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = ""
});

app.UseHttpsRedirection();

app.MapFallbackToFile("index.html", new StaticFileOptions
{
    FileProvider = fileProvider
});

app.MapFermentableEndpoints();
app.MapHopEndpoints();
app.MapYeastEndpoints();
app.MapMiscEndpoints();
app.MapRecipeEndpoints();
app.MapSyncEndpoints();

app.Run();