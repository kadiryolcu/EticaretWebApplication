
using System;
using System.ComponentModel.DataAnnotations;
namespace Eticaret.Models
{
  public class Urun
    {
        public int Id { get; set; }                 // Ürün ID
        public string Name { get; set; }            // Ürün Adı
        public string Description { get; set; }     // Ürün Açıklaması
        public decimal Price { get; set; }          // Güncel Fiyat (TL)
        public decimal OldPrice { get; set; }       // Eski Fiyat (TL)
        public string ImageUrl { get; set; }        // Resim Yolu
    }
}
