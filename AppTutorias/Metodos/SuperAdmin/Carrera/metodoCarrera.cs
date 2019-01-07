using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Carrera;

namespace AppTutorias.Metodos.SuperAdmin.Carrera
{
    public class metodoCarrera
    {
        conexionDataContext db = new conexionDataContext();


        //Insertar Carrera

        public void ingresarCarrera(carrera carrera)
        {
            try
            {
                db.IngresarCarrera(carrera.CodigoFacultad,carrera.NombreCarrera,carrera.CodigoCarrera);
            }
            catch
            {

            }
        }


        //Actualizar  Carrera

        public void actualizarCarrera(carrera carrera)
        {
            try
            {
                db.ActualizarCarrera(carrera.CodigoCarrera, carrera.NombreCarrera);
            }
            catch
            {

            }
        }

        //Eliminar  Carrera


        public void eliminarCarrera(string codigoCarrera)
        {
            try
            {
                db.EliminarCarrera(codigoCarrera);
            }
            catch
            {

            }
        }

        //Conultar  Carrera

        public List<carrera> consultarCarreras()
        {
            var carreras = from facult in db.FACULTAD
                           from carr in db.CARRERA
                           where facult.Id_Facultad == carr.Id_Facultad
                           select new
                             {
                                 facult.CodigoFacultad,
                                 carr.NombreCarrera,
                                 carr.CodigoCarrera
                             };

            List<carrera> listaCarrera = new List<carrera>();


            foreach (var consulta in carreras)
            {
                carrera carrera= new carrera();

                carrera.CodigoFacultad = consulta.CodigoFacultad;
                carrera.NombreCarrera = consulta.NombreCarrera;
                carrera.CodigoCarrera = consulta.CodigoCarrera;

                listaCarrera.Add(carrera);
            }

            return listaCarrera;

        }


        //consultar si existe  Carrera

        public bool consultaExisteCarrera(string codigoCarrera)
        {
            bool existe = false;

            var Carrera = from x in db.CARRERA
                           where x.CodigoCarrera == codigoCarrera
                           select new
                           {
                               x.CodigoCarrera,
                               x.NombreCarrera
                               
                           };

            if (Carrera.ToList().Count != 0)
            {
                existe = true;

            }

            return existe;
        }

        //consultar Carrera

        public carrera consultaCarrera(string codigoCarrera)
        {


            var carre = from facult in db.FACULTAD
                        from carr in db.CARRERA
                        where facult.Id_Facultad == carr.Id_Facultad
                        where carr.CodigoCarrera == codigoCarrera 
                        select new
                        {
                            facult.CodigoFacultad,
                            carr.NombreCarrera,
                            carr.CodigoCarrera
                        };

            carrera carrera = new carrera();

            foreach (var consulta in carre)
            {

                carrera.CodigoFacultad = consulta.CodigoFacultad;
                carrera.NombreCarrera = consulta.NombreCarrera;
                carrera.CodigoCarrera = consulta.CodigoCarrera;
            }

            return carrera;
        }

    }
}
