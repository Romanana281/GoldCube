namespace GoldCube
{
    public static class BetTypeExtensions
    {
        public static string ToRussia(this BetType type)
        {
            return type switch
            {
                BetType.Red => "красное",
                BetType.Black => "черное",
                BetType.Even => "четное",
                BetType.Odd => "нечетное",
                BetType.Number => "число",
                _ => "неизвестно"
            };
        }

        public static string ToRussia(this Colors type)
        {
            return type switch
            {
                Colors.Red => "красное",
                Colors.Black => "черное",
                Colors.Green => "зеленое",
                _ => "неизвестно"
            };
        }

        public static string FormatBet(this TemplateBank bet)
        {
            return bet.Type switch
            {
                BetType.Red =>
                    $"{bet.User.Name} — {bet.Amount:N0} на красное",

                BetType.Black =>
                    $"{bet.User.Name} — {bet.Amount:N0} на чёрное",

                BetType.Even =>
                    $"{bet.User.Name} — {bet.Amount:N0} на чётное",

                BetType.Odd =>
                    $"{bet.User.Name} — {bet.Amount:N0} на нечётное",

                BetType.Number =>
                    $"{bet.User.Name} — {bet.Amount:N0} на {bet.Number}",

                _ => "Unknown"
            };
        }
    }
}