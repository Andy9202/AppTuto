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
            var horariosTutorias = db.horarioTutoriasFull();
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
            var horariosTutorias = db.horarioTutoriasDocente(Cedula).Where(x=>x.Fecha.Date>=DateTime.Now.Date);

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
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //consulta turorias del docente por asistencia


        //consutar horarios de tutorias del docente
        public List<horarioTutorias> consultaHorarioTutoriasDocenteAsistencia(string Cedula,string Asistencia)
        {
            var horariosTutorias = db.horarioTutoriasDocente(Cedula).Where(x => x.Asistencia == Asistencia);

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


        //consulta horario de tutorias del docente por periodo
        //consutar horarios de tutorias del docente
        public List<horarioTutorias> consultaHorarioTutoriasDocentePeriodo(string Cedula, string Periodo)
        {
            var horariosTutorias = db.horarioTutoriasDocente(Cedula).Where(x => x.CodigoPeriodo == Periodo);

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

       
    

        //consulta horario tutorias del estudiante

        public List<horarioTutorias> consultaHorarioTutoriasEstudiantePeriodo(string Matricula,string CodigoPeriodo)
        {
            var horariosTutorias = db.horarioTutoriasEstudiante(Matricula).Where(x => x.CodigoPeriodo == CodigoPeriodo);
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

        public List<horarioTutorias> consultaHorarioTutoriasEstudianteAsistencia(string Matricula, string Asistencia)
        {
            var horariosTutorias = db.horarioTutoriasEstudiante(Matricula).Where(x => x.Asistencia == Asistencia);
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


        //auper admin
        //consulta horario tutorias por carrera

        public List<horarioTutorias> consultaHorarioTutoriasCarrera(string CodigoCarrera)
        {
            var horariosTutorias = db.horarioTutoriasFull().Where(x => x.CodigoCarrera==CodigoCarrera);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }



        //consulta horario tutorias por carrera

        public List<horarioTutorias> consultaHorarioTutoriasFacultad(string CodigoFacultad)
        {
            var horariosTutorias = db.horarioTutoriasFull().Where(x => x.CodigoFacultad == CodigoFacultad);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }

        //asistencia tutorias


        public List<horarioTutorias> consultaHorarioTutoriasAsistencia(string Asistencia)
        {
            var horariosTutorias = db.horarioTutoriasFull().Where(x => x.Asistencia==Asistencia);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.CodigoCarrera= consulta.CodigoCarrera;
                horarioTutorias.NombreCarrera = consulta.NombreCarrera;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }

        //total de tutorias

        public List<horarioTutorias> consultaHorarioTutoriasTotal()
        {
            var horariosTutorias = db.horarioTutoriasFull();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreCarrera = consulta.NombreCarrera;
                horarioTutorias.CodigoCarrera = consulta.CodigoCarrera;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //tutorias de tercera matricula por materia

        public List<horarioTutorias> consultaHorarioTutoriasRepitenciaMateria (string CodigoMateria,string Repitencia)
        {
            var horariosTutorias = db.horarioTutoriasFull().Where(x => x.CodigoMeteria == CodigoMateria && x.Repitencia == Char.Parse(Repitencia));
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }


        //tutorias  por materia

        public List<horarioTutorias> consultaHorarioTutoriasMateria(string CodigoMateria)
        {
            var horariosTutorias = db.horarioTutoriasFull().Where(x => x.CodigoMeteria == CodigoMateria);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            foreach (var consulta in horariosTutorias)
            {
                horarioTutorias horarioTutorias = new horarioTutorias();

                horarioTutorias.EstadoObligatorio = consulta.EstadoObligatorio;
                horarioTutorias.Id_HorarioTutorias = consulta.Id_HorioTutorias;
                horarioTutorias.Matricula = consulta.Matricula.ToString();
                horarioTutorias.NombreDocente = consulta.Docente;
                horarioTutorias.CedulaDocente = consulta.Cedula;
                horarioTutorias.NombreFacultad = consulta.NombreFacultad;
                horarioTutorias.CodigoFacultad = consulta.CodigoFacultad;
                horarioTutorias.NombreEstudiante = consulta.Estudiante;
                horarioTutorias.CodigoMateria = consulta.CodigoMeteria;
                horarioTutorias.NombreMateria = consulta.NombreMateria;
                horarioTutorias.Modulo = consulta.CodigoModulo + " " + consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + '-' + consulta.HoraFin.ToString(@"h\:mm");
                horarioTutorias.CodigoPeriodo = consulta.CodigoPeriodo;
                horarioTutorias.Fecha = String.Format("{0:dd/MM/yyyy}", consulta.Fecha);
                horarioTutorias.Aula = consulta.Aula;
                horarioTutorias.Asistencia = consulta.Asistencia;
                horarioTutorias.Repitencia = consulta.Repitencia.ToString();

                listaHorariosTutorias.Add(horarioTutorias);
            }

            return listaHorariosTutorias;
        }

    }

}
