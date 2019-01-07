using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppTutorias.Models.SuperAdmin.Docente;
using Microsoft.AspNet.SignalR;

namespace AppTutorias
{
    public class Notificaciones : Hub
    {
        conexionDataContext db = new conexionDataContext();



        public void mensajeEstudiante()
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<Notificaciones>();
            context.Clients.All.notificacionDocente("Tu mensaje fue enviado");
            
           

        }

        public void mensajeDocente(string mensaje)
        {

        }
    }
}