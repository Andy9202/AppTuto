using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppTutorias.Models;
using AppTutorias.Metodos;
using System.Web.Security;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Metodos.SuperAdmin.Docente;
using AppTutorias.Metodos.Docente;
using AppTutorias.Models.Docente;
using AppTutorias.Models.SuperAdmin.Materia;
using AppTutorias.Metodos.SuperAdmin.Materia;
using AppTutorias.Models.SuperAdmin.Modulo;
using AppTutorias.Metodos.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Estudiante;
using AppTutorias.Metodos.SuperAdmin.Estudiante;
using System.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using AppTutorias.Models.Estudiante;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace AppTutorias.Controllers

    {


    [Authorize]

    public class DocenteController : Controller
    {
        //login

        private ApplicationDbContext db = new ApplicationDbContext();
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private string graphResourceID = "https://graph.windows.net";

        // GET: UserProfile
        public async Task<ActionResult> Index()
        {
            if (Session["usuarioLogueado"] != null)
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


        //[HttpPost]
        //public ActionResult validarLogin(loginUsuario user)
        //{
        //    metodoLoginUsuario metodoLoginUsuario = new metodoLoginUsuario();
        //    masterDocente masterUsuario = new masterDocente();

        //    if (metodoLoginUsuario.validarLogin(user) == true)
        //    {
        //        masterUsuario = metodoLoginUsuario.docenteLogueado(user);
        //        Session["usuarioLogueado"] = masterUsuario;
        //        Session["nombreUsuario"] = masterUsuario.docente.Nombre + " "+ masterUsuario.docente.NombreDos + " " + masterUsuario.docente.Apellido + " " + masterUsuario.docente.ApellidoDos ;
        //        Session["correoUsuario"] = masterUsuario.loginUsuario.EmailUsuario;
        //        Session["cedula"] = masterUsuario.docente.Cedula;

        //        if (masterUsuario.rol.nombreRol == "DOCENTE")
        //        {


        //            FormsAuthentication.SetAuthCookie(masterUsuario.rol.nombreRol, false);
        //            datosUsuario();

        //            return View("Index");

        //        }
        //        else
        //        {
        //            return View("LoginDocente");
        //        }


        //    }
        //    else
        //    {
        //        return View("LoginDocente");
        //    }


        //}

        public void datosUsuario()
        {
            metodoDocente metodoDocente = new metodoDocente();
            docente  user = new docente();
            user = metodoDocente.consultaDocente(Session["cedula"].ToString());

            ViewBag.nombre = user.Nombre +" "+ user.NombreDos + " " + user.Apellido + " " + user.ApellidoDos;
            ViewBag.cedula = user.Cedula;
        }

        // GET: Docente

     

  
        public ActionResult CerrarSesion()
        {
            if (Session["usuarioLogueado"] != null)
            {
                FormsAuthentication.SignOut();
                Session["usuarioLogueado"] = null;
                Session["nombreUsuario"] = null;
                Session["correoUsuario"] = null;
                Session["cedula"] = null;

                return View("LoginDocente");

            }
            else
            {
                return View("LoginDocente");
            }
        }


        //perfil del docente
 
        public ActionResult PerfilDocente()
        {
            

                metodoDocente metodoDocente = new metodoDocente();
                docente docente = new docente();

                docente = metodoDocente.consultaDocente(Session["cedula"].ToString());

                return PartialView("PerfilDocente", docente);
            
         
       }

     
        [HttpPost]
        public ActionResult editarDocente()
        {

            if (Session["usuarioLogueado"] != null)
            {
                metodoDocente metodoDocente = new metodoDocente();
            docente docente= new docente();
            docente = metodoDocente.consultaDocente(Session["cedula"].ToString());

            return PartialView("editarDocente", docente);

             }
            else
            {
                return CerrarSesion();
            }
        }

        [HttpPost]
        public ActionResult EstudiantesDocente()
        {
            

            return PartialView("Estudiantes");


        }

        [HttpPost]
        public ActionResult moduloTutoria()
        {
            

            return PartialView("moduloTutoria");


        }


        [HttpPost]
        public ActionResult actualizarDocente(string Celular)
        {
            if (Session["usuarioLogueado"] != null)
            {
                metodoDocente metodoDocente = new metodoDocente();
            docente docente = new docente();
            docente = metodoDocente.consultaDocente(Session["cedula"].ToString());

            docente.Celular = Celular;

            metodoDocente.actualizarDocente(docente);

            docente = metodoDocente.consultaDocente(Session["cedula"].ToString());

            return Json("Docente Actualizado",JsonRequestBehavior.AllowGet);
            }
            else
            {
                return CerrarSesion();
            }
        }


        //verHorario

        public ActionResult moduloMateria()
        {

            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
       

            return PartialView("moduloMateria",listaHorarioDocente);
        
        }


        public ActionResult moduloHorario()
        {


            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Session["cedula"].ToString());
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Session["cedula"].ToString());

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;


            return PartialView("moduloHorario",listaHorarioDocente);

        }

        public ActionResult moduloHorarioDocente(string Cedula)
        {


            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Cedula);
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Cedula);
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Cedula);

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);

        }

        //MATERIAS DEL DOCENTE

        public ActionResult listaMateriasDocente()
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMateriasDocente(Session["cedula"].ToString());

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }


        //ESTUDIANTE DE LA MATERIA DEL DOCENTE

        public ActionResult estudiantesMateriaDocente( string CodigoMateria)
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<estudiantesDocente> listaEstudiantes = new List<estudiantesDocente>();

            listaEstudiantes = metodoDocente.consultarEstudiantesMateriaDocente(Session["cedula"].ToString(), CodigoMateria);

            return Json(listaEstudiantes, JsonRequestBehavior.AllowGet);
        }


        //MODULOS DEL DOCENTE

        public ActionResult listaModulosDocente(string CodigoPeriodo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocente(Session["cedula"].ToString(),CodigoPeriodo);

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        //LISTA Materias del estudainte del doncete logueado

        public ActionResult listaMateriasEstudiante(string Matricula)
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMateriasEstudianteDocente(Matricula, Session["cedula"].ToString());

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        ///INGRESA LA TUTORIA AL ESTUDIAN Y ENVIA UN MENSAJE AL CORREO ESTUDIANTE

        public ActionResult ingresarTutorias(horarioTutorias horarioTutorias)
        {
            conexionDataContext db = new conexionDataContext();

            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();

            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());

            int contador = listaHorariosTutorias.ToList().Count;

            metodoHorarioTutorias.ingresarHorarioTutoriaDocente(horarioTutorias, Session["cedula"].ToString());

            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());


            var estudiante = (from x in db.ESTUDIANTE
                              where x.Matricula == int.Parse(horarioTutorias.Matricula)
                              select new
                              {
                                  x.Nombre,
                                  x.NombreDos,
                                  x.Apellido,
                                  x.ApellidoDos,
                                  x.Email,
                                  x.Matricula
                              }).FirstOrDefault();


            string mensaje="";

            int contadorDos = listaHorariosTutorias.ToList().Count;

            if (contadorDos > contador)
            {
                contactoApp(horarioTutorias, Session["cedula"].ToString());

                mensaje = "El tutoria para el estudiante "+ estudiante.Nombre + " " + estudiante.Apellido + " ha sido ingresado con exito!";

            }
            else
            {
                mensaje = "Ya existe una turoia asignada en la misma fecha, por favor asigne una fecha diferente o un modulo disitinto";

            }

            var data = new
            {
                ListaHorariosTutorias=listaHorariosTutorias,
                Mensaje = mensaje

            };

            return Json(data, JsonRequestBehavior.AllowGet);


        }

        //envia mensjae al estudiante
        static async Task contactoApp(horarioTutorias horarioTutorias,string Cedula)
        {
            conexionDataContext db = new conexionDataContext();
    

            var estudiante = (from x in db.ESTUDIANTE
                              where x.Matricula == int.Parse(horarioTutorias.Matricula)
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
                              where x.Cedula == Cedula
                              select new
                              {
                                  x.Nombre,
                                  x.NombreDos,
                                  x.Apellido,
                                  x.ApellidoDos,
                                  x.Email
                              }).FirstOrDefault();

            var modulo = (from x in db.MODULO
                          where x.CodigoModulo == horarioTutorias.CodigoModulo
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

    
                mensaje.From = new EmailAddress(docente.Email, docente.Nombre +" " +docente.Apellido);
                mensaje.AddTo(estudiante.Email, estudiante.Nombre + " " + estudiante.Apellido);
                mensaje.Subject = "Tutoria Nueva";



            mensaje.AddSubstitution("-estudiante-", estudiante.Nombre + " " + estudiante.Apellido);
            mensaje.AddSubstitution("-docente-", docente.Nombre + " " + docente.Apellido);
           mensaje.AddSubstitution("-materia-", horarioTutorias.CodigoMateria+" "+horarioTutorias.NombreMateria);
           mensaje.AddSubstitution("-aula-", horarioTutorias.Aula);
           mensaje.AddSubstitution("-fecha-", horarioTutorias.Fecha);
           mensaje.AddSubstitution("-modulo-", modulo.CodigoModulo +" "+modulo.Dia+" "+modulo.HoraInicio+" - " +modulo.HoraFin);




            mensaje.TemplateId = "f74f2974-3701-4ecf-ba96-99cfdf21f9df";


            var response = await client.SendEmailAsync(mensaje);
        }


        //LISTA TUTORIAS TOTALES
        public ActionResult listaTutoriasDocenteTotal()
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocente(Session["cedula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

        ///LISTA TUTORIAS DEL DOCENTE ACTUALES

        public ActionResult listaTutoriasDocente()
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

        //List tutorias docente por asistencia

        public ActionResult listaTutoriasDocenteAsistencia(string Asistencia)
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteAsistencia(Session["cedula"].ToString(),Asistencia);

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }

        //List tutorias docente por periodo academico

        public ActionResult listaTutoriasDocentePeriodo(string CodigoPeriodo)
        {

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocentePeriodo(Session["cedula"].ToString(), CodigoPeriodo);

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);
        }



        //consulta el horario del docente desde el estudiante

        public ActionResult moduloHorarioConsulta(string cedula)
        {


            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(cedula);



            return PartialView("moduloMateria", listaHorarioDocente);

        }

        public ActionResult moduloHorarioDocenteConsulta(string cedula)
        {


            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(cedula);
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(cedula);
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(cedula);

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);

        }

        



        //MODULOS DEL DOCENTE DISPONIBLES CONSULTADOS DESDE EL ESTUDIANTE

        public ActionResult listaModulosDocenteDisponible(string cedula,string CodigoPeriodo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocente(cedula,CodigoPeriodo);

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult listaModulosDocenteDisponibleLogueado(string CodigoPeriodo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocente(Session["cedula"].ToString(),CodigoPeriodo);

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaModulosDocenteDisponibleAsignacion()
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulosDocenteAsignacion(Session["cedula"].ToString());

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult listaHorasLibres()
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaHorasLibres(Session["cedula"].ToString());

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaHorasObligatorias()
        {
            metodoModulo metodoModulo = new metodoModulo();
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaHorasObligatorias(Session["cedula"].ToString());

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }

        ///NOTIFICACIONES---------------------------------------------
        /////--------------------------------------------------------

        public ActionResult mensajesEstudiantes()
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<Solicitud> listaSolicitudes = new List<Solicitud>();

            listaSolicitudes = metodoDocente.consultaMensajesEstudiante(Session["cedula"].ToString());

            return PartialView("Notificaciones",listaSolicitudes);
        }

        //CONFIRMAR TUTOTIA SOLICITADA DLE ESTUDIANTE

        public ActionResult confirmaSolicitud(string Id_Notificacion,string Confirma, horarioTutorias horarioTutorias)
        {

            metodoDocente metodoDocente = new metodoDocente();
            List<Solicitud> listaSolicitudes = new List<Solicitud>();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();
            string mensaje = "";
            bool valido = false;
            if (horarioTutorias.Aula != null)
            {
            

                horarioTutorias.CedulaDocente = Session["cedula"].ToString();
                metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
                listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());
                int contador = listaHorariosTutorias.ToList().Count;

                metodoHorarioTutorias.ingresarHorarioTutoria(horarioTutorias);

                listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());
                int contadorDos = listaHorariosTutorias.ToList().Count;


        
                if (contadorDos > contador)
                {
                    metodoDocente.confirmaSolictudEstudiante(Id_Notificacion, Confirma);
                    contactoApp(horarioTutorias, Session["cedula"].ToString());
                    mensaje = "Solictud confirmada";
                    valido = true;
                }
                else
                {
                    mensaje = "este horario no esta disponible";
                }
        


            }

            listaSolicitudes = metodoDocente.consultaMensajesEstudiante(Session["cedula"].ToString());

            var data = new
            {
                ListaSolicitudes = listaSolicitudes,
                Mensaje = mensaje,
                Valido =valido

            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //INGRESO DE HORAS LIBRES

        public ActionResult ingresarHorasLibres(string CodigoModulo ,string CodigoPeriodo)
        {

            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.ingresarHorasLibres(Session["cedula"].ToString(), CodigoModulo,CodigoPeriodo);

            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Session["cedula"].ToString()).Where(x => x.CodigoPeriodo == CodigoPeriodo).ToList();
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Session["cedula"].ToString()).Where(x => x.CodigoPeriodo == CodigoPeriodo).ToList();

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);
        }

        //INGREO DE HORAS OBLIGATORIAS


        public ActionResult ingresarHorasObligatorias(string CodigoM, string CodigoPeriodo)
        {

            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.ingresarHorasObligatorias(Session["cedula"].ToString(), CodigoM,CodigoPeriodo);

            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Session["cedula"].ToString()).Where(x => x.CodigoPeriodo == CodigoPeriodo).ToList();
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Session["cedula"].ToString()).Where(x => x.CodigoPeriodo == CodigoPeriodo).ToList();

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);
        }

        //ELIMINAR HORAS LIBRES

        public ActionResult eliminarHorasLibres(string CodigoModulo,string CodigoPeriodo)
        {

            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.eliminarHorasLibres(Session["cedula"].ToString(), CodigoModulo,CodigoPeriodo);

            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Session["cedula"].ToString());
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Session["cedula"].ToString());

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);
        }

        //ELIMINAR HORAS LIBRES

        public ActionResult eliminarHorasObligatorias(string CodigoModulo, string CodigoPeriodo)
        {

            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.eliminarHorasObligatorias(Session["cedula"].ToString(), CodigoModulo,CodigoPeriodo);

            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            List<horarioClaseDocente> listaHorarioDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioLibreDocente = new List<horarioClaseDocente>();
            List<horarioClaseDocente> consultaHorarioObligatorioDocente = new List<horarioClaseDocente>();

            listaHorarioDocente = metodoHorarioClaseDocente.consultaHorariosDocenteCedula(Session["cedula"].ToString());
            consultaHorarioLibreDocente = metodoHorarioClaseDocente.consultaHorarioLibreDocente(Session["cedula"].ToString());
            consultaHorarioObligatorioDocente = metodoHorarioClaseDocente.consultaHorarioObligatorioDocente(Session["cedula"].ToString());

            ViewBag.horasLibres = consultaHorarioLibreDocente;
            ViewBag.horasObligatorias = consultaHorarioObligatorioDocente;

            return PartialView("moduloHorario", listaHorarioDocente);
        }


        //CONFIRMAR ASISTENCIA DEL ESTUDIANTE

        public ActionResult confirmarAsistencia(int Id,string Confirma)
        {
            conexionDataContext db = new conexionDataContext();
            db.confirmarAsistenciaTutoria(Id,Confirma);

            metodoHorarioTutorias metodoHorarioTutorias = new metodoHorarioTutorias();
            List<horarioTutorias> listaHorariosTutorias = new List<horarioTutorias>();


            listaHorariosTutorias = metodoHorarioTutorias.consultaHorarioTutoriasDocenteFecha(Session["cedula"].ToString());

            return Json(listaHorariosTutorias, JsonRequestBehavior.AllowGet);

        }

        //REPORTES TUTORIAS

       public ActionResult Reportes()
        {
            return PartialView("Reportes");
        }

    }
}
