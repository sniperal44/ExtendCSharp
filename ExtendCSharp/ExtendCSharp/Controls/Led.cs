using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class Led : UserControl
    {
        LedType _type = LedType.Ellisse;
        public LedType ledType
        {
            get => _type;
            set => _type = value;
        }


        SolidBrush _ColorAcceso = new SolidBrush(Color.Green);
        public Color ColorAcceso
        {
            get => _ColorAcceso.Color;
            set
            {
                _ColorAcceso =new SolidBrush(value);
                Invalidate();
            }
        }
        SolidBrush _ColorSpento = new SolidBrush(Color.DarkGreen);
        public Color ColorSpento
        {
            get => _ColorSpento.Color;
            set
            {
                _ColorSpento = new SolidBrush(value);
                Invalidate();
            }
        }
        SolidBrush _ColorBorder = new SolidBrush(Color.DarkOliveGreen);
        public Color ColorBorder
        {
            get => _ColorBorder.Color;
            set
            {
                _ColorBorder =  new SolidBrush(value);
                Invalidate();
            }
        }

        int _BorderWidth = 0;
        public int BorderWidth
        {
            get => _BorderWidth;
            set
            {
                _BorderWidth = value;
                Invalidate();
            }
        }


        bool _Acceso = false;
        public bool Acceso
        {
            get => _Acceso;
            set => _Acceso = value;
        }



        public Led(LedType type)
        {
            InitializeComponent();
            this._type = type;
        }

        //TODO: finisco la pain del Led
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if( BorderWidth!=0)
            {
                if(_type==LedType.Ellisse)
                {
                    e.Graphics.FillEllipse(_ColorBorder, this.Bounds);
                }
                else if (_type == LedType.Quadrato)
                {
                    e.Graphics.FillRectangle(_ColorBorder, this.Bounds);
                }

            }



            if (_type == LedType.Ellisse)
            {
                e.Graphics.FillEllipse(_ColorBorder, this.Bounds.X+_BorderWidth);
            }
            else if (_type == LedType.Quadrato)
            {
                e.Graphics.FillRectangle(_ColorBorder, this.Bounds);
            }

        }


    }



    public enum LedType
    {
        Ellisse,
        Quadrato
    }
}
