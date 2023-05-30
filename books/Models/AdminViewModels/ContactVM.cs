using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public partial class ContactVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "İsim, soyisim boş geçilemez!")]
    public string Isim { get; set; } = null!;

    [Required(ErrorMessage = "E-posta boş geçilemez!")]
    public string Eposta { get; set; } = null!;

    [Required(ErrorMessage = "Konu boş geçilemez!")]
    public string Konu { get; set; } = null!;

    [Required(ErrorMessage = "Mesaj boş geçilemez!")]
    public string Mesaj { get; set; } = null!;
}
