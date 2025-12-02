# Eticaret Web Application

## 📄 Genel Bilgi
Bu proje, ASP.NET Core MVC ile geliştirilmiş bir e-ticaret web uygulamasıdır. MSSQL veritabanı kullanılarak kullanıcı kayıt/giriş işlemleri ve sipariş yönetimi sağlanmaktadır.  

## 💻 Teknolojiler
- ASP.NET Core MVC
- C#
- MSSQL Server
- HTML5, CSS3, JavaScript
- Bootstrap 5
- Toastr, Quill.js ve diğer UI kütüphaneleri

## ⚙️ Özellikler
- Kullanıcı kayıt, giriş ve şifre sıfırlama
- Ürün listeleme, filtreleme ve detay sayfaları
- Sepet yönetimi ve sipariş oluşturma

## 🗄️ Veritabanı
Proje, MSSQL Server üzerinde çalışmaktadır. Önerilen yapı:

- `[Kullanicilar]` tablosu: Kullanıcı bilgileri
- `[Sepettekiler]` tablosu: Sepet bilgileri
- `[Siparisler]` tablosu: Sipariş bilgileri
- `[TeslimatSecenekleri]` tablosu: Teslimat detayları

> **Not:** `appsettings.json` dosyası yüklenmemiştir. E-posta ayarları olduğu için

