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

        model.ListaDemografias = new SelectList(items, "Value", "Text");

        // 3. Mandamos el modelo lleno
        return View(model);
    }

    // ----- CREATE (POST)
    [HttpPost]
    public IActionResult Create(MangaCreateViewModel model)
    {
        // 1. Control de seguridad rápido
        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction("AccesoDenegado", "Login");
        }

        // 2. Si el formulario es válido (cumple los [Required], rangos, etc.)
        if (ModelState.IsValid)
        {
            // 3. MAPEO MANUAL: De ViewModel a Modelo de Dominio
            var nuevoManga = new Models.Manga
            {
                Titulo = model.Titulo,
                TomosPublicados = model.TomosPublicados,
                Demografia = model.Demografia // Acá pasamos el enum
            };

            // 4. Guardamos en la base de datos
            _mangaRepo.Add(nuevoManga);

            // 5. Volvemos al listado
            return RedirectToAction("Index");
        }

        // 6. Si hay error en el formulario (ej: año 3000), recargamos la lista del desplegable
        var items = Enum.GetValues(typeof(Demografia))
                        .Cast<Demografia>()
                        .Select(c => new SelectListItem
                        {
                            Value = c.ToString(),
                            Text = c.ToString()
                        }).ToList();

        model.ListaDemografias = new SelectList(items, "Value", "Text");

        // Y le devolvemos la vista con los errores
        return View(model);
    }

    // ----- UPDATE / MODIFICAR (GET)
    public IActionResult Update(int id)
    {
        // 1. Seguridad
        if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");
        if (!_authService.HasAccessLevel("Administrador")) return RedirectToAction("AccesoDenegado", "Login");

        // 2. Buscar la peli
        var manga = _mangaRepo.GetById(id);
        if (manga == null) return NotFound();

        // 3. MAPEO MANUAL al ViewModel de Update
        var model = new MangaUpdateViewModel(manga);

        // 4. EL MACHETE: Cargar las categorías para el desplegable
        var items = Enum.GetValues(typeof(Demografia))
                        .Cast<Demografia>()
                        .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() }).ToList();
        
        model.ListaDemografias = new SelectList(items, "Value", "Text");

        return View(model);
    }

    // ----- UPDATE / MODIFICAR (POST)
    [HttpPost]
    public IActionResult Update(MangaUpdateViewModel model)
    {
        // 1. Seguridad en el POST
        if (!_authService.HasAccessLevel("Administrador")) return RedirectToAction("AccesoDenegado", "Login");

        if (ModelState.IsValid)
        {
            // 2. Mapeo inverso (ViewModel -> Modelo de Dominio)
            var mangaEditar = new Manga
            {
                Id = model.Id,
                Titulo = model.Titulo,
                TomosPublicados = model.TomosPublicados,
                Demografia = model.Demografia
            };

            // 3. Guardar y volver
            _mangaRepo.Update(mangaEditar);
            return RedirectToAction(nameof(Index));
        }

        // 4. Si falla la validación, recargar el SelectList para que no explote la vista
        var items = Enum.GetValues(typeof(Demografia)).Cast<Demografia>().Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() }).ToList();
        model.ListaDemografias = new SelectList(items, "Value", "Text");
        return View(model);
    }

    // ----- DELETE (GET)
    public IActionResult Delete(int id)
    {
        // 1. Seguridad
        if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");
        if (!_authService.HasAccessLevel("Administrador")) return RedirectToAction("AccesoDenegado", "Login");

        // 2. Buscar
        var manga = _mangaRepo.GetById(id);
        if (manga == null) return NotFound();

        // 3. Usamos el IndexViewModel para mostrar los datos seguros en la vista de confirmación
        var model = new MangaIndexViewModel(manga);
        
        return View(model);
    }

    // ----- DELETE (POST)
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        // 1. Seguridad en el POST
        if (!_authService.HasAccessLevel("Administrador")) return RedirectToAction("AccesoDenegado", "Login");

        // 2. Borrar y volver
        _mangaRepo.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}