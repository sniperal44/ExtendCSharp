/*
 * Creato da SharpDevelop.
 * Utente: Alpa
 * Data: 13/06/2017
 * Ora: 10:43
 * 
 * Per modificare questo modello usa Strumenti | Opzioni | Codice | Modifica Intestazioni Standard
 */
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendCSharp.Services
{
	/// <summary>
	/// Description of EnumService.
	/// </summary>
	public class EnumService : IService
	{

        /// <summary>
        /// Ritorna un IEnumerable di TUPLE del'enum passato come template
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<String, T> GetDictionary<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(e => e.ToStringEnum());
        }

        public IEnumerable<T> GetIEnumerable<T>() where T : struct
        {
            foreach (T flag in Enum.GetValues(typeof(T)).Cast<T>())
            {
                yield return flag;
            }
        }
    }
}
