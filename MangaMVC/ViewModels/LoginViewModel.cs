using System.ComponentModel.DataAnnotations;
namespace Manga.ViewModels;
public class LoginViewModel
{
    [Required]
    public string Username { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public bool ?IsAuthenticated { get; set; } 
}
