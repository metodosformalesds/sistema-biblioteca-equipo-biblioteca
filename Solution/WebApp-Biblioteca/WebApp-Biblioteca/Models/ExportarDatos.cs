using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_Biblioteca.Models
{
    public class ExportarDatos
    {
        public ExportarDatos() { }

        public ExportarDatos(int matricula, string Carrera, string Sexo, string Instituto, string Edad, string Etnia, string Semestre) {
            this.matricula = matricula;
            this.Carrera = Carrera;
            this.Sexo = Sexo;
            this.Instituto = Instituto;
            this.Edad = Edad;
            this.Etnia = Etnia;
            this.Semestre = Semestre;
        }

        public int matricula { get; set; }
        public string Carrera { get; set; }
        public string Sexo { get; set; }
        public string Instituto { get; set; }
        public string Edad { get; set; }
        public string Etnia { get; set; }
        public string Semestre { get; set; }
        
    }
}