using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class UsersVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı boş geçilemez!")]
    [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter olmalıdır!")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
    public string? Password { get; set; }
}