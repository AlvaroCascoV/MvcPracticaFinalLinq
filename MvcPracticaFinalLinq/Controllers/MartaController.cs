using Microsoft.AspNetCore.Mvc;
using MvcPracticaFinalLinq.Models;
using MvcPracticaFinalLinq.Repositories;

namespace MvcPracticaFinalLinq.Controllers
{
    public class MartaController : Controller
    {
        RepositoryUsuarios repo;

        public MartaController()
        {
            this.repo = new RepositoryUsuarios();
        }

        public IActionResult Index()
        {
            List<Marta> usuarios = this.repo.GetUsuarios();
            return View(usuarios);
        }
        public IActionResult Details(int idusuario)
        {
            Marta usuario = this.repo.FindUsuario(idusuario);
            return View(usuario);
        }
    }
}
