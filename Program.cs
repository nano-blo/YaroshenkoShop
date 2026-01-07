using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YaroshenkoShop.Data;
using YaroshenkoShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Настройка подключения к БД
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Настройка Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Добавляем контроллеры
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Проверка подключения (добавьте после создания app)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        Console.WriteLine($"✅ Успешно подключено к БД!");
        Console.WriteLine($"- Игр: {dbContext.Games.Count()}");
        Console.WriteLine($"- Ключей: {dbContext.Keys.Count()}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка: {ex.Message}");
    }
}

// Middleware в правильном порядке
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();    // Важно: ДО UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();