using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

  // GET: api/Lookup/{numara}
  [HttpGet("{numara}")]
  public async Task<ActionResult<KisiDetayDto>> LookupByNumber(string numara)
  {
    var normalizedNumber = PhoneNumberHelper.Normalize(numara);
    if (string.IsNullOrEmpty(normalizedNumber))
    {
      return BadRequest("Geçersiz telefon numarası formatı.");
    }

    var telefon = await _context.TelefonNumaralari
        .Include(t => t.Kisi)
            .ThenInclude(k => k.TelefonNumaralari)
        .FirstOrDefaultAsync(t => t.Numara == normalizedNumber);

    if (telefon == null)
    {
      return NotFound("Bu numaraya kayıtlı bir kişi bulunamadı.");
    }

    var kisiDto = new KisiDetayDto
    {
      Id = telefon.Kisi.Id,
      AdSoyad = telefon.Kisi.AdSoyad,
      Notlar = telefon.Kisi.Notlar,
      TelefonNumaralari = telefon.Kisi.TelefonNumaralari.Select(tn => new TelefonNumarasiDto
      {
        Id = tn.Id,
        Numara = tn.Numara
      }).ToList()
    };

    return Ok(kisiDto);
  }

  // POST: api/Lookup
  [HttpPost]
  public async Task<ActionResult<KisiDetayDto>> CreateKisi(KisiOlusturDto kisiDto)
  {
    var normalizedNumber = PhoneNumberHelper.Normalize(kisiDto.Numara);
    if (string.IsNullOrEmpty(normalizedNumber))
    {
      return BadRequest("Geçersiz telefon numarası formatı.");
    }

    var existingNumber = await _context.TelefonNumaralari.FirstOrDefaultAsync(t => t.Numara == normalizedNumber);
    if (existingNumber != null)
    {
      return BadRequest("Bu telefon numarası zaten sisteme kayıtlı.");
    }

    var yeniKisi = new Kisi
    {
      AdSoyad = kisiDto.AdSoyad,
      Notlar = kisiDto.Notlar
    };

    var yeniNumara = new TelefonNumarasi
    {
      Numara = normalizedNumber,
      Kisi = yeniKisi
    };

    _context.TelefonNumaralari.Add(yeniNumara);
    await _context.SaveChangesAsync();

    var responseDto = new KisiDetayDto
    {
      Id = yeniKisi.Id,
      AdSoyad = yeniKisi.AdSoyad,
      Notlar = yeniKisi.Notlar,
      TelefonNumaralari = new() { new TelefonNumarasiDto { Id = yeniNumara.Id, Numara = yeniNumara.Numara } }
    };

    return CreatedAtAction(nameof(LookupByNumber), new { numara = yeniNumara.Numara }, responseDto);
  }
}
