/*
 * Creato da SharpDevelop.
 * Utente: Alpa
 * Data: 13/06/2017
 * Ora: 10:09
 * 
 * Per modificare questo modello usa Strumenti | Opzioni | Codice | Modifica Intestazioni Standard
 */

using ExtendCSharp.Exceptions;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;

namespace ExtendCSharp.Services
{
    /// <summary>
    /// Description of ServicesManager.
    /// </summary>
    public static class ServicesManager
    {
        static readonly Dictionary<Type, Object> services = new Dictionary<Type, Object>();

        public static void RegistService<T>(T Service) where T : IService
        {
            services.Add(typeof(T), Service);
        }

        public static T Get<T>() where T : IService
        {
            if (!services.ContainsKey(typeof(T)))
                throw new MissingServiceException(typeof(T));

            return (T)services[typeof(T)];
        }
        public static bool Remove<T>() where T : IService
        {

            return services.Remove(typeof(T));
        }

        public static bool IsSet<T>() where T : IService
        {
            return services.ContainsKey(typeof(T));
        }

        public static T GetOrSet<T>(Func<T> Generatore) where T : IService
        {
            if (!ServicesManager.IsSet<T>())
                ServicesManager.RegistService(Generatore());
            return  ServicesManager.Get<T>();
        }


        

    }
	
}
