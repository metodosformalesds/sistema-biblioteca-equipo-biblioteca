using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;

namespace WebApp_Biblioteca.Controllers
{
    public class CuestionarioController : Controller
    {
        [HttpGet]
        public ActionResult Cuestionario()
        {
            

            using (Entities dbcon = new Entities())
            {
                int matricula = int.Parse(Session["matricula"].ToString());
                Alumno alumno = new Alumno();
                alumno = dbcon.Alumnoes.Where(x => x.matricula == matricula).FirstOrDefault();
                Session["idAlumno"] = alumno.idAlumno;
                var rawData = dbcon.SP_S_preguntasFaltantes(alumno.idAlumno).ToList();

                List<SelectListItem> listaRespuestas = new List<SelectListItem>();

                foreach (SP_S_preguntasFaltantes_Result sp in rawData) {
                    if (sp.tipoDeDato == "Opción múltiple") {
                        string[] categories = sp.respuestas.Split(',');
                        foreach(string category in categories)
                        {
                            listaRespuestas.Add(new SelectListItem
                            {
                                Text = category,
                                Value = sp.idPregunta.ToString()
                            });
                        }
                    }
                }
                ViewBag.listaRespuestas = new SelectList(listaRespuestas, "Value", "Text");

                return View(rawData);
            }
        }

        [HttpPost]
        public ActionResult Cuestionario(FormCollection form) {
            int idAlumno = int.Parse(Session["idAlumno"].ToString());
            using (Entities dbcon = new Entities())
            {
                foreach (var key in form.AllKeys)
                {
                    dbcon.SP_I_respuesta(int.Parse(key),form[key], idAlumno);
                    dbcon.SP_U_completoCuestionario(idAlumno);
                }
            }
            return RedirectToAction("ElegirPrestamo","Prestamo");
        }
    }
}