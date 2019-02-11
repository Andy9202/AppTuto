using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using AppTutorias.Models;
using AppTutorias.Metodos;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace AppTutorias.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            // Enviar una solicitud de inicio de sesión a OpenID Connect.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/Account/Usuario" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {

                // Redirigir a la página principal si el usuario está autenticado.
                return RedirectToAction("Usuario", "Account");
            }

            return View();
        }

        [Authorize]
        public ActionResult Usuario()
        {
            if (Request.IsAuthenticated)
            {

                loginUsuario user = new loginUsuario();
                user.EmailUsuario = User.Identity.Name;

                metodoLoginUsuario metodoLoginUsuario = new metodoLoginUsuario();
                masterDocente masterUsuario = new masterDocente();


                masterUsuario = metodoLoginUsuario.docenteLogueado(user);

                

                if (masterUsuario.rol.nombreRol == "DOCENTE")
                {
                    Session["usuarioLogueado"] = masterUsuario;
                    //Session["nombreUsuario"] = masterUsuario.docente.Nombre + " " + masterUsuario.docente.NombreDos + " " + masterUsuario.docente.Apellido + " " + masterUsuario.docente.ApellidoDos;
                    Session["nombreUsuario"] = 
                    Session["correoUsuario"] = masterUsuario.loginUsuario.EmailUsuario;
                    Session["cedula"] = masterUsuario.docente.Cedula;



                    return RedirectToAction("Index", "Docente");

                }

                masterSuperAdmin masterSuperAdmin = new masterSuperAdmin();
                masterSuperAdmin = metodoLoginUsuario.SuperAdminLogueado(user);

                if (masterSuperAdmin.rol.nombreRol == "SUPERADMIN")
                {
                    Session["superAdmin"] = masterSuperAdmin;
                    User.IsInRole("SUPERADMIN");

                    return RedirectToAction("Index", "SuperAdmin");

                }

                masterAdmin masterAdmin = new masterAdmin();
                masterAdmin = metodoLoginUsuario.AdminLogueado(user);

                if (masterAdmin.rol.nombreRol == "ADMIN")
                {

                    User.IsInRole("ADMIN");
                    return RedirectToAction("Index", "Admin");
                }

                masterEstudiante masterEstudiante = new masterEstudiante();
                masterEstudiante = metodoLoginUsuario.EstudianteLogueado(user);

                if (masterEstudiante.rol.nombreRol == "ESTUDIANTE")
                {
                    Session["email"] = masterEstudiante.estudiante.Email;
                    Session["matricula"] = masterEstudiante.estudiante.Matricula;

                    User.IsInRole("ESTUDIANTE");
                    return RedirectToAction("Index", "Estudiante");
                }

                return RedirectToAction("SignOut");

            }
            return RedirectToAction("SignIn");

        }

    }
}
