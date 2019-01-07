using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Estudiante
{
    public class horarioTutorias
    {
        public int Id_HorarioTutorias { get; set; }
        public string Matricula { get; set; }
        public string NombreDocente { get; set; }
        public string NombreEstudiante { get; set; }
        public string CedulaDocente { get; set; }
        public string CodigoModulo { get; set; }
        public string NombreMateria { get; set; }
        public string CodigoMateria { get; set; }
        public string CodigoPeriodo { get; set; }
        public string Modulo { get; set; }
        public string EstadoObligatorio{ get; set; }
        public string Fecha { get; set; }
        public string Aula { get; set; }
        public string Asistencia { get; set; }


    }
}
