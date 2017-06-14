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
        public IEnumerable<Tuple<T, String>> GetIEnumerable<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new Tuple<T, String>(e, e.ToStringEnum()));
 
        }
      


    }
}
