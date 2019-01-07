using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppTutorias.Metodos.Estudiante;
using AppTutorias.Metodos.SuperAdmin.Estudiante;
using AppTutorias.Models.Estudiante;
using AppTutorias.Models.SuperAdmin.Materia;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using AppTutorias.Metodos.SuperAdmin.Docente;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Models.SuperAdmin.Estudiante;
using AppTutorias.Metodos.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Modulo;

namespace AppTutorias.Controllers
{
    [Authorize]
    public class EstudianteController : Controller
    {
        conexionDataContext db = new conexionDataContext();
        // GET: Estudiante
        public async Task<ActionResult> Index()
        {
            IUser user = await datosEstudiante();
            return View(user);
        }

        public async Task<IUser> datosEstudiante()
        {
            DatosEstudiante datosEstudiante = new DatosEstudiante();
            
            IUser user = await datosEstudiante.estudiante365();

            return user;
        }

        //Perfil del estudiante
        public ActionResult PerfilEstudiante()
        {

            Models.Estudiante.estudiante estudiante = new Models.Estudiante.estudiante();
            metodoEstudianteClass metodoEstudiante = new metodoEstudianteClass();
            estudiante = metodoEstudiante.datosEstudiante(Session["email"].ToString());

            return PartialView("perfilEstudiante",estudiante);
        }

        //tutorias del estudiante
        public ActionResult TutoriasEstudiante()
        {
            
            return PartialView("tutoriasEstudiante");
        }

        //modulo horario estudiante
        public ActionResult moduloHorarioEstudiante()
        {
            Metodos.Estudiante.metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new Metodos.Estudiante.metodoHorarioClaseEstudiante();
            List<Models.Estudiante.horarioClaseEstudiante> listaHorarioEstudiante = new List<Models.Estudiante.horarioClaseEstudiante>();

            listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudianteMatricula(Session["matricula"].ToString());


            return PartialView("moduloHorarioEstudiante",listaHorarioEstudiante);
        }

        public ActionResult moduloHorarioEstudianteMatricula(string Matricula)
        {
            Metodos.Estudiante.metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new Metodos.Estudiante.metodoHorarioClaseEstudiante();
            List<Models.Estudiante.horarioClaseEstudiante> listaHorarioEstudiante = new List<Models.Estudiante.horarioClaseEstudiante>();

            listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudianteMatricula(Matricula);


            return PartialView("moduloHorarioEstudiante", listaHorarioEstudiante);
        }

        //lista de materias del estudiante


        public ActionResult listaMateriasEstudiante()
        {
            metodoEstudianteClass metodoEstudianteClass = new metodoEstudianteClass();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoEstudianteClass.materiasEstudiante(Session["matricula"].ToString());

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaMateriasEstudianteMatricula(string Matricula)
        {
            metodoEstudianteClass metodoEstudianteClass = new metodoEstudianteClass();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoEstudianteClass.materiasEstudiante(Matricula);

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        //lista docentes por codigo de materia

        public ActionResult listaDocentesMateria(string CodigoMateria)
        {
            metodoDocente metodoDocente = new metodoDocente();

            List<docente> listaDocentes = new List<docente>();

            listaDocentes = metodoDocente.listaDocentesMateria(CodigoMateria);

            return Json(listaDocentes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaDocentesMateriaFull(string CodigoMateria)
        {
            metodoDocente metodoDocente = new metodoDocente();

            List<docente> listaDocentes = new List<docente>();

            listaDocentes = metodoDocente.listaDocentesMateriaFull(CodigoMateria);

            return Json(listaDocentes, JsonRequestBehavior.AllowGet);
        }


        //tutorias del estudiante
        public ActionResult listaTutoriasEstudiante()
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasEstudiante(Session["matricula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

       
        //ENVIAR UNA NOTIFICACION AL DOCENTE

        public ActionResult notificacionEstudiante(Solicitud solicitud)
        {
            metodoEstudianteClass metodoEstudianteClass = new metodoEstudianteClass();
            solicitud.Matricula = Session["matricula"].ToString();

            metodoEstudianteClass.ingresoNotificacion(solicitud);




            return Json("Solicitud Enviada", JsonRequestBehavior.AllowGet);
        }


        //modulos disponibles docente estudainte

        public ActionResult listaModulosDocenteEstudianteDisponible(string cedula)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocenteEstudiante(cedula, Session["matricula"].ToString());

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }

    }
}