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
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Security.Claims;
using AppTutorias.Models;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace AppTutorias.Controllers
{
    [Authorize]
    public class EstudianteController : Controller
    {
       


        private ApplicationDbContext db = new ApplicationDbContext();
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private string graphResourceID = "https://graph.windows.net";

        public async Task<ActionResult> Index()
        {
            if (Session["matricula"] != null)
            {
                string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                try
                {
                    Uri servicePointUri = new Uri(graphResourceID);
                    Uri serviceRoot = new Uri(servicePointUri, tenantID);
                    ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                          async () => await GetTokenForApplication());

                    // utiliza el token para consultar el gráfico y obtener detalles sobre el usuario

                    var result = await activeDirectoryClient.Users
                        .Where(u => u.ObjectId.Equals(userObjectID))
                        .ExecuteAsync();
                    IUser user = result.CurrentPage.ToList().First();

                    return View("Index", user);
                }
                catch (AdalException)
                {
                    // Devuelve a la página de error.
                    return View("Error");
                }
                // si lo anterior no funciona, el usuario deberá reautenticarse explícitamente para la aplicación para obtener el token necesario
                catch (Exception)
                {
                    return RedirectToAction("SignIn", "Account");
                }
            }
            else
            {
                return RedirectToAction("SignOut", "Account");
            }
        }

        public void RefreshSession()
        {
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = "/UserProfile" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // obtén un token para el gráfico sin provocar ninguna interacción del usuario (desde la caché, a través de un token de actualización multirecursos, etc.)
            ClientCredential clientcred = new ClientCredential(clientId, appKey);
            // inicializa AuthenticationContext con la caché del token del usuario con sesión iniciada según lo almacenado en la base de datos de la aplicación
            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID, new ADALTokenCache(signedInUserID));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(graphResourceID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
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


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasEstudianteFecha(Session["matricula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaTutoriasEstudianteTotal()
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasEstudiante(Session["matricula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }


        public ActionResult listaTutoriasEstudianteAsistencia(string Asistencia)
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasEstudianteAsistencia(Session["matricula"].ToString(),Asistencia);

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaTutoriasEstudiantePeriodo(string CodigoPeriodo)
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasEstudiantePeriodo(Session["matricula"].ToString(),CodigoPeriodo);

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }





        //ENVIAR UNA NOTIFICACION AL DOCENTE

        public ActionResult notificacionEstudiante(Solicitud solicitud)
        {
            metodoEstudianteClass metodoEstudianteClass = new metodoEstudianteClass();
            solicitud.Matricula = Session["matricula"].ToString();

            metodoEstudianteClass.ingresoNotificacion(solicitud);

            contactoApp(solicitud, Session["matricula"].ToString());


            return Json("Solicitud Enviada", JsonRequestBehavior.AllowGet);
        }


        static async Task contactoApp(Solicitud solicitud, string Matricula)
        {
            conexionDataContext db = new conexionDataContext();


            var estudiante = (from x in db.ESTUDIANTE
                              where x.Matricula == int.Parse(Matricula)
                              select new
                              {
                                  x.Nombre,
                                  x.NombreDos,
                                  x.Apellido,
                                  x.ApellidoDos,
                                  x.Email,
                                  x.Matricula
                              }).FirstOrDefault();

            var docente = (from x in db.DOCENTE
                           where x.Cedula == solicitud.Cedula
                           select new
                           {
                               x.Nombre,
                               x.NombreDos,
                               x.Apellido,
                               x.ApellidoDos,
                               x.Email
                           }).FirstOrDefault();

            var modulo = (from x in db.MODULO
                          where x.CodigoModulo == solicitud.CodigoModulo
                          select new
                          {
                              x.Dia,
                              x.HoraInicio,
                              x.HoraFin,
                              x.CodigoModulo
                          }).FirstOrDefault();


            SendGridMessage mensaje = new SendGridMessage();
            var apiKey = "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";
            var client = new SendGridClient(apiKey);


            mensaje.From = new EmailAddress(estudiante.Email, estudiante.Nombre + " " + estudiante.Apellido);
            mensaje.AddTo(docente.Email, docente.Nombre + " " + docente.Apellido);
            mensaje.Subject = "Solicitud Tutoria";

            


            mensaje.AddSubstitution("-mensaje-",solicitud.Mensaje);
            mensaje.AddSubstitution("-estudiante-", estudiante.Nombre + " " + estudiante.Apellido);
            mensaje.AddSubstitution("-materia-", solicitud.CodigoMateria + " " + solicitud.NombreMateria);
            mensaje.AddSubstitution("-fecha-", solicitud.Fecha.ToShortDateString());
            mensaje.AddSubstitution("-modulo-", modulo.CodigoModulo + " " + modulo.Dia + " " + modulo.HoraInicio + " - " + modulo.HoraFin);




            mensaje.TemplateId = "e2d5c12f-e77a-4174-8e9a-10ca82bc345d";


            var response = await client.SendEmailAsync(mensaje);
        }



        //modulos disponibles docente estudainte

        public ActionResult listaModulosDocenteEstudianteDisponible(string cedula,string CodigoPeriodo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocenteEstudiante(cedula, Session["matricula"].ToString(),CodigoPeriodo);

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        //REPORTES

        public ActionResult Reportes()
        {
            return PartialView("Reportes");
        }

    }
}