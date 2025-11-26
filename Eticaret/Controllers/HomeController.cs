using System.Diagnostics;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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


    public IActionResult Index()
    {
        // Eğer kullanıcı zaten giriş yapmışsa ana sayfaya yönlendir
        if (HttpContext.Session.GetString("KullaniciAdi") != null)
            return View("Anasayfa");

        return RedirectToAction("OturumAc");
    }

    [HttpGet]
    public IActionResult OturumAc()
    {
        return View();
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
