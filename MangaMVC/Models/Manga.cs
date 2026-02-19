namespace TiendaManga.Models;

public enum Demografia
{
    Shonen = 1,
    Seinen = 2,
    Shojo = 3,
    Josei = 4
}

public class Manga
{
    public int Id {get; set;}
    public string Titulo {get; set;}
    public int TomosPublicados {get; set;}
    public Demografia Demografia {get; set;}
}