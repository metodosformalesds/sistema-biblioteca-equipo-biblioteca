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
    public class PreguntaController : Controller
    {
        // GET: Pregunta
        public ActionResult ListaPreguntas()
        {
            using (Entities dbcon = new Entities())
            {
                var rawData = dbcon.SP_S_Preguntas().ToList();
                return View(rawData);
            }
            
        }

        [HttpGet]
        public ActionResult CrearPregunta() {
            List<SelectListItem> listaTipoDato = new List<SelectListItem>();
            int selectedItem = 0;

            listaTipoDato.Add(new SelectListItem
            {
                Text = "Seleccione una opción",
                Value = "0"
            });
            listaTipoDato.Add(new SelectListItem
            {
                Text = "Texto",
                Value = "Texto"
            });

            listaTipoDato.Add(new SelectListItem
            {
                Text = "Número",
                Value = "Número entero"
            });

            listaTipoDato.Add(new SelectListItem
            {
                Text = "Si/No",
                Value = "Si/No"
            });

            listaTipoDato.Add(new SelectListItem
            {
                Text = "Opción múltiple",
                Value = "Opción múltiple"
            });


            ViewBag.listaTipoDato = new SelectList(listaTipoDato, "Value", "Text", selectedItem);
            return View();
        }

        [HttpPost]
        public ActionResult CrearPregunta(Pregunta pregunta, FormCollection form) {
            string respuestas = form["respuestas"].ToString();
            string tipoDato = form["dropTipoDato"].ToString();

            if (tipoDato != "Opción múltiple") {
                respuestas = "";
            }
            //
            //
            //
            /////////////////////////////
            /////////////////////////////
            ////Hay que poner el ID de la sesion del ADMINISTRADOR
            int creadaPor = 1;/////////////////////////////
            //////////////////////////////////////////////////////////
            /////////////////////////////
            /////////////////////////////
            /////////////////////////////
            ////////////////////////////////
            /////////////////////////////

            using (Entities dbcon = new Entities())
            {
                dbcon.SP_I_pregunta(pregunta.titulo, tipoDato, respuestas, creadaPor);
                dbcon.SP_U_desacompletarCuestionario();
            }
                return Redirect("ListaPreguntas");
        }

        [HttpGet]
        public ActionResult BorrarPregunta(int idPregunta) {
            using (Entities dbcon = new Entities())
            {
                Pregunta pregunta = new Pregunta();
                pregunta = dbcon.Preguntas.Where(x => x.idPregunta == idPregunta).FirstOrDefault();
                return View(pregunta);
            }
                
        }
        [HttpPost]
        public ActionResult BorrarPregunta(Pregunta p) {
            using (Entities dbcon = new Entities())
            {
                dbcon.SP_D_borrarPregunta(p.idPregunta);
                return Redirect("ListaPreguntas");
            }
                
        }

    }
}


