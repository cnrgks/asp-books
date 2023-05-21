using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class DillerVM
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Dil adı boş geçilemez!")]
    public string? DilAdi { get; set; }
}