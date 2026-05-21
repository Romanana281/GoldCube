namespace GoldCube
{
    public class LongPollSetting
    {
        public ResponseData? Response { get; set; }
    }

    public class ResponseData
    {
        public string? Key { get; set; }

        public string? Server { get; set; }

        public string? Ts { get; set; }
    }
}