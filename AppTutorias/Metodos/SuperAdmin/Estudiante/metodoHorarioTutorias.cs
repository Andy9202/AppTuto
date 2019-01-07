using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Estudiante
{
    public class metodoHorarioTutorias
    {
        conexionDataContext db = new conexionDataContext();


        //ingrersar horario  tutorias

        public void ingresarHorarioTutoria(horarioTutorias horarioTutorias )
        {
            db.IngresarTutoria(int.Parse(horarioTutorias.Matricula),horarioTutorias.CedulaDocente,horarioTutorias.CodigoModulo,horarioTutorias.CodigoMateria,
                horarioTutorias.CodigoPeriodo,horarioTutorias.EstadoObligatorio, DateTime.Parse(horarioTutorias.Fecha),horarioTutorias.Aula);
        }


        //ingrersar horario  tutorias

        public void ingresarHorarioTutoriaDocente(horarioTutorias horarioTutorias, string Cedula)
        {
            try
            {
                db.IngresarTutoria(int.Parse(horarioTutorias.Matricula), Cedula, horarioTutorias.CodigoModulo, horarioTutorias.CodigoMateria,
                    horarioTutorias.CodigoPeriodo, horarioTutorias.EstadoObligatorio, DateTime.Parse(horarioTutorias.Fecha), horarioTutorias.Aula);
            }
            catch
            {
                
            }
        }

        //consutar horarios de clases generales
        public List<horarioTutorias> consultaHorarioTutorias()
        {
            var horariosTutorias = db.horarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
               horarioTutorias horarioTutorias= new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria= consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo +" "+consulta.Dia +" "+ consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }

        //consutar horarios de tutorias del docente
        public List<horarioTutorias> consultaHorarioTutoriasDocente(string Cedula)
        {
            var horariosTutorias = db.horarioTutoriasDocente(Cedula);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:MM/dd/yy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //TUTORIAS MAYOR A LA FECHA ACTUAL


        //consutar horarios de tutorias del docente
        public List<horarioTutorias> consultaHorarioTutoriasDocenteFecha(string Cedula)
        {
            var horariosTutorias = db.horarioTutoriasDocente(Cedula).Where(x=>x.Fecha>=DateTime.Now);

            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //consulta horario tutorias del estudiante

        public List<horarioTutorias> consultaHorarioTutoriasEstudiante(string Matricula)
        {
            var horariosTutorias = db.horarioTutoriasEstudiante(Matricula);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //consulta horario tutorias del estudiante

        public List<horarioTutorias> consultaHorarioTutoriasEstudianteFecha(string Matricula)
        {
            var horariosTutorias = db.horarioTutoriasEstudiante(Matricula).Where(x => x.Fecha >= DateTime.Now);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }

       
    }

}
