using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YaroshenkoShop.Models;

namespace YaroshenkoShop.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
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
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Developer>().ToTable("Developer", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Game>().ToTable("Games", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Key>().ToTable("Keys", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<GameGenre>().ToTable("Games_Genre", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Favorite>().ToTable("Favorites", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<BuyHistory>().ToTable("BuyHistory", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<User>().ToTable("AspNetUsers");

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

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Developer)
                .WithMany(d => d.Games)
                .HasForeignKey(g => g.id_разработчика)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Key>()
                .HasOne(k => k.Game)
                .WithMany(g => g.Keys)
                .HasForeignKey(k => k.id_игры)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
