using TiendaManga.Models;
namespace TiendaManga.ViewModels;

public class MangaIndexViewModel
{
    public int Id {get; set;}
    public string Titulo {get; set;}
    public int TomosPublicados {get; set;}
    public Demografia Demografia {get; set;}

    public MangaIndexViewModel()
    {
    }

    public MangaIndexViewModel(Models.Manga manga)
    {
        Id = manga.Id;
        Titulo = manga.Titulo;
        TomosPublicados = manga.TomosPublicados;
        Demografia = manga.Demografia;
    }
}
