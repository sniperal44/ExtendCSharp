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
            set
            {
                _type = value;
                Invalidate();
            }
        }


        SolidBrush _ColorAcceso = new SolidBrush(Color.LawnGreen);
        public Color ColorAcceso
        {
            get => _ColorAcceso.Color;
            set
            {
                _ColorAcceso =new SolidBrush(value);
                Invalidate();
            }
        }
        SolidBrush _ColorSpento = new SolidBrush(Color.ForestGreen);
        public Color ColorSpento
        {
            get => _ColorSpento.Color;
            set
            {
                _ColorSpento = new SolidBrush(value);
                Invalidate();
            }
        }
        SolidBrush _ColorBorder = new SolidBrush(Color.DarkGreen);
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
            set
            {
                _Acceso = value;
                Invalidate();
            }
        }


        public Led()
        {
            InitializeComponent();
            Invalidate();

        }
        public Led(LedType type):this()
        {
            
            this._type = type;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Brush FillBrush = _Acceso ? _ColorAcceso : _ColorSpento;
            FillByData borderFunc = null;
            FillByData fillfunc = null;
            if (_type == LedType.Ellisse)
            {
                borderFunc = e.Graphics.FillEllipse;
                fillfunc = e.Graphics.FillEllipse;
            }
            else if (_type == LedType.Quadrato)
            {
                borderFunc = e.Graphics.FillRectangle;
                fillfunc = e.Graphics.FillRectangle;
            }


            if ( BorderWidth!=0)
            {
                borderFunc(_ColorBorder, 0,0, Width, Height);
            }

            fillfunc(FillBrush, _BorderWidth, _BorderWidth, Width - (_BorderWidth*2), Height - (_BorderWidth*2));
        }


    }


    public delegate void FillByData(Brush b, float x, float y, float width, float heigth);

    public enum LedType
    {
        Ellisse,
        Quadrato
    }
}
