using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models
{
    public class loginUsuario
    {
        public int Id_LoginUsuario { get; set; }
        public int Id_Rol { get; set; }
        public string EmailUsuario { get; set; }
        public string Pass { get; set; }
    }
}
