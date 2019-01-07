using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Models
{
    public class masterEstudiante
    {
        public rol rol { get; set; }
        public estudiante estudiante { get; set; }
        public loginUsuario loginUsuario { get; set; }
    }
}
