using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc; // Libreria para tener las utilidades del modelo mvc
namespace AppStore.Controllers;

public class HomeController : Controller 
{
    private readonly ILibroService _libroService;

    public HomeController(ILibroService libroService)
    {
        _libroService = libroService;
    }
    // Definimos que habra una pagina que se llame Index
    // IActionResult permite procesar los resultados que vienen de la pagina razor compilada con html/css
    public IActionResult Index(string term="", int currentPage=1)
    {
        var libroListVm = _libroService.List(term, true,currentPage);
        return View(libroListVm); // Retornamos la vista
    }
    
    public IActionResult LibroDetail(int libroId)
    {
        var libro = _libroService.GetById(libroId);
        return View(libro);
    }

    // Redireccionamiento a la pagina About
    public IActionResult About()
    {
        return View();
    }

}