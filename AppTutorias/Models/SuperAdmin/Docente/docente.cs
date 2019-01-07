using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Docente
{
    public class docente
    {
        public int Id_Docente { get; set; }
        public string Nombre { get;set; }
        public string NombreDos { get; set; }
        public string Apellido { get; set; }
        public string ApellidoDos { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Cedula { get; set; }
        public HashSet<string> ConectionId { get; set; }
    }
}
