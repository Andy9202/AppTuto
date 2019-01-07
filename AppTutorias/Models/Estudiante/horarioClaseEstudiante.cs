using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.Estudiante
{
    public class horarioClaseEstudiante
    {

        public string CodigoFacultad { get; set; }
        public string NombreFacultad { get; set; }
        public string CodigoCarrera { get; set; }
        public string NombreCarrera { get; set; }
        public string CodigoMateria { get; set; }
        public string NombreMateria { get; set; }
        public string Aula { get; set; }
        public string Paralelo { get; set; }
        public string Estudiante { get; set; }
        public string CodigoModulo { get; set; }
        public string Dia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Periodo { get; set; }
        public string CodigoPeriodo { get; set; }
        public string Fecha { get; set; }
    }
}