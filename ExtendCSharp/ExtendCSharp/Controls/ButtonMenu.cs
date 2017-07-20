using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;


namespace ExtendCSharp.Controls
{
    public partial class ButtonMenu : UserControl
    {
        //TODO: testo il ButtonWrapper
        String DownArrow = "▼";
        String UpArrow = "▲";

        ContextMenuStripStatus MenuContextStatus = ContextMenuStripStatus.Close;


        ContextMenuStrip _contextMenuStrip = null;
        bool ClosedLunchedByButton = false;

        public ContextMenuStrip MenuContext {
            get
            {
                return _contextMenuStrip;
            }
            set
            {
                _contextMenuStrip = value;
                if (_contextMenuStrip != null)
                {
                    _contextMenuStrip.ItemRemoved += _contextMenuStrip_ItemAdded_Removed;
                    _contextMenuStrip.ItemAdded += _contextMenuStrip_ItemAdded_Removed;
                    _contextMenuStrip.Opened += ContextMenuStrip_Opened;
                    _contextMenuStrip.Closed += ContextMenuStrip_Closed;
                    _contextMenuStrip.Closing += _contextMenuStrip_Closing;
                }
            }

        }

        private void _contextMenuStrip_ItemAdded_Removed(object sender, ToolStripItemEventArgs e)
        {
            if (_contextMenuStrip == null || _contextMenuStrip.Items.Count == 0)
                button_arrow.Disable();
            else
                button_arrow.Enable();
        }


        public ButtonMenuWrapper buttonMenu = null;
       

        
        public ButtonMenu()
        {
            InitializeComponent();
            buttonMenu = new ButtonMenuWrapper(button_int);
            if (_contextMenuStrip == null || _contextMenuStrip.Items.Count == 0)
                button_arrow.Disable();
            else
                button_arrow.Enable();

            button_int.Click += ButtonMenu_Click;
        }

        private void ButtonMenu_Click(object sender, EventArgs e)
        {
            ButtonMenuClick?.Invoke(sender, e);
        }

        private void ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            button_arrow.Text = UpArrow;
            MenuContextStatus = ContextMenuStripStatus.Open;
        }
        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            button_arrow.Text = DownArrow;
            MenuContextStatus = ContextMenuStripStatus.Close;
        }
        private void _contextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (!ClosedLunchedByButton && button_arrow.ClientRectangle.Contains(button_arrow.PointToClient(Control.MousePosition)))
            {
                e.Cancel = true;
            }
            ClosedLunchedByButton = false;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (_contextMenuStrip != null && _contextMenuStrip.Items.Count!=0)
            {
                if(MenuContextStatus==ContextMenuStripStatus.Close)
                {
                    _contextMenuStrip.Show(button_int.PointToScreen( button_int.ClientRectangle.GetLocation(ContentAlignment.BottomLeft)));
                }
                else if (MenuContextStatus == ContextMenuStripStatus.Open)
                {
                    ClosedLunchedByButton = true;
                    _contextMenuStrip.Close();
                }
                
            }
            
            
        }



        public event EventHandler ButtonMenuClick;


        public class ButtonMenuWrapper
        {

            System.Windows.Forms.Button inter;



            public Control GetControl()
            {
                return inter._Cast<Control>();
            }
            public ButtonMenuWrapper()
            {
                inter = new System.Windows.Forms.Button();
                EventAssotiation();
            }
            public ButtonMenuWrapper(System.Windows.Forms.Button b)
            {
                inter = b;
                EventAssotiation();
            }

            public void NotifyDefault(bool value)
            {
                inter.NotifyDefault(value);
            }

            public void PerformClick()
            {
                inter.PerformClick();
            }

            public new String ToString()
            {
                return inter.ToString();
            }

            public Size GetPreferredSize(System.Drawing.Size proposedSize)
            {
                return inter.GetPreferredSize(proposedSize);
            }


            public IAsyncResult BeginInvoke(System.Delegate method)
            {
                return inter.BeginInvoke(method);
            }

            public IAsyncResult BeginInvoke(System.Delegate method, System.Object[] args)
            {
                return inter.BeginInvoke(method, args);
            }

            public void BringToFront()
            {
                inter.BringToFront();
            }

            public Boolean Contains(System.Windows.Forms.Control ctl)
            {
                return inter.Contains(ctl);
            }

            public Graphics CreateGraphics()
            {
                return inter.CreateGraphics();
            }

            public void CreateControl()
            {
                inter.CreateControl();
            }

            public DragDropEffects DoDragDrop(object data, System.Windows.Forms.DragDropEffects allowedEffects)
            {
                return inter.DoDragDrop(data, allowedEffects);
            }

            public void DrawToBitmap(System.Drawing.Bitmap bitmap, System.Drawing.Rectangle targetBounds)
            {
                inter.DrawToBitmap(bitmap, targetBounds);
            }

            public Object EndInvoke(System.IAsyncResult asyncResult)
            {
                return inter.EndInvoke(asyncResult);
            }

            public Form FindForm()
            {
                return inter.FindForm();
            }

            public Boolean Focus()
            {
                return inter.Focus();
            }

            public Control GetChildAtPoint(System.Drawing.Point pt, System.Windows.Forms.GetChildAtPointSkip skipValue)
            {
                return inter.GetChildAtPoint(pt, skipValue);
            }

            public Control GetChildAtPoint(System.Drawing.Point pt)
            {
                return inter.GetChildAtPoint(pt);
            }

            public IContainerControl GetContainerControl()
            {
                return inter.GetContainerControl();
            }

            public Control GetNextControl(System.Windows.Forms.Control ctl, bool forward)
            {
                return inter.GetNextControl(ctl, forward);
            }

            public void Invalidate(System.Drawing.Region region)
            {
                inter.Invalidate(region);
            }

            public void Invalidate(System.Drawing.Region region, bool invalidateChildren)
            {
                inter.Invalidate(region, invalidateChildren);
            }

            public void Invalidate()
            {
                inter.Invalidate();
            }

            public void Invalidate(bool invalidateChildren)
            {
                inter.Invalidate(invalidateChildren);
            }

            public void Invalidate(System.Drawing.Rectangle rc)
            {
                inter.Invalidate(rc);
            }

            public void Invalidate(System.Drawing.Rectangle rc, bool invalidateChildren)
            {
                inter.Invalidate(rc, invalidateChildren);
            }

            public Object Invoke(System.Delegate method)
            {
                return inter.Invoke(method);
            }

            public Object Invoke(System.Delegate method, System.Object[] args)
            {
                return inter.Invoke(method, args);
            }

            public void PerformLayout()
            {
                inter.PerformLayout();
            }

            public void PerformLayout(System.Windows.Forms.Control affectedControl, String affectedProperty)
            {
                inter.PerformLayout(affectedControl, affectedProperty);
            }

            public Point PointToClient(System.Drawing.Point p)
            {
                return inter.PointToClient(p);
            }

            public Point PointToScreen(System.Drawing.Point p)
            {
                return inter.PointToScreen(p);
            }

            public Boolean PreProcessMessage(ref System.Windows.Forms.Message msg)
            {
                return inter.PreProcessMessage(ref msg);
            }

            public PreProcessControlState PreProcessControlMessage(ref System.Windows.Forms.Message msg)
            {
                return inter.PreProcessControlMessage(ref msg);
            }

            public void ResetBackColor()
            {
                inter.ResetBackColor();
            }

            public void ResetCursor()
            {
                inter.ResetCursor();
            }

            public void ResetFont()
            {
                inter.ResetFont();
            }

            public void ResetForeColor()
            {
                inter.ResetForeColor();
            }

            public void ResetRightToLeft()
            {
                inter.ResetRightToLeft();
            }

            public Rectangle RectangleToClient(System.Drawing.Rectangle r)
            {
                return inter.RectangleToClient(r);
            }

            public Rectangle RectangleToScreen(System.Drawing.Rectangle r)
            {
                return inter.RectangleToScreen(r);
            }

            public void Refresh()
            {
                inter.Refresh();
            }

            public void ResetText()
            {
                inter.ResetText();
            }

            public void ResumeLayout()
            {
                inter.ResumeLayout();
            }

            public void ResumeLayout(bool performLayout)
            {
                inter.ResumeLayout(performLayout);
            }

            public void Select()
            {
                inter.Select();
            }

            public Boolean SelectNextControl(System.Windows.Forms.Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
            {
                return inter.SelectNextControl(ctl, forward, tabStopOnly, nested, wrap);
            }

            public void SendToBack()
            {
                inter.SendToBack();
            }


            public void SuspendLayout()
            {
                inter.SuspendLayout();
            }

            public void Update()
            {
                inter.Update();
            }

            public void ResetImeMode()
            {
                inter.ResetImeMode();
            }


            public Object GetLifetimeService()
            {
                return inter.GetLifetimeService();
            }

            public Object InitializeLifetimeService()
            {
                return inter.InitializeLifetimeService();
            }



            public new Boolean Equals(object obj)
            {
                return inter.Equals(obj);
            }

            public new Int32 GetHashCode()
            {
                return inter.GetHashCode();
            }

            public new Type GetType()
            {
                return inter.GetType();
            }

            public System.Windows.Forms.AutoSizeMode AutoSizeMode
            {
                get
                {
                    return inter.AutoSizeMode;
                }
            }
            public System.Windows.Forms.DialogResult DialogResult
            {
                get
                {
                    return inter.DialogResult;
                }
                set
                {
                    inter.DialogResult = value;
                }
            }
            public bool AutoEllipsis
            {
                get
                {
                    return inter.AutoEllipsis;
                }
                set
                {
                    inter.AutoEllipsis = value;
                }
            }
            public bool AutoSize
            {
                get
                {
                    return inter.AutoSize;
                }
            }
            public System.Drawing.Color BackColor
            {
                get
                {
                    return inter.BackColor;
                }
                set
                {
                    inter.BackColor = value;
                }
            }
            public System.Windows.Forms.FlatStyle FlatStyle
            {
                get
                {
                    return inter.FlatStyle;
                }
                set
                {
                    inter.FlatStyle = value;
                }
            }
            public System.Windows.Forms.FlatButtonAppearance FlatAppearance
            {
                get
                {
                    return inter.FlatAppearance;
                }
            }
            public System.Drawing.Image Image
            {
                get
                {
                    return inter.Image;
                }
                set
                {
                    inter.Image = value;
                }
            }
            public System.Drawing.ContentAlignment ImageAlign
            {
                get
                {
                    return inter.ImageAlign;
                }
                set
                {
                    inter.ImageAlign = value;
                }
            }
            public Int32 ImageIndex
            {
                get
                {
                    return inter.ImageIndex;
                }
                set
                {
                    inter.ImageIndex = value;
                }
            }
            public String ImageKey
            {
                get
                {
                    return inter.ImageKey;
                }
                set
                {
                    inter.ImageKey = value;
                }
            }
            public System.Windows.Forms.ImageList ImageList
            {
                get
                {
                    return inter.ImageList;
                }
                set
                {
                    inter.ImageList = value;
                }
            }
            public System.Windows.Forms.ImeMode ImeMode
            {
                get
                {
                    return inter.ImeMode;
                }
                set
                {
                    inter.ImeMode = value;
                }
            }
            public String Text
            {
                get
                {
                    return inter.Text;
                }
                set
                {
                    inter.Text = value;
                }
            }
            public System.Drawing.ContentAlignment TextAlign
            {
                get
                {
                    return inter.TextAlign;
                }
                set
                {
                    inter.TextAlign = value;
                }
            }
            public System.Windows.Forms.TextImageRelation TextImageRelation
            {
                get
                {
                    return inter.TextImageRelation;
                }
                set
                {
                    inter.TextImageRelation = value;
                }
            }
            public bool UseMnemonic
            {
                get
                {
                    return inter.UseMnemonic;
                }
                set
                {
                    inter.UseMnemonic = value;
                }
            }
            public bool UseCompatibleTextRendering
            {
                get
                {
                    return inter.UseCompatibleTextRendering;
                }
                set
                {
                    inter.UseCompatibleTextRendering = value;
                }
            }
            public bool UseVisualStyleBackColor
            {
                get
                {
                    return inter.UseVisualStyleBackColor;
                }
                set
                {
                    inter.UseVisualStyleBackColor = value;
                }
            }
            public System.Windows.Forms.AccessibleObject AccessibilityObject
            {
                get
                {
                    return inter.AccessibilityObject;
                }
            }
            public String AccessibleDefaultActionDescription
            {
                get
                {
                    return inter.AccessibleDefaultActionDescription;
                }
                set
                {
                    inter.AccessibleDefaultActionDescription = value;
                }
            }
            public String AccessibleDescription
            {
                get
                {
                    return inter.AccessibleDescription;
                }
                set
                {
                    inter.AccessibleDescription = value;
                }
            }
            public String AccessibleName
            {
                get
                {
                    return inter.AccessibleName;
                }
                set
                {
                    inter.AccessibleName = value;
                }
            }
            public System.Windows.Forms.AccessibleRole AccessibleRole
            {
                get
                {
                    return inter.AccessibleRole;
                }
                set
                {
                    inter.AccessibleRole = value;
                }
            }
            public bool AllowDrop
            {
                get
                {
                    return inter.AllowDrop;
                }
                set
                {
                    inter.AllowDrop = value;
                }
            }
            public System.Windows.Forms.AnchorStyles Anchor
            {
                get
                {
                    return inter.Anchor;
                }
            }
            public System.Drawing.Point AutoScrollOffset
            {
                get
                {
                    return inter.AutoScrollOffset;
                }
                set
                {
                    inter.AutoScrollOffset = value;
                }
            }
            public System.Windows.Forms.Layout.LayoutEngine LayoutEngine
            {
                get
                {
                    return inter.LayoutEngine;
                }
            }
            public System.Drawing.Image BackgroundImage
            {
                get
                {
                    return inter.BackgroundImage;
                }
                set
                {
                    inter.BackgroundImage = value;
                }
            }
            public System.Windows.Forms.ImageLayout BackgroundImageLayout
            {
                get
                {
                    return inter.BackgroundImageLayout;
                }
                set
                {
                    inter.BackgroundImageLayout = value;
                }
            }
            public System.Windows.Forms.BindingContext BindingContext
            {
                get
                {
                    return inter.BindingContext;
                }
                set
                {
                    inter.BindingContext = value;
                }
            }
            public Int32 Bottom
            {
                get
                {
                    return inter.Bottom;
                }
            }
            public System.Drawing.Rectangle Bounds
            {
                get
                {
                    return inter.Bounds;
                }
            }
            public bool CanFocus
            {
                get
                {
                    return inter.CanFocus;
                }
            }
            public bool CanSelect
            {
                get
                {
                    return inter.CanSelect;
                }
            }
            public bool Capture
            {
                get
                {
                    return inter.Capture;
                }
                set
                {
                    inter.Capture = value;
                }
            }
            public bool CausesValidation
            {
                get
                {
                    return inter.CausesValidation;
                }
                set
                {
                    inter.CausesValidation = value;
                }
            }
            public System.Drawing.Rectangle ClientRectangle
            {
                get
                {
                    return inter.ClientRectangle;
                }
            }
            public System.Drawing.Size ClientSize
            {
                get
                {
                    return inter.ClientSize;
                }
            }
            public String CompanyName
            {
                get
                {
                    return inter.CompanyName;
                }
            }
            public bool ContainsFocus
            {
                get
                {
                    return inter.ContainsFocus;
                }
            }
            public System.Windows.Forms.ContextMenu ContextMenu
            {
                get
                {
                    return inter.ContextMenu;
                }
                set
                {
                    inter.ContextMenu = value;
                }
            }
            public System.Windows.Forms.ContextMenuStrip ContextMenuStrip
            {
                get
                {
                    return inter.ContextMenuStrip;
                }
                set
                {
                    inter.ContextMenuStrip = value;
                }
            }
            public System.Windows.Forms.Control.ControlCollection Controls
            {
                get
                {
                    return inter.Controls;
                }
            }
            public bool Created
            {
                get
                {
                    return inter.Created;
                }
            }
            public System.Windows.Forms.Cursor Cursor
            {
                get
                {
                    return inter.Cursor;
                }
                set
                {
                    inter.Cursor = value;
                }
            }
            public System.Windows.Forms.ControlBindingsCollection DataBindings
            {
                get
                {
                    return inter.DataBindings;
                }
            }
            public System.Drawing.Rectangle DisplayRectangle
            {
                get
                {
                    return inter.DisplayRectangle;
                }
            }
            public bool IsDisposed
            {
                get
                {
                    return inter.IsDisposed;
                }
            }
            public bool Disposing
            {
                get
                {
                    return inter.Disposing;
                }
            }
            public System.Windows.Forms.DockStyle Dock
            {
                get
                {
                    return inter.Dock;
                }
            }
            public bool Enabled
            {
                get
                {
                    return inter.Enabled;
                }
                set
                {
                    inter.Enabled = value;
                }
            }
            public bool Focused
            {
                get
                {
                    return inter.Focused;
                }
            }
            public System.Drawing.Font Font
            {
                get
                {
                    return inter.Font;
                }
                set
                {
                    inter.Font = value;
                }
            }
            public System.Drawing.Color ForeColor
            {
                get
                {
                    return inter.ForeColor;
                }
                set
                {
                    inter.ForeColor = value;
                }
            }
            public IntPtr Handle
            {
                get
                {
                    return inter.Handle;
                }
            }
            public bool HasChildren
            {
                get
                {
                    return inter.HasChildren;
                }
            }
            public Int32 Height
            {
                get
                {
                    return inter.Height;
                }
                set
                {
                    inter.Height = value;
                }
            }
            public bool IsHandleCreated
            {
                get
                {
                    return inter.IsHandleCreated;
                }
            }
            public bool InvokeRequired
            {
                get
                {
                    return inter.InvokeRequired;
                }
            }
            public bool IsAccessible
            {
                get
                {
                    return inter.IsAccessible;
                }
                set
                {
                    inter.IsAccessible = value;
                }
            }
            public bool IsMirrored
            {
                get
                {
                    return inter.IsMirrored;
                }
            }
            public Int32 Left
            {
                get
                {
                    return inter.Left;
                }
            }
            public System.Drawing.Point Location
            {
                get
                {
                    return inter.Location;
                }
            }
            public System.Windows.Forms.Padding Margin
            {
                get
                {
                    return inter.Margin;
                }
            }
            public System.Drawing.Size MaximumSize
            {
                get
                {
                    return inter.MaximumSize;
                }

            }
            public System.Drawing.Size MinimumSize
            {
                get
                {
                    return inter.MinimumSize;
                }

            }
            public String Name
            {
                get
                {
                    return inter.Name;
                }
                set
                {
                    inter.Name = value;
                }
            }
            public System.Windows.Forms.Control Parent
            {
                get
                {
                    return inter.Parent;
                }
            }
            public String ProductName
            {
                get
                {
                    return inter.ProductName;
                }
            }
            public String ProductVersion
            {
                get
                {
                    return inter.ProductVersion;
                }
            }
            public bool RecreatingHandle
            {
                get
                {
                    return inter.RecreatingHandle;
                }
            }
            public System.Drawing.Region Region
            {
                get
                {
                    return inter.Region;
                }

            }
            public Int32 Right
            {
                get
                {
                    return inter.Right;
                }
            }
            public System.Windows.Forms.RightToLeft RightToLeft
            {
                get
                {
                    return inter.RightToLeft;
                }
                set
                {
                    inter.RightToLeft = value;
                }
            }
            public ISite Site
            {
                get
                {
                    return inter.Site;
                }

            }
            public System.Drawing.Size Size
            {
                get
                {
                    return inter.Size;
                }

            }
            public Int32 TabIndex
            {
                get
                {
                    return inter.TabIndex;
                }
                set
                {
                    inter.TabIndex = value;
                }
            }
            public bool TabStop
            {
                get
                {
                    return inter.TabStop;
                }
                set
                {
                    inter.TabStop = value;
                }
            }
            public object Tag
            {
                get
                {
                    return inter.Tag;
                }
                set
                {
                    inter.Tag = value;
                }
            }
            public Int32 Top
            {
                get
                {
                    return inter.Top;
                }
            }
            public System.Windows.Forms.Control TopLevelControl
            {
                get
                {
                    return inter.TopLevelControl;
                }
            }
            public bool UseWaitCursor
            {
                get
                {
                    return inter.UseWaitCursor;
                }
                set
                {
                    inter.UseWaitCursor = value;
                }
            }
            public bool Visible
            {
                get
                {
                    return inter.Visible;
                }
                set
                {
                    inter.Visible = value;
                }
            }
            public Int32 Width
            {
                get
                {
                    return inter.Width;
                }
            }
            public System.Windows.Forms.IWindowTarget WindowTarget
            {
                get
                {
                    return inter.WindowTarget;
                }
            }
            public System.Drawing.Size PreferredSize
            {
                get
                {
                    return inter.PreferredSize;
                }
            }
            public System.Windows.Forms.Padding Padding
            {
                get
                {
                    return inter.Padding;
                }
            }
            public System.ComponentModel.IContainer Container
            {
                get
                {
                    return inter.Container;
                }
            }
            public event EventHandler DoubleClick;
            public event System.Windows.Forms.MouseEventHandler MouseDoubleClick;
            public event EventHandler AutoSizeChanged;
            public event EventHandler ImeModeChanged;
            public event EventHandler BackColorChanged;
            public event EventHandler BackgroundImageChanged;
            public event EventHandler BackgroundImageLayoutChanged;
            public event EventHandler BindingContextChanged;
            public event EventHandler CausesValidationChanged;
            public event EventHandler ClientSizeChanged;
            public event EventHandler ContextMenuChanged;
            public event EventHandler ContextMenuStripChanged;
            public event EventHandler CursorChanged;
            public event EventHandler DockChanged;
            public event EventHandler EnabledChanged;
            public event EventHandler FontChanged;
            public event EventHandler ForeColorChanged;
            public event EventHandler LocationChanged;
            public event EventHandler MarginChanged;
            public event EventHandler RegionChanged;
            public event EventHandler RightToLeftChanged;
            public event EventHandler SizeChanged;
            public event EventHandler TabIndexChanged;
            public event EventHandler TabStopChanged;
            public event EventHandler TextChanged;
            public event EventHandler VisibleChanged;
            public event EventHandler Click;
            public event System.Windows.Forms.ControlEventHandler ControlAdded;
            public event System.Windows.Forms.ControlEventHandler ControlRemoved;
            public event System.Windows.Forms.DragEventHandler DragDrop;
            public event System.Windows.Forms.DragEventHandler DragEnter;
            public event System.Windows.Forms.DragEventHandler DragOver;
            public event EventHandler DragLeave;
            public event System.Windows.Forms.GiveFeedbackEventHandler GiveFeedback;
            public event EventHandler HandleCreated;
            public event EventHandler HandleDestroyed;
            public event System.Windows.Forms.HelpEventHandler HelpRequested;
            public event System.Windows.Forms.InvalidateEventHandler Invalidated;
            public event EventHandler PaddingChanged;
            public event System.Windows.Forms.PaintEventHandler Paint;
            public event System.Windows.Forms.QueryContinueDragEventHandler QueryContinueDrag;
            public event System.Windows.Forms.QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
            public event EventHandler Enter;
            public event EventHandler GotFocus;
            public event System.Windows.Forms.KeyEventHandler KeyDown;
            public event System.Windows.Forms.KeyPressEventHandler KeyPress;
            public event System.Windows.Forms.KeyEventHandler KeyUp;
            public event System.Windows.Forms.LayoutEventHandler Layout;
            public event EventHandler Leave;
            public event EventHandler LostFocus;
            public event System.Windows.Forms.MouseEventHandler MouseClick;
            public event EventHandler MouseCaptureChanged;
            public event System.Windows.Forms.MouseEventHandler MouseDown;
            public event EventHandler MouseEnter;
            public event EventHandler MouseLeave;
            public event EventHandler MouseHover;
            public event System.Windows.Forms.MouseEventHandler MouseMove;
            public event System.Windows.Forms.MouseEventHandler MouseUp;
            public event System.Windows.Forms.MouseEventHandler MouseWheel;
            public event EventHandler Move;
            public event System.Windows.Forms.PreviewKeyDownEventHandler PreviewKeyDown;
            public event EventHandler Resize;
            public event System.Windows.Forms.UICuesEventHandler ChangeUICues;
            public event EventHandler StyleChanged;
            public event EventHandler SystemColorsChanged;
            public event CancelEventHandler Validating;
            public event EventHandler Validated;
            public event EventHandler ParentChanged;
            public event EventHandler Disposed;
            private void EventAssotiation()
            {
                inter.DoubleClick += DoubleClick;
                inter.MouseDoubleClick += MouseDoubleClick;
                inter.AutoSizeChanged += AutoSizeChanged;
                inter.ImeModeChanged += ImeModeChanged;
                inter.BackColorChanged += BackColorChanged;
                inter.BackgroundImageChanged += BackgroundImageChanged;
                inter.BackgroundImageLayoutChanged += BackgroundImageLayoutChanged;
                inter.BindingContextChanged += BindingContextChanged;
                inter.CausesValidationChanged += CausesValidationChanged;
                inter.ClientSizeChanged += ClientSizeChanged;
                inter.ContextMenuChanged += ContextMenuChanged;
                inter.ContextMenuStripChanged += ContextMenuStripChanged;
                inter.CursorChanged += CursorChanged;
                inter.DockChanged += DockChanged;
                inter.EnabledChanged += EnabledChanged;
                inter.FontChanged += FontChanged;
                inter.ForeColorChanged += ForeColorChanged;
                inter.LocationChanged += LocationChanged;
                inter.MarginChanged += MarginChanged;
                inter.RegionChanged += RegionChanged;
                inter.RightToLeftChanged += RightToLeftChanged;
                inter.SizeChanged += SizeChanged;
                inter.TabIndexChanged += TabIndexChanged;
                inter.TabStopChanged += TabStopChanged;
                inter.TextChanged += TextChanged;
                inter.VisibleChanged += VisibleChanged;
                inter.Click += Click;
                inter.ControlAdded += ControlAdded;
                inter.ControlRemoved += ControlRemoved;
                inter.DragDrop += DragDrop;
                inter.DragEnter += DragEnter;
                inter.DragOver += DragOver;
                inter.DragLeave += DragLeave;
                inter.GiveFeedback += GiveFeedback;
                inter.HandleCreated += HandleCreated;
                inter.HandleDestroyed += HandleDestroyed;
                inter.HelpRequested += HelpRequested;
                inter.Invalidated += Invalidated;
                inter.PaddingChanged += PaddingChanged;
                inter.Paint += Paint;
                inter.QueryContinueDrag += QueryContinueDrag;
                inter.QueryAccessibilityHelp += QueryAccessibilityHelp;
                inter.Enter += Enter;
                inter.GotFocus += GotFocus;
                inter.KeyDown += KeyDown;
                inter.KeyPress += KeyPress;
                inter.KeyUp += KeyUp;
                inter.Layout += Layout;
                inter.Leave += Leave;
                inter.LostFocus += LostFocus;
                inter.MouseClick += MouseClick;
                inter.MouseCaptureChanged += MouseCaptureChanged;
                inter.MouseDown += MouseDown;
                inter.MouseEnter += MouseEnter;
                inter.MouseLeave += MouseLeave;
                inter.MouseHover += MouseHover;
                inter.MouseMove += MouseMove;
                inter.MouseUp += MouseUp;
                inter.MouseWheel += MouseWheel;
                inter.Move += Move;
                inter.PreviewKeyDown += PreviewKeyDown;
                inter.Resize += Resize;
                inter.ChangeUICues += ChangeUICues;
                inter.StyleChanged += StyleChanged;
                inter.SystemColorsChanged += SystemColorsChanged;
                inter.Validating += Validating;
                inter.Validated += Validated;
                inter.ParentChanged += ParentChanged;
                inter.Disposed += Disposed;
            }
        }

    }
    enum ContextMenuStripStatus
    {
        Open,
        Close
    }



   
    



}
