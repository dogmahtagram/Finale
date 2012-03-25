using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

public class ScrollingPictureBox : System.Windows.Forms.UserControl
{
    #region Members

    private System.Windows.Forms.PictureBox PicBox;
    private System.Windows.Forms.Panel OuterPanel;
    private System.ComponentModel.Container components = null;
    private string m_sPicName = "";

    // Grid variables
    private int mGridWidth;
    private int mGridHeight;

    // Selection variables
    private Rectangle mSelRectangle;
    public Rectangle SelectionRect
    {
        get { return mSelRectangle; }
    }
    private Point mSelStartPoint;
    private Point mSelEndPoint;
    private bool mSelIsDrawing;

    #endregion
    
    #region Designer generated code

    private void InitializeComponent()
    {
        this.PicBox = new System.Windows.Forms.PictureBox();
        this.OuterPanel = new System.Windows.Forms.Panel();
        this.OuterPanel.SuspendLayout();
        this.SuspendLayout();
        // 
        // PicBox
        // 
        this.PicBox.Location = new System.Drawing.Point(0, 0);
        this.PicBox.Name = "PicBox";
        this.PicBox.Size = new System.Drawing.Size(512, 512);
        this.PicBox.TabStop = false;
        this.PicBox.BackgroundImageLayout = ImageLayout.None;
        // 
        // OuterPanel
        // 
        this.OuterPanel.AutoScroll = true;
        this.OuterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.OuterPanel.Controls.Add(this.PicBox);
        this.OuterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.OuterPanel.Location = new System.Drawing.Point(0, 0);
        this.OuterPanel.Name = "OuterPanel";
        this.OuterPanel.Size = new System.Drawing.Size(512,512);
        this.OuterPanel.TabStop = false;
        // 
        // PictureBox
        // 
        this.Controls.Add(this.OuterPanel);
        this.Name = "PictureBox";
        this.Size = new System.Drawing.Size(512, 512);
        this.OuterPanel.ResumeLayout(false);
        this.ResumeLayout(false);

    }
    #endregion

    #region Constructors

    public ScrollingPictureBox()
    {
        InitializeComponent();

        PicBox.SizeMode = PictureBoxSizeMode.Normal;
        PicBox.Location = new System.Drawing.Point(0, 0);
        OuterPanel.Dock = DockStyle.Fill;
        OuterPanel.AutoScroll = true;

        InitCtrl();

        mGridWidth = 0;
        mGridHeight = 0;

        this.PicBox.Cursor = Cursors.Cross;
        mSelStartPoint = new Point();
        mSelEndPoint = new Point();

        this.PicBox.Paint += new PaintEventHandler(PicBoxPaint);
    }

    #endregion

    #region Properties

    [Browsable(false)]
    public string Picture
    {
        get { return m_sPicName; }
        set
        {
            if (value != null)
            {
                if (System.IO.File.Exists(value))
                {
                    try
                    {
                        PicBox.Image = Image.FromFile(value);
                        this.PicBox.Size = new System.Drawing.Size(PicBox.Image.Width, PicBox.Image.Height);
                        m_sPicName = value;
                    }
                    catch (OutOfMemoryException ex)
                    {
                        if (ex != null) { }
                        RedCross();
                    }
                }
                else
                {
                    RedCross();
                }
            }
        }
    }

    [Browsable(false)]
    public BorderStyle Border
    {
        get { return OuterPanel.BorderStyle; }
        set { OuterPanel.BorderStyle = value; }
    }

    #endregion

    #region Other Methods

    private void InitCtrl()
    {
        this.PicBox.MouseEnter += new EventHandler(PicBox_MouseEnter);
        this.PicBox.MouseDown += new MouseEventHandler(PicBox_MouseDown);
        this.PicBox.MouseMove += new MouseEventHandler(PicBox_MouseMove);
        this.PicBox.MouseUp += new MouseEventHandler(PicBox_MouseUp);
    }

    /// <summary>
    /// Create a simple red cross as a bitmap and display it in the PictureBox
    /// </summary>
    private void RedCross()
    {
        Bitmap bmp = new Bitmap(OuterPanel.Width, OuterPanel.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
        Graphics gr;
        gr = Graphics.FromImage(bmp);
        Pen pencil = new Pen(System.Drawing.Color.Red, 5);
        gr.DrawLine(pencil, 0, 0, OuterPanel.Width, OuterPanel.Height);
        gr.DrawLine(pencil, 0, OuterPanel.Height, OuterPanel.Width, 0);
        PicBox.Image = bmp;
        gr.Dispose();
    }

    #endregion

    #region Mouse events

    private void PicBox_MouseEnter(object sender, EventArgs e)
    {
        if (this.PicBox.Focused == false)
        {
            this.PicBox.Focus();
        }
    }

    protected void PicBox_MouseDown(object sender, MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button == MouseButtons.Left)
        {
            mSelIsDrawing = true;

            // Set selection start point to nearest grid point
            double minDistance = double.MaxValue;
            for (int gx = 0; gx <= PicBox.Image.Width; gx += mGridWidth)
            {
                for (int gy = 0; gy <= PicBox.Image.Height; gy += mGridHeight)
                {
                    // Mouse Point: (e.X, e.Y), Grid Point: (gx, gy)
                    double distance = System.Math.Sqrt(Math.Pow(gx-e.X, 2) + Math.Pow(gy-e.Y, 2));
                    if (distance < minDistance)
                    {
                        mSelStartPoint = new System.Drawing.Point(gx, gy);
                        minDistance = distance;
                    }
                }
            }
        }
    }

    protected void PicBox_MouseMove(object sender, MouseEventArgs e)
    {
        base.OnMouseMove(e);

        Point fakeEndPoint = new Point();
        if (mSelIsDrawing)
        {
            // Get mouse position and map to nearest grid point
            double minDistance = double.MaxValue;
            for (int gx = 0; gx <= PicBox.Image.Width; gx += mGridWidth)
            {
                for (int gy = 0; gy <= PicBox.Image.Height; gy += mGridHeight)
                {
                    // Mouse Point: (e.X, e.Y), Grid Point: (gx, gy)
                    double distance = System.Math.Sqrt(Math.Pow(gx-e.X, 2) + Math.Pow(gy-e.Y, 2));
                    if (distance < minDistance)
                    {
                        fakeEndPoint.X = gx;
                        fakeEndPoint.Y = gy;
                        minDistance = distance;
                    }
                }
            }

            if (e.X > mSelStartPoint.X)
            {
                mSelRectangle = new Rectangle(
                    mSelStartPoint.X <= fakeEndPoint.X ? mSelStartPoint.X : fakeEndPoint.X,
                    mSelStartPoint.Y <= fakeEndPoint.Y ? mSelStartPoint.Y : fakeEndPoint.Y,
                    fakeEndPoint.X - mSelStartPoint.X <= 0 ? mSelStartPoint.X - fakeEndPoint.X : fakeEndPoint.X - mSelStartPoint.X,
                    fakeEndPoint.Y - mSelStartPoint.Y <= 0 ? mSelStartPoint.Y - fakeEndPoint.Y : fakeEndPoint.Y - mSelStartPoint.Y);
            }
            else
            {
                mSelRectangle = new Rectangle(
                    fakeEndPoint.X <= mSelStartPoint.X ? fakeEndPoint.X : mSelStartPoint.X,
                    mSelEndPoint.Y <= mSelStartPoint.Y ? fakeEndPoint.Y : mSelStartPoint.Y,
                    mSelStartPoint.X - fakeEndPoint.X <= 0 ? fakeEndPoint.X - mSelStartPoint.X : mSelStartPoint.X - fakeEndPoint.X,
                    mSelStartPoint.Y - fakeEndPoint.Y <= 0 ? fakeEndPoint.Y - mSelStartPoint.Y : mSelStartPoint.Y - fakeEndPoint.Y);
            }

            this.PicBox.Invalidate(true);
        }
    }

    protected void PicBox_MouseUp(object sender, MouseEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.Button == MouseButtons.Left && mSelIsDrawing)
        {
            // Set selection end point to nearest grid point
            double minDistance = double.MaxValue;
            for (int gx = 0; gx <= PicBox.Image.Width; gx += mGridWidth)
            {
                for (int gy = 0; gy <= PicBox.Image.Height; gy += mGridHeight)
                {
                    // Mouse Point: (e.X, e.Y), Grid Point: (gx, gy)
                    double distance = System.Math.Sqrt(Math.Pow(gx-e.X, 2) + Math.Pow(gy-e.Y, 2));
                    if (distance < minDistance)
                    {
                        mSelEndPoint = new Point(gx, gy); 
                        minDistance = distance;
                    }
                }
            }

            if (e.X > mSelStartPoint.X)
            {
                mSelRectangle = new Rectangle(
                    mSelStartPoint.X <= mSelEndPoint.X ? mSelStartPoint.X : mSelEndPoint.X,
                    mSelStartPoint.Y <= mSelEndPoint.Y ? mSelStartPoint.Y : mSelEndPoint.Y,
                    mSelEndPoint.X - mSelStartPoint.X <= 0 ? mSelStartPoint.X - mSelEndPoint.X : mSelEndPoint.X - mSelStartPoint.X,
                    mSelEndPoint.Y - mSelStartPoint.Y <= 0 ? mSelStartPoint.Y - mSelEndPoint.Y : mSelEndPoint.Y - mSelStartPoint.Y);
            }
            else
            {
                mSelRectangle = new Rectangle(
                    mSelEndPoint.X <= mSelStartPoint.X ? mSelEndPoint.X : mSelStartPoint.X,
                    mSelEndPoint.Y <= mSelStartPoint.Y ? mSelEndPoint.Y : mSelStartPoint.Y,
                    mSelStartPoint.X - mSelEndPoint.X <= 0 ? mSelEndPoint.X - mSelStartPoint.X : mSelStartPoint.X - mSelEndPoint.X,
                    mSelStartPoint.Y - mSelEndPoint.Y <= 0 ? mSelEndPoint.Y - mSelStartPoint.Y : mSelStartPoint.Y - mSelEndPoint.Y);
            }

            mSelIsDrawing = false;
            this.PicBox.Invalidate(true);
        }        
    }

    #endregion

    public void Grid(int x, int y)
    {
        mGridWidth = x;
        mGridHeight = y;

        this.Invalidate(true);
    }

    protected void PicBoxPaint(object sender, PaintEventArgs e)
    {
        base.OnPaint(e);

        if (!DesignMode)
        {
            Graphics gridGraphics = e.Graphics;
            Pen gridPen = new Pen(Color.Orange, 1);
            Brush gridBrush = new SolidBrush(Color.Orange);
            
            for (int x = 0; x <= PicBox.Image.Width; x += mGridWidth)
            {
                gridGraphics.DrawLine(gridPen, x, 0, x, PicBox.Image.Height);
            }

            for (int y = 0; y <= PicBox.Image.Height; y += mGridHeight)
            {
                gridGraphics.DrawLine(gridPen, 0, y, PicBox.Image.Width, y);
                //gridGraphics.FillRectangle(gridBrush, 0, y, 512, (int)mGridOffsetNumber.Value);
                //y += (int)mGridOffsetNumber.Value;
            }

            using (Pen pen = new Pen(Color.Magenta, 2))
            {
                pen.DashStyle = DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, mSelRectangle);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
                components.Dispose();
        }
        base.Dispose(disposing);
    }
}