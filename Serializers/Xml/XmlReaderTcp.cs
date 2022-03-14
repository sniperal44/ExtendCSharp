using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ExtendCSharp.ExtendedClass;

namespace ExtendCSharp.Serializers.Xml
{
    class XmlReaderTcp : XmlReader
    {
        XmlReader inter = null;
        public XmlReaderTcp(NetworkStream s)
        {
            XmlReaderSettings sett = new XmlReaderSettings();
            sett.Async = true;
            sett.ConformanceLevel = ConformanceLevel.Fragment;


            if (!s.HasAttribute("StreamWrap"))
            {
                s.SetAttribute("StreamWrap", new NetworkStreamWrap(s));
            }
            NetworkStreamWrap wrap = s.GetAttribute<NetworkStreamWrap>("StreamWrap");
            inter = XmlReader.Create(wrap, sett);
        }

        public override XmlNodeType NodeType => inter.NodeType;

        public override string LocalName => inter.LocalName;

        public override string NamespaceURI => inter.NamespaceURI;

        public override string Prefix => inter.Prefix;

        public override string Value => inter.Value;

        public override int Depth => inter.Depth;

        public override string BaseURI => inter.BaseURI;

        public override bool IsEmptyElement => inter.IsEmptyElement;

        public override int AttributeCount => inter.AttributeCount;

        public override bool EOF => inter.EOF;

        public override ReadState ReadState => inter.ReadState;

        public override XmlNameTable NameTable => inter.NameTable;

        public override string GetAttribute(string name)
        {
            return inter.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return inter.GetAttribute(name, namespaceURI);
        }

        public override string GetAttribute(int i)
        {
            return inter.GetAttribute(i);
        }

        public override string LookupNamespace(string prefix)
        {
            return inter.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name)
        {
            return inter.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return inter.MoveToAttribute(name, ns);
        }

        public override bool MoveToElement()
        {
            return inter.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return inter.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return inter.MoveToNextAttribute();
        }

        public override bool Read()
        {
            return inter.Read();
        }

        public override bool ReadAttributeValue()
        {
            return inter.ReadAttributeValue();
        }

        public override void ResolveEntity()
        {
            inter.ResolveEntity();
        }
    }

}
