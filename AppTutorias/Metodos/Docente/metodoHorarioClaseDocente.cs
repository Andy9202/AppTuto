using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.Docente;

namespace AppTutorias.Metodos.Docente
{
    public class metodoHorarioClaseDocente
    {
        conexionDataContext db = new conexionDataContext();

        //consultar horario clase docente

        public List<horarioClaseDocente> consultaHorariosDocenteCedula(string Cedula)
        {
            var horarioDocente = db.horarioDocenteCedula(Cedula);

            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            foreach (var consulta in horarioDocente)
            {
                horarioClaseDocente horarioClaseDocente = new horarioClaseDocente();

                horarioClaseDocente.CodigoFacultad = consulta.CodigoFacultad;
                horarioClaseDocente.NombreFacultad = consulta.NombreFacultad;
                horarioClaseDocente.CodigoCarrera = consulta.CodigoCarrera;
                horarioClaseDocente.NombreCarrera = consulta.NombreCarrera;
                horarioClaseDocente.CodigoMateria = consulta.CodigoMeteria;
                horarioClaseDocente.NombreMateria = consulta.NombreMateria;
                horarioClaseDocente.Aula = consulta.Aula;
                horarioClaseDocente.Paralelo = consulta.Paralelo;
                horarioClaseDocente.Docente = consulta.Nombre + " " + consulta.NombreDos + " " + consulta.Apellido + " " + consulta.ApellidoDos;
                horarioClaseDocente.Dia = consulta.Dia;
                horarioClaseDocente.CodigoModulo = consulta.CodigoModulo;
                horarioClaseDocente.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                horarioClaseDocente.HoraFin = consulta.HoraFin.ToString(@"h\:mm");
                horarioClaseDocente.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioClaseDocente.Periodo = consulta.Periodo;
            

                listaHorarioDocente.Add(horarioClaseDocente);
            }

            return listaHorarioDocente;
        }

        public List<horarioClaseDocente> consultaHorarioLibreDocente(string Cedula)
        {
            var horarioDocente = db.conusltaHorarioLibreDocente(Cedula);

            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            foreach (var consulta in horarioDocente)
            {
                horarioClaseDocente horarioClaseDocente = new horarioClaseDocente();

          
                horarioClaseDocente.Aula = consulta.Descripcion;
                horarioClaseDocente.Docente = consulta.Nombre + " " + consulta.NombreDos + " " + consulta.Apellido + " " + consulta.ApellidoDos;
                horarioClaseDocente.Dia = consulta.Dia;
                horarioClaseDocente.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioClaseDocente.Periodo = consulta.Periodo;
                horarioClaseDocente.CodigoModulo = consulta.CodigoModulo;
                horarioClaseDocente.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                horarioClaseDocente.HoraFin = consulta.HoraFin.ToString(@"h\:mm");
    


                listaHorarioDocente.Add(horarioClaseDocente);
            }

            return listaHorarioDocente;
        }

        public List<horarioClaseDocente> consultaHorarioObligatorioDocente(string Cedula)
        {
            var horarioDocente = db.conusltaHorarioObligatorioDocente(Cedula);

            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            foreach (var consulta in horarioDocente)
            {
                horarioClaseDocente horarioClaseDocente = new horarioClaseDocente();


                horarioClaseDocente.Aula = consulta.Descripcion;
                horarioClaseDocente.Docente = consulta.Nombre + " " + consulta.NombreDos + " " + consulta.Apellido + " " + consulta.ApellidoDos;
                horarioClaseDocente.Dia = consulta.Dia;
                horarioClaseDocente.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioClaseDocente.Periodo = consulta.Periodo;
                horarioClaseDocente.CodigoModulo = consulta.CodigoModulo;
                horarioClaseDocente.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                horarioClaseDocente.HoraFin = consulta.HoraFin.ToString(@"h\:mm");



                listaHorarioDocente.Add(horarioClaseDocente);
            }

            return listaHorarioDocente;
        }

        //

        public List<horarioClaseDocente> listaParalelosMateria(string CodigoMateria)
        {
            var horarioDocente = db.consultaParalelosMateria(CodigoMateria);

            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            foreach (var consulta in horarioDocente)
            {
                horarioClaseDocente horarioClaseDocente = new horarioClaseDocente();
                
                horarioClaseDocente.CodigoMateria = consulta.CodigoMeteria;
                horarioClaseDocente.NombreMateria = consulta.NombreMateria;
                horarioClaseDocente.Paralelo = consulta.Paralelo;
           

                listaHorarioDocente.Add(horarioClaseDocente);
            }

            return listaHorarioDocente;

        }



    }
}
