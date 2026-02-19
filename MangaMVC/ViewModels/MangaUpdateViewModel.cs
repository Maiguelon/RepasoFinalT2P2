using TiendaManga.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // el de las listas desplegables
namespace TiendaManga.ViewModels;

// para contar con las otras validaciones
public class MangaUpdateViewModel : MangaCreateViewModel 
{
    [Required]
    public int Id { get; set; } 

    public MangaUpdateViewModel() : base()
    {}

    public MangaUpdateViewModel(Models.Manga manga) : base(manga)
    {
        Id = manga.Id;
    }
}