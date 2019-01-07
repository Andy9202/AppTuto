using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.HorarioClases;

namespace AppTutorias.Metodos.SuperAdmin.HorarioClases
{
    public class metodoHorarioClases
    {
        conexionDataContext db = new conexionDataContext();


        //ingrersar horario

            public void ingresarHorarioClases(horarioClases horarioClases)
        {
            db.IngresarHorarioClase(horarioClases.CodigoMateria, horarioClases.CedulaDocente, horarioClases.Modulo,horarioClases.Periodo,horarioClases.Aula,horarioClases.Paralelo);
        }

        //elminar horario clases

        public void eliminarHorarioClases(horarioClases horarioClases)
        {
            db.EliminarHorarioClase(horarioClases.CedulaDocente, horarioClases.CodigoMateria);
        }


        //consutar horarios de clases generales
        public List<horarioClases> consultaHorarioClases()
        {
            var horariosclases = db.horarioClaseDocentes();

            List<horarioClases> listaHorariosClases = new List<horarioClases>();
            foreach (var consulta in horariosclases)
            {
                horarioClases horarioClases = new horarioClases();

                horarioClases.NombreDocente = consulta.Nombre+" "+ consulta.NombreDos+" "+consulta.Apellido + " " + consulta.ApellidoDos;
                horarioClases.CedulaDocente = consulta.Cedula;
                horarioClases.Email = consulta.Email;
                horarioClases.Celular = consulta.Celular;
                horarioClases.Materia = consulta.CodigoMeteria + " " + consulta.NombreMateria;
                horarioClases.Modulo= consulta.CodigoModulo;
                horarioClases.Periodo =  consulta.Periodo;
                horarioClases.Aula = consulta.Aula;
                horarioClases.Paralelo = consulta.Paralelo;
                

                listaHorariosClases.Add(horarioClases);
            }

            return listaHorariosClases;
        }
    }
}
