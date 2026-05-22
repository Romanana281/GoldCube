using Microsoft.EntityFrameworkCore;

namespace GoldCube
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<TemplateBank> TemplateBanks => Set<TemplateBank>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? throw new InvalidOperationException(
                    "DATABASE_URL не задан. Для Docker: docker-compose. Локально: .env или переменная окружения.");

            optionsBuilder.UseNpgsql(connectionString, npgsql =>
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), errorCodesToAdd: null));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemplateBank>()
                .HasOne(b => b.Game)
                .WithMany(g => g.Bank)
                .HasForeignKey(b => b.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TemplateBank>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}