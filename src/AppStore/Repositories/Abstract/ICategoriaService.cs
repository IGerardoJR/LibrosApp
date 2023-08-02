using AppStore.Models.Domain;

namespace AppStore.Repositories.Abstract;

public interface ICategoriaService
{
    public IQueryable<Categoria>List();
}