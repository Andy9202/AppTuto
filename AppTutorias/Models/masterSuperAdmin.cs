using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin;

namespace AppTutorias.Models
{
    public class masterSuperAdmin
    {
        public rol rol { get; set; }
        public superAdmin superAdmin { get; set; }
        public loginUsuario loginUsuario { get; set; }
    }
}
