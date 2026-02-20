using System.Collections.Generic;
using TiendaManga.Models; 

namespace TiendaManga.Interfaces;

public interface IMangaRepository
{
    public List<Models.Manga> GetAll();
    public Models.Manga GetById(int id);
    public void Add(Models.Manga manga);
    public void Update(Manga manga);
    public void Delete(int id);
}
