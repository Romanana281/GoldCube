namespace GoldCube
{
    public class Message
    {
        public long UserId { get; set; }
        public string? Text { get; set; } = "";
        public VkKeyboard? Keyboard { get; set; } = null;
    }
}