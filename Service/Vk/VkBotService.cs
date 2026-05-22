using System.Text.Json;

namespace GoldCube
{
    public class VkBotService
    {
        private readonly VkApiService _vkApi;
        private readonly MessageHandler _messageHandler;

        public VkBotService(VkApiService vkApi, MessageHandler messageHandler)
        {
            _vkApi = vkApi;
            _messageHandler = messageHandler;
        }

        public async Task Run()
        {
            var (server, key, ts) = await GetLongPollCredentials();

            while (true)
            {
                try
                {
                    var updateJson = await _vkApi.GetUpdates(server, key, ts);

                    var doc = JsonDocument.Parse(updateJson);

                    if (doc.RootElement.TryGetProperty("failed", out var failed))
                    {
                        var code = failed.GetInt32();
                        Console.WriteLine($"LongPoll failed: {code}");

                        if (code == 1)
                        {
                            ts = doc.RootElement.GetProperty("ts").GetString()!;
                            continue;
                        }

                        (server, key, ts) = await GetLongPollCredentials();
                        continue;
                    }

                    ts = doc.RootElement.GetProperty("ts").GetString()!;
                    await _messageHandler.Handle(updateJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(1000);
                }
            }
        }

        private async Task<(string server, string key, string ts)> GetLongPollCredentials()
        {
            var json = await _vkApi.GetLongPollServer();

            var settings = JsonSerializer.Deserialize<LongPollSetting>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidOperationException("Не удалось получить Long Poll сервер VK.");

            var response = settings.Response
                ?? throw new InvalidOperationException("VK Long Poll: пустой response.");

            return (
                response.Server ?? throw new InvalidOperationException("VK Long Poll: нет server."),
                response.Key ?? throw new InvalidOperationException("VK Long Poll: нет key."),
                response.Ts ?? throw new InvalidOperationException("VK Long Poll: нет ts."));
        }
    }
}
