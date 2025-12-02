using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eticaret.Models
{
    public class Siparis
    {
        [Key]
        public int Id { get; set; }

        // Siparişi veren kullanıcı
        [Required]
        public int KullaniciId { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; } = null!;

        // Siparişin teslim edileceği adres
        [Required]
        public int? AdresId { get; set; }

        [ForeignKey("AdresId")]
        public virtual Adres Adres { get; set; } = null!;

        // Sipariş Tarihi
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;

        // Toplam tutar (Sepet üzerinden hesaplanacak)
        public decimal ToplamTutar { get; set; }

        // Sipariş Durumu
        [Required]
        public string Durum { get; set; } = "Hazırlanıyor";

         [Required]
        public int TeslimatSecenegiId { get; set; }

        [ForeignKey("TeslimatSecenegiId")]
        public virtual TeslimatSecenegi TeslimatSecenegi { get; set; } = null!;
    }
}
