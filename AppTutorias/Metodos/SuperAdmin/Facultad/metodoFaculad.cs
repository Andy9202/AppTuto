using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin;

namespace AppTutorias.Metodos.SuperAdmin
{
    public class metodoFacultad
    {
        conexionDataContext db = new conexionDataContext();


        //Insertar Facultad

        public void ingresarFacultad(facultad facultad)
        {
            try
            {
                db.IngresarFacultad(facultad.NombreFacultad, facultad.CodigoFacultad);
            }
            catch
            {

            }
        }



        //Actualizar Facultad

        public void actualizarFacultad(facultad facultad)
        {
            try
            {
                db.ActualizarFacultad(facultad.CodigoFacultad, facultad.NombreFacultad);
            }
            catch
            {

            }
        }

        //Eliminar Facultad


        public void eliminarFacultad(string codigoFacultad)
        {
            try
            {
                db.EliminarFacultad(codigoFacultad);
            }
            catch
            {

            }
        }

        //Conultar Facultades

        public List<facultad> consultarFacultades()
        {
            var facultades = from facult in db.FACULTAD
                             select new
                             {
                                 facult.Id_Facultad,
                                 facult.CodigoFacultad,
                                 facult.NombreFacultad
                             };

            List<facultad> listaFacultad = new List<facultad>();
            

            foreach (var consulta in facultades)
            {
                facultad facultad = new facultad();

                facultad.Id_Facultad = consulta.Id_Facultad;
                facultad.NombreFacultad = consulta.NombreFacultad;
                facultad.CodigoFacultad = consulta.CodigoFacultad;

                listaFacultad.Add(facultad);
            }

            return listaFacultad;

        }


        //consultar si existe facultad

        public bool consultaExisteFacultad(string codigoFacultad)
        {
            bool existe = false;

            var facultad = from x in db.FACULTAD
                           where x.CodigoFacultad == codigoFacultad
                           select new
                           {
                               x.Id_Facultad,
                               x.NombreFacultad,
                               x.CodigoFacultad
                           };

            if(facultad.ToList().Count !=0)
            {
                existe = true;

            }

            return existe;
        }

        //consultar Facltad

        public facultad consultaFacultad(string codigoFacultad)
        {
      

            var facult = from x in db.FACULTAD
                           where x.CodigoFacultad == codigoFacultad
                           select new
                           {
                               x.Id_Facultad,
                               x.NombreFacultad,
                               x.CodigoFacultad
                           };

            facultad facultad = new facultad();

            foreach(var consulta in facult)
            {
                facultad.Id_Facultad = consulta.Id_Facultad;
                facultad.CodigoFacultad = consulta.CodigoFacultad;
                facultad.NombreFacultad = consulta.NombreFacultad;
            }

            return facultad;
        }

    }
}
