
using System;
using System.ComponentModel.DataAnnotations;
namespace Eticaret.Models
{
    public class Kullanici
    {
        public virtual ICollection<Adres> Adresler { get; set; } = new List<Adres>();

        public int Id { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Sifre { get; set; } = null!;
        public bool EmailOnayli { get; set; } = false;
        public string? EmailDogrulamaKodu { get; set; }


    }
}
