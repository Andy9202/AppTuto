using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.Modulo
{
    public class modulo
    {

        public int Id_Modulo { get; set; }
        public string CodigoModulo { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Dia{ get; set; }
    }
}
