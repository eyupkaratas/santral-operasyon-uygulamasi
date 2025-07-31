using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SantralOpsAPI.DTOs;
using SantralOpsAPI.Entities;
using SantralOpsAPI.Helpers;
using SantralOpsAPI.Persistence;

namespace SantralOpsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupController(SantralOpsDbContext context) : ControllerBase
{
  private readonly SantralOpsDbContext _context = context;

  [HttpGet("{numara}")]
  [Authorize(Roles = "Admin, Operator")]
  public async Task<ActionResult<KisiDetayDto>> LookupByNumber(string numara)
  {
    var normalizedNumber = PhoneNumberHelper.Normalize(numara);
    if (string.IsNullOrEmpty(normalizedNumber))
      return BadRequest("Geçersiz telefon numarası formatı.");

    var telefon = await _context.TelefonNumaralari
        .Include(t => t.Kisi)
            .ThenInclude(k => k.TelefonNumaralari)
        .FirstOrDefaultAsync(t => t.Numara == normalizedNumber);

    if (telefon == null)
      return NotFound("Bu numaraya kayıtlı bir kişi bulunamadı.");

    var sonAramalar = await _context.TelefonAramaKayitlari
        .Where(a => a.TelefonNumarasi.KisiId == telefon.KisiId)
        .Include(a => a.Personel)
        .OrderByDescending(a => a.AramaZamani)
        .Take(10)
        .Select(a => new AramaKaydiOzetDto
        {
          Yonu = a.Yonu.ToString(),
          AramaZamani = a.AramaZamani,
          PersonelAdi = a.Personel.AdSoyad,
          Notlar = a.Notlar
        }).ToListAsync();

    var kisiDto = new KisiDetayDto
    {
      Id = telefon.Kisi.Id,
      AdSoyad = telefon.Kisi.AdSoyad,
      Notlar = telefon.Kisi.Notlar,
      TelefonNumaralari = telefon.Kisi.TelefonNumaralari.Select(tn => new TelefonNumarasiDto
      {
        Id = tn.Id,
        Numara = tn.Numara
      }).ToList(),
      SonAramalar = sonAramalar
    };

    return Ok(kisiDto);
  }

  [HttpPost]
  [Authorize(Roles = "Admin, Operator")]
  public async Task<IActionResult> LogCall(AramaKaydiOlusturDto aramaDto)
  {
    var normalizedNumber = PhoneNumberHelper.Normalize(aramaDto.Numara);
    if (string.IsNullOrEmpty(normalizedNumber))
      return BadRequest("Geçersiz telefon numarası formatı.");

    var telefonNumarasi = await _context.TelefonNumaralari.FirstOrDefaultAsync(t => t.Numara == normalizedNumber);

    if (telefonNumarasi == null)
    {
      if (string.IsNullOrWhiteSpace(aramaDto.YeniKisiAdi))
        return BadRequest("Numara kayıtlı değil. Yeni kişi oluşturmak için 'YeniKisiAdi' alanı zorunludur.");

      var yeniKisi = new Kisi { AdSoyad = aramaDto.YeniKisiAdi };
      telefonNumarasi = new TelefonNumarasi { Numara = normalizedNumber, Kisi = yeniKisi };
      _context.TelefonNumaralari.Add(telefonNumarasi);
    }

    var aramaKaydi = new TelefonAramaKaydi
    {
      TelefonNumarasi = telefonNumarasi,
      Yonu = aramaDto.Yonu,
      PersonelId = aramaDto.PersonelId,
      Notlar = aramaDto.Notlar
    };

    _context.TelefonAramaKayitlari.Add(aramaKaydi);
    await _context.SaveChangesAsync();

    return Ok();
  }
}
