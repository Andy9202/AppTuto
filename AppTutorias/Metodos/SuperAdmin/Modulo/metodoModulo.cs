using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models.SuperAdmin.Modulo;

namespace AppTutorias.Metodos.SuperAdmin.Modulo
{
    public class metodoModulo
    {
        conexionDataContext db = new conexionDataContext();


        //Insertar Modulo

        public void ingresarModulo(modulo modulo )
        {
            try
            {
                db.IngresarModulo(modulo.CodigoModulo,TimeSpan.Parse(modulo.HoraInicio),TimeSpan.Parse(modulo.HoraFin),modulo.Dia);
            }
            catch
            {

            }
        }

        //Eliminar modulo

        public void eliminarModulo(string CodigoModulo)
        {
            try
            {
                db.EliminarModulo(CodigoModulo);
            }
            catch
            {

            }
        }

        //consulta la lista de modulos qu existe
        public List<modulo> consultaModulos()
        {
            var modulos= (from mo in db.MODULO
                           select new
                           {
                               mo.Id_Modulo,
                               mo.HoraInicio,
                               mo.HoraFin,
                               mo.CodigoModulo,
                               mo.Dia
                           }).OrderBy(x=>x.CodigoModulo); 

            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
               modulo modulo = new modulo();

                modulo.Id_Modulo = consulta.Id_Modulo;
                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");

                listaModulo.Add(modulo);
            }


            return listaModulo;

        }


        //modulos del docente disponible

        public List<modulo> consultaModulosDocente(string Cedula)
        {
            var modulos = db.modulosDocenteDisponible(Cedula);

            
            
            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");

                listaModulo.Add(modulo);
            }


            return listaModulo;

        }

        //modulos del docente disponible

        public List<modulo> consultaModulosDocenteAsignacion(string Cedula)
        {
            var modulos = db.modulosDocenteDisponibleAsignacion(Cedula);



            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");

                listaModulo.Add(modulo);
            }


            return listaModulo;

        }

        ///modulos del estudiante y docente disponibles
        ///
        public List<modulo> consultaModulosDocenteEstudiante(string Cedula,string Matricula)
        {
            var modulos = db.modulosDocenteDisponible(Cedula);
            var modulosEstudainte = db.horarioClaseEstudianteMatricula(int.Parse(Matricula));

            //estudiante
            List<modulo> listaModuloEstudiante = new List<modulo>();

            foreach (var x in modulosEstudainte)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = x.CodigoModulo;
                modulo.Dia = x.Dia;
                modulo.HoraInicio = x.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = x.HoraFin.ToString(@"h\:mm");

                listaModuloEstudiante.Add(modulo);
            }


            //docnete

            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");


            
                        listaModulo.Add(modulo);

            }

            var l2 = listaModuloEstudiante.Select(x=>x.CodigoModulo);
            IEnumerable<modulo> clasifica = listaModulo.Where(x => !l2.Contains(x.CodigoModulo));


            return clasifica.ToList();

        }

        public void ingresarHorasLibres(string cedula, string codigoModulo)
        {
            try
            {
                db.IngresarHoraLibre(cedula, codigoModulo);


            }
            catch
            {

            }
        }

        public void eliminarHorasLibres(string cedula, string codigoModulo)
        {
            try
            {
                db.EliminarHoraLibre(cedula, codigoModulo);


            }
            catch
            {

            }
        }

        public void ingresarHorasObligatorias(string cedula, string codigoModulo)
        {
            try
            {
                db.IngresarHoraObligatoria(cedula, codigoModulo);


            }
            catch
            {

            }
        }

        public void eliminarHorasObligatorias(string cedula, string codigoModulo)
        {
            try
            {
                db.EliminarHoraObligatoria(cedula, codigoModulo);


            }
            catch
            {

            }
        }

        //modulos del docente libres

        public List<modulo> consultaHorasLibres(string Cedula)
        {
            var modulos = db.conusltaHorarioLibreDocente(Cedula);



            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");

                listaModulo.Add(modulo);
            }


            return listaModulo;

        }

        //consulta horario obligatorio del docente

        public List<modulo> consultaHorasObligatorias(string Cedula)
        {
            var modulos = db.conusltaHorarioObligatorioDocente(Cedula);



            List<modulo> listaModulo = new List<modulo>();

            foreach (var consulta in modulos)
            {
                modulo modulo = new modulo();

                modulo.CodigoModulo = consulta.CodigoModulo;
                modulo.Dia = consulta.Dia;
                modulo.HoraInicio = consulta.HoraInicio.ToString(@"h\:mm");
                modulo.HoraFin = consulta.HoraFin.ToString(@"h\:mm");

                listaModulo.Add(modulo);
            }


            return listaModulo;

        }
    }
}
