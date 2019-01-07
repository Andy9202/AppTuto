using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AppTutorias.Models
{
    public class ADALTokenCache : TokenCache
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId;
        private UserTokenCache Cache;

        public ADALTokenCache(string signedInUserId)
        {
            // asociar la caché al usuario actual de la aplicación web
            userId = signedInUserId;
            this.AfterAccess = AfterAccessNotification;
            this.BeforeAccess = BeforeAccessNotification;
            this.BeforeWrite = BeforeWriteNotification;
            // buscar la entrada en la base de datos
            Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            // colocar la entrada en la memoria
            this.Deserialize((Cache == null) ? null : MachineKey.Unprotect(Cache.cacheBits,"ADALCache"));
        }

        // limpiar la base de datos
        public override void Clear()
        {
            base.Clear();
            var cacheEntry = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            db.UserTokenCacheList.Remove(cacheEntry);
            db.SaveChanges();
        }

        // Se genera una notificación antes de que ADAL acceda a la caché.
        // Es tu oportunidad para actualizar la copia almacenada en la memoria desde la base de datos, si la versión de la memoria es obsoleta
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            if (Cache == null)
            {
                // primer acceso
                Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            }
            else
            { 
                // recuperar la última escritura de la base de datos
                var status = from e in db.UserTokenCacheList
                             where (e.webUserUniqueId == userId)
                select new
                {
                    LastWrite = e.LastWrite
                };

                // si la copia almacenada en la memoria es más antigua que la copia persistente
                if (status.First().LastWrite > Cache.LastWrite)
                {
                    // leer del almacenamiento, actualizar la memoria almacenada en la memoria
                    Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
                }
            }
            this.Deserialize((Cache == null) ? null : MachineKey.Unprotect(Cache.cacheBits, "ADALCache"));
        }

        // Se genera una notificación después de que ADAL acceda a la caché.
        // Si se establece la marca HasStateChanged, ADAL cambia el contenido de la caché
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // si ha cambiado el estado
            if (this.HasStateChanged)
            {
                if (Cache == null)
                {
                    Cache = new UserTokenCache
                    {
                        webUserUniqueId = userId
                    };
                }

                Cache.cacheBits = MachineKey.Protect(this.Serialize(), "ADALCache");
                Cache.LastWrite = DateTime.Now;

                // actualice la base de datos y la última escritura 
                db.Entry(Cache).State = Cache.UserTokenCacheId == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                this.HasStateChanged = false;
            }
        }

        void BeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // si quieres asegurarte de que no se produce ninguna escritura concurrente, utiliza esta notificación para bloquear la entrada
        }

        public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
        }
    }
}
