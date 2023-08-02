using AppStore.Models.Domain;

namespace AppStore.Models.DTO;

public class LibroListVm
{
//  Indicamos que tendremos una coleccion de libros

    public IQueryable<Libro>? LibroList {get;set;}

    public int PageSize {get;set;}

    public int CurrentPage {get;set;}

    public int TotalPages {get;set;}

    public string? Term {get;set;}



}