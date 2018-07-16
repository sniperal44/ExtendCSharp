/*
 * Creato da SharpDevelop.
 * Utente: Alpa
 * Data: 13/06/2017
 * Ora: 10:59
 * 
 * Per modificare questo modello usa Strumenti | Opzioni | Codice | Modifica Intestazioni Standard
 */
using System;

namespace ExtendCSharp.Exceptions
{
    /// <summary>
    /// Description of MissingServiceException.
    /// </summary>
    public class MissingServiceException :Exception
	{
        Type t;
		public MissingServiceException(Type t):base("Il servizio " + t.ToString() + " è stato chiamato senza essere registrato.")
		{
            this.t = t;
        }

    }
}
