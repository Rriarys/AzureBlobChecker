using Serilog;
using Serilog.AspNetCore;

namespace AzureBlobChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Настройка Serilog для записи логов в файл
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Hour) // Записываем логи в файл, каждый день новый
                .CreateLogger();

            // Создаем builder для веб-приложения
            var builder = WebApplication.CreateBuilder(args);

            // Очистка стандартных провайдеров логирования и добавление Serilog
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(); // Здесь используем метод AddSerilog

            // Добавляем Razor Pages
            builder.Services.AddRazorPages();

            // Строим и запускаем приложение
            var app = builder.Build();

            // Конфигурация HTTP-запросов
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // Устанавливаем HSTS для продакшн окружения
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Маршруты для Razor Pages
            app.MapRazorPages();

            // Запуск приложения
            app.Run();
        }
    }
}
