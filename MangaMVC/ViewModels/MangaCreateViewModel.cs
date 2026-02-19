using TiendaManga.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // el de las listas desplegables
namespace TiendaManga.ViewModels;

public class MangaCreateViewModel
{
    [Display(Name = "Titulo del manga")]
    [Required(ErrorMessage = "Titulo obligatorio")]
    [StringLength(150, MinimumLength = 2 ,ErrorMessage = "Titulo entre 2 y 150 caracteres.")]
    public string Titulo {get; set;}

    [Display(Name = "Tomos publicados")]
    [Required(ErrorMessage = "Numero de tomos obligatorio.")]
    [Range(1, 500, ErrorMessage = "Se aceptan entre 1 y 500 tomos")]
    public int TomosPublicados {get; set;}

    [Required(ErrorMessage = "Demografia obligatoria.")]
    public Demografia Demografia {get; set;}
    public SelectList ListaCategorias { get; set; } // para el desplegable

    public MangaCreateViewModel()
    {}

    public MangaCreateViewModel(Models.Manga manga)
    {
        Titulo = manga.Titulo;
        TomosPublicados = manga.TomosPublicados;
        Demografia = manga.Demografia;
    }
}