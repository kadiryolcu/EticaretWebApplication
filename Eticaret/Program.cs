using Microsoft.EntityFrameworkCore;
using Eticaret.Models;
using Eticaret.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<MailService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// SQL Server cache servisi
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "Sessions";
});



// Session ayarları
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyECommerce.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 dakika
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware buraya
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
