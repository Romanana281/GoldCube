using System.Net.Http;
using System.Text.Json;

namespace GoldCube
{
    public class VkApiService
    {
        private readonly HttpClient _client;
        private readonly VkSettings _vkSettings;

        public VkApiService(VkSettings vkSettings)
        {
            _client = new HttpClient();
            _vkSettings = vkSettings;
        }

        public async Task<string> GetLongPollServer()
        {
            var url =
                $"https://api.vk.com/method/groups.getLongPollServer?group_id={_vkSettings.GroupID}&access_token={_vkSettings?.Token}&v=5.199";
            return await _client.GetStringAsync(url);
        }

        public async Task<string> GetUpdates(string server, string key, string ts)
        {
            var url =
                $"{server}?act=a_check&key={key}&ts={ts}&wait=25";
            return await _client.GetStringAsync(url);
        }

        public async Task SendMessage(Message message)
        {
            var encodedText = Uri.EscapeDataString(message.Text);
            var url =
                $"https://api.vk.com/method/messages.send" +
                $"?peer_id={message.UserId}" +
                $"&message={encodedText}" +
                $"&random_id={Random.Shared.Next()}" +
                $"&access_token={_vkSettings?.Token}" +
                "&v=5.199";

            if (message.Keyboard != null)
            {
                var keyboard = Uri.EscapeDataString(JsonSerializer.Serialize(message.Keyboard));
                url += $"&keyboard={keyboard}";
            }

            var response = await _client.GetStringAsync(url);

            Console.WriteLine($"[VK] Message sent to {message.UserId}: {response}");
        }

        public async Task<string> GetName(long id)
        {
            var url =
               $"https://api.vk.com/method/users.get" +
               $"?user_ids={id}" +
               $"&access_token={_vkSettings?.Token}" +
               "&v=5.199";

            var response = await _client.GetStringAsync(url);
            var doc = JsonDocument.Parse(response);

            return doc.RootElement
                .GetProperty("response")[0]
                .GetProperty("first_name")
                .GetString() ?? "Unknown";
        }
    }
}
