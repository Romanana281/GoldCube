using Microsoft.Extensions.Configuration;

namespace GoldCube
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            DotNetEnv.Env.Load();
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
    }
}