using ExtendCSharp.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendCSharp;

namespace ExtendCSharp.Controls
{
    public partial class CartesianPlan : PanelPlus
    {

        #region Variable

        int RefIndex = 0;
        Dictionary<int, CartesianAction> CartesianActions = new Dictionary<int, CartesianAction>();
    
        #endregion

        #region Constructor

        public CartesianPlan()
        {
            InitializeComponent();
        }
        public CartesianPlan(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        Point origin = new Point(0, 0);
        public PointReadOnly Origin
        {
            get { return origin; }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //e.Graphics.DrawRectangle(Pens.Black, GetBackgroundRect().Truncate());
            //e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0,0,2,2));
            //e.Graphics.DrawLine(Pens.Black, 0,0,2,2);

            e.Graphics.ScaleTransform(1, -1);
            e.Graphics.TranslateTransform(0, -Height);
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
            ID++;

            Invalidate();
            return ID;
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

    }


    public class CartesianAction
    {
        public PenDataObject DefaultPen = Pens.Black;
        public virtual void Paint(Graphics g, PenDataObject pen = null)
        {
            
        }
    }

    public class CartesianActionLine:CartesianAction
    {
        public float X1, X2, Y1, Y2;

        public PenDataObject pen;
        

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


    public class CartesianActionLines : CartesianAction
    {
    }


    public class CartesianActionArc : CartesianAction
    {
    }
    public class CartesianActionCircle : CartesianAction
    {
    }
    public class CartesianActionEllipse : CartesianAction
    {
    }
    public class CartesianActionIcon : CartesianAction
    {
    }
    public class CartesianActionImage : CartesianAction
    {
    }
    public class CartesianActionPolygon : CartesianAction
    {
    }
    public class CartesianActionRectangle : CartesianAction
    {
    }
    public class CartesianActionRectangles : CartesianAction
    {
    }
    public class CartesianActionString : CartesianAction
    {
    }

    public class CartesianActionGROUP : CartesianAction
    {
        //permette di raggruppare altri CartesianAction
    }
}
