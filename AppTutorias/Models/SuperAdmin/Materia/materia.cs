 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Materia
{
    public class materia
    {
        public int Id_Carrera { get; set; }
        public string CodigoFacultad { get; set; }
        public string CodigoCarrera { get; set; }
        public string NombreMateria { get; set; }
        public string CodigoMateria { get; set; }
        public char Repitencia { get; set; }
    }
}
