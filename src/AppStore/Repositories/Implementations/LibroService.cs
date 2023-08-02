using System.Runtime.InteropServices;
using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
// Invocamos a entity framework core


namespace AppStore.Repositories.Implementations;


public class LibroService : ILibroService
{
    private readonly DatabaseContext ctx;

    public LibroService(DatabaseContext ctxParameter)
    {
        ctx = ctxParameter;
    }

    // Metodo para la insercion de un libro
    public bool Add(Libro libro)
    {
        try
        {
            ctx.Libros!.Add(libro);
            ctx.SaveChanges();

            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria
                {
                    LibroId = libro.Id,
                    CategoriaId = categoriaId
                };

                ctx.LibrosCategorias!.Add(libroCategoria);
            }
            ctx.SaveChanges();
            return true;

        }catch(Exception ex)
        {
            Console.WriteLine($"{ex}");
            return false;
        }
    }

    // Metodo para la eliminacion de un libro u articulo
    public bool Delete(int id)
    {
        try 
        {
            var data = GetById(id);
            if(data is null)
            {
                return false;
            }
        // Buscamos todos los records de libro-categorias
        var libroCategorias = ctx.LibrosCategorias!.Where(a => a.LibroId == data.Id);
        ctx.LibrosCategorias!.RemoveRange(libroCategorias);
        ctx.Libros!.Remove(data);
        ctx.SaveChanges();
        return true;

        }catch(Exception ex)
        {
            throw new Exception($"{ex}");
            
        }
    }

    // Metodo para encontrar un libro en especifico
    public Libro GetById(int id)
    {
        return ctx.Libros!.Find(id)!;
    }

    // Buscaremos obtener una categoria en especial
    public List<int> GetCategoriaByLibroId(int libroId)
    {
        return ctx.LibrosCategorias!.Where(x =>  x.LibroId == libroId).Select(a => a.CategoriaId).ToList();
    }

    public LibroListVm List(string term = "", bool paging = false, int currentPage = 0)
    {
        var data = new LibroListVm();
        // Hacemos la consulta de todos los libros que estan en nuestra DB
        var list = ctx.Libros!.ToList();
        if(!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();
            list = list.Where(a => a.Titulo!.ToLower().StartsWith(term)).ToList();
        }

        if(paging)
        {
            int pageSize = 5;
            int count = list.Count;
            int TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            list.Skip((currentPage-1)*pageSize).Take(pageSize).ToList();
            data.PageSize = pageSize;

            data.CurrentPage = currentPage;
            data.TotalPages = TotalPages;
        }

        foreach(var libro in list)
        {
            var categorias = (
                from categoria in ctx.Categorias
                join lc in ctx.LibrosCategorias!
                on categoria.Id equals lc.CategoriaId
                where lc.LibroId == libro.Id
                select categoria.Nombre
            ).ToList();
            string categoriaNombres = string.Join(",", categorias); // drama, horror, accion, etc
            libro.CategoriasNames = categoriaNombres;
        }
        data.LibroList = list.AsQueryable();
        
        return data;

    }

    // Metodo para actualizar un libro
    public bool Update(Libro libro)
    {
        try{

            var categoriasParaEliminar = ctx.LibrosCategorias!.Where(a => a.LibroId == libro.Id);
            foreach(var categoria in categoriasParaEliminar)
            {
                ctx.LibrosCategorias!.Remove(categoria);
            }

            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategorias = new LibroCategoria { CategoriaId = categoriaId, LibroId = libro.Id};
                ctx.LibrosCategorias!.Add(libroCategorias);
                
            }
            ctx.Libros!.Update(libro);
            ctx.SaveChanges();
            return true;

        }catch(Exception )
        {
                return false;
        }
    }
}