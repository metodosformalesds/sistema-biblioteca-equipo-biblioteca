using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;
using WebApp_Biblioteca.Models.Filter;

namespace WebApp_Biblioteca.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Login
        public ActionResult IniciarSesion() {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(Administrador admin) {
            using (Entities dbcon = new Entities())
            {
                var nombre = dbcon.SP_login_administrador(admin.correo, admin.contrasena).ToList();
                if (nombre.Count != 0)
                {
                    Administrador admins = new Administrador();
                    admins = dbcon.Administradors.Where(x => x.correo == admin.correo).FirstOrDefault();
                    Session["idAdministrador"] = admins.idAdministrador;
                    return Redirect("Menu");
                }
                else
                {
                    ViewBag.Error = " <label class='alert alert-danger text - center' >Credenciales incorrectas</label>";
                    return View();
                }
                
            }
        }

        [UserAuthFilter]
        public ActionResult Menu() {
            return View();
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index","Home");
        }

    }
}