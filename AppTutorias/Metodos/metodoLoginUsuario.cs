using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppTutorias.Models;
using AppTutorias.Models.Admin;
using AppTutorias.Models.SuperAdmin;
using AppTutorias.Models.SuperAdmin.Docente;
using AppTutorias.Models.SuperAdmin.Estudiante;

namespace AppTutorias.Metodos
{
    public class metodoLoginUsuario
    {
        conexionDataContext db = new conexionDataContext();
        //validar
        //public bool validarLogin(loginUsuario user)
        //{
        //    bool valido = false;

        //    int? resultado = 0;

        //    db.validarDocente(user.EmailUsuario, user.Pass, ref resultado);

        //    if (resultado == 1)
        //    {
        //        valido = true;
        //    }

        //    return valido;

        //}


        public masterDocente docenteLogueado(loginUsuario user)
        {
            var docente = from ro in db.ROL
                          from usu in db.DOCENTE
                          where usu.Id_Rol == ro.Id_Rol
                          where usu.Email == user.EmailUsuario
                          select new
                          {
                              ro.Rol,
                              usu.Nombre,
                              usu.NombreDos,
                              usu.Apellido,
                              usu.ApellidoDos,
                              usu.Email,
                              usu.Cedula
                          };

            masterDocente master = new masterDocente();
            rol rol = new rol();
            docente userNUevo = new docente();
            loginUsuario login = new loginUsuario();

            foreach (var consulta in docente)
            {
                rol.nombreRol = consulta.Rol;
                userNUevo.Nombre = consulta.Nombre;
                userNUevo.NombreDos = consulta.NombreDos;
                userNUevo.Apellido = consulta.Apellido;
                userNUevo.ApellidoDos = consulta.ApellidoDos;
                userNUevo.Cedula = consulta.Cedula;
                login.EmailUsuario = consulta.Email;

            }

            master.rol = rol;
            master.docente= userNUevo;
            master.loginUsuario = login;


            return master;
        }


        //SUPERADMIN LOGUEADO

        public masterSuperAdmin SuperAdminLogueado(loginUsuario user)
        {
            var superAdmin = from ro in db.ROL
                          from usu in db.SUPERADMIN
                          where usu.Id_Rol == ro.Id_Rol
                          where usu.Email == user.EmailUsuario
                          select new
                          {
                              ro.Rol,
                              usu.Nombre,
                              usu.NombreDos,
                              usu.Apellido,
                              usu.ApellidoDos,
                              usu.Email

                          };

            masterSuperAdmin master = new masterSuperAdmin();
            rol rol = new rol();
            superAdmin userNUevo = new superAdmin() ;
            loginUsuario login = new loginUsuario();

            foreach (var consulta in superAdmin)
            {
                rol.nombreRol = consulta.Rol;
                userNUevo.Nombre = consulta.Nombre;
                userNUevo.NombreDos = consulta.NombreDos;
                userNUevo.Apellido = consulta.Apellido;
                userNUevo.ApellidoDos = consulta.ApellidoDos;
                login.EmailUsuario = consulta.Email;

            }

            master.rol = rol;
            master.superAdmin = userNUevo;
            master.loginUsuario = login;


            return master;
        }


        //ADMIN LOGUEADO

        public masterAdmin AdminLogueado(loginUsuario user)
        {
            var admin = from ro in db.ROL
                          from usu in db.ADMINIS
                          where usu.Id_Rol == ro.Id_Rol
                          where usu.Email == user.EmailUsuario
                          select new
                          {
                              ro.Rol,
                              usu.Nombre,
                              usu.NombreDos,
                              usu.Apellido,
                              usu.ApellidoDos,
                              usu.Email

                          };

            masterAdmin master = new masterAdmin();
            rol rol = new rol();
            admin userNUevo = new admin();
            loginUsuario login = new loginUsuario();

            foreach (var consulta in admin)
            {
                rol.nombreRol = consulta.Rol;
                userNUevo.Nombre = consulta.Nombre;
                userNUevo.NombreDos = consulta.NombreDos;
                userNUevo.Apellido = consulta.Apellido;
                userNUevo.ApellidoDos = consulta.ApellidoDos;
                login.EmailUsuario = consulta.Email;

            }

            master.rol = rol;
            master.admin = userNUevo;
            master.loginUsuario = login;


            return master;
        }

        //ESTUDIANTE LOGUEADO

        public masterEstudiante EstudianteLogueado(loginUsuario user)
        {
            var estudiant = from ro in db.ROL
                          from usu in db.ESTUDIANTE
                          where usu.Id_Rol == ro.Id_Rol
                          where usu.Email == user.EmailUsuario
                          select new
                          {
                              ro.Rol,
                              usu.Nombre,
                              usu.NombreDos,
                              usu.Apellido,
                              usu.ApellidoDos,
                              usu.Email,
                              usu.Matricula
  

                          };

            masterEstudiante master = new masterEstudiante();
            rol rol = new rol();
            estudiante userNUevo = new estudiante();
            loginUsuario login = new loginUsuario();

            foreach (var consulta in estudiant)
            {
                rol.nombreRol = consulta.Rol;
                userNUevo.Nombre = consulta.Nombre;
                userNUevo.NombreDos = consulta.NombreDos;
                userNUevo.Apellido = consulta.Apellido;
                userNUevo.ApellidoDos = consulta.ApellidoDos;
                login.EmailUsuario = consulta.Email;
                userNUevo.Email = consulta.Email;
                userNUevo.Matricula = consulta.Matricula.ToString();

            }

            master.rol = rol;
            master.estudiante = userNUevo;
            master.loginUsuario = login;


            return master;
        }

    }
}
