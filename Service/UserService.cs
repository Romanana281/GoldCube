using Microsoft.EntityFrameworkCore;

namespace GoldCube
{
    public class UserService
    {
        public const int StartingBalance = 1000;

        private readonly VkApiService _vk;
        private readonly RouletteService _roulette;

        public UserService(VkApiService vk, RouletteService roulette)
        {
            _vk = vk;
            _roulette = roulette;
        }

        public async Task<User> GetUser(ApplicationContext db, long id)
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                user = new User
                {
                    Id = id,
                    Name = await _vk.GetName(id),
                    Balance = StartingBalance
                };

                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<Game> GetOrCreateGame(ApplicationContext db, long convId)
        {
            var game = await db.Games
                .Include(g => g.Bank)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(g => g.ConvId == convId);

            if (game != null)
                return game;

            await _roulette.CreateGame(convId);

            return await db.Games
                .Include(g => g.Bank)
                .ThenInclude(b => b.User)
                .FirstAsync(g => g.ConvId == convId);
        }
    }
}