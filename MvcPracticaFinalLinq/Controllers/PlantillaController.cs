using Microsoft.AspNetCore.Mvc;
using MvcPracticaFinalLinq.Models;
using MvcPracticaFinalLinq.Repositories;

namespace MvcPracticaFinalLinq.Controllers
{
    public class PlantillaController : Controller
    {
        private RepositoryPlantilla repo;

        public PlantillaController()
        {
            this.repo = new RepositoryPlantilla();
        }

        public IActionResult Index()
        {
            List<Plantilla> plantillas = this.repo.GetPlantillas();
            if (plantillas == null)
            {
                ViewData["MENSAJE"] = "No existen plantillas";
                return View();
            }
            else
            {
                return View(plantillas);
            }
        }
        public IActionResult Upsert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(int hospitalcod, int salacod, int empleadono, string apellido, string funcion, string turno, int salario)
        {
            this.repo.UpsertPlantilla(hospitalcod, salacod, empleadono, apellido, funcion, turno, salario);
            ViewData["MENSAJEUP"] = "Operación realizada con éxito";
            return View();
        }

        public IActionResult Delete(int empleadono)
        {
            this.repo.DeletePlantilla(empleadono);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorPlantillas()
        {
            List<string> funciones = this.repo.GetFunciones();
            ViewData["FUNCIONES"] = funciones;
            return View();
        }
        [HttpPost]
        public IActionResult BuscadorPlantillas(string funcion)
        {
            ResumenPlantilla model = this.repo.GetPlantillaFuncion(funcion);
            List<string> funciones = this.repo.GetFunciones();
            ViewData["FUNCIONES"] = funciones;


            if (model == null)
            {
                ViewData["MENSAJE"] = "No existen empleados con funcion '" +
                                        funcion + "'";
                return View();
            }
            else
            {
                return View(model);
            }
        }
    }
}
