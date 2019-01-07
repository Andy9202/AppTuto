using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppTutorias.Models.SuperAdmin;
using AppTutorias.Metodos.SuperAdmin;
using AppTutorias.Metodos.SuperAdmin.Carrera;
using AppTutorias.Models.SuperAdmin.Carrera;
using AppTutorias.Metodos.SuperAdmin.Materia;
using AppTutorias.Models.SuperAdmin.Materia;
using AppTutorias.Metodos.SuperAdmin.Docente;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Metodos.SuperAdmin.Estudiante;
using AppTutorias.Models.SuperAdmin.Estudiante;
using AppTutorias.Models;
using AppTutorias.Metodos.SuperAdmin.Modulo;
using AppTutorias.Metodos.SuperAdmin.Periodo;
using AppTutorias.Models.SuperAdmin.Modulo;
using AppTutorias.Models.SuperAdmin.Periodo;
using AppTutorias.Metodos.SuperAdmin.HorarioClases;
using AppTutorias.Models.SuperAdmin.HorarioClases;
using System.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using AppTutorias;
using AppTutorias.Metodos.Docente;
using AppTutorias.Models.Docente;

namespace TutoriasApp.Controllers
{

    [Authorize]
    public class SuperAdminController : Controller
    {
        //login

        private ApplicationDbContext db = new ApplicationDbContext();
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private string graphResourceID = "https://graph.windows.net";



        public async Task<ActionResult> Index()
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




        // GET: SuperAdmin

        //FACULTAD

        public ActionResult Facultad()
        {
            metodoFacultad metodoFacultad = new metodoFacultad();
            List<facultad> listaFaculta = new List<facultad>();
            listaFaculta = metodoFacultad.consultarFacultades();

            return PartialView("Facultad",listaFaculta );
        }

        public ActionResult ingresarFacultad(facultad facultad)
        {
            
            metodoFacultad metodoFacultad = new metodoFacultad();

            if(metodoFacultad.consultaExisteFacultad(facultad.CodigoFacultad)!=true)
            {
                metodoFacultad.ingresarFacultad(facultad);
           
            }
            else
            {
                metodoFacultad.actualizarFacultad(facultad);
            }

            List<facultad> listaFacultad = new List<facultad>();
            listaFacultad = metodoFacultad.consultarFacultades();

            return Json(listaFacultad, JsonRequestBehavior.AllowGet);
        }


        public ActionResult eliminarFacultad(string CodigoFacultad)
        {
            metodoFacultad metodoFacultad = new metodoFacultad();
            metodoFacultad.eliminarFacultad(CodigoFacultad);
            List<facultad> listaFacultad = new List<facultad>();
            listaFacultad = metodoFacultad.consultarFacultades();

            return Json(listaFacultad, JsonRequestBehavior.AllowGet);
        }

        public ActionResult editarFacultad(string CodigoFacultad)
        {
            metodoFacultad metodoFacultad = new metodoFacultad();
            facultad facultad = new facultad();
            facultad = metodoFacultad.consultaFacultad(CodigoFacultad);

            return Json(facultad, JsonRequestBehavior.AllowGet);
        }


        //CARRERA
        public ActionResult Carrera()
        {
            metodoCarrera metodoCarrera = new metodoCarrera();
            List<carrera> listaCarrera = new List<carrera>();
            listaCarrera = metodoCarrera.consultarCarreras();

            return PartialView("Carrera", listaCarrera);
        }

        public ActionResult editarCarrera(string CodigoCarrera)
        {
            metodoCarrera metodoCarrera = new metodoCarrera();
            carrera carrera = new carrera();
            carrera = metodoCarrera.consultaCarrera(CodigoCarrera);

            return Json(carrera, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ingresarCarrera(carrera carrera)
        {

            metodoCarrera metodoCarrera = new metodoCarrera();

            if (metodoCarrera.consultaExisteCarrera(carrera.CodigoCarrera) != true)
            {
                metodoCarrera.ingresarCarrera(carrera);

            }
            else
            {
                metodoCarrera.actualizarCarrera(carrera);
            }

            List<carrera> listaCarrera = new List<carrera>();
            listaCarrera = metodoCarrera.consultarCarreras();

            return Json(listaCarrera, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminarCarrera(string CodigoCarrera)
        {
            metodoCarrera metodoCarrera = new metodoCarrera();
            metodoCarrera.eliminarCarrera(CodigoCarrera);
            List<carrera> listaCarrera = new List<carrera>();
            listaCarrera = metodoCarrera.consultarCarreras();

            return Json(listaCarrera, JsonRequestBehavior.AllowGet);
        }

        //MATERIA-----------------


        public ActionResult Materia()
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMaterias();

            return PartialView("Materia",listaMateria);
        }

        public ActionResult listaCarreras()
        {
            metodoCarrera metodoCarrera = new metodoCarrera();
            List<carrera> listaCarrera = new List<carrera>();
            listaCarrera = metodoCarrera.consultarCarreras();
            return Json(listaCarrera, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ingresarMateria(materia materia)
        {

            metodoMateria metodoMateria = new metodoMateria();

            if (metodoMateria.consultaExisteMateria(materia.CodigoMateria) != true)
            {
                metodoMateria.ingresarMateria(materia);

            }
            else
            {
                metodoMateria.actualizarMateria(materia);
            }

            List<materia> listaMateria = new List<materia>();
            listaMateria = metodoMateria.consultarMaterias();

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminarMateria(string CodigoMateria)
        {
            metodoMateria metodoMateria = new metodoMateria();
            metodoMateria.eliminarMateria(CodigoMateria);
            List<materia> listaMateria = new List<materia>();
            listaMateria = metodoMateria.consultarMaterias();

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult editarMateria(string CodigoMateria)
        {
            metodoMateria metodoMateria = new metodoMateria();
            materia materia = new materia();
            materia = metodoMateria.consultaMateria(CodigoMateria);

            return Json(materia, JsonRequestBehavior.AllowGet);
        }


      //DOCENTE



        public ActionResult Docente()
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<docente> listaDocente = new List<docente>();

            listaDocente = metodoDocente.consultarDocentes();

            return PartialView("Docente",listaDocente);
        }

        public ActionResult ingresarDocente(docente docente)
        {
            metodoDocente metodoDocente = new metodoDocente();

            if (metodoDocente.consultaExisteDocente(docente.Cedula) != true)
            {
                metodoDocente.ingresarDocente(docente);

            }
            else
            {
                metodoDocente.actualizarDocente(docente);
            }

            List<docente> listaDocente = new List<docente>();
            listaDocente = metodoDocente.consultarDocentes();

            return Json(listaDocente, JsonRequestBehavior.AllowGet);
        }

        //ingresar docentes desde excel
        [HttpPost]
        public ActionResult ingresoDocenteExcel(HttpPostedFileBase excelFile)
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<docente> listaDocente = new List<docente>();

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                listaDocente = metodoDocente.consultarDocentes();
                return Json(listaDocente, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx")){

                    string path = Server.MapPath("~/Content/" + excelFile.FileName);
               

                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);

                    excelFile.SaveAs(path);

                    //LEER DATOS DEL EXCEL

                    Excel.Application application = new Excel.Application();

                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                
        

                    for ( var x =2;x <=range.Rows.Count;x++)
                    {
                        docente d = new docente();

                        d.Nombre = ((Excel.Range)range.Cells[x, 1]).Text;
                        d.NombreDos = ((Excel.Range)range.Cells[x, 2]).Text;
                        d.Apellido = ((Excel.Range)range.Cells[x, 3]).Text;
                        d.ApellidoDos = ((Excel.Range)range.Cells[x, 4]).Text;
                        d.Cedula = ((Excel.Range)range.Cells[x, 5]).Text;
                        d.Email = ((Excel.Range)range.Cells[x, 6]).Text;
                        d.Celular = ((Excel.Range)range.Cells[x, 7]).Text;

                       

                        if (metodoDocente.consultaExisteDocente(d.Cedula) != true)
                        {
                            metodoDocente.ingresarDocente(d);

                        }
                        else
                        {
                            metodoDocente.actualizarDocente(d);
                        }

                   
                    }

                   
                    listaDocente = metodoDocente.consultarDocentes();

                    workbook.Close(false, excelFile.FileName, null);
                    Marshal.FinalReleaseComObject(workbook);
                    application.Quit();
                    


                    return Json(listaDocente, JsonRequestBehavior.AllowGet);
                }
                else{
                 
                    listaDocente = metodoDocente.consultarDocentes();
                    return Json(listaDocente, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //editar doncente

        public ActionResult editarDocente(string Cedula)
        {
            metodoDocente metodoDocente = new metodoDocente();
            docente docente = new docente();
            docente = metodoDocente.consultaDocente(Cedula);

            return Json(docente, JsonRequestBehavior.AllowGet);
        }

        //eliminar docente

        public ActionResult eliminarDocente(string Cedula)
        {
            metodoDocente metodoDocente = new metodoDocente();
            metodoDocente.eliminarDocente(Cedula);
            List<docente> listaDocente = new List<docente>();
            listaDocente = metodoDocente.consultarDocentes();

            return Json(listaDocente, JsonRequestBehavior.AllowGet);
        }


        //ESTUDIANTE-------
        public ActionResult Estudiante()
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();
            List<estudiante> listaEstudiante = new List<estudiante>();
            listaEstudiante = metodoEstudiante.consultarEtudiantes();

            return PartialView("Estudiante",listaEstudiante);
        }

        public ActionResult ingresarEstudiante(estudiante estudiante)
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();

            if (metodoEstudiante.consultaExisteEstudiante(int.Parse(estudiante.Matricula)) != true)
            {
                metodoEstudiante.ingresarEstudiante(estudiante);

            }
            else
            {
                metodoEstudiante.actualizarEstudiante(estudiante);
            }

            List<estudiante> listaEstudiante = new List<estudiante>();
            listaEstudiante = metodoEstudiante.consultarEtudiantes(); 

            return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
        }

        //ingresar estudiantes desde excel
        [HttpPost]
        public ActionResult ingresoEstudianteExcel(HttpPostedFileBase excelFile)
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();
            List<estudiante> listaEstudiante = new List<estudiante>();

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                listaEstudiante = metodoEstudiante.consultarEtudiantes();
                return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {

                    string path = Server.MapPath("~/Content/" + excelFile.FileName);


                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excelFile.SaveAs(path);

                    //LEER DATOS DEL EXCEL

                    Excel.Application application = new Excel.Application();

                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;




                    for (var i = 2; i <= range.Rows.Count; i++)
                    {
                        estudiante estudiante = new estudiante();

                        estudiante.Nombre = ((Excel.Range)range.Cells[i, 1]).Text;
                        estudiante.NombreDos = ((Excel.Range)range.Cells[i, 2]).Text;
                        estudiante.Apellido = ((Excel.Range)range.Cells[i, 3]).Text;
                        estudiante.ApellidoDos = ((Excel.Range)range.Cells[i, 4]).Text;
                        estudiante.Matricula = ((Excel.Range)range.Cells[i, 5]).Text;
                        estudiante.Email = ((Excel.Range)range.Cells[i, 6]).Text;
                        estudiante.Celular = ((Excel.Range)range.Cells[i, 7]).Text;
                        estudiante.CodCarrera = ((Excel.Range)range.Cells[i, 8]).Text;


                        if (estudiante.Matricula != "")
                        {
                            if (metodoEstudiante.consultaExisteEstudiante(int.Parse(estudiante.Matricula)) != true)
                            {
                                metodoEstudiante.ingresarEstudiante(estudiante);

                            }
                            else
                            {
                                metodoEstudiante.actualizarEstudiante(estudiante);
                            }
                        }


                    }


                    listaEstudiante = metodoEstudiante.consultarEtudiantes();

                    workbook.Close(false, excelFile.FileName, null);
                    Marshal.FinalReleaseComObject(workbook);
                    application.Quit();



                    return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    listaEstudiante = metodoEstudiante.consultarEtudiantes();
                    return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
                }
            }
        }


        //editar estudiante

        public ActionResult editarEstudiante(int Matricula)
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();
            estudiante estudiante = new estudiante();
            estudiante =metodoEstudiante.consultaEstudiante(Matricula);

            return Json(estudiante, JsonRequestBehavior.AllowGet);
        }


        public ActionResult eliminarEstudiante(int Matricula)
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();
            metodoEstudiante.eliminarEstudiante(Matricula);
            List<estudiante> listaEstudiante = new List<estudiante>();
            listaEstudiante = metodoEstudiante.consultarEtudiantes(); 

            return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaEstudiantesCarrera(string CodigoCarrera)
        {
            metodoEstudiante metodoEstudiante = new metodoEstudiante();
            
            List<estudiante> listaEstudiante = new List<estudiante>();
            listaEstudiante = metodoEstudiante.consultarEstudiantesCarrera(CodigoCarrera);

            return Json(listaEstudiante, JsonRequestBehavior.AllowGet);
        }



        //MODULO PERIODO ACADEMICO


        public ActionResult moduloPeriodo()
        {
            masterModuloPeriodo masterModuloPeriodo = new masterModuloPeriodo();
            metodoModulo metodoModulo = new metodoModulo();
            metodoPeriodo metodoPeriodo = new metodoPeriodo();


            masterModuloPeriodo.listaModulo = metodoModulo.consultaModulos();
            masterModuloPeriodo.listaPeriodo = metodoPeriodo.consultaPeriodos();


            return PartialView("modulosPeriodo",masterModuloPeriodo);
        }


        ///ingresar modulo

        public ActionResult ingresarModulo(modulo modulo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.ingresarModulo(modulo);

            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulos();

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult eliminarModulo(string CodigoModulo)
        {
            metodoModulo metodoModulo = new metodoModulo();
            metodoModulo.eliminarModulo(CodigoModulo);

            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulos();

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }


        ///PERIODO ACADEMICO
        ///

        ///ingresar modulo

        public ActionResult ingresarPeriodo(periodo periodo)
        {
            metodoPeriodo metodoPeriodo = new metodoPeriodo();
            metodoPeriodo.ingresarPeriodo(periodo);

            List<periodo> listaPeriodo = new List<periodo>();
            listaPeriodo = metodoPeriodo.consultaPeriodos();

            return Json(listaPeriodo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult eliminarPeriodo(string CodigoPeriodo)
        {
            metodoPeriodo metodoPeriodo = new metodoPeriodo();
            metodoPeriodo.eliminarPeriodo(CodigoPeriodo);

            List<periodo> listaPeriodo = new List<periodo>();
            listaPeriodo = metodoPeriodo.consultaPeriodos();

            return Json(listaPeriodo, JsonRequestBehavior.AllowGet);
        }


        //HORARIO DE CLASES GENERAL

        public ActionResult horarioClase()
        {
            metodoHorarioClases metodoHorarioClases = new metodoHorarioClases();
            List<horarioClases> listaHorariosClases = new List<horarioClases>();

            listaHorariosClases = metodoHorarioClases.consultaHorarioClases();

            return PartialView("HorarioClases",listaHorariosClases);
        }

        //LISTA DE MATERIAS POR CARRERA

        public ActionResult listaMaterias(string CodigoCarrera)
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMateriasCarrera(CodigoCarrera);

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        //LISTA Docentes

        public ActionResult listaDocentes()
        {
            metodoDocente metodoDocente = new metodoDocente();
            List<docente> listaDocentes = new List<docente>();

            listaDocentes = metodoDocente.consultarDocentes();

            return Json(listaDocentes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaMateriasDocenteCedula(string Cedula)
        {
            metodoMateria metodoMateria = new metodoMateria();
            List<materia> listaMateria = new List<materia>();

            listaMateria = metodoMateria.consultarMateriasDocente(Cedula);

            return Json(listaMateria, JsonRequestBehavior.AllowGet);
        }

        //LISTA MODULOS
        public ActionResult listaModulos()
        {
            metodoModulo metodoModulo = new metodoModulo();
         
            List<modulo> listaModulo = new List<modulo>();
            listaModulo = metodoModulo.consultaModulos();

            return Json(listaModulo, JsonRequestBehavior.AllowGet);
        }

        //LISTA MODULOS
        public ActionResult listaPeriodo()
        {
            metodoPeriodo metodoPeriodo = new metodoPeriodo();

            List<periodo> listaPeriodo = new List<periodo>();
            listaPeriodo = metodoPeriodo.consultaPeriodos();

            return Json(listaPeriodo, JsonRequestBehavior.AllowGet);
        }


        //HORARIO DE CLASES

        public ActionResult ingresarHorarioClases(horarioClases horarioClases)
        {

            metodoHorarioClases metodoHorarioClases = new metodoHorarioClases();
            List<horarioClases> listaHorariosClases = new List<horarioClases>();


            int contador = metodoHorarioClases.consultaHorarioClases().ToList().Count;

            metodoHorarioClases.ingresarHorarioClases(horarioClases);

            int contadorDos = metodoHorarioClases.consultaHorarioClases().ToList().Count;

            string mensaje="";

            if (contadorDos > contador)
            {
                mensaje = "EL Modulo ha sido Ingresado";
            }
            if (contadorDos == contador)
            {
                mensaje = "Ya existe el Modulo ingresado en el Horario Actual,Ingrese uno diferente";
            }

         

            listaHorariosClases = metodoHorarioClases.consultaHorarioClases();

            var data = new
            {
                Mensaje = mensaje,
                ListaHorarioClases = listaHorariosClases

            };


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //HORARIO DE CLASES DESDE EXCEL 

        public ActionResult ingresoHoraioDocenteExcel(HttpPostedFileBase excelFile)
        {

            metodoHorarioClases metodoHorarioClases = new metodoHorarioClases();
            List<horarioClases> listaHorariosClases = new List<horarioClases>();

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                listaHorariosClases = metodoHorarioClases.consultaHorarioClases();
                return Json(listaHorariosClases, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {

                    string path = Server.MapPath("~/Content/" + excelFile.FileName);


                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excelFile.SaveAs(path);

                    //LEER DATOS DEL EXCEL

                    Excel.Application application = new Excel.Application();

                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;




                    for (var i = 2; i <= range.Rows.Count; i++)
                    {
                      
                        horarioClases horarioClases = new horarioClases();

                        horarioClases.CodigoMateria = ((Excel.Range)range.Cells[i, 1]).Text;
                        horarioClases.CedulaDocente = ((Excel.Range)range.Cells[i, 2]).Text;
                        horarioClases.Modulo = ((Excel.Range)range.Cells[i, 3]).Text;
                        horarioClases.Periodo = ((Excel.Range)range.Cells[i, 4]).Text;
                        horarioClases.Aula = ((Excel.Range)range.Cells[i, 5]).Text;
                        horarioClases.Paralelo= ((Excel.Range)range.Cells[i, 6]).Text;

                        metodoDocente metodoDocente = new metodoDocente();
                      
                            metodoHorarioClases.ingresarHorarioClases(horarioClases);

                      
                      
                      

                    }


                    listaHorariosClases = metodoHorarioClases.consultaHorarioClases();

                    workbook.Close(false, excelFile.FileName, null);
                    Marshal.FinalReleaseComObject(workbook);
                    application.Quit();





                    listaHorariosClases = metodoHorarioClases.consultaHorarioClases();

                    return Json(listaHorariosClases, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    listaHorariosClases = metodoHorarioClases.consultaHorarioClases();

                    return Json(listaHorariosClases, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult eliminarHorarioClases(horarioClases horarioClases)
        {

            metodoHorarioClases metodoHorarioClases = new metodoHorarioClases();

            metodoHorarioClases.eliminarHorarioClases(horarioClases);
        

            return Json("Materia Eliminada", JsonRequestBehavior.AllowGet);
        }

        //HORARIO CLASE DEL ESUTUDIANTE



            public ActionResult ingresarHorarioClasesEstudiantes(string Matricula, string CedulaDocente, string CodigoMateria,string Paralelo,string NumMatricula)
        {
            conexionDataContext db = new conexionDataContext();
            metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new metodoHorarioClaseEstudiante();
            metodoDocente metodoDocente = new metodoDocente();

            var existe = db.horarioClaseEstudianteMatricula(int.Parse(Matricula)).Where(x => x.CodigoMeteria == CodigoMateria).FirstOrDefault();
            var modulosMateria = db.horarioDocenteCedula(CedulaDocente).Where(x=>x.CodigoMeteria == CodigoMateria);


            int contador = metodoHorarioClaseEstudiante.consultaHorariosEstudainte().ToList().Count;



            if ( existe == null)
                {
                    foreach (var x in modulosMateria)
                    {
                        metodoHorarioClaseEstudiante.ingresarHorarioClaseEstudiante(Matricula, CedulaDocente, CodigoMateria, x.CodigoModulo, Paralelo);

                      
                    }
                       db.IngresarRepitencia(int.Parse(Matricula), CodigoMateria, char.Parse(NumMatricula));
                }
                if (existe != null)
                {
                    if (existe.Paralelo == Paralelo)
                    {
                        foreach (var x in modulosMateria)
                        {
                            metodoHorarioClaseEstudiante.ingresarHorarioClaseEstudiante(Matricula, CedulaDocente, CodigoMateria, x.CodigoModulo, Paralelo);


                        }
                     }
                }
            
                
            List<horarioClaseEstudiante> listaHorarioEstudiante = new List<horarioClaseEstudiante>();
            listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();

           

    

            int contadorDos = metodoHorarioClaseEstudiante.consultaHorariosEstudainte().ToList().Count;

            string mensaje = "";

            if (contadorDos > contador)
            {
                mensaje = "Horario Ingresado";
            }
            if (contadorDos == contador)
            {
                mensaje = "Ya existe un horario en los modulos ingresados,Ingrese un horario diferente";
            }



      
            var data = new
            {
                Mensaje = mensaje,
                ListaHorarioClasesEstudiante = listaHorarioEstudiante

            };



            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //HORARIO DE CLASES DESDE EXCEL 

        public ActionResult ingresarHorarioClasesEstudiantesExcel(HttpPostedFileBase excelFile)
        {
            conexionDataContext db = new conexionDataContext();
            metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new metodoHorarioClaseEstudiante();
            metodoDocente metodoDocente = new metodoDocente();

            metodoHorarioClases metodoHorarioClases = new metodoHorarioClases();
   
            List<horarioClaseEstudiante> listaHorarioEstudiante = new List<horarioClaseEstudiante>();


            if (excelFile == null || excelFile.ContentLength == 0)
            {
                listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();
                return Json(listaHorarioEstudiante, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {

                    string path = Server.MapPath("~/Content/" + excelFile.FileName);


                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excelFile.SaveAs(path);

                    //LEER DATOS DEL EXCEL

                    Excel.Application application = new Excel.Application();

                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;




                    for (var i = 2; i <= range.Rows.Count; i++)
                    {

                        horarioClaseEstudiante horarioClaseEstudiante = new horarioClaseEstudiante();

                        horarioClaseEstudiante.Matricula = ((Excel.Range)range.Cells[i, 1]).Text;
                        horarioClaseEstudiante.Docente = ((Excel.Range)range.Cells[i, 2]).Text;
                        horarioClaseEstudiante.Materia = ((Excel.Range)range.Cells[i, 3]).Text;
                        string NumMatricula = ((Excel.Range)range.Cells[i, 4]).Text;
                        string Paralelo = ((Excel.Range)range.Cells[i, 5]).Text;


                        if (horarioClaseEstudiante.Matricula != "")
                        {
                            var modulosMateria = db.horarioDocenteCedula(horarioClaseEstudiante.Docente).Where(x => x.CodigoMeteria == horarioClaseEstudiante.Materia);


                            foreach (var x in modulosMateria)
                            {

                                metodoHorarioClaseEstudiante.ingresarHorarioClaseEstudiante(horarioClaseEstudiante.Matricula, horarioClaseEstudiante.Docente, horarioClaseEstudiante.Materia, x.CodigoModulo, Paralelo);

                            }

                            db.IngresarRepitencia(int.Parse(horarioClaseEstudiante.Matricula), horarioClaseEstudiante.Materia, char.Parse(NumMatricula));



                        }

                    }
                  
                    workbook.Close(false, excelFile.FileName, null);
                    Marshal.FinalReleaseComObject(workbook);
                    application.Quit();



                    listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();

                    return Json(listaHorarioEstudiante, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();

                    return Json(listaHorarioEstudiante, JsonRequestBehavior.AllowGet);
                }
            }
        }



        public ActionResult horarioClaseEstudiante()
        {

            metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new metodoHorarioClaseEstudiante();

            List<horarioClaseEstudiante> listaHorarioEstudiante = new List<horarioClaseEstudiante>();
            listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();


            return PartialView("horarioClaseEstudiante",listaHorarioEstudiante);
        }

        public ActionResult eliminarHorarioClasesEstudiantes(string Matricula,string CodigoMateria)
        {
            conexionDataContext db = new conexionDataContext();
            metodoHorarioClaseEstudiante metodoHorarioClaseEstudiante = new metodoHorarioClaseEstudiante();

            var horariosClases = db.horarioClaseEstudianteMatricula(int.Parse(Matricula)).Where(x => x.CodigoMeteria == CodigoMateria);

            foreach (var x in horariosClases) {

                metodoHorarioClaseEstudiante.eliminarHorarioClaseEstudiante(Matricula, x.Id_HorarioClases.ToString());
            }
            db.EliminarRepitencia(int.Parse(Matricula),CodigoMateria);
            List<horarioClaseEstudiante> listaHorarioEstudiante = new List<horarioClaseEstudiante>();
            listaHorarioEstudiante = metodoHorarioClaseEstudiante.consultaHorariosEstudainte();

            return Json("Materia Eliminada", JsonRequestBehavior.AllowGet);
        }


        //----------------REPITENCIA-------------
        //----------------------------------------

        public ActionResult repitencia()
        {

            metodoRepitencia metodoRepitencia = new metodoRepitencia();

            List<repitencia> listaRepitencia = new List<repitencia>();
            listaRepitencia = metodoRepitencia.consultaRepitencias();

            return PartialView("repitenciaEstudiante",listaRepitencia);
        }



        public ActionResult ingresarRepitencia(repitencia RepitenciaDatos)
        {

            metodoRepitencia metodoRepitencia = new metodoRepitencia();

            metodoRepitencia.ingresarRepitencia(RepitenciaDatos);
            List<repitencia> listaRepitencia = new List<repitencia>();
            listaRepitencia = metodoRepitencia.consultaRepitencias();

            return Json(listaRepitencia,JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminarRepitencia(repitencia RepitenciaDatos)
        {

            metodoRepitencia metodoRepitencia = new metodoRepitencia();

            metodoRepitencia.eliminarRepitencia(RepitenciaDatos);
            List<repitencia> listaRepitencia = new List<repitencia>();
            listaRepitencia = metodoRepitencia.consultaRepitencias();

            return Json(listaRepitencia, JsonRequestBehavior.AllowGet);
        }

        //consulta el paralelolo de la materia
        public ActionResult consultaParaleloMateria(string CodigoMateria)
        {
           
            List<horarioClaseDocente> listaParalelosMateria = new List<horarioClaseDocente>();
            metodoHorarioClaseDocente metodoHorarioClaseDocente = new metodoHorarioClaseDocente();
            listaParalelosMateria = metodoHorarioClaseDocente.listaParalelosMateria(CodigoMateria);



            return Json(listaParalelosMateria, JsonRequestBehavior.AllowGet);
        }

        //consulta el doncente del paralelo de la materia

        public ActionResult consultaDocenteParalelo(string CodigoMateria,string Paralelo)
        {

            List<docente> listaDocentes = new List<docente>();
            metodoDocente metodoDocente = new metodoDocente();

            listaDocentes = metodoDocente.consultaDocenteParalelo(CodigoMateria, Paralelo);



            return Json(listaDocentes, JsonRequestBehavior.AllowGet);
        }

    }
}
