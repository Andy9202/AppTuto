using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Estudiante
{
    public class metodoEstudiante
    {
        conexionDataContext db = new conexionDataContext();

        //ingresar Estudiante

        public void ingresarEstudiante(estudiante estudiante)
        {
            try
            {
                db.IngresarEstudiante(estudiante.Nombre,estudiante.NombreDos,estudiante.Apellido,estudiante.ApellidoDos,estudiante.Email,estudiante.Celular,
                    estudiante.CodCarrera,int.Parse(estudiante.Matricula));
            }
            catch
            {

            }
        }

        public void actualizarEstudiante(estudiante estudiante)
        {
            try
            {
                db.ActualizarEstudiante(estudiante.Nombre, estudiante.NombreDos, estudiante.Apellido, estudiante.ApellidoDos, estudiante.Celular,estudiante.Email, int.Parse(estudiante.Matricula));

            }
            catch
            {

            }
        }
        public void eliminarEstudiante(int matricula)
        {
            try
            {
                db.EliminarEstudiante(matricula);
            }
            catch
            {

            }
        }


        //Conultar Estudiantes

        public List<estudiante> consultarEtudiantes()
        {
            var estudiantes = from estu in db.ESTUDIANTE
                              from carre in db.CARRERA
                              from facu in db.FACULTAD
                              where estu.Id_Carrera == carre.Id_Carrera
                              where carre.Id_Facultad == facu.Id_Facultad

                           select new
                           {
                               estu.Nombre,
                               estu.NombreDos,
                               estu.Apellido,
                               estu.ApellidoDos,
                               estu.Email,
                               estu.Celular,
                               estu.Matricula,
                               carre.CodigoCarrera,
                               facu.CodigoFacultad
                           };

            List<estudiante> listaEstudiante = new List<estudiante>();


            foreach (var consulta in estudiantes)
            {
                estudiante estudiante = new estudiante ();

                estudiante.Nombre = consulta.Nombre;
                estudiante.NombreDos = consulta.NombreDos;
                estudiante.Apellido = consulta.Apellido;
                estudiante.ApellidoDos = consulta.ApellidoDos;
                estudiante.Email = consulta.Email;
                estudiante.Celular = consulta.Celular;
                estudiante.Matricula = consulta.Matricula.ToString();
                estudiante.CodCarrera = consulta.CodigoCarrera;
                estudiante.CodFacultad = consulta.CodigoFacultad;

                listaEstudiante.Add(estudiante);
            }

            return listaEstudiante;

        }

        //Conultar Estudiantes por carrera

        public List<estudiante> consultarEstudiantesCarrera(string CodigoCarrera)
        {
            var estudiantes = from estu in db.ESTUDIANTE
                              from carre in db.CARRERA
                              from facu in db.FACULTAD
                              where estu.Id_Carrera == carre.Id_Carrera
                              where carre.Id_Facultad == facu.Id_Facultad
                              where carre.CodigoCarrera == CodigoCarrera

                              select new
                              {
                                  estu.Nombre,
                                  estu.NombreDos,
                                  estu.Apellido,
                                  estu.ApellidoDos,
                                  estu.Email,
                                  estu.Celular,
                                  estu.Matricula,
                                  carre.CodigoCarrera,
                                  facu.CodigoFacultad
                              };

            List<estudiante> listaEstudiante = new List<estudiante>();


            foreach (var consulta in estudiantes)
            {
                estudiante estudiante = new estudiante();

                estudiante.Nombre = consulta.Nombre;
                estudiante.NombreDos = consulta.NombreDos;
                estudiante.Apellido = consulta.Apellido;
                estudiante.ApellidoDos = consulta.ApellidoDos;
                estudiante.Email = consulta.Email;
                estudiante.Celular = consulta.Celular;
                estudiante.Matricula = consulta.Matricula.ToString();
                estudiante.CodCarrera = consulta.CodigoCarrera;
                estudiante.CodFacultad = consulta.CodigoFacultad;

                listaEstudiante.Add(estudiante);
            }

            return listaEstudiante;

        }



        //consultar si existe Estudiante

        public bool consultaExisteEstudiante(int matricula)
        {
            bool existe = false;

            var estudiante = from estu in db.ESTUDIANTE
                          where estu.Matricula == matricula
                          select new
                          {
                              estu.Nombre,
                              estu.NombreDos,
                              estu.Apellido,
                              estu.ApellidoDos,
                              estu.Email,
                              estu.Celular,
                              estu.Matricula
                          };

            if (estudiante.ToList().Count != 0)
            {
                existe = true;

            }

            return existe;
        }


        //consultar estudiante

        public estudiante consultaEstudiante(int matricula)
        {


            var estud = from estu in db.ESTUDIANTE
                       where estu.Matricula == matricula
                       select new
                       {
                           estu.Nombre,
                           estu.NombreDos,
                           estu.Apellido,
                           estu.ApellidoDos,
                           estu.Celular,
                           estu.Matricula,
                           estu.Email
                       };

            estudiante estudiante = new estudiante();

            foreach (var consulta in estud)
            {
                estudiante.Nombre = consulta.Nombre;
                estudiante.NombreDos = consulta.NombreDos;
                estudiante.Apellido = consulta.Apellido;
                estudiante.ApellidoDos = consulta.ApellidoDos;
                estudiante.Celular = consulta.Celular;
                estudiante.Matricula = consulta.Matricula.ToString();
                estudiante.Email = consulta.Email;

            }

            return estudiante;
        }


    }
}
