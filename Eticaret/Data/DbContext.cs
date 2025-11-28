using Microsoft.EntityFrameworkCore;
using Eticaret.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SessionEntry> Sessions { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; } = null!;
    public DbSet<Urun> Urunler { get; set; } = null!;
    public DbSet<Sepet> Sepettekiler { get; set; } = null!;
    public DbSet<Adres> Adresler { get; set; } = null!;
    public DbSet<Siparis> Siparisler { get; set; } = null!;
    public DbSet<TeslimatSecenegi> TeslimatSecenekleri { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Kullanıcı silinse bile Siparişleri silme
        modelBuilder.Entity<Siparis>()
            .HasOne(s => s.Kullanici)
            .WithMany()
            .HasForeignKey(s => s.KullaniciId)
            .OnDelete(DeleteBehavior.Restrict); // CASCADE kaldırıldı

        // Adres silinse bile Siparişleri silme
        modelBuilder.Entity<Siparis>()
            .HasOne(s => s.Adres)
            .WithMany()
            .HasForeignKey(s => s.AdresId)
            .OnDelete(DeleteBehavior.Restrict); // CASCADE kaldırıldı
    }








}
