using System.Text.Json.Serialization;

namespace GoldCube
{
    public class VkKeyboard
    {
        [JsonPropertyName("inline")]
        public bool Inline { get; set; } = false;

        [JsonPropertyName("one_time")]
        public bool OneTime { get; set; } = false;

        [JsonPropertyName("buttons")]
        public List<List<VkButton>> Buttons { get; set; } = new();
    }

    public class VkButton
    {
        [JsonPropertyName("action")]
        public VkButtonAction Action { get; set; } = new();

        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }

    public class VkButtonAction
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("label")]
        public string Label { get; set; } = "";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("payload")]
        public string? Payload { get; set; }
    }
}