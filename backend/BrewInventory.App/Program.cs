using BrewInventory.Infrastructure.Brewfather;
using BrewInventory.Infrastructure.Brewfather.Settings;
using BrewInventory.Infrastructure.Persistence;
using BrewInventory.Infrastructure.Persistence.Repositories;
using BrewInventory.Infrastructure.Services;
using BrewInventory.Application.Repositories;
using BrewInventory.Application.Services;
using BrewInventory.App.Endpoints;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<BrewInventoryContext>();

builder.Services.Configure<BrewfatherSettings>(
    builder.Configuration.GetSection("Brewfather"));

builder.Services.AddHttpClient<IBrewfatherClient, BrewfatherClient>();
builder.Services.AddScoped<IBrewfatherSyncService, BrewfatherSyncService>();

builder.Services.AddScoped<IFermentableRepository, FermentableRepository>();
builder.Services.AddScoped<IHopRepository, HopRepository>();
builder.Services.AddScoped<IYeastRepository, YeastRepository>();
builder.Services.AddScoped<IMiscRepository, MiscRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientPurchaseService, IngredientPurchaseService>();
builder.Services.AddScoped<IExcelExporter, ExcelExporter>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BrewInventoryContext>();
    await dbContext.Database.MigrateAsync();
}

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
app.MapIngredientPurchaseEndpoints();

await app.RunAsync();
