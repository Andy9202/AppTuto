using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Materia;

namespace AppTutorias.Metodos.SuperAdmin.Materia
{
    public class metodoMateria
    {


        conexionDataContext db = new conexionDataContext();


        //Insertar Materia

        public void ingresarMateria (materia materia)
        {
            try
            {
                db.IngresarMateria(materia.CodigoCarrera,materia.NombreMateria, materia.CodigoMateria);
            }
            catch
            {

            }
        }
        //Actualizar  Materias

        public void actualizarMateria(materia materia)
        {
            try
            {
                db.ActualizarMateria(materia.CodigoMateria, materia.NombreMateria);
            }
            catch
            {

            }
        }
        //Eliminar  Carrera


        public void eliminarMateria(string codigoMateria)
        {
            try
            {
                db.EliminarMateria(codigoMateria);
            }
            catch
            {

            }
        }

        //Conultar Materias

        public List<materia> consultarMaterias()
        {
            var materias = from facult in db.FACULTAD
                           from carr in db.CARRERA
                           from mate in db.MATERIA
                           where facult.Id_Facultad == carr.Id_Facultad
                           where mate.Id_Carrera == carr.Id_Carrera
                           select new
                           {

                               facult.CodigoFacultad,
                               carr.CodigoCarrera,
                               mate.NombreMateria,
                               mate.CodigoMeteria
                           };

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();


                materia.CodigoFacultad = consulta.CodigoFacultad;
                materia.CodigoCarrera = consulta.CodigoCarrera;
                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;


                listaMateria.Add(materia);
            }

            return listaMateria;

        }

        //consultar si existe  Materia

        public bool consultaExisteMateria(string codigoMateria)
        {
            bool existe = false;

            var Materia = from x in db.MATERIA
                          where x.CodigoMeteria == codigoMateria
                          select new
                          {
                              x.NombreMateria,
                              x.CodigoMeteria

                          };

            if (Materia.ToList().Count != 0)
            {
                existe = true;

            }

            return existe;
        }

        //consultar Materia

        public materia consultaMateria(string codigoMateria)
        {


            var materi = from carr in db.CARRERA
                         from mate in db.MATERIA
                         where mate.Id_Carrera == carr.Id_Carrera
                         where mate.CodigoMeteria == codigoMateria
                         select new
                         {

                             carr.CodigoCarrera,
                             mate.NombreMateria,
                             mate.CodigoMeteria
                         };

            materia materia = new materia();

            foreach (var consulta in materi)
            {

                materia.CodigoCarrera = consulta.CodigoCarrera;
                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;
            }

            return materia;
        }


        //Conultar Materias

        public List<materia> consultarMateriasCarrera(string CodigoCarrera)
        {
            var materias = from facult in db.FACULTAD
                           from carr in db.CARRERA
                           from mate in db.MATERIA
                           where facult.Id_Facultad == carr.Id_Facultad
                           where mate.Id_Carrera == carr.Id_Carrera
                           where carr.CodigoCarrera == CodigoCarrera
                           select new
                           {

                               facult.CodigoFacultad,
                               carr.CodigoCarrera,
                               mate.NombreMateria,
                               mate.CodigoMeteria
                           };

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();


                materia.CodigoFacultad = consulta.CodigoFacultad;
                materia.CodigoCarrera = consulta.CodigoCarrera;
                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;


                listaMateria.Add(materia);
            }

            return listaMateria;

        }

        //Conultar Materias

        public List<materia> consultarMateriasFacultad(string CodigoFacultad)
        {
            var materias = from mate in db.MATERIA
                           from facu in db.FACULTAD
                           from carr in db.CARRERA
                           where facu.Id_Facultad == carr.Id_Facultad
                           where mate.Id_Carrera == mate.Id_Carrera
                           where facu.CodigoFacultad == CodigoFacultad

                           select new
                           {
                               carr.CodigoCarrera,
                               facu.CodigoFacultad,
                               mate.NombreMateria,
                               mate.CodigoMeteria
                           };

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();

                materia.CodigoCarrera = consulta.CodigoCarrera;
                materia.CodigoFacultad = consulta.CodigoFacultad;
                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;


                listaMateria.Add(materia);
            }

            return listaMateria;

        }


        //Conultar Materias estudiante

        public List<materia> consultarMateriasMatricula(string Matricula)
        {
            var materias= db.materiasEstudiante(int.Parse(Matricula));

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();


               
                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;
    

                listaMateria.Add(materia);
            }

            return listaMateria;

        }


        //consulta materias del docente
        //Conultar Docentes

        public List<materia> consultarMateriasDocente(string Cedula)
        {
            var materias = db.materiasDocente(Cedula);

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();

                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;
   
                listaMateria.Add(materia);
            }

            return listaMateria;

        }



        //Conultar Materias estudiante

        public List<materia> consultarMateriasEstudianteDocente(string Matricula,string Cedula)
        {
            if(Matricula == "" || Matricula == null){
                Matricula = "0";
            } 


            var materias = db.materiasEstudianteDocente(int.Parse(Matricula),Cedula);

            List<materia> listaMateria = new List<materia>();


            foreach (var consulta in materias)
            {
                materia materia = new materia();

                materia.NombreMateria = consulta.NombreMateria;
                materia.CodigoMateria = consulta.CodigoMeteria;

                listaMateria.Add(materia);
            }

            return listaMateria;

        }

    }
}
