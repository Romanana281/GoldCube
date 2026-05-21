using System.Collections.Concurrent;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GoldCube
{
    public class EndGameService
    {
        private static readonly ConcurrentDictionary<long, SemaphoreSlim> GameLocks = new();

        private readonly VkApiService _vk;

        public EndGameService(VkApiService vk)
        {
            _vk = vk;
        }

        public async Task EndGame(long convId)
        {
            var gate = GameLocks.GetOrAdd(convId, _ => new SemaphoreSlim(1, 1));
            await gate.WaitAsync();
            try
            {
                using var db = new ApplicationContext();

                var game = await db.Games
                    .Include(g => g.Bank)
                    .ThenInclude(b => b.User)
                    .FirstOrDefaultAsync(g => g.ConvId == convId);

                if (game == null)
                    return;

                var result = RouletteService.Cells.First(x => x.Value == game.Result);

                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine($"Выпало число {result.Value}, {result.Color.ToRussia()}!\n");

                var winBuilder = new StringBuilder();
                var loseBuilder = new StringBuilder();

                foreach (var bet in game.Bank)
                {
                    var (hasWon, multiplier) = IsWin(bet, result);

                    string betTypeRu = bet.Type.ToRussia();
                    string betTarget = betTypeRu == "число" ? $"{betTypeRu} {bet.Number}" : betTypeRu;
                    string userName = bet.User?.Name ?? "Игрок";

                    if (hasWon)
                    {
                        int winAmount = bet.Amount * multiplier;
                        int netProfit = winAmount - bet.Amount;

                        winBuilder.AppendLine($"🟢 {userName} ставка {bet.Amount:N0} на {betTarget} выиграла (+{netProfit:N0})");

                        if (bet.User != null)
                        {
                            bet.User.Balance += winAmount;
                            db.Users.Update(bet.User);
                        }
                    }
                    else
                    {
                        loseBuilder.AppendLine($"🔴 {userName} ставка {bet.Amount:N0} на {betTarget} проиграла");
                    }
                }

                messageBuilder.Append(winBuilder);
                messageBuilder.Append(loseBuilder);
                messageBuilder.AppendLine($"\n\nХэш игры (sha256): {game.Hash}");
                messageBuilder.Append($"Проверка честности: {result.Value}|{game.SecretWord}");

                await _vk.SendMessage(new Message
                {
                    UserId = game.ConvId,
                    Text = messageBuilder.ToString()
                });

                db.Games.Remove(game);
                await db.SaveChangesAsync();
            }
            finally
            {
                gate.Release();
            }
        }

        public async Task PreEndGame(long convId)
        {
            await _vk.SendMessage(new Message
            {
                UserId = convId,
                Text = "Итак, результаты раунда..."
            });
        }

        private static (bool isWin, int multiplier) IsWin(TemplateBank bet, Cell result)
        {
            return bet.Type switch
            {
                BetType.Red => (result.Color == Colors.Red, 2),
                BetType.Black => (result.Color == Colors.Black, 2),
                BetType.Even => (result.Value != 0 && result.IsEven, 2),
                BetType.Odd => (result.Value != 0 && !result.IsEven, 2),
                BetType.Number => (bet.Number == result.Value, 36),
                _ => (false, 0)
            };
        }
    }
}
