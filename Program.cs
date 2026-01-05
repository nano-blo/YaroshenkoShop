using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using YaroshenkoShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры и представления
builder.Services.AddControllersWithViews();

// Подключаем БД
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Проверяем подключение при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Простая проверка - попробуем получить количество игр
        var gameCount = dbContext.Games.Count();
        Console.WriteLine($"✅ Успешно подключено к БД! Игр в базе: {gameCount}");

        // Выведем список таблиц для проверки
        Console.WriteLine("\n📋 Структура базы данных:");
        Console.WriteLine($"- Games: {dbContext.Games.Count()} записей");
        Console.WriteLine($"- Keys: {dbContext.Keys.Count()} записей");
        Console.WriteLine($"- Developers: {dbContext.Developers.Count()} записей");
        Console.WriteLine($"- Genres: {dbContext.Genres.Count()} записей");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка подключения к базе данных:");
        Console.WriteLine($"Сообщение: {ex.Message}");
        Console.WriteLine($"\nПроверьте:");
        Console.WriteLine($"1. Запущен ли SQL Server");
        Console.WriteLine($"2. Правильность имени сервера: LAPTOP-SLJ9QJNM");
        Console.WriteLine($"3. Существует ли база данных 'Keys'");
        Console.WriteLine($"4. Имеете ли вы права доступа");
    }
}

// Остальной код...
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();