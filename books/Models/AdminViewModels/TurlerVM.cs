using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class TurlerVM
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Tür adı boş geçilemez!")]
    public string? TurAdi { get; set; }

    public int Sira { get; set; }
}