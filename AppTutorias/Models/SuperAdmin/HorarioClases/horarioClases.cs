using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Models.SuperAdmin.HorarioClases
{
    public class horarioClases
    {
      public int   Id_HorarioClases { get; set; }
        public string CedulaDocente { get; set; }
      public string  NombreDocente { get; set; } 
      public string Email { get; set; }
      public string Celular { get; set; }
        public string CodigoMateria { get; set; }
        public string Materia { get; set; }
      public string Modulo { get; set; }
      public string Periodo { get; set; }
        public string Aula { get; set; }
        public string Paralelo { get; set; }
    }
}
