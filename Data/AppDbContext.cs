using Microsoft.EntityFrameworkCore;
using YaroshenkoShop.Models;

namespace YaroshenkoShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<Key> Keys { get; set; }
        // Добавьте остальные модели по мере необходимости

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи многие-ко-многим между Game и Genre
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.id_игры, gg.id_жанра });

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.id_игры);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.id_жанра);

            // Убираем каскадное удаление для некоторых связей
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Developer)
                .WithMany(d => d.Games)
                .HasForeignKey(g => g.id_разработчика)
                .OnDelete(DeleteBehavior.SetNull); // При удалении разработчика игры остаются

            modelBuilder.Entity<Key>()
                .HasOne(k => k.Game)
                .WithMany(g => g.Keys)
                .HasForeignKey(k => k.id_игры)
                .OnDelete(DeleteBehavior.Cascade); // При удалении игры удаляются ключи
        }
    }
}
