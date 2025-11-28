using System.Diagnostics;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Eticaret.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; // DbContext

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context; // DI ile gelen context
    }


    [HttpGet]
    public async Task<IActionResult> Index(int? addProductId, int quantity = 1)
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
            return RedirectToAction("OturumAc");

        var userId = HttpContext.Session.GetString("KullaniciId");

        // Sepete ekleme
        if (addProductId.HasValue && !string.IsNullOrEmpty(userId))
        {
            var product = _context.Urunler.Find(addProductId.Value);
            if (product != null)
            {
                var cartItem = _context.Sepettekiler
                    .FirstOrDefault(c => c.ProductId == addProductId.Value && c.UserId == userId);

                if (cartItem != null)
                    cartItem.Quantity += quantity;
                else
                    _context.Sepettekiler.Add(new Sepet
                    {
                        ProductId = addProductId.Value,
                        UserId = userId,
                        Quantity = quantity
                    });

                _context.SaveChanges();
            }
        }

        var products = _context.Urunler.ToList();

        int sepetSayisi = 0;
        var sepetList = new List<Sepet>();
        if (!string.IsNullOrEmpty(userId))
        {
            sepetList = _context.Sepettekiler
                .Include(s => s.Urunler)
                .Where(s => s.UserId == userId)
                .ToList();

            sepetSayisi = sepetList.Sum(s => s.Quantity);
        }

        var Teslimat = _context.TeslimatSecenekleri.ToList();


        var model = new HomeViewModel
        {
            Urunler = products,
            SepetList = sepetList,
            SepetSayisi = sepetSayisi,
            TeslimatSecenekleri = Teslimat
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult OturumAc()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Sepettekiler(int? addProductId, int quantity = 1)
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
            return RedirectToAction("OturumAc");

        var userId = HttpContext.Session.GetString("KullaniciId");

        // Sepete ekleme
        if (addProductId.HasValue && !string.IsNullOrEmpty(userId))
        {
            var product = _context.Urunler.Find(addProductId.Value);
            if (product != null)
            {
                var cartItem = _context.Sepettekiler
                    .FirstOrDefault(c => c.ProductId == addProductId.Value && c.UserId == userId);

                if (cartItem != null)
                    cartItem.Quantity += quantity;
                else
                    _context.Sepettekiler.Add(new Sepet
                    {
                        ProductId = addProductId.Value,
                        UserId = userId,
                        Quantity = quantity
                    });

                _context.SaveChanges();
            }
        }

        var products = _context.Urunler.ToList();

        int sepetSayisi = 0;
        var sepetList = new List<Sepet>();
        if (!string.IsNullOrEmpty(userId))
        {
            sepetList = _context.Sepettekiler
                .Include(s => s.Urunler)
                .Where(s => s.UserId == userId)
                .ToList();

            sepetSayisi = sepetList.Sum(s => s.Quantity);
        }

        var kullanici = _context.Kullanicilar.ToList();
        // Kullanıcıların tüm adreslerini listele, her adresle birlikte KullaniciId'yi al
        var adresler = _context.Adresler
     .Include(a => a.Kullanici)  // ✅ Navigation property
     .ToList();

        var Teslimat = _context.TeslimatSecenekleri.ToList();

        var model = new HomeViewModel
        {
            Urunler = products,
            SepetList = sepetList,
            SepetSayisi = sepetSayisi,
            KullaniciList = kullanici,
            AdreslerList = adresler,
            TeslimatSecenekleri = Teslimat
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AdresEkle(HomeViewModel model)
    {



        // Kullanıcı Id session'dan alınır
        var kullaniciIdStr = HttpContext.Session.GetString("KullaniciId");

        if (string.IsNullOrEmpty(kullaniciIdStr) || !int.TryParse(kullaniciIdStr, out int kullaniciId))
        {
            ModelState.AddModelError("", "Kullanıcı giriş yapmamış.");
            return View(model);
        }

        try
        {
            model.Adres.KullaniciId = kullaniciId;

            _context.Adresler.Add(model.Adres);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Adres başarıyla eklendi!";
            return RedirectToAction("Sepettekiler");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Adres eklenirken bir hata oluştu.");
            Console.WriteLine(ex.Message);
            return View("Sepettekiler");
        }
    }
    [HttpGet]
    public async Task<IActionResult> AdresSil(int id)
    {

        var adres = await _context.Adresler.FindAsync(id);

        if (adres == null)
        {
            TempData["ErrorMessage"] = "Adres bulunamadı.";
            return RedirectToAction("Index");
        }

        _context.Adresler.Remove(adres);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Adres silindi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> SecilenTeslimat(int id)
    {

        var kullaniciIdStr = HttpContext.Session.GetString("KullaniciId");

        // Güvenli int dönüşümü
        if (!int.TryParse(kullaniciIdStr, out int kullaniciId))
        {
            // Hatalı veya login olmamışsa yönlendir
            return RedirectToAction("OturumAc", "Home");
        }


        var sepetList = _context.Sepettekiler
                          .Where(s => s.UserId == kullaniciIdStr)
                          .Include(s => s.Urunler)
                          .ToList();

        var adresList = _context.Adresler
                           .Where(a => a.KullaniciId == kullaniciId)
                           .ToList();

        // İlk adresi al
        var firstAdres = adresList.FirstOrDefault();

        if (!sepetList.Any())
            return RedirectToAction("Sepettekiler"); // Sepet boşsa geri dön

        // 3. Toplam tutarı hesapla
        decimal toplamTutar = sepetList.Sum(x => x.TotalPrice);

        // 4. Sipariş oluştur
        var siparis = new Siparis
        {
            KullaniciId = kullaniciId,
            AdresId = firstAdres.Id, // Örnek: ilk adresi kullan
            TeslimatSecenegiId = id,
            ToplamTutar = toplamTutar,
            SiparisTarihi = DateTime.Now,
            Durum = "Hazırlanıyor"
        };

        _context.Siparisler.Add(siparis);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetInt32("SecilenTeslimatId", id);

    

        return RedirectToAction("Sepettekiler"); // Örn: Siparişler sayfasına yönlendir
    }



    public IActionResult SepetGuncelle(int id, int arttir = 0, int azalt = 0, int sil = 0)
    {
        var sepetItem = _context.Sepettekiler.FirstOrDefault(s => s.Urunler.Id == id);
        if (sepetItem != null)
        {
            if (sil == 1)
            {
                _context.Sepettekiler.Remove(sepetItem);
            }
            else if (arttir == 1)
            {
                sepetItem.Quantity += 1;
            }
            else if (arttir == 0 && sepetItem.Quantity > 1)
            {
                sepetItem.Quantity -= 1;
            }
            _context.SaveChanges();
        }

        return RedirectToAction("Sepettekiler");
    }


    [HttpPost]
    public async Task<IActionResult> OturumAc(Kullanici model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Sifre))
        {
            ViewBag.Hata = "Lütfen tüm alanları doldurun.";
            return View();
        }

        // Kullanıcıyı veritabanından sorgula
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == model.Email);

        if (kullanici == null || kullanici.Sifre != model.Sifre)
        {
            ViewBag.Hata = "Kullanıcı adı/e-posta veya şifre hatalı.";
            return View();
        }

        // Session oluştur
        HttpContext.Session.SetString("KullaniciId", kullanici.Id.ToString());
        HttpContext.Session.SetString("KullaniciAdi", kullanici.Email);

        // Ana sayfaya yönlendir
        return RedirectToAction("Index", "Home");
    }




    public IActionResult Cikis()
    {
        HttpContext.Session.Clear(); // Session temizle
        return RedirectToAction("OturumAc");
    }
    [HttpGet]
    public IActionResult KayitOl()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> KayitOl(Kullanici model)
    {
        // Alanların boş olup olmadığını kontrol et
        if (string.IsNullOrEmpty(model.KullaniciAdi) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Sifre))
        {
            ViewBag.Hata = "Lütfen tüm alanları doldurun.";
            return View();
        }

        // Kullanıcı zaten kayıtlı mı kontrol et
        var mevcutKullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == model.Email);

        if (mevcutKullanici != null)
        {
            ViewBag.Hata = "Bu e-posta zaten kayıtlı.";
            return View();
        }

        // Kullanıcıyı veritabanına ekle
        _context.Kullanicilar.Add(model);
        await _context.SaveChangesAsync();

        ViewBag.Basarili = "Kayıt başarılı! Giriş yapabilirsiniz.";

        return RedirectToAction("OturumAc");
    }

    public IActionResult Anasayfa()
    {
        // Eğer session yoksa giriş sayfasına yönlendir
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
            return RedirectToAction("OturumAc");

        ViewBag.Kullanici = HttpContext.Session.GetString("KullaniciAdi");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
