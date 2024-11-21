using Serilog;
using Serilog.AspNetCore;

namespace AzureBlobChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ��������� Serilog ��� ������ ����� � ����
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Hour) // ���������� ���� � ����, ������ ���� �����
                .CreateLogger();

            // ������� builder ��� ���-����������
            var builder = WebApplication.CreateBuilder(args);

            // ������� ����������� ����������� ����������� � ���������� Serilog
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(); // ����� ���������� ����� AddSerilog

            // ��������� Razor Pages
            builder.Services.AddRazorPages();

            // ������ � ��������� ����������
            var app = builder.Build();

            // ������������ HTTP-��������
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // ������������� HSTS ��� �������� ���������
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // �������� ��� Razor Pages
            app.MapRazorPages();

            // ������ ����������
            app.Run();
        }
    }
}
