using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class YazarlarVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yazar adı boş geçilemez!")]
    public string Adi { get; set; } = null!;

    [Required(ErrorMessage = "Yazar soyadı boş geçilemez!")]
    public string Soyadi { get; set; } = null!;

    [Required(ErrorMessage = "Yazar doğum tarihi boş geçilemez!")]
    public DateOnly DogumTarihi { get; set; }

    [Required(ErrorMessage = "Yazar doğum yeri boş geçilemez!")]
    public string DogumYeri { get; set; } = null!;

    [Required(ErrorMessage = "Yazar cinsiyeti boş geçilemez!")]
    public bool Cinsiyeti { get; set; }
}