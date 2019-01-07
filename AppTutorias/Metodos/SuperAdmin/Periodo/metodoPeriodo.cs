using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Periodo;

namespace AppTutorias.Metodos.SuperAdmin.Periodo
{
    public class metodoPeriodo
    {
        conexionDataContext db = new conexionDataContext();


        //Insertar Periodo

        public void ingresarPeriodo(periodo periodo)
        {
            try
            {
                db.IngresarPeriodoAcademico(periodo.CodigoPeriodo, periodo.NombrePeriodo);
            }
            catch
            {

            }
        }

        //Eliminar Periodo

        public void eliminarPeriodo(string codigoPeriodo)
        {
            try
            {
                db.EliminarPeridoAcademico(codigoPeriodo);
            }
            catch
            {

            }
        }

        // consulta la lista de periodos que existe
        public List<periodo> consultaPeriodos()
        {
            var periodos = from pe in db.PERIODO_ACADEMICO
                           select new
                           {
                               pe.Id_Periodo,
                               pe.Periodo,
                               pe.CodigoPeriodo
                           };

            List<periodo> listaPeriodo = new List<periodo>();

            foreach(var consulta in periodos)
            {
                periodo periodo = new periodo();

                periodo.Id_Periodo = consulta.Id_Periodo;
                periodo.CodigoPeriodo = consulta.CodigoPeriodo;
                periodo.NombrePeriodo = consulta.Periodo;

                listaPeriodo.Add(periodo);
            }


            return listaPeriodo;

        }


    }
}
