using AppTutorias.Models.Estudiante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTutorias.Metodos.Estudiante
{
    public class metodoHorarioClaseEstudiante
    {

        conexionDataContext db = new conexionDataContext();

        //consultar horario clase estudiante

        public List<horarioClaseEstudiante> consultaHorariosEstudianteMatricula(string Matricula)
        {
            var horarioEstudiante = db.horarioClaseEstudianteMatricula(int.Parse(Matricula));

            List<horarioClaseEstudiante> listaHorarioEstudiante= new List<horarioClaseEstudiante>();

            foreach (var consulta in horarioEstudiante)
            {
                horarioClaseEstudiante horarioClaseEstudiante = new horarioClaseEstudiante();

                horarioClaseEstudiante.CodigoFacultad = consulta.CodigoFacultad;
                horarioClaseEstudiante.NombreFacultad = consulta.NombreFacultad;
                horarioClaseEstudiante.CodigoCarrera = consulta.CodigoCarrera;
                horarioClaseEstudiante.NombreCarrera = consulta.NombreCarrera;
                horarioClaseEstudiante.CodigoMateria = consulta.CodigoMeteria;
                horarioClaseEstudiante.NombreMateria = consulta.NombreMateria;
                horarioClaseEstudiante.Aula = consulta.Aula;
                horarioClaseEstudiante.Paralelo = consulta.Paralelo;
                horarioClaseEstudiante.Estudiante = consulta.Estudiante;
                horarioClaseEstudiante.CodigoModulo = consulta.CodigoModulo;
                horarioClaseEstudiante.Dia = consulta.Dia;
                horarioClaseEstudiante.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                horarioClaseEstudiante.HoraFin = consulta.HoraFin.ToString(@"h\:mm");
                horarioClaseEstudiante.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioClaseEstudiante.Periodo = consulta.Periodo;


                listaHorarioEstudiante.Add(horarioClaseEstudiante);
            }

            return listaHorarioEstudiante;
        }

    }
}