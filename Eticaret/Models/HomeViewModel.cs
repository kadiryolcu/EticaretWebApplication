
using System;
using System.ComponentModel.DataAnnotations;
namespace Eticaret.Models
{
    public class HomeViewModel
    {
        public List<Urun> Urunler { get; set; }
        public List<Sepet> SepetList { get; set; }
        public List<Kullanici> KullaniciList { get; set; }
        public List<Adres> AdreslerList { get; set; }
        public List<TeslimatSecenegi> TeslimatSecenekleri { get; set; }
        public int SepetSayisi { get; set; }
        public Adres Adres { get; set; }
    }
}
