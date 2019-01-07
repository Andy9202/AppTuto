using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.Estudiante;
using AppTutorias.Models.SuperAdmin.Materia;

namespace AppTutorias.Metodos.Estudiante
{
    public class metodoEstudianteClass
    {

        conexionDataContext db = new conexionDataContext();


        //consulta los datos del estudiante logueado
        public estudiante datosEstudiante(string Email)
        {
            var estu = db.consultaDatosEstudiante(Email);

            estudiante estudiante = new estudiante();

            foreach(var x in estu)
            {
                estudiante.Nombre = x.Nombre;
                estudiante.NombreDos = x.NombreDos;
                estudiante.Apellido = x.Apellido;
                estudiante.ApellidoDos = x.ApellidoDos;
                estudiante.Celular = x.Celular;
                estudiante.Matricula = x.Matricula;
                estudiante.NombreCarrera = x.NombreCarrera;
                estudiante.Email = Email;

            }

            return estudiante;

        }

        //consulta las materias del estudiante actual
        public List<materia> materiasEstudiante(string matricula)
        {
            var consultaMaterias = db.materiasEstudiante(int.Parse(matricula));

            List<materia> listaMateria = new List<materia>();

            foreach (var x in consultaMaterias)
            {
                materia materia = new materia();

                materia.CodigoMateria = x.CodigoMeteria;
                materia.NombreMateria = x.NombreMateria;

                
                listaMateria.Add(materia);
            }


            return listaMateria;
        }

        //ingresar notificaciones del estudiante 

        public void ingresoNotificacion(Solicitud solicitud)
        {
            db.ingresarNotificacionEstudiante(solicitud.Cedula, int.Parse(solicitud.Matricula), solicitud.CodigoMateria, solicitud.CodigoModulo, solicitud.Mensaje,solicitud.Fecha);
        }

     

    }
}