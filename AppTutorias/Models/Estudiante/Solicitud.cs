using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.Estudiante
{
    public class Solicitud
    {
        public int Id_Solicitud { get; set; }
        public string CodigoMateria  { get; set; }
        public string NombreMateria { get; set; }
        public string Cedula { get; set; }
        public string Matricula { get; set; }
        public string CodigoModulo { get; set; }
        public string Mensaje { get; set; }
        public string Estudiante { get; set; }
        public string Hora { get; set; }
        public string Confirmar { get; set; }
        public DateTime Fecha { get; set; }

    }
}