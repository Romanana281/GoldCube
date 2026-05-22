using Microsoft.EntityFrameworkCore;

namespace GoldCube
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            if (File.Exists(".env"))
                DotNetEnv.Env.Load();

            await WaitForDatabaseAsync();
            await ApplyMigrationsAsync();
            // var config = new ConfigurationBuilder()
            //     .AddUserSecrets<Program>()
            //     .Build();

            var vkApi = new VkApiService(
                new VkSettings
                {
                    // Token = config["VK:ApiKey"]!
                    Token = Environment.GetEnvironmentVariable("VKApiKey")
                });

            var EndGameService = new EndGameService(vkApi);
            var Roulette = new RouletteService(EndGameService);

            var UserService = new UserService(vkApi, Roulette);
            var CommandHandler = new CommandHandler(UserService, vkApi);
            var MessageHandler = new MessageHandler(CommandHandler);

            _ = Task.Run(async () =>
            {
                await Roulette.GameSession();
            });

            var bot = new VkBotService(vkApi, MessageHandler);
            await bot.Run();
        }

        private static async Task WaitForDatabaseAsync()
        {
            const int maxAttempts = 30;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await using var db = new ApplicationContext();
                    if (await db.Database.CanConnectAsync())
                    {
                        Console.WriteLine("[DB] Подключение к PostgreSQL установлено.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB] Попытка {attempt}/{maxAttempts}: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            throw new InvalidOperationException("PostgreSQL недоступен после ожидания.");
        }

        private static async Task ApplyMigrationsAsync()
        {
            await using var db = new ApplicationContext();
            await db.Database.MigrateAsync();
            Console.WriteLine("[DB] Миграции применены.");
        }
    }
}