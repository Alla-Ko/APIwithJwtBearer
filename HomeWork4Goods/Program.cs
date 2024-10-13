using HomeWork4Products.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);
// Налаштування культури
var defaultCulture = new CultureInfo("uk-UA"); // Українська культура
var supportedCultures = new[]
{
                new CultureInfo("uk-UA"), // Українська культура
                new CultureInfo("en-US")  // Англійська культура
            };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("productContext") ?? throw new InvalidOperationException("Connection string 'productContext' not found.")));

builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("productContext"));
});
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UserContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //confirmed email
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequiredUniqueChars = 0;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserContext>();
builder.Services.AddScoped<IServiceProducts, ServiceProducts>();
// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MyAdminRole", policy => policy.RequireRole("admin"));
//});
var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
SeedData.Initialize(services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Products/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// Використовуємо налаштування культури
app.UseRequestLocalization(localizationOptions);
app.UseRouting();



app.UseAuthentication(); // Додаємо автентифікацію
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
