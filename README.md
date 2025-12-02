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
>### Kullanıcı Üye Ol Sayfası
<img width="800" alt="image" src="https://github.com/user-attachments/assets/cb501ede-e436-4b41-a49b-a06b47f14402" />

>### Kullanıcı Üyelsk Doğrulama
<img width="800" alt="image" src="https://github.com/user-attachments/assets/3da012bc-1997-4373-8b72-f0661237e7b0" />


> ### Kullanıcı Giriş Sayfası

<img width="800" alt="Kullanıcı Giriş Sayfası" src="https://github.com/user-attachments/assets/3a8ee0ea-0cfe-4577-be35-e753c365db47" />

> ### İki Adımlı Doğrulama

<img width="800"  alt="image" src="https://github.com/user-attachments/assets/92086f38-5637-43d0-ac78-5e4834d6f5b6" />

> ### E posta Gönderimi
<img width="800" alt="image" src="https://github.com/user-attachments/assets/90a9a266-9559-4ef4-b9fe-cb0a68abc7b8" />


> ### Mağaza sayfası

<img width="800" alt="image" src="https://github.com/user-attachments/assets/f2cea707-e88a-4261-b371-1eb97a35937d" />

> ### Sepet sayfası
> 
<img width="800" alt="image" src="https://github.com/user-attachments/assets/6d6ca4cf-1382-43ff-a4f5-388ee894165c" />

> ### Adres sayfası
> 
<img width="800" alt="image" src="https://github.com/user-attachments/assets/f420f343-f70e-4240-9961-6d816502f753" />

> ### Ödeme sayfası
<img width="800" alt="image" src="https://github.com/user-attachments/assets/3905f931-baa0-4671-bd78-819322b6d23a" />

> ### Sipariş Onay sayfası
<img width="800" alt="image" src="https://github.com/user-attachments/assets/fcddd7f9-5292-4a22-8f42-85987bd59efe" />

> ### MSSQL Veritabanı
<img width="800" alt="image" src="https://github.com/user-attachments/assets/43e186b7-c19e-4145-906e-52a4762e734d" />


> ### Api Kullanımı
> http://localhost:5261/api/Siparislerim/1
> GET /api/Siparislerim/{id}

Kullanıcıya ait tüm siparişleri getirir.

URL Parametresi
Parametre	Tip	Açıklama
id	int	Siparişleri getirilecek kullanıcının ID'si


<img width="800" alt="image" src="https://github.com/user-attachments/assets/670baa94-8a0a-41d8-a764-a17de08442b4" />

<img width="800" alt="image" src="https://github.com/user-attachments/assets/c3e0a18a-b0f3-4bd0-a5b8-57402756f820" />







