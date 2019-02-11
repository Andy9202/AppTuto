using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;
using AppTutorias.Models.SuperAdmin.Docente;
namespace AppTutorias.Models
{
    public class estadisticaTutoria
    {
        public docente Docente { get; set; }
        public estudiante Estudiante { get; set; }
        public int HorasPlanificadas { get; set; }
        public int HorasDadas { get; set; }
        public int HorasLibres { get; set; }
        public  int horasObligatorias { get; set; }
        public int numeroEstudiantes { get; set; }
        public int horasCarrera { get; set; }
        public int horasOtrasCarreras { get; set; }
        public string CodigoMateria { get; set; }
        public string repitencia { get; set; }

    }
}