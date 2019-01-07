using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Docente;

namespace AppTutorias.Models
{
    public class masterDocente
    {
        public rol rol { get; set; }
        public docente docente { get; set; }
        public loginUsuario loginUsuario { get; set; }
    }
}
