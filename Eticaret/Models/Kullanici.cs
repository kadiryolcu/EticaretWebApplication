
using System;
using System.ComponentModel.DataAnnotations;
namespace Eticaret.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Sifre { get; set; } = null!;

    }
}
