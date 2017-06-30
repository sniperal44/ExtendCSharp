using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Wrapper
{
    public class PenDataObject
    {

        Color _color;
        float _width;

        public Color color
        {
            get => _color;
            set {
                _color = value;
                RecreatePen();
            }
        }
        public float width
        {
            get => _width;
            set
            {
                _width = value;
                RecreatePen();
            }

        }


        private Pen _internalPen;
        [JsonIgnore]
        public Pen pen
        {
            get
            {
                return _internalPen;
                //TODO: attenzione! se uno fa la get, puoi modificare la penna interna! trovare un modo ( interfaccia, classe readonly... ) per imperdire
            }
        }

        public PenDataObject()
        {
        }
        public PenDataObject(Pen p)
        {
            if (p == null)
                return;
            color = p.Color;
            width = p.Width;
        }


        private void RecreatePen()
        {
            _internalPen = new Pen(_color, _width);
        }


        public static implicit operator PenDataObject(Pen p)
        {
            return new PenDataObject(p);
        }

        public static implicit operator Pen(PenDataObject p)
        {
            return p.pen;
        }
    }



    
}
