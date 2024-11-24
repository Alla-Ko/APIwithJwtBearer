using HomeWork4Products.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Налаштування культури
var defaultCulture = new CultureInfo("uk-UA");
var supportedCultures = new[]
{
    new CultureInfo("uk-UA"),
    new CultureInfo("en-US")
};
        
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// Налаштовуємо підключення до бази даних
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("productContext")
        ?? throw new InvalidOperationException("Connection string 'productContext' not found.")));

builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("productContext"));
});

// Додаємо Identity з підтримкою ролей та бази даних
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Налаштування для підтвердження email та вимоги до паролів
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<UserContext>()
.AddDefaultTokenProviders();  // Для генерації токенів (наприклад, для відновлення паролю)

// Додаємо JWT аутентифікацію для API
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})

.AddCookie(options =>
 {
     // Додаємо класичну аутентифікацію для веб-сайту
     options.LoginPath = "/User/Auth"; // Налаштуйте шлях до сторінки входу
     options.LogoutPath = "/User/Logout"; // Налаштуйте шлях до сторінки виходу
     options.AccessDeniedPath = "/Products/Index"; // Налаштуйте шлях до сторінки доступу
 });
// Додаємо авторизацію для ролей
builder.Services.AddAuthorization(options =>
{
    // Можна додати політики для ролей, якщо необхідно
    //options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()  // Дозволяє всі домени
               .AllowAnyMethod()  // Дозволяє всі HTTP методи
               .AllowAnyHeader(); // Дозволяє всі заголовки
    });
});


// Реєструємо сервіс для продуктів
builder.Services.AddScoped<IServiceProducts, ServiceProducts>();

// Додаємо підтримку контролерів і переглядів
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ініціалізуємо базу даних
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
SeedData.Initialize(services);

// Налаштовуємо pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Products/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);
app.UseRouting();

// Додаємо підтримку CORS
app.UseCors("AllowAllOrigins");

// Додаємо автентифікацію та авторизацію
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();
