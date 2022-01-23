using ExtendCSharp.Services;
using ExtendCSharp.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public partial class CartesianPlan : PanelPlus
    {

        #region Variable

        int RefIndex = 0;
        Dictionary<int, CartesianAction> CartesianActions = new Dictionary<int, CartesianAction>();

        Point origin = new Point(0, 0);

             
        CartesianLayout _cartesianLayout = CartesianLayout.Stretch;
        float XScale = 1;
        float YScale = 1;

        float? _XMax = null;
        float? _YMax = null;

        
        #endregion


        #region Proprierty
        public PointReadOnly Origin
        {
            get { return origin; }
        }

        public float? XMax
        {
            get => _XMax;
            set
            {
                _XMax = value;
              

                if(_cartesianLayout==CartesianLayout.Zoom)
                {
                    RecalculateScale();
                }
                else
                {
                    if (_XMax == null)
                        XScale = 1;
                    else
                        XScale = (float)(InternalWidth / _XMax);
                }
            }
        }
        public float? YMax
        {
            get => _YMax;
            set
            {
                _YMax = value;
                

                if (_cartesianLayout == CartesianLayout.Zoom)
                {
                    RecalculateScale();
                }
                else
                {
                    if (_YMax == null)
                        YScale = 1;
                    else
                        YScale = (float)(InternalHeight / _YMax);
                }
            }
        }
        public CartesianLayout cartesianLayout
        {
            get => _cartesianLayout;
            set
            {
                _cartesianLayout = value;
                if(_cartesianLayout==CartesianLayout.Stretch)
                {
                    //Ricalcolo le scale
                    if (_YMax == null)
                        YScale = 1;
                    else
                        YScale = (float)(InternalHeight / _YMax);

                    if (_XMax == null)
                        XScale = 1;
                    else
                        XScale = (float)(InternalWidth / _XMax);


                }
                if (_cartesianLayout == CartesianLayout.Zoom)
                {
                    //Ricalcolo le scale
                    if (_YMax == null)
                        YScale = 1;
                    else
                        YScale = (float)(InternalHeight / _YMax);

                    if (_XMax == null)
                        XScale = 1;
                    else
                        XScale = (float)(InternalWidth / _XMax);

                    YScale=Math.Min(YScale, XScale);
                    XScale = YScale;

                }
            }
        }

        public bool ManualInvalidation { get; set; } = false;
        #endregion
        #region Constructor

        public CartesianPlan()
        {
            InitializeComponent();
            Resize += CartesianPlan_Resize;
        }

       
        public CartesianPlan(IContainer container):this()
        {
            container.Add(this);
        }

        #endregion




        private void RecalculateScale()
        {
            if (_XMax == null)
                XScale = 1;
            else
                XScale = (float)(InternalWidth / _XMax);

            if (_YMax == null)
                YScale = 1;
            else
                YScale = (float)(InternalHeight / _YMax);

            if (_cartesianLayout == CartesianLayout.Zoom)
            {
                YScale = Math.Min(YScale, XScale);
                XScale = YScale;
            }
        }
        private void CartesianPlan_Resize(object sender, EventArgs e)
        {
            RecalculateScale();
            InternalInvalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            
            base.OnPaint(e);

            //e.Graphics.DrawRectangle(Pens.Black, GetBackgroundRect().Truncate());
            //e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0,0,2,2));
            //e.Graphics.DrawLine(Pens.Black, 0,0,2,2);

            e.Graphics.ScaleTransform(1, -1);
            e.Graphics.TranslateTransform(0, -Height);
            e.Graphics.ScaleTransform(XScale, YScale);
            e.Graphics.TranslateTransform(origin.X, origin.Y);
            


            foreach ( KeyValuePair<int,CartesianAction> kca in CartesianActions)
            {
                kca.Value.Paint(e.Graphics);
            }


            
            
        }



        public int AddAction(CartesianAction ca)
        {
            CartesianActions.Add(RefIndex, ca);
            int ID = RefIndex;
            RefIndex++;

            InternalInvalidate();
            return ID;
        }
        public bool RemoveAction(CartesianAction ca)
        {
            if (CartesianActions.ContainsValue(ca))
            {
                CartesianActions.Remove(ca);
                InternalInvalidate();
                return true;
            }
            return false;
        }
        public bool RemoveAction(int index)
        {
            if (CartesianActions.ContainsKey(index))
            {
                CartesianActions.Remove(index);
                InternalInvalidate();
                return true;
            }
            return false;

        }
        public void ClearAction()
        {
            CartesianActions.Clear();
        }





        public void SetRelativeOrigin(Size RelativeOrigin)
        {
            origin.X += RelativeOrigin.Width;
            origin.Y += RelativeOrigin.Height;
        }
        public void SetAbsoluteOrigin(Point AbsoluteOrigin)
        {
            origin.X = AbsoluteOrigin.X;
            origin.Y = AbsoluteOrigin.Y;
        }
        public void SetOriginAtBackground()
        {
            Rectangle rect= GetBackgroundRect();
            if( rect != Rectangle.Empty)
            {
                SetAbsoluteOrigin(ConvertToCartesian(rect).Location.Add(0,1));
            }
        }

        /// <summary>
        /// Converte un punto relativo al CartesianPlan in punto PER il CartesianPlan
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point ConvertToCartesian(Point p)
        {
            return new Point(p.X, InternalHeight - p.Y);
        }
        public PointF ConvertToCartesian(PointF p)
        {
            return new PointF(p.X, InternalHeight - p.Y);
        }


        public Rectangle ConvertToCartesian(Rectangle r)
        {
            Point t = ConvertToCartesian(r.Location);
            t.Y -= r.Height;

            return new Rectangle(t,r.Size);
        }
        public RectangleF ConvertToCartesian(RectangleF r)
        {
            PointF t = ConvertToCartesian(r.Location);
            t.Y -= r.Height;

            return new RectangleF(t, r.Size);
        }

        /// <summary>
        /// Permette di covertire un point relativo al control CartesianPlan in un Point relativo al background image a dimensioni reali in modalità cartesiana
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point ConvertToCartesianBackground(Point p)
        {
            Point t = ConvertToCartesian(p);
            Rectangle rect=GetBackgroundRect();

            t.X -= rect.X;
            t.Y -= rect.Y;

            t.X = (int)(t.X/GetBackgroundWidthProportion());
            t.Y = (int)(t.Y / GetBackgroundHeightProportion());

            return t;
        }
        public PointF ConvertToCartesianBackground(PointF p)
        {
            PointF t = ConvertToCartesian(p);
            RectangleF rect = GetBackgroundRect();

            t.X -= rect.X;
            t.Y -= rect.Y;

            t.X = (float)(t.X * GetBackgroundWidthProportion());
            t.Y = (float)(t.Y * GetBackgroundWidthProportion());

            return t;
        }


        //TODO: controllo
        public Rectangle ConvertToCartesianBackground(Rectangle r)
        {
            Point t = ConvertToCartesianBackground(r.Location);
            t.Y -= r.Height;

            Size s = r.Size;
            s.Width = (int)(s.Width/GetBackgroundWidthProportion());
            s.Height = (int)(s.Width / GetBackgroundWidthProportion());

            return new Rectangle(t, s);
        }
        public RectangleF ConvertToCartesianBackground(RectangleF r)
        {
            PointF t = ConvertToCartesianBackground(r.Location);
            t.Y -= r.Height;

            SizeF s = r.Size;
            s.Width = (float)(s.Width / GetBackgroundWidthProportion());
            s.Height = (float)(s.Width / GetBackgroundWidthProportion());

            return new RectangleF(t, s);
        }



        private void InternalInvalidate()
        {
            if(!ManualInvalidation)
            {
                base.Invalidate();
            }
        }

    }


    public abstract class CartesianAction
    {
        protected PenDataObject DefaultPen = Pens.Black;
        public PenDataObject pen=null;
        public abstract void Paint(Graphics g, PenDataObject pen = null);
    }

    public class CartesianActionLine:CartesianAction
    {
        public float X1, X2, Y1, Y2;
        
        

        public CartesianActionLine(float X1, float Y1, float X2, float Y2)
        {
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }
        public CartesianActionLine(int X1,int Y1,int X2,int Y2)
        {
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }
        public CartesianActionLine(Point p1, Point p2):this(p1.X,p1.Y,p2.X,p2.Y)
        {

        }
        public CartesianActionLine(PointF p1, PointF p2) : this(p1.X, p1.Y, p2.X, p2.Y)
        {

        }

        

        public override void Paint(Graphics g,PenDataObject pen=null)
        {
            Pen p;
            if (pen != null)
                p = pen;
            else if (this.pen != null)
                p = this.pen;
            else
                p = DefaultPen;


            g.DrawLine(p, X1, Y1, X2, Y2);
        }
    }
    public class CartesianActionCircle : CartesianAction
    {

        public float  radius;
        public PointF Center;
       

        public CartesianActionCircle(float X, float Y, float radius)
        {
            Center = new PointF(X, Y);
            this.radius = radius;
        }

        public CartesianActionCircle(PointF p, float radius) : this(p.X, p.Y, radius)
        {

        }
        public CartesianActionCircle(Point p, float radius) : this(p.X, p.Y, radius)
        {

        }


        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            Pen p;
            if (pen != null)
                p = pen;
            else if (this.pen != null)
                p = this.pen;
            else
                p = DefaultPen;


            g.DrawCircle(p,Center, radius);
        }
    }
    public class CartesianActionRectangle : CartesianAction
    {
        public float X, Y, width,height;


        public CartesianActionRectangle(float X,float Y,float Width,float Height) 
        {
            this.X = X;
            this.Y = Y;
            this.width = Width;
            this.height = Height;
        }
        public CartesianActionRectangle(RectangleF rectangle) : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
        {
            

        }

        public CartesianActionRectangle(PointF BottomLeft, Size size) : this(BottomLeft.X,BottomLeft.Y,size.Width,size.Height)
        {

        }



        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            Pen p;
            if (pen != null)
                p = pen;
            else if (this.pen != null)
                p = this.pen;
            else
                p = DefaultPen;

            g.DrawRectangle(p, X, Y, width, height);
        }

        public RectangleF GetRect()
        {
            return new RectangleF(X, Y, width, height);
        }
    }



    public class CartesianActionArc : CartesianAction
    {
        public RectangleF rect;
        public Point Center;
        public float startAngle, sweepAngle;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Center">Punto centrale dell'arco</param>
        /// <param name="radius">Raggio dell'arco</param>
        /// <param name="StartPoint">Punto iniziale dell'arco</param>
        /// <param name="EndPoint">Punto Finale dell'arco</param>
        /// <param name="Direzione">Direzione dell'arco</param>
        public CartesianActionArc(PointF Center,float radius, PointF StartPoint, PointF EndPoint, AngleOrientation Direzione)//:this(center.CreateRectangle(radius),StartPoint,EndPoint)
        {

            MathService ms = ServicesManager.GetOrSet(() => { return new MathService(); });
            RectangleF rect = Center.CreateRectangle(radius);
            float startAngle = ms.NormalizeAngle((float)Center.Orientamento(StartPoint));
            float endAngle = ms.NormalizeAngle((float)Center.Orientamento(EndPoint));

            if (Direzione == AngleOrientation.Antiorario && endAngle < startAngle)
                endAngle += 360;

            if (Direzione == AngleOrientation.Orario && startAngle < endAngle)
                startAngle += 360;
            sweepAngle = endAngle - startAngle;
            Setup(rect, startAngle, sweepAngle);

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Coordinata X dell'angolo inferiore sinistro del rettangolo che definisce l'arco. </param>
        /// <param name="y">Coordinata Y dell'angolo superiore sinistro del rettangolo che definisce l'arco.</param>
        /// <param name="width">Larghezza del rettangolo che definisce l'arco.</param>
        /// <param name="height">Altezza del rettangolo che definisce l'arco.</param>
        /// <param name="startAngle">Angolo misurato in gradi in senso orario dall'asse X al punto iniziale dell'arco.</param>
        /// <param name="sweepAngle">Angolo misurato in gradi in senso orario dal parametro startAngle al punto finale dell'arco.</param>
        public CartesianActionArc( float x, float y, float width, float height, float startAngle, float sweepAngle):this(new RectangleF(x,y,width,height),startAngle,sweepAngle)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">Struttura System.Drawing.RectangleF che definisce i limiti dell'arco.</param>
        /// <param name="startAngle">Angolo misurato in gradi in senso orario dall'asse X al punto iniziale dell'arco.</param>
        /// <param name="sweepAngle">Angolo misurato in gradi in senso orario dal parametro startAngle al punto finale dell'arco.</param>
        public CartesianActionArc( RectangleF rect, float startAngle, float sweepAngle)
        {
            Setup(rect, startAngle, sweepAngle);
        
        }

        private void Setup(RectangleF rect, float startAngle, float sweepAngle)
        {
            this.rect = rect;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;

        }


        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            Pen p;
            if (pen != null)
                p = pen;
            else if (this.pen != null)
                p = this.pen;
            else
                p = DefaultPen;



            g.DrawArc(p, rect, startAngle, sweepAngle);
        }


        
    }



    //TODO: termino le CartesianAction

    public class CartesianActionLines : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }


    

    public class CartesianActionEllipse : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }
    public class CartesianActionIcon : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }
    public class CartesianActionImage : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }
    public class CartesianActionPolygon : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }
    public class CartesianActionRectangles : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }
    public class CartesianActionString : CartesianAction
    {
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }

    public class CartesianActionGROUP : CartesianAction
    {
        //permette di raggruppare altri CartesianAction
        public override void Paint(Graphics g, PenDataObject pen = null)
        {
            throw new NotImplementedException();
        }
    }




    public enum CartesianLayout
    {
        Stretch,
        Zoom
    }
    public enum AngleOrientation
    {
        Orario,
        Antiorario
    }

}
