namespace SantralOpsAPI.DTOs;

public class AramaKaydiOzetDto
{
  public string Yonu { get; set; } = string.Empty;
  public DateTime AramaZamani { get; set; }
  public string PersonelAdi { get; set; } = string.Empty;
  public string? Notlar { get; set; }
}
