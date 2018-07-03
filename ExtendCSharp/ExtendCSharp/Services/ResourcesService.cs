using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ExtendCSharp.Services
{
    public class ResourcesService: IService
    {
        private Assembly targetAssembly;

        private Dictionary<Type, ResourceParser> parsers;

        public ResourcesService(Assembly targetAssembly)
        {
            this.targetAssembly = targetAssembly;


            //Inizializa parser
            parsers = new Dictionary<Type, ResourceParser>();
            RegisterParser<String>(new ResourceStringParse());
            RegisterParser<Image>(new ResourceImageParse());


        }
        /// <summary>
        /// Ritorna uno stream ad una RISORSA INCORPORATA ( proprietà della risorsa -> "Azione di compilazione" = Risorsa incorporata )
        /// </summary>
        /// <param name="assembly">assembly dove recuperare la risorsa 
        /// <para /> System.Reflection.Assembly.GetExecutingAssembly() -> assembly corrente
        /// <para />System.Reflection.Assembly.GetEntryAssembly() -> primo assembly eseguito
        /// </param>
        /// <param name="ResourcePath">Path della RISORSA INCORPORATA nello stream.
        /// <para /> 
        /// <para />Esempio:
        /// <para />extendCSharpTest.Gif.test.gif
        /// <para />extendCSharpTest = Nome del progetto ( assembly di esecuzione ) 
        /// <para />Gif = sottocartella ( aggiungere altre sottocartelle separate dal . ) 
        /// <para />test.gif = nome del file
        /// </param>
        /// <returns></returns>
        public Stream GetStream(String ResourcePath)
        {
            return targetAssembly.GetManifestResourceStream(ResourcePath);
        }


        public T GetObject<T>( String ResourcePath)
        {
            ResourceParser rp = GetParser<T>();
            if( rp==null)
            {
                throw new NotImplementedException("Parser corrispondente non implementato");
            }
            else
            {
                T temp;
                using (Stream s = GetStream(ResourcePath))
                {
                    temp= rp.Parse(s)._Cast<T>();
                }
                return temp;
            }
        }


        public void RegisterParser<T>(ResourceParser parser)
        {
            parsers.Add(parser.GetType(), parser);
        }
        private ResourceParser GetParser<T>()
        {
            if (parsers.ContainsKey(typeof(T)))
                return parsers[typeof(T)];
            return null;
        }
    }



    public abstract class ResourceParser
    {
        Type resourceType;
        protected ResourceParser(Type resourceType)
        {
            this.resourceType = resourceType;
        }
        public abstract object Parse(Stream s);

        public new Type GetType()
        {
            return resourceType;
        }
    }

    public class ResourceStringParse : ResourceParser
    {
        public ResourceStringParse():base(typeof(String))
        {

        }
        protected ResourceStringParse(Type resourceType) : base(resourceType)
        {
        }

        public override object Parse(Stream s)
        {
            string text;
            using (StreamReader reader = new StreamReader(s))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
    public class ResourceImageParse : ResourceParser
    {
        public ResourceImageParse():base(typeof(Image))
        {

        }
        protected ResourceImageParse(Type resourceType) : base(resourceType)
        {
        }

        public override object Parse(Stream s)
        {
            return Image.FromStream(s);  
        }
    }


}
