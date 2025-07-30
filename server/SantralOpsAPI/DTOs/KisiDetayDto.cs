namespace SantralOpsAPI.DTOs;

public class KisiDetayDto
{
  public int Id { get; set; }
  public string AdSoyad { get; set; }
  public string? Notlar { get; set; }
  public List<TelefonNumarasiDto> TelefonNumaralari { get; set; } = [];
}
