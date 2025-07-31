using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SantralOpsAPI.Entities;
using SantralOpsAPI.Enums;

namespace SantralOpsAPI.Persistence;

public static class DataSeeder
{
  private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
  {
    using var hmac = new HMACSHA512();
    passwordSalt = hmac.Key;
    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
  }

  public static async Task SeedAsync(SantralOpsDbContext context)
  {
    if (context.Birimler.Any()) return;

    // 1. Birimler
    var birimler = new List<Birim>
        {
            new() { Ad = "Bilgi İşlem Dairesi Başkanlığı" },
            new() { Ad = "İnsan Kaynakları ve Eğitim Dairesi Başkanlığı" },
            new() { Ad = "Fen İşleri Dairesi Başkanlığı" },
            new() { Ad = "Kent Estetiği Dairesi Başkanlığı" },
            new() { Ad = "Emlak ve İstimlak Dairesi Başkanlığı" },
            new() { Ad = "İmar ve Şehircilik Dairesi Başkanlığı" },
            new() { Ad = "Santral" }
        };
    await context.Birimler.AddRangeAsync(birimler);
    await context.SaveChangesAsync();

    // 2. Personeller
    var personeller = new List<Personel>();
    string defaultPassword = "123456";

    var personelData = new[] {
            new { AdSoyad = "Ayşe Tekin", Unvan = "Daire Başkanı", Dahili = "1000", Eposta = "ayse.tekin@belediye.gov.tr", BirimId = 0, Rol = "Admin" },
            new { AdSoyad = "Murat Sönmez", Unvan = "Yazılım Geliştirme Şube Müdürü", Dahili = "1001", Eposta = "murat.sonmez@belediye.gov.tr", BirimId = 0, Rol = "DepartmentUser" },
            new { AdSoyad = "Elif Çalışkan", Unvan = "Kıdemli Yazılım Geliştirici", Dahili = "1010", Eposta = "elif.caliskan@belediye.gov.tr", BirimId = 0, Rol = "DepartmentUser" },
            new { AdSoyad = "Caner Işık", Unvan = "Yazılım Geliştirici", Dahili = "1011", Eposta = "caner.isik@belediye.gov.tr", BirimId = 0, Rol = "DepartmentUser" },
            new { AdSoyad = "Fatma Güneş", Unvan = "Sistem ve Ağ Şube Müdürü", Dahili = "1002", Eposta = "fatma.gunes@belediye.gov.tr", BirimId = 0, Rol = "DepartmentUser" },
            new { AdSoyad = "Hakan Kurt", Unvan = "Sistem Uzmanı", Dahili = "1020", Eposta = "hakan.kurt@belediye.gov.tr", BirimId = 0, Rol = "DepartmentUser" },
            new { AdSoyad = "Kadir İnanır", Unvan = "Daire Başkanı", Dahili = "2000", Eposta = "kadir.inanir@belediye.gov.tr", BirimId = 1, Rol = "Admin" },
            new { AdSoyad = "Türkan Şoray", Unvan = "Şube Müdürü", Dahili = "2001", Eposta = "turkan.soray@belediye.gov.tr", BirimId = 1, Rol = "DepartmentUser" },
            new { AdSoyad = "Sadri Alışık", Unvan = "İK Uzmanı", Dahili = "2010", Eposta = "sadri.alisik@belediye.gov.tr", BirimId = 1, Rol = "DepartmentUser" },
            new { AdSoyad = "Adile Naşit", Unvan = "Eğitim Sorumlusu", Dahili = "2011", Eposta = "adile.nasit@belediye.gov.tr", BirimId = 1, Rol = "DepartmentUser" },
            new { AdSoyad = "Münir Özkul", Unvan = "Memur", Dahili = "2012", Eposta = "munir.ozkul@belediye.gov.tr", BirimId = 1, Rol = "DepartmentUser" },
            new { AdSoyad = "Cüneyt Arkın", Unvan = "Daire Başkanı", Dahili = "3000", Eposta = "cuneyt.arkin@belediye.gov.tr", BirimId = 2, Rol = "Admin" },
            new { AdSoyad = "Tarık Akan", Unvan = "Yol Yapım Şube Müdürü", Dahili = "3001", Eposta = "tarik.akan@belediye.gov.tr", BirimId = 2, Rol = "DepartmentUser" },
            new { AdSoyad = "Fatma Girik", Unvan = "İnşaat Mühendisi", Dahili = "3010", Eposta = "fatma.girik@belediye.gov.tr", BirimId = 2, Rol = "DepartmentUser" },
            new { AdSoyad = "Kadir Savun", Unvan = "Tekniker", Dahili = "3011", Eposta = "kadir.savun@belediye.gov.tr", BirimId = 2, Rol = "DepartmentUser" },
            new { AdSoyad = "Hulusi Kentmen", Unvan = "Şef", Dahili = "3005", Eposta = "hulusi.kentmen@belediye.gov.tr", BirimId = 2, Rol = "DepartmentUser" },
            new { AdSoyad = "Erol Taş", Unvan = "Saha Sorumlusu", Dahili = "3012", Eposta = "erol.tas@belediye.gov.tr", BirimId = 2, Rol = "DepartmentUser" },
            new { AdSoyad = "Emel Sayın", Unvan = "Daire Başkanı", Dahili = "4000", Eposta = "emel.sayin@belediye.gov.tr", BirimId = 3, Rol = "Admin" },
            new { AdSoyad = "Müjde Ar", Unvan = "Peyzaj Mimarı", Dahili = "4010", Eposta = "mujde.ar@belediye.gov.tr", BirimId = 3, Rol = "DepartmentUser" },
            new { AdSoyad = "Şener Şen", Unvan = "Şehir Plancısı", Dahili = "4011", Eposta = "sener.sen@belediye.gov.tr", BirimId = 3, Rol = "DepartmentUser" },
            new { AdSoyad = "İlyas Salman", Unvan = "Grafik Tasarımcı", Dahili = "4012", Eposta = "ilyas.salman@belediye.gov.tr", BirimId = 3, Rol = "DepartmentUser" },
            new { AdSoyad = "Perran Kutman", Unvan = "Sanat Yönetmeni", Dahili = "4001", Eposta = "perran.kutman@belediye.gov.tr", BirimId = 3, Rol = "DepartmentUser" },
            new { AdSoyad = "Metin Akpınar", Unvan = "Daire Başkanı", Dahili = "5000", Eposta = "metin.akpinar@belediye.gov.tr", BirimId = 4, Rol = "Admin" },
            new { AdSoyad = "Zeki Alasya", Unvan = "Şube Müdürü", Dahili = "5001", Eposta = "zeki.alasya@belediye.gov.tr", BirimId = 4, Rol = "DepartmentUser" },
            new { AdSoyad = "Halit Akçatepe", Unvan = "Emlak Uzmanı", Dahili = "5010", Eposta = "halit.akcatepe@belediye.gov.tr", BirimId = 4, Rol = "DepartmentUser" },
            new { AdSoyad = "Ayşen Gruda", Unvan = "Değerleme Uzmanı", Dahili = "5011", Eposta = "aysen.gruda@belediye.gov.tr", BirimId = 4, Rol = "DepartmentUser" },
            new { AdSoyad = "Kemal Sunal", Unvan = "Memur", Dahili = "5012", Eposta = "kemal.sunal.2@belediye.gov.tr", BirimId = 4, Rol = "DepartmentUser" },
            new { AdSoyad = "Gülşen Bubikoğlu", Unvan = "Daire Başkanı", Dahili = "6000", Eposta = "gulsen.bubikoglu@belediye.gov.tr", BirimId = 5, Rol = "Admin" },
            new { AdSoyad = "Hale Soygazi", Unvan = "Şube Müdürü", Dahili = "6001", Eposta = "hale.soygazi@belediye.gov.tr", BirimId = 5, Rol = "DepartmentUser" },
            new { AdSoyad = "Itır Esen", Unvan = "Mimar", Dahili = "6010", Eposta = "itir.esen@belediye.gov.tr", BirimId = 5, Rol = "DepartmentUser" },
            new { AdSoyad = "Ediz Hun", Unvan = "Şehir Plancısı", Dahili = "6011", Eposta = "ediz.hun@belediye.gov.tr", BirimId = 5, Rol = "DepartmentUser" },
            new { AdSoyad = "Filiz Akın", Unvan = "Harita Mühendisi", Dahili = "6012", Eposta = "filiz.akin.2@belediye.gov.tr", BirimId = 5, Rol = "DepartmentUser" },
            new { AdSoyad = "Kartal Tibet", Unvan = "Proje Sorumlusu", Dahili = "6013", Eposta = "kartal.tibet@belediye.gov.tr", BirimId = 5, Rol = "DepartmentUser" },
            new { AdSoyad = "Güdük Necmi", Unvan = "Santral Operatörü", Dahili = "0001", Eposta = "guduk.necmi@belediye.gov.tr", BirimId = 6, Rol = "Operator" },
            new { AdSoyad = "Damat Ferit", Unvan = "Santral Operatörü", Dahili = "0002", Eposta = "damat.ferit@belediye.gov.tr", BirimId = 6, Rol = "Operator" },
            new { AdSoyad = "İnek Şaban", Unvan = "Santral Operatörü", Dahili = "0003", Eposta = "inek.saban@belediye.gov.tr", BirimId = 6, Rol = "Operator" }
        };

    foreach (var pData in personelData)
    {
      CreatePasswordHash(defaultPassword, out byte[] hash, out byte[] salt);
      personeller.Add(new Personel
      {
        AdSoyad = pData.AdSoyad,
        Unvan = pData.Unvan,
        Dahili = pData.Dahili,
        Eposta = pData.Eposta,
        BirimId = birimler[pData.BirimId].Id,
        Rol = pData.Rol,
        PasswordHash = hash,
        PasswordSalt = salt
      });
    }
    await context.Personeller.AddRangeAsync(personeller);
    await context.SaveChangesAsync();

    // 3. Kişiler (Vatandaş) ve Diğer Veriler
    var ilkKisi = new Kisi { AdSoyad = "Ali Veli", Notlar = "Daha önce su kesintisi için aramıştı." };
    ilkKisi.TelefonNumaralari.Add(new TelefonNumarasi { Numara = "+905551112233" });
    await context.Kisiler.AddAsync(ilkKisi);
    await context.SaveChangesAsync();

    var ilkPersonel = await context.Personeller.FirstAsync();
    var ilkTelefonNumarasi = await context.TelefonNumaralari.FirstAsync();

    var tickets = new List<Ticket>
        {
            new() { Konu = "Su Faturası Hakkında Bilgi", Aciklama = "Son gelen su faturam çok yüksek.", TalebiYapanKisiId = ilkKisi.Id, OlusturanPersonelId = ilkPersonel.Id, AtananPersonelId = ilkPersonel.Id },
            new() { Konu = "Parktaki Kırık Oyuncaklar", Aciklama = "Mahalle parkındaki salıncak kırık.", TalebiYapanKisiId = ilkKisi.Id, OlusturanPersonelId = ilkPersonel.Id }
        };
    await context.Tickets.AddRangeAsync(tickets);

    var aramaKayitlari = new List<TelefonAramaKaydi>
        {
            new() { Yonu = AramaYonu.Gelen, Notlar = "Fatura hakkında bilgi aldı.", TelefonNumarasiId = ilkTelefonNumarasi.Id, PersonelId = ilkPersonel.Id },
            new() { Yonu = AramaYonu.Giden, Notlar = "Geri arama yapıldı.", TelefonNumarasiId = ilkTelefonNumarasi.Id, PersonelId = ilkPersonel.Id, AramaZamani = DateTime.UtcNow.AddHours(-1) }
        };
    await context.TelefonAramaKayitlari.AddRangeAsync(aramaKayitlari);

    await context.SaveChangesAsync();
  }
}
