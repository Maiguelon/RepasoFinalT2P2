using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TiendaManga.Models; 
using TiendaManga.ViewModels; 
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
using TiendaManga.Interfaces;

namespace TiendaManga.Controllers;

public class MangaController : Controller
{
    private IMangaRepository _mangaRepo;
    private IAuthenticationService _authService;

    public MangaController(IMangaRepository mangaRepo, IAuthenticationService authService)
    {
        _mangaRepo = mangaRepo;
        _authService = authService;
    }

    // ### Listar Index
    public IActionResult Index()
    {
        // Controles de autenticación
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        var mangas = _mangaRepo.GetAll();

        // mapeo manual
        var listaViewModels = new List<MangaIndexViewModel>();
        foreach (var manga in mangas)
        {
            listaViewModels.Add(new MangaIndexViewModel(manga));
        }
        return View(listaViewModels);
    }

    // GetByID
    public IActionResult GetById(int id)
    {
        Models.Manga manga = _mangaRepo.GetById(id);
        if (manga == null)
        {
            return NotFound();
        }
        return View(manga);
    }

    // ----- CREATE 
    public IActionResult Create()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction("AccesoDenegado", "Login");
        }

        // Instanciamos el modelo
        var model = new MangaCreateViewModel();

        // Cargamos las opciones ANTES de ir a la vista
        var items = Enum.GetValues(typeof(Demografia))
                        .Cast<Demografia>()
                        .Select(c => new SelectListItem
                        {
                            Value = c.ToString(),
                            Text = c.ToString()
                        }).ToList();

        model.ListaCategorias = new SelectList(items, "Value", "Text");

        // 3. Mandamos el modelo lleno
        return View(model);
    }

    
}