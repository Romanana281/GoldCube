using System.Text.Json;

namespace GoldCube
{
    public class User
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public int Balance { get; set; }

        public bool IsAdmin { get; set; } = false;

        public UserState State { get; set; }

        public BetType? PendingBetType { get; set; }

        public int? PendingNumber { get; set; }

        public long? PeerId { get; set; }
    }
}