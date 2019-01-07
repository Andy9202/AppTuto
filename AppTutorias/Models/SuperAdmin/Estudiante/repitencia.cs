using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Estudiante
{
    public class repitencia
    {
        public int Matricula { get; set; }
        public string NombreEstudiante { get; set; }
        public string Materia{ get; set; }
        public string CodigoMateria { get; set; }
        public char Repitencia { get; set; }
    }
}
