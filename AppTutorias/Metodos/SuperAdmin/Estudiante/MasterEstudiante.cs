using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Estudiante
{
    public class MasterEstudiante
    {
        public List<repitencia> ListaRepitencia { get; set; }
        public List<repitencia> ListaHorarioClaseEstudiante{ get; set; }
    }
}
