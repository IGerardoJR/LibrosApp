namespace AppStore.Models.Domain;

using System.ComponentModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


// Representa la referencia a entity framework
public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    // Cuando se instancia DbContext, va a solicitar que le pasemos los siguientes params
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Libro>().HasMany(x => x.CategoriaRelationList)
        .WithMany(y => y.LibroRelationList)
        .UsingEntity<LibroCategoria>(
            // LibroCategoria - Categoria
            j => j.HasOne(p => p.Categoria)
            .WithMany(p => p.LibroCategoriaRelationList)
            .HasForeignKey(p => p.CategoriaId),
      
            // LibroCategoria - Libro
            j => j.HasOne(p => p.Libro)
            .WithMany(p => p.LibroCategoriaRelationList)
            .HasForeignKey(p => p.LibroId),
        // Records dentro del libro categoria
         j =>
         {
            j.HasKey(t => new { t.LibroId, t.CategoriaId});
         }
        );
    }
    public DbSet<Categoria>? Categorias {get;set;}
    public DbSet<Libro>? Libros {get;set;}

    public DbSet<LibroCategoria>? LibrosCategorias {get;set;}



}