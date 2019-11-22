using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApp_Biblioteca.Models;
using WebApp_Biblioteca.Models.EntidadesBiblioteca;
using WebApp_Biblioteca.Models.Filter;

namespace WebApp_Biblioteca.Controllers
{
    [UserAuthFilter]
    public class ExportarController : Controller
    {
        // GET: Exportar
        public ActionResult Datos()
        {
            using (Entities dbcon = new Entities())
            {
                var table = new DataTable();
                var cmd = dbcon.Database.Connection.CreateCommand();
                cmd.CommandText = "exec SP_Select_dataset";

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());

                //var result = dbcon.SP_Select_dataset().ToList();
                //var rawdata = dbcon.Database.SqlQuery<ExportarDatos>("Select p.matricula, p.Carrera, f.Sexo, i.Instituto, e.Edad, PE.Etnia, S.Semestre from (	select matricula,titulo,respuesta as Carrera from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1 and titulo = 'Carrera')p left join (		select matricula,titulo,respuesta as Sexo from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1	and titulo = 'Sexo' )f on p.matricula = f.matricula 	left join (select matricula,titulo,respuesta as Instituto from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1 	and titulo = 'Instituto')i on i.matricula = p.matricula left join (select matricula,titulo,respuesta as Edad from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1	and titulo = 'Edad')E on e.matricula = p.matricula		left join (select matricula,titulo,respuesta as Etnia from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1	and titulo = 'Etnia')PE on PE.matricula = p.matricula left join (select matricula,titulo,respuesta as Semestre from pregunta left join respuesta on pregunta.idPregunta = respuesta.idPregunta inner join alumno on alumno.idAlumno = respuesta.idAlumno where visible = 1	and titulo = 'Semestre')S on S.matricula = p.matricula").ToList();

                return View(table);
            }
        }


        public FileResult DescargarReporte()
        {
            using (Entities dbcon = new Entities())
            {
                var table = new DataTable();
                var cmd = dbcon.Database.Connection.CreateCommand();
                cmd.CommandText = "exec SP_Select_dataset";

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());

                var sb = new StringBuilder();
                IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in table.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));
                }
                return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "ReportePreguntas.csv");
            }
        }

        public FileResult ReportePrestamos()
        {
            using (Entities dbcon = new Entities())
            {
                dbcon.SP_respaldarPrestamos();
                var lstData = dbcon.vw_reportePrestamos.ToList();// var lstData = new DataDbHandler().GetDetails();
                var sb = new StringBuilder();
                sb.AppendLine("id" + "," + "idAlumno" + "," + "TipoPrestamo" + ", " + "Fecha" );

                foreach (var data in lstData)
                {
                    sb.AppendLine(data.id + "," + data.idAlumno + "," + data.TipoPrestamo + ", " + data.fecha);
                }
                return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "ReportePrestamos.csv");
            }
        }

        public FileResult RecuperarDatos() {
            using (Entities dbcon = new Entities())
            {
                var lstData = dbcon.vw_datosHistoricos.ToList();// var lstData = new DataDbHandler().GetDetails();
                var sb = new StringBuilder();
                sb.AppendLine("IdRegistro" + "," + "Pregunta" + "," + "Respuesta" + ", " + "idAlumno" + ", " + "fechaRespuesta" + ", " + "fechaBorrado");

                foreach (var data in lstData)
                {
                    sb.AppendLine(data.idRegistro + "," + data.titulo + ","+data.respuesta+", "+data.idAlumno+", "+data.fechaRespuesta + ", " + data.fechaBorrado);
                }
                return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "Reporte.csv");
            }
        }



        [HttpPost]
        public ActionResult Datos(FormCollection form) {
            foreach (var key in form.AllKeys)
            {
                using (Entities dbcon = new Entities())
                {
                    int x = int.Parse(form[key]);

                    if (x == 1)
                    {
                        //borrar datos
                        dbcon.sp_respaldarInformacion();
                        dbcon.sp_D_borrarRespuestas();
                    }
                    else
                    {
                        return Redirect("RecuperarDatos");
                    }

                }


            }

            return Redirect("Datos");
        }
    }
}