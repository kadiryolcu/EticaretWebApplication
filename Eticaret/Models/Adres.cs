using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eticaret.Models
{
    public class Adres
    {
        public int Id { get; set; }

        [Display(Name = "Adres Türü")]
        public string AdresTuru { get; set; }  // Ev / İş

        [Display(Name = "Kullanıcı Id")]
        public int KullaniciId { get; set; }   // Kullanıcı ile ilişki

        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Display(Name = "İl")]
        public string Il { get; set; }

        [Display(Name = "İlçe")]
        public string Ilce { get; set; }

        [Display(Name = "Mahalle / Semt")]
        public string Mahalle { get; set; }   // Tabloda AdresDetay olabilir, ya da ayrı

        [Display(Name = "Adres Detayı")]
        public string AdresDetay { get; set; } // Cadde, Sokak vs.

        [Display(Name = "Posta Kodu")]
        public string PostaKodu { get; set; }

        [Display(Name = "Fatura Adresi Mi?")]
        public bool FaturaAdresiMi { get; set; }
        
    // Navigation property
    public virtual Kullanici Kullanici { get; set; }

    }
}