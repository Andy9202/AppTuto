using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Estudiante
{
    public class metodoHorarioClaseEstudiante
    {
        conexionDataContext db = new conexionDataContext();


        //ingresar horario clase
        public void ingresarHorarioClaseEstudiante(string Matricula,string CedulaDocente,string CodigoMateria,string CodigoModulo,string Paralelo)
        {
            db.IngresarHorarioClaseEstudiante(int.Parse(Matricula), CedulaDocente,CodigoMateria,CodigoModulo,Paralelo);
        }
        public void eliminarHorarioClaseEstudiante(string Matricula, string Id_HorarioClase)
        {
            db.EliminarMateriaEstudiante(int.Parse(Matricula), int.Parse(Id_HorarioClase));
        }

        //consultar horario clase estudiante

        public List<horarioClaseEstudiante> consultaHorariosEstudainte()
        {
             var horarioEstudiante = db.horarioClaseEstudiante();

            List<horarioClaseEstudiante> listaHorarioEstudiante = new List<horarioClaseEstudiante>();

            foreach (var consulta in horarioEstudiante )
            {
                horarioClaseEstudiante horarioClaseEstudiante = new horarioClaseEstudiante();

                horarioClaseEstudiante.Id_HorarioEstudiante = consulta.Id_HoraioEstudiante;
                horarioClaseEstudiante.Docente = consulta.Docente;
                horarioClaseEstudiante.Estudiante = consulta.Estudiante;
                horarioClaseEstudiante.Matricula = consulta.Matricula.ToString();
                horarioClaseEstudiante.Materia = consulta.Materia;
                horarioClaseEstudiante.Horario = consulta.Dia+" "+ consulta.HoraInicio.ToString(@"h\:mm")+'-'+consulta.HoraFin.ToString(@"h\:mm");
                horarioClaseEstudiante.Periodo = consulta.Periodo;

                listaHorarioEstudiante.Add(horarioClaseEstudiante);
            }

            return listaHorarioEstudiante;
        }
    }
}
