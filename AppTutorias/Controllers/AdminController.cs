using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Metodos.SuperAdmin.Docente;
using AppTutorias.Metodos.SuperAdmin.Materia;
using AppTutorias.Models.SuperAdmin.Materia;
using AppTutorias.Metodos.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Estudiante;
using AppTutorias.Metodos.SuperAdmin.Estudiante;

namespace AppTutorias.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        //------TUTORIAS------------
        //---------------------------

        public ActionResult Tutorias()
        {
            
            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutorias();
  
            return PartialView("Tutorias", listaHorariosTutorias);
        }
        //LISTA Docentes del Estudiante 

        public ActionResult listaDocentesEstudiante(string Matricula,string CodigoMateria)
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<docente> listaDocentes = new List<docente>();

            listaDocentes = metodoDocente.consultarDocentesMatricula(Matricula,CodigoMateria);

            return Json(listaDocentes, JsonRequestBehavior.AllowGet);
        }

        //LISTA Materias del estudainte

        public ActionResult listaMateriasEstudiante(string Matricula)
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMateriasMatricula(Matricula);

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }


        public ActionResult listaModulosDocente(string Cedula, string CodigoPeriodo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocente(Cedula, CodigoPeriodo);

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        //ingresar tutorias

        //HORARIO DE CLASES

        public ActionResult ingresarTutorias(horarioTutorias horarioTutorias)
        {


            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();

            metodoHorarioTutorias.ingresarHorarioTutoria(horarioTutorias);
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutorias();

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }


        //
    }
}
