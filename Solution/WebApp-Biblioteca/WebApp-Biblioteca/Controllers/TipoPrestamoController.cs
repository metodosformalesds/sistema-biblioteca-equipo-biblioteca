using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;
using WebApp_Biblioteca.Models.Filter;

namespace WebApp_Biblioteca.Controllers
{
    [UserAuthFilter]
    public class TipoPrestamoController : Controller
    {
        // GET: TipoPrestamo
        public ActionResult ListaTipoPrestamos()
        {
            using (Entities dbcon = new Entities())
            {
                var rawData = dbcon.SP_S_tipoPrestamo().ToList();
                return View(rawData);
            }
        }

        [HttpGet]
        public ActionResult AgregarTipoPrestamo() {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarTipoPrestamo(TipoPrestamo tipoPrestamo)
        {
            using (Entities dbcon = new Entities())
            {
                dbcon.SP_I_tipoPrestamo(tipoPrestamo.nombrePrestamo, tipoPrestamo.descripcion);
                return Redirect("ListaTipoPrestamos");
            }
        }

        [HttpGet]
        public ActionResult BorrarTipoPrestamo(int idTipoPrestamo)
        {
            using (Entities dbcon = new Entities())
            {
                TipoPrestamo tipoPrestamo = new TipoPrestamo();
                tipoPrestamo = dbcon.TipoPrestamoes.Where(x => x.idTipoPrestamo == idTipoPrestamo).FirstOrDefault();
                return View(tipoPrestamo);
            }
        }

        [HttpPost]
        public ActionResult BorrarTipoPrestamo(TipoPrestamo tipoPrestamo)
        {
            using (Entities dbcon = new Entities())
            {
                dbcon.SP_D_tipoPrestamo(tipoPrestamo.idTipoPrestamo);
                return Redirect("ListaTipoPrestamos");
            }
        }

    }
}