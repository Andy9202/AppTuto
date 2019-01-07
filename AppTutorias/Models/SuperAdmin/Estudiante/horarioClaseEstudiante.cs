using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Estudiante
{
    public class horarioClaseEstudiante
    {
        public int Id_HorarioEstudiante { get; set; }
        public string Docente { get; set; }
        public string Estudiante { get; set; }
        public string Matricula { get; set; }
        public string Materia { get; set; }
        public string Horario { get; set; }
        public string Periodo { get; set; }
    }
}
