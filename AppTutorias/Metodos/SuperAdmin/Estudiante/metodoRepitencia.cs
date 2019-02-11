using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos.SuperAdmin.Estudiante
{
    public class metodoRepitencia
    {
        conexionDataContext db = new conexionDataContext();


        //ingresar repitencia
        public void ingresarRepitencia(repitencia repitencia  )
        {
            db.IngresarRepitencia(repitencia.Matricula, repitencia.CodigoMateria,  repitencia.Repitencia);
        }
        public void eliminarRepitencia(repitencia repitencia)
        {
            db.EliminarRepitencia(repitencia.Matricula, repitencia.CodigoMateria);
          
        }


        //lista Repitencia

        public List<repitencia> consultaRepitencias()
        {
            var repiten = from rep in db.REPITENCIA
                             from mate in db.MATERIA
                             from materia in db.MATERIA
                             from estu in db.ESTUDIANTE
                             where rep.Id_Estudiante == estu.Id_Estudiante
                             where rep.Id_Materia== mate.Id_Materia
                              where rep.Id_Materia== mate.Id_Materia
                          where materia.Id_Materia == mate.Id_Materia
                          select new
                             {
                                 estu.Nombre,
                                 estu.NombreDos,
                                 estu.Apellido,
                                 estu.ApellidoDos,
                                 estu.Matricula,
                                 materia.CodigoMeteria,
                                 materia.NombreMateria,
                                 rep.Repitencia
                             };

            List<repitencia> listaRepitencia = new List<repitencia>();

            foreach(var consulta in repiten)
            {
                repitencia repitencia = new repitencia();

                repitencia.NombreEstudiante = consulta.Nombre + " " + consulta.NombreDos + " " + consulta.Apellido + " " + consulta.ApellidoDos;
                repitencia.Matricula = consulta.Matricula;
                repitencia.CodigoMateria = consulta.CodigoMeteria;
                repitencia.Materia = consulta.NombreMateria;
                repitencia.Repitencia = consulta.Repitencia;

                listaRepitencia.Add(repitencia);

            }

            return listaRepitencia;

        }


    }
}
