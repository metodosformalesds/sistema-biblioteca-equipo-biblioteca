using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;

namespace WebApp_Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int matricula) {
            using (Entities dbcon = new Entities())
            {
                Session["matricula"] = matricula;
                
                Alumno alumno = new Alumno();
                alumno = dbcon.Alumnoes.Where(x => x.matricula == matricula).FirstOrDefault();

                if (alumno is null)
                {
                    dbcon.SP_I_alumno(matricula);
                    //redirect to cuestionario
                    return RedirectToAction("Cuestionario", "Cuestionario");
                }

                if (alumno.completoCuestionario == false)
                {
                    //cuestionario redirect
                    return RedirectToAction("Cuestionario", "Cuestionario");
                }
                else {
                    Session["idAlumno"] = alumno.idAlumno;
                    return RedirectToAction("ElegirPrestamo","Prestamo");
                }

            }
         }

        public ActionResult Exito()
        {
            return View();
        }

    }
}