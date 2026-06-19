using ConnectFour;
using ConnectFour.Data;
using ConnectFour.Models;
using ConnectFour.Services;
using ConnectFour.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var dbPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "SmartSpendDb.sqlite");
    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
    options.UseSqlite(SqliteConnectionFactory.CreateConnectionString(dbPath));
});
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, DefaultAuthenticationStateProvider>();

builder.Services.AddSingleton<GameState>();

// Persist data protection keys to disk so antiforgery tokens work across restarts/containers
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtection-Keys")))
    .SetApplicationName("ConnectFourApp");

builder.Services.AddAntiforgery();

// Respect proxy headers (X-Forwarded-For, X-Forwarded-Proto) when running behind a reverse proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Clear default restrictions so common hosting providers' proxies are accepted
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Seed the database (create if missing)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    DatabaseSeeder.Seed(dbContext).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseForwardedHeaders();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<ConnectFour.Components.App>()
    .AddInteractiveServerRenderMode();

// Simple fallback endpoint to delete a transaction and redirect back to the transactions page.
app.MapGet("/delete-transaction/{id:int}", async (int id, HttpContext http, ApplicationDbContext db) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    var transaction = await db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
    if (transaction is not null)
    {
        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync();
    }

    return Results.Redirect("/transactions");
});

app.Run();
