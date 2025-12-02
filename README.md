# 🛒 Eticaret Web Application

Modern, güvenli ve kullanıcı dostu bir alışveriş deneyimi sunmak için geliştirilmiş **ASP.NET Core MVC** tabanlı e-ticaret uygulaması.

---

## 📘 Genel Bakış

Bu proje, kullanıcı kayıt/giriş yönetimi, ürün listeleme, sepet işlemleri ve sipariş oluşturma süreçlerini kapsayan tam fonksiyonel bir e‑ticaret platformudur. Hem backend hem frontend bileşenleri modern teknolojiler kullanılarak geliştirilmiştir.

---

## 🚀 Kullanılan Teknolojiler

* **ASP.NET Core MVC 7.0**
* **C#**
* **MSSQL Server**
* **Entity Framework Core**
* **HTML5 / CSS3 / JavaScript**
* **Bootstrap 5**
* **Toastr Notification**
* **Quill.js Editör**
* **Material Design Icons**

---

## 🎯 Uygulama Özellikleri

### 👤 Kullanıcı İşlemleri

* Üyelik oluşturma
* E‑posta ile iki adımlı doğrulama
* Güvenli kullanıcı girişi
* Şifre sıfırlama / yenileme işlemleri

### 🛍️ Ürün & Mağaza

* Ürün listeleme
* Kategoriye göre filtreleme
* Ürün detay sayfaları

### 🛒 Sepet Yönetimi

* Ürün ekleme/çıkarma
* Gerçek zamanlı sepet güncellemeleri

### 📦 Sipariş Süreci

* Adres seçimi
* Teslimat seçeneği belirleme
* Ödeme adımı
* Sipariş özeti
* Siparişin başarılı şekilde oluşturulması

### 🗄️ Veritabanı Yapısı

* **Kullanicilar**: Kullanıcı hesap bilgileri
* **Sepettekiler**: Kullanıcının sepetindeki ürünler
* **Siparisler**: Sipariş kayıtları
* **TeslimatSecenekleri**: Teslimat yöntemleri

> ⚠️ **Not:** Güvenlik nedeniyle `appsettings.json` dosyasında bulunan e-posta ayarları repository'e yüklenmemiştir.

---

## 📷 Arayüz Görselleri

### 📱 Responsive Tasarım

<img width="800" src="https://github.com/user-attachments/assets/014270d5-6a3c-44ff-bb17-5e02d337705f" />

### 👤 Kullanıcı Üyelik Oluşturma

<img width="800" src="https://github.com/user-attachments/assets/cb501ede-e436-4b41-a49b-a06b47f14402" />

### 📩 Kullanıcı Üyelik Doğrulama

<img width="800" src="https://github.com/user-attachments/assets/3da012bc-1997-4373-8b72-f0661237e7b0" />

### 🔐 Kullanıcı Giriş Sayfası

<img width="800" src="https://github.com/user-attachments/assets/3a8ee0ea-0cfe-4577-be35-e753c365db47" />

### 🔑 İki Adımlı Doğrulama

<img width="800" src="https://github.com/user-attachments/assets/92086f38-5637-43d0-ac78-5e4834d6f5b6" />

### 📬 E‑posta Gönderimi

<img width="800" src="https://github.com/user-attachments/assets/90a9a266-9559-4ef4-b9fe-cb0a68abc7b8" />

### 🏬 Mağaza Sayfası

<img width="800" src="https://github.com/user-attachments/assets/f2cea707-e88a-4261-b371-1eb97a35937d" />

### 🛒 Sepet Sayfası

<img width="800" src="https://github.com/user-attachments/assets/6d6ca4cf-1382-43ff-a4f5-388ee894165c" />

### 📍 Adres Sayfası

<img width="800" src="https://github.com/user-attachments/assets/f420f343-f70e-4240-9961-6d816502f753" />

### 💳 Ödeme Sayfası

<img width="800" src="https://github.com/user-attachments/assets/3905f931-baa0-4671-bd78-819322b6d23a" />

### ✔️ Sipariş Onay Sayfası

<img width="800" src="https://github.com/user-attachments/assets/fcddd7f9-5292-4a22-8f42-85987bd59efe" />

### 🗄️ MSSQL Veritabanı Görüntüsü

<img width="800" src="https://github.com/user-attachments/assets/43e186b7-c19e-4145-906e-52a4762e734d" />

---

## 🔌 API Kullanımı

### 📦 Sipariş Getirme API

**Endpoint:**

```
GET /api/Siparislerim/{id}
```

**Açıklama:** Belirtilen kullanıcı ID’sine ait tüm sipariş kayıtlarını döndürür.

**URL Örnek:**

```
http://localhost:5261/api/Siparislerim/1
```

### 📌 Parametreler

| Parametre | Tip | Açıklama               |
| --------- | --- | ---------------------- |
| id        | int | Kullanıcının ID değeri |

### 📄 Örnek Çıktı

<img width="800" src="https://github.com/user-attachments/assets/670baa94-8a0a-41d8-a764-a17de08442b4" />
<img width="800" src="https://github.com/user-attachments/assets/c3e0a18a-b0f3-4bd0-a5b8-57402756f820" />

---

## 📧 E-posta Doğrulama Tasarımı (Kullanılan Şablon)

Aşağıdaki HTML yapı, kullanıcıya gönderilen doğrulama kodu e‑postasında kullanılmaktadır:

```html
<div style='font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 30px;'>
    <div style='max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); padding: 30px; text-align: center;'>
        <h2 style='color: #333333;'>E-posta Doğrulama Kodunuz</h2>
        <p style='font-size: 16px; color: #555555;'>Aşağıdaki kodu kullanarak e-posta adresinizi doğrulayabilirsiniz:</p>
        <div style='margin: 20px 0; padding: 20px; background-color: #4CAF50; color: #ffffff; font-size: 24px; font-weight: bold; border-radius: 8px; display: inline-block;'>
            {dogrulamaKodu}
        </div>
        <p style='font-size: 14px; color: #888888;'>Kodunuz 10 dakika içinde geçerlidir.</p>
    </div>
</div>
```

---

## 📌 Sonuç

Bu proje, modern bir e-ticaret platformu oluşturmak isteyenler için güçlü bir başlangıç noktası sunar. Hem frontend hem backend mimarisi profesyonel şekilde yapılandırılmıştır.


