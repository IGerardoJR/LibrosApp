using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.Domain;

public class LoadDatabase
{
    public static async Task  InsertarData(
        DatabaseContext context, UserManager<ApplicationUser> usuarioManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Insertando un rol, siempre cuando no haya en existencia un rol
        if(!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));
        }

        if(!usuarioManager.Users.Any())
        {
            var usuario = new ApplicationUser {
                Nombre = "Isaias Cordova",
                Email = "igerardo0808@gmail.com",
                UserName = "igerardojr"
            };
//                                  usuario, password
            await usuarioManager.CreateAsync(usuario,"PasswordIsa-554");
            await usuarioManager.AddToRoleAsync(usuario, "ADMIN");
        }

//  Anadiendo las categorias
        if(!context.Categorias!.Any())
        {
            context.Categorias!.AddRange(
              new Categoria { Nombre = "Drama" },
              new Categoria { Nombre = "Comedia" },
              new Categoria { Nombre = "Terror" },
              new Categoria { Nombre = "Aventura" }  
            );
        }
// Cargando la data de Libros
       if(!context.Libros!.Any())
       {
        context.Libros!.AddRange(
            new Libro { 
                Titulo = "Don quijote de la mancha",
                CreateDate = "06/06/2020",
                Imagen = "quijote.jpg",
                Autor = "Miguel de Cervantes"},
            new Libro {
                Titulo = "Harry Potter",
                CreateDate = "06/01/2021",
                Imagen = "harry.jpg",
                Autor = "Juan de la vega"
            }
        );
       } 


       if(!context.LibrosCategorias!.Any())
       {
        context.LibrosCategorias!.AddRange(
            new LibroCategoria { CategoriaId = 1, LibroId = 1},
            new LibroCategoria { CategoriaId = 1, LibroId = 2}
        );
       }

// Disparando los datos a la db
    context.SaveChanges();
    }

}