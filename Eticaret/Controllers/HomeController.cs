using System.Diagnostics;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eticaret.Services;

namespace Eticaret.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; // DbContext
    private readonly MailService _mailService; // DI ile MailService

    public HomeController(ILogger<HomeController> logger, AppDbContext context, MailService mailService)
    {
        _logger = logger;
        _context = context;
        _mailService = mailService;
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
            var product = await _context.Urunler.FindAsync(addProductId.Value);
            if (product != null)
            {
                var cartItem = await _context.Sepettekiler
                    .FirstOrDefaultAsync(c => c.ProductId == addProductId.Value && c.UserId == userId);

                if (cartItem != null)
                    cartItem.Quantity += quantity;
                else
                    _context.Sepettekiler.Add(new Sepet
                    {
                        ProductId = addProductId.Value,
                        UserId = userId,
                        Quantity = quantity
                    });

                await _context.SaveChangesAsync();
            }
        }

        var products = await _context.Urunler.ToListAsync();

        int sepetSayisi = 0;
        var sepetList = new List<Sepet>();
        if (!string.IsNullOrEmpty(userId))
        {
            sepetList = await _context.Sepettekiler
                .Include(s => s.Urunler)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            sepetSayisi = sepetList.Sum(s => s.Quantity);
        }

        var teslimatSecenekleri = await _context.TeslimatSecenekleri.ToListAsync();

        var model = new HomeViewModel
        {
            Urunler = products,
            SepetList = sepetList,
            SepetSayisi = sepetSayisi,
            TeslimatSecenekleri = teslimatSecenekleri
        };

        return View(model);
    }



    [HttpGet]
    public async Task<IActionResult> Sepettekiler(int? addProductId, int quantity = 1)
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
            return RedirectToAction("OturumAc");

        var userId = HttpContext.Session.GetString("KullaniciId");

        // Sepete ürün ekleme
        if (addProductId.HasValue && !string.IsNullOrEmpty(userId))
        {
            var product = await _context.Urunler.FindAsync(addProductId.Value);
            if (product != null)
            {
                var cartItem = await _context.Sepettekiler
                    .FirstOrDefaultAsync(c => c.ProductId == addProductId.Value && c.UserId == userId);

                if (cartItem != null)
                    cartItem.Quantity += quantity;
                else
                    _context.Sepettekiler.Add(new Sepet
                    {
                        ProductId = addProductId.Value,
                        UserId = userId,
                        Quantity = quantity
                    });

                await _context.SaveChangesAsync();
            }
        }

        // Kullanıcının sepeti
        var sepetList = await _context.Sepettekiler
            .Include(s => s.Urunler)
            .Where(s => s.UserId == userId)
            .ToListAsync();

        int sepetSayisi = sepetList.Sum(s => s.Quantity);

        var kullaniciList = await _context.Kullanicilar.ToListAsync();
        var adreslerList = await _context.Adresler.Include(a => a.Kullanici).ToListAsync();
        var teslimatList = await _context.TeslimatSecenekleri.ToListAsync();

        // Mevcut sipariş
        var mevcutSiparis = await _context.Siparisler
            .FirstOrDefaultAsync(s => s.KullaniciId.ToString() == userId && s.Durum == "Hazırlanıyor");

        if (mevcutSiparis == null)
        {
            var secilenAdres = await _context.Adresler
                .FirstOrDefaultAsync(a => a.KullaniciId.ToString() == userId);

            if (secilenAdres == null)
            {
                var dummyAdres = new Adres
                {
                    KullaniciId = int.Parse(userId),
                    AdresTuru = "Ev",
                    Ad = "Test",
                    Soyad = "Kullanıcı",
                    Ulke = "Türkiye",
                    Il = "İstanbul",
                    Ilce = "Kadıköy",
                    Mahalle = "Moda",
                    AdresDetay = "Sokak, No:0",
                    PostaKodu = "00000"
                };
                _context.Adresler.Add(dummyAdres);
                await _context.SaveChangesAsync();
                secilenAdres = dummyAdres;
            }

            mevcutSiparis = new Siparis
            {
                KullaniciId = int.Parse(userId),
                AdresId = secilenAdres.Id,
                SiparisTarihi = DateTime.Now,
                Durum = "Hazırlanıyor",
                TeslimatSecenegiId = 1,
                ToplamTutar = sepetList.Sum(x => x.TotalPrice)
            };

            _context.Siparisler.Add(mevcutSiparis);
            await _context.SaveChangesAsync();
        }
        else
        {
            mevcutSiparis.ToplamTutar = sepetList.Sum(x => x.TotalPrice);
            await _context.SaveChangesAsync();
        }

        var siparisler = await _context.Siparisler
            .Where(s => s.KullaniciId.ToString() == userId)
            .ToListAsync();

        var model = new HomeViewModel
        {
            Urunler = await _context.Urunler.ToListAsync(),
            SepetList = sepetList,
            SepetSayisi = sepetSayisi,
            KullaniciList = kullaniciList,
            AdreslerList = adreslerList,
            TeslimatSecenekleri = teslimatList,
            SiparisList = siparisler
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AdresEkle(HomeViewModel model)
    {
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
        catch
        {
            ModelState.AddModelError("", "Adres eklenirken bir hata oluştu.");
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

        if (!int.TryParse(kullaniciIdStr, out int kullaniciId))
            return RedirectToAction("OturumAc");

        var sepetList = await _context.Sepettekiler
            .Where(s => s.UserId == kullaniciIdStr)
            .Include(s => s.Urunler)
            .ToListAsync();

        var adresList = await _context.Adresler
            .Where(a => a.KullaniciId == kullaniciId)
            .ToListAsync();

        var firstAdres = adresList.FirstOrDefault();
        if (!sepetList.Any() || firstAdres == null)
            return RedirectToAction("Sepettekiler");

        decimal toplamTutar = sepetList.Sum(x => x.TotalPrice);

        var siparis = new Siparis
        {
            KullaniciId = kullaniciId,
            AdresId = firstAdres.Id,
            TeslimatSecenegiId = id,
            ToplamTutar = toplamTutar,
            SiparisTarihi = DateTime.Now,
            Durum = "Hazırlanıyor"
        };

        _context.Siparisler.Add(siparis);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetInt32("SecilenTeslimatId", id);

        return RedirectToAction("Sepettekiler");
    }

    public IActionResult SepetGuncelle(int id, int arttir = 0, int azalt = 0, int sil = 0)
    {
        var sepetItem = _context.Sepettekiler.FirstOrDefault(s => s.Urunler.Id == id);
        if (sepetItem != null)
        {
            if (sil == 1) _context.Sepettekiler.Remove(sepetItem);
            else if (arttir == 1) sepetItem.Quantity += 1;
            else if (arttir == 0 && sepetItem.Quantity > 1) sepetItem.Quantity -= 1;

            _context.SaveChanges();
        }

        var userId = HttpContext.Session.GetString("KullaniciId");
        if (!string.IsNullOrEmpty(userId))
        {
            var mevcutSiparis = _context.Siparisler
                .FirstOrDefault(s => s.KullaniciId.ToString() == userId && s.Durum == "Hazırlanıyor");

            if (mevcutSiparis != null)
            {
                var sepetList = _context.Sepettekiler
                    .Include(s => s.Urunler)
                    .Where(s => s.UserId == userId)
                    .ToList();

                mevcutSiparis.ToplamTutar = sepetList.Sum(x => x.TotalPrice);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Sepettekiler");
    }
    [HttpGet]
    public IActionResult OturumAc() => View();
    [HttpPost]
    public async Task<IActionResult> OturumAc(Kullanici model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Sifre))
        {
            ViewBag.Hata = "Lütfen tüm alanları doldurun.";
            return View();
        }

        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == model.Email);

        if (kullanici == null || kullanici.Sifre != model.Sifre)
        {
            ViewBag.Hata = "Kullanıcı adı/e-posta veya şifre hatalı.";
            return View();
        }

        // Kullanıcı girişi doğruysa, e-posta doğrulama kodu oluştur
        var dogrulamaKodu = new Random().Next(100000, 999999);
        kullanici.EmailDogrulamaKodu = dogrulamaKodu.ToString();
        await _context.SaveChangesAsync();

        string subject = "Giriş Doğrulama Doğrulama Kodunuz";
        string message = $"<h3>Doğrulama Kodunuz:</h3><h2>{dogrulamaKodu}</h2>";

        await _mailService.SendEmailAsync(model.Email, subject, message);


        // TempData ile e-posta gönderimi için view'a aktar
        HttpContext.Session.SetString("Email", model.Email);

        // Kullanıcıyı doğrulama sayfasına yönlendir
        return View("GirisDogrula");
    }

    public IActionResult GirisDogrula()
    {
        // Kullanıcı e-posta doğrulama kodunu girecek sayfa
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GirisDogrula(string EmailDogrulamaKodu)
    {
        var email = HttpContext.Session.GetString("Email");
        if (email == null)
        {
            return RedirectToAction("OturumAc");
        }

         email = email as string;
        if (string.IsNullOrEmpty(EmailDogrulamaKodu))
        {
            ViewBag.Hata = "Lütfen doğrulama kodunu girin.";
            return View();
        }

        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == email);

        if (kullanici == null)
        {
            return RedirectToAction("OturumAc");
        }

        if (kullanici.EmailDogrulamaKodu == EmailDogrulamaKodu)
        {
            kullanici.EmailOnayli = true;
            await _context.SaveChangesAsync();

            // Giriş başarılı, session ayarla
            HttpContext.Session.SetString("KullaniciId", kullanici.Id.ToString());
            HttpContext.Session.SetString("KullaniciAdi", kullanici.Email);

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Hata = "Doğrulama kodu hatalı.";
        return View();
    }


    public IActionResult CikisYap()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("OturumAc");
    }

    [HttpGet]
    public IActionResult KayitOl() => View();



    [HttpPost]
    public async Task<IActionResult> KayitOl(Kullanici model)
    {
        if (string.IsNullOrEmpty(model.KullaniciAdi) ||
            string.IsNullOrEmpty(model.Email) ||
            string.IsNullOrEmpty(model.Sifre))
        {
            ViewBag.Hata = "Lütfen tüm alanları doldurun.";
            return View();
        }

        var mevcutKullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == model.Email);

        if (mevcutKullanici != null)
        {
            ViewBag.Hata = "Bu e-posta zaten kayıtlı.";
            return View();
        }

        string dogrulamaKodu = new Random().Next(100000, 999999).ToString();
        model.EmailOnayli = false;
        model.EmailDogrulamaKodu = dogrulamaKodu;

        _context.Kullanicilar.Add(model);
        await _context.SaveChangesAsync();

        string subject = "E-posta Doğrulama Kodunuz";
        string message = $"<h3>Doğrulama Kodunuz:</h3><h2>{dogrulamaKodu}</h2>";

        await _mailService.SendEmailAsync(model.Email, subject, message);

        TempData["YeniKayitId"] = model.Id;
        TempData["Email"] = model.Email;



        return RedirectToAction("MailDogrula");
    }



    [HttpGet]
    public IActionResult MailDogrula()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MailDogrula(int EmailDogrulamaKodu)
    {
        if (!TempData.TryGetValue("YeniKayitId", out var idObj))
            return RedirectToAction("KayitOl");

        int kullaniciId = (int)idObj;
        var kullanici = await _context.Kullanicilar.FindAsync(kullaniciId);
        if (kullanici == null) return RedirectToAction("KayitOl");

        if (kullanici.EmailDogrulamaKodu == EmailDogrulamaKodu.ToString())
        {
            kullanici.EmailOnayli = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("OturumAc");
        }


        ViewBag.Hata = "Doğrulama kodu hatalı.";
        return View();
    }

    public IActionResult Anasayfa()
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
            return RedirectToAction("OturumAc");

        ViewBag.Kullanici = HttpContext.Session.GetString("KullaniciAdi");
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
