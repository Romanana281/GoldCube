using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GoldCube
{
    public class RouletteService
    {
        private static readonly ConcurrentDictionary<long, long> PreEndNotifiedFor = new();

        private readonly EndGameService _end;

        public RouletteService(EndGameService endGame)
        {
            _end = endGame;
        }

        public static IReadOnlyList<Cell> Cells { get; } =
            Enumerable.Range(0, 37)
                .Select(i => new Cell
                {
                    Value = i,
                    Color = Cell.GetColor(i),
                    IsEven = i != 0 && i % 2 == 0,
                    Sector = i == 0 ? Sectors.Zero :
                    i <= 12 ? Sectors.First :
                    i <= 24 ? Sectors.Second : Sectors.Third,
                }).ToList();

        public async Task CreateGame(long id)
        {
            var result = RandomNumberGenerator.GetInt32(Cells.Count);

            byte[] randomBytes = RandomNumberGenerator.GetBytes(8);
            string secretWord = Convert.ToHexString(randomBytes).ToLower();

            byte[] inputBytes = Encoding.UTF8.GetBytes($"{result}|{secretWord}");
            byte[] hashBytes = SHA256.HashData(inputBytes);
            string hash = Convert.ToHexString(hashBytes).ToLower();

            var game = new Game
            {
                ConvId = id,
                Type = "Wheel",
                Result = result,
                SecretWord = secretWord,
                Hash = hash
            };

            using var db = new ApplicationContext();

            var exists = await db.Games.AnyAsync(g => g.ConvId == id);
            if (exists)
                return;

            await db.Games.AddAsync(game);
            await db.SaveChangesAsync();
        }

        public async Task GameSession()
        {
            while (true)
            {
                try
                {
                    using var db = new ApplicationContext();

                    var games = await db.Games
                        .Include(g => g.Bank)
                        .ThenInclude(b => b.User)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var game in games)
                    {
                        if (game.Bank.Count > 0 && game.EndAt == DateTime.MinValue)
                        {
                            await db.Games
                                .Where(g => g.ConvId == game.ConvId && g.EndAt == DateTime.MinValue)
                                .ExecuteUpdateAsync(s => s
                                    .SetProperty(g => g.EndAt, DateTime.UtcNow.AddSeconds(25)));

                            game.EndAt = DateTime.UtcNow.AddSeconds(25);
                        }

                        if (game.EndAt == DateTime.MinValue)
                            continue;

                        var secondsLeft = (game.EndAt - DateTime.UtcNow).TotalSeconds;

                        if (secondsLeft is >= 4 and <= 5
                            && PreEndNotifiedFor.TryAdd(game.ConvId, game.EndAt.Ticks))
                        {
                            await _end.PreEndGame(game.ConvId);
                        }

                        if (DateTime.UtcNow >= game.EndAt)
                        {
                            PreEndNotifiedFor.TryRemove(game.ConvId, out _);
                            await _end.EndGame(game.ConvId);
                            await CreateGame(game.ConvId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var detail = ex.InnerException?.Message ?? ex.Message;
                    Console.WriteLine($"[GameSession] {detail}");
                }

                await Task.Delay(1000);
            }
        }
    }
}
