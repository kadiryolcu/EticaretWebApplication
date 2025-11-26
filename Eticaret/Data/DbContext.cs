using Microsoft.EntityFrameworkCore;
using Eticaret.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SessionEntry> Sessions { get; set; }
          public DbSet<Kullanici> Kullanicilar { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionEntry>(entity =>
        {
            entity.ToTable("Sessions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Value).IsRequired();
            entity.Property(e => e.ExpiresAtTime).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
