using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Estudiante
{
    public class estudiante
    {

        public int Id_Estudiante{ get; set; }
        public string Nombre { get; set; }
        public string NombreDos { get; set; }
        public string Apellido { get; set; }
        public string ApellidoDos { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Matricula { get; set; }
        public string CodCarrera { get; set; }
        public string CodFacultad { get; set; }
    }
}
