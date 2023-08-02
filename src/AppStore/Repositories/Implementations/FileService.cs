using AppStore.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.IO;

namespace AppStore.Repositories.Implementations;


public class FileServices : IFileService
{
    // Buscamos tener acceso a la carpeta wwwroot
    private readonly  IWebHostEnvironment _environment;

    public FileServices(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public bool DeleteImage(string imageFileName)
    {
        try{
            var wwwPath = _environment.WebRootPath;
            var path = Path.Combine(wwwPath,"Uploads\\",imageFileName);
            if(System.IO.File.Exists(path)){
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }catch(Exception)
        {
            return false;
        }
    }

    public Tuple<int, string> SaveImage(IFormFile imageFile)
    {
        try{
            var wwwPath = this._environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new String[] {".jpg",".png","jpeg"};
            if(!allowedExtensions.Contains(ext))
            {
                var message = $"Solo estan permitidas las extensiones{allowedExtensions}";
                return new Tuple<int,string>(0,message);
            }

            var uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path,newFileName);

            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();
            return new Tuple<int,string>(1,newFileName);


        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Tuple<int,string>(0, "Errores guardando la imagen");
            
        }
    }
}