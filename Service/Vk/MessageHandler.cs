using System.Text.Json;
using System.Threading.Tasks;

namespace GoldCube
{
    public class MessageHandler
    {
        private readonly CommandHandler _commandHandler;

        public MessageHandler(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task Handle(string json)
        {
            var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;
            var updates = root.GetProperty("updates");

            foreach (var update in updates.EnumerateArray())
            {
                var type = update.GetProperty("type").GetString();

                if (type != "message_new")
                    continue;

                var message = update
                    .GetProperty("object")
                    .GetProperty("message");

                await _commandHandler.Handle(message);
            }
        }
    }
}