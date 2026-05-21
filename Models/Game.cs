using System.ComponentModel.DataAnnotations;

namespace GoldCube
{
    public class Game
    {
        [Key]
        public long ConvId { get; set; }

        public string? Type { get; set; }

        public int Result { get; set; }

        public string? SecretWord { get; set; }

        public string? Hash { get; set; }

        public DateTime EndAt { get; set; }

        public List<TemplateBank> Bank { get; set; } = [];
    }

    public class TemplateBank
    {
        public int Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int Amount { get; set; }

        public BetType Type { get; set; }

        public int? Number { get; set; }
    }
}