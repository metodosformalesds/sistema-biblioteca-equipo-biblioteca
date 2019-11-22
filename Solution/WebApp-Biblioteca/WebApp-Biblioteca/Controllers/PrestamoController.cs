using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;

namespace WebApp_Biblioteca.Controllers
{
    public class PrestamoController : Controller
    {
        [HttpGet]
        public ActionResult ElegirPrestamo()
        {
            using (Entities dbcon = new Entities())
            {
                var rawData = dbcon.SP_S_tipoPrestamo().ToList();
                return View(rawData);
            }
        }

        [HttpPost]
        public ActionResult ElegirPrestamo(FormCollection form) {
            int idAlumno = int.Parse(Session["idAlumno"].ToString());
            using (Entities dbcon = new Entities())
            {
                foreach (var key in form.AllKeys)
                {
                    dbcon.SP_I_Prestamo(idAlumno, int.Parse(form[key]));
                }
                return RedirectToAction("Exito", "Home");
            }
        }
    }
}