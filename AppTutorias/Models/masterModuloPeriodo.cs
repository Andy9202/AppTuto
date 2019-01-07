using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Periodo;

namespace AppTutorias.Models
{
    public class masterModuloPeriodo
    {
        public List<modulo> listaModulo { get; set; }
        public List<periodo> listaPeriodo { get; set; }
    }
}
