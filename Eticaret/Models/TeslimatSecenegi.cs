using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eticaret.Models
{
    public class TeslimatSecenegi
{
    public int Id { get; set; }
    public string Ad { get; set; }          // Standart / Ekspres / Aynı Gün
    public string Icon { get; set; }        // ti ti-users, ti ti-crown, ti ti-brand-telegram
    public decimal Ucret { get; set; }      // 0, 150, 500
    public string Aciklama { get; set; }    // Teslimat süresi
    public bool Varsayilan { get; set; }    // İlk seçili seçenek
}

}