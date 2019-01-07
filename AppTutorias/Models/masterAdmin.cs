using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.Admin;

namespace AppTutorias.Models
{
    public class masterAdmin
    {
        public rol rol { get; set; }
        public admin admin { get; set; }
        public loginUsuario loginUsuario { get; set; }

    }
}
