using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.Docente;
using AppTutorias.Models.Estudiante;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Docente
{
    public class metodoDocente
    {
        conexionDataContext db = new conexionDataContext();

        //ingresar docente

        public void ingresarDocente(docente docente )
        {
            try
            {
                db.IngresarDocente(docente.Nombre, docente.NombreDos, docente.Apellido, docente.ApellidoDos, docente.Email, docente.Celular,docente.Cedula);
            }
            catch
            {

            }
        }

        public void actualizarDocente(docente docente)
        {
            try
            {
                db.ActualizarDocente(docente.Nombre, docente.NombreDos, docente.Apellido, docente.ApellidoDos, docente.Email, docente.Celular,docente.Cedula);
            }
            catch
            {

            }
        }
        public void eliminarDocente(string email)
        {
            try
            {
                db.EliminarDocente(email);
            }
            catch
            {

            }
        }


        //Conultar Docentes

        public List<docente> consultarDocentes()
        {
            var docentes = (from docen in db.DOCENTE
                      
                             select new
                             {
                                 docen.Nombre,
                                 docen.NombreDos,
                                 docen.Apellido,
                                 docen.ApellidoDos,
                                 docen.Email,
                                 docen.Celular,
                                 docen.Cedula,
                                 docen.Id_Docente
                             }).ToList();

            List<docente> listaDocente = new List<docente>();


            foreach (var consulta in docentes)
            {
                docente docente = new docente();

                docente.Nombre = consulta.Nombre;
                docente.NombreDos = consulta.NombreDos;
                docente.Apellido = consulta.Apellido;
                docente.ApellidoDos = consulta.ApellidoDos;
                docente.Email = consulta.Email;
                docente.Celular = consulta.Celular;
                docente.Cedula = consulta.Cedula;
                docente.Id_Docente = consulta.Id_Docente;

                listaDocente.Add(docente);
            }

            return listaDocente;

        }




        //consultar si existe Docente

        public bool consultaExisteDocente(string cedula)
        {
            bool existe = false;

            var docente = from docen in db.DOCENTE
                           where docen.Cedula==cedula
                           select new
                           {
                               docen.Nombre,
                               docen.NombreDos,
                               docen.Apellido,
                               docen.ApellidoDos,
                               docen.Email,
                               docen.Celular,
                               docen.Cedula
                           };

            if (docente.ToList().Count != 0)
            {
                existe = true;

            }

            return existe;
        }


        //consultar Docente

        public docente consultaDocente(string cedula)
        {


            var doce = from docen in db.DOCENTE
                       where docen.Cedula==cedula
                       select new
                       {
                           docen.Nombre,
                           docen.NombreDos,
                           docen.Apellido,
                           docen.ApellidoDos,
                           docen.Email,
                           docen.Celular,
                           docen.Cedula,
                           docen.Id_Docente
                       };

            docente docente = new docente();

            foreach (var consulta in doce)
            {
                docente.Id_Docente = consulta.Id_Docente;
                docente.Nombre = consulta.Nombre;
                docente.NombreDos = consulta.NombreDos;
                docente.Apellido = consulta.Apellido;
                docente.ApellidoDos = consulta.ApellidoDos;
                docente.Email = consulta.Email;
                docente.Celular = consulta.Celular;
                docente.Cedula = consulta.Cedula;
            }

            return docente;
        }



        //Conultar Docentes

        public List<docente> consultarDocentesMatricula(string Matricula, string CodigoMateria)
        {
            var docentes = db.docentesEstudiante(int.Parse(Matricula),CodigoMateria);

            List<docente> listaDocente = new List<docente>();


            foreach (var consulta in docentes)
            {
                docente docente = new docente();

                docente.Nombre = consulta.Nombre;
                docente.NombreDos = consulta.NombreDos;
                docente.Apellido = consulta.Apellido;
                docente.ApellidoDos = consulta.ApellidoDos;
                docente.Cedula = consulta.Cedula;

                listaDocente.Add(docente);
            }

            return listaDocente;

        }

        //Conultar Docentes

        public List<estudiantesDocente> consultarEstudiantesMateriaDocente(string Cedula, string CodigoMateria)
        {
            var estudiantes = db.estudiantesDocenteMateria(Cedula, CodigoMateria);

            List<estudiantesDocente> listaEstudiante = new List<estudiantesDocente>();


            foreach (var consulta in estudiantes)
            {
                estudiantesDocente estudiante = new estudiantesDocente();

                
                estudiante.Matricula = consulta.Matricula.ToString();
                estudiante.Estudiante = consulta.Estudiante;
                estudiante.EstudianteDatos = consulta.Email + " " + consulta.Celular;
                estudiante.CodigoMateria = consulta.CodigoMeteria;
                estudiante.NombreMateria = consulta.NombreMateria;
                estudiante.Repitencia = consulta.Repitencia.ToString();
                //estudiante.Horario = consulta.CodigoModulo+": "+ consulta.Dia + " " + consulta.HoraInicio.ToString(@"h\:mm") + "-" + consulta.HoraFin.ToString(@"h\:mm");
                //estudiante.Periodo = consulta.CodigoPeriodo + " " + consulta.Periodo;

                listaEstudiante.Add(estudiante);
            }

            return listaEstudiante;

        }


        //CONSULTA docentes por materia

        public List<docente> listaDocentesMateria(string CodigoMateria)
        {

            var docentes = db.consultaDocentesPorMateria(CodigoMateria);

            List<docente> listaDocentes = new List<docente>();


            foreach (var x in docentes.Distinct())
            {
                docente docente = new docente();

                docente.Nombre = x.Docente;
                docente.Cedula = x.Cedula;

                listaDocentes.Add(docente);
            }

            return listaDocentes;
        }


        public List<docente> listaDocentesMateriaFull(string CodigoMateria)
        {
            var docentes = db.consultaDocentesPorMateriaFull(CodigoMateria);

            List<docente> listaDocente = new List<docente>();


            foreach (var consulta in docentes)
            {
                docente docente = new docente();

                docente.Nombre = consulta.Nombre;
                docente.NombreDos = consulta.NombreDos;
                docente.Apellido = consulta.Apellido;
                docente.ApellidoDos = consulta.ApellidoDos;
                docente.Cedula = consulta.Cedula;

                listaDocente.Add(docente);
            }

            return listaDocente;

        }


        //consulta lista de mesnajes de los estudiantes

        public List<Solicitud> consultaMensajesEstudiante(string cedula)
        {

            var consulta = db.consultaMensajesEstudiante(cedula);

            List<Solicitud> listaSolicitud = new List<Solicitud>();

            foreach(var x in consulta)
            {
                Solicitud solicitud = new Solicitud();

                solicitud.Id_Solicitud = x.Id_Notifacion;
                solicitud.Estudiante = x.Estudiante;
                solicitud.CodigoMateria = x.CodigoMeteria;
                solicitud.NombreMateria = x.NombreMateria;
                solicitud.CodigoModulo = x.CodigoModulo;
                solicitud.Hora = x.HoraInicio.ToString(@"h\:mm") +" - " + x.HoraFin.ToString(@"h\:mm");
                solicitud.Matricula = x.Matricula.ToString();
                solicitud.Mensaje = x.Mensaje;
                solicitud.Confirmar = x.Confirmar;
                solicitud.Fecha = x.Fecha;

                listaSolicitud.Add(solicitud);
            }

            return listaSolicitud;
        }

        //confirma solicitudes del estudiante

        public void confirmaSolictudEstudiante(string Id_Notificacion,string Confirma)
        {
            try
            {
                db.confirmarTutoriaSolicitud(int.Parse(Id_Notificacion), Confirma);
            }
            catch
            {

            }
        }


        public List<docente> consultaDocenteParalelo(string CodigoMateria, string Paralelo)
        {
            var horarioDocente = db.consultaDocentesPorParaleloFull(CodigoMateria, Paralelo);

            List<docente> listaDocente = new List<docente>();

            foreach (var consulta in horarioDocente)
            {
                docente docente = new docente();

                docente.Nombre = consulta.Nombre;
                docente.NombreDos = consulta.NombreDos;
                docente.Apellido = consulta.Apellido;
                docente.ApellidoDos = consulta.ApellidoDos;
                docente.Cedula = consulta.Cedula;

                listaDocente.Add(docente);
            }

            return listaDocente;

        }


        public List<horarioClaseDocente> consultaParaleloMateriaDocente(string CodigoMateria, string CedulaDocente)
        {
            var horarioDocente = db.consultaParaleloMateriaDocente(CodigoMateria,CedulaDocente);

            List<horarioClaseDocente> listaParalelo = new List<horarioClaseDocente>();

            foreach (var consulta in horarioDocente)
            {
                horarioClaseDocente horarioClaseDocente= new horarioClaseDocente();

                horarioClaseDocente.Docente = consulta.Nombre+' '+consulta.NombreDos+' '+  consulta.Apellido + ' ' + consulta.ApellidoDos;
                horarioClaseDocente.Paralelo = consulta.Paralelo;
                horarioClaseDocente.CodigoMateria = consulta.CodigoMeteria;
                horarioClaseDocente.NombreMateria = consulta.NombreMateria;

                listaParalelo.Add(horarioClaseDocente);
            }

            return listaParalelo;

        }

        public List<docente> consultaDocenteCoincidencia(string Palabra)
        {
            var horarioDocente = db.consultaDocentesCoicidencias(Palabra);

            List<docente> listaDocente = new List<docente>();

            foreach (var consulta in horarioDocente)
            {
                docente docente = new docente();

                docente.Nombre = consulta.Docente; 
                docente.Cedula = consulta.cedula;

                listaDocente.Add(docente);
            }

            return listaDocente;

        }

    }
}
