
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Eticaret.Models;

namespace Eticaret.Models
{
public class Sepet
    {
        [Key]
        public int Id { get; set; }

        // Sepetteki ürün
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Urun Urunler { get; set; } // Ürün detaylarını bağlamak için

        // Kullanıcı
        [Required]
        public string UserId { get; set; } // Identity kullanıyorsanız string

        // Sepetteki ürün adedi
        [Required]
        public int Quantity { get; set; } = 1;

        // Ürünün sepetteki toplam fiyatı (Quantity * Product.Price)
        [NotMapped]
        public decimal TotalPrice => Quantity * Urunler.Price;
    }
}
