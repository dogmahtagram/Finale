using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FNA.AnimationEditor
{
    public partial class ImageView : UserControl
    {
        private static int X_OFFSET = 80;
        private static int BORDER_OFFSET = 2;
        private static int IMAGE_SIZE = 64;
        private Point HELL = new Point(-200, 0);

        private int mImageCount;
        public int ImageCount
        {
            get { return mImageCount; }
        }

        private List<Bitmap> mImageList;
        //public List<Image> Images
        //{
        //    get { return mImageList; }
        //}

        private List<PictureBox> mPictureBoxes;
        public List<PictureBox> PictureBoxes
        {
            get { return mPictureBoxes; }
        }

        private List<int> mSelectedIndices;
        public List<int> SelectedIndices
        {
            get { return mSelectedIndices; }
            //set { mSelectedIndices = value; }
        }

        private int mLastSelected;

        public ImageView()
        {
            InitializeComponent();

            mImageCount = 0;
            mImageList = new List<Bitmap>();
            mPictureBoxes = new List<PictureBox>();
            mSelectedIndices = new List<int>();

            mLastSelected = -1;
        }

        public void AddImage(Bitmap img)
        {
            PictureBox pb = new PictureBox();
            pb.BackColor = Color.Transparent;
            pb.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
            panel1.Controls.Add(pb);
            pb.Location = new Point(BORDER_OFFSET + (mImageList.Count) * X_OFFSET, BORDER_OFFSET);
            pb.MouseDown += new System.Windows.Forms.MouseEventHandler(panel1_MouseDown);

            Bitmap resizedImage = new Bitmap(img, IMAGE_SIZE, IMAGE_SIZE);
            mImageList.Add(resizedImage);
            pb.Image = mImageList[mImageList.Count - 1];
            mPictureBoxes.Add(pb);

            mImageCount++;
            panel1.Refresh();
        }

        public Image GetImage(int index)
        {
            return mImageList[index];
        }

        public void SetImage(int index, Bitmap img)
        {
            mImageList[index] = img;
        }

        public void SwapImages(int index, int index2)
        {
            Bitmap tempImage = (Bitmap)mImageList[index];//.Clone();
            mImageList[index] = (Bitmap)mImageList[index2];//.Clone();
            mImageList[index2] = tempImage;

            mPictureBoxes[index].Image = mImageList[index];
            mPictureBoxes[index2].Image = mImageList[index2];

            int x = mSelectedIndices.IndexOf(index);
            mSelectedIndices[x] = index2;

            //mPictureBoxes.ForEach(t => t.Location = new Point(t.Location.X + 50, t.Location.Y));

            panel1.Refresh();
        }

        public void RemoveImage(int index)
        {
            mImageList[index] = null;
            mImageList.RemoveAt(index);

            if (index < mPictureBoxes.Count-1)
            {
                for (int i = index; i < mPictureBoxes.Count - 1; i++)
                {
                    // Shift all box Images after index down one
                    mPictureBoxes[i].Image = mPictureBoxes[i + 1].Image;
                }
            }

            // Remove the last box in the list
            mPictureBoxes[mPictureBoxes.Count-1].Image = null;
            mPictureBoxes[mPictureBoxes.Count - 1].Location = HELL;
            mPictureBoxes.RemoveAt(mPictureBoxes.Count-1);
            mImageCount--;

            panel1.Refresh();
        }

        public void RemoveImage(Bitmap img)
        {
            RemoveImage(mImageList.IndexOf(img));
        }

        public void Clear()
        {
            mSelectedIndices.Clear();

            for (int i = mImageCount-1; i >= 0; i--)
            {
                RemoveImage(i);
            }

            mImageCount = 0;

            panel1.AutoScrollPosition = new Point(0, 0);
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            //shiftPictureBoxes();
            drawRectangles();
        }

        public Image GetItemAt(int x, int y)
        {
            Image itemAtPoint = null;

            for (int i = 0; i < mImageList.Count; i++)
            {
                if ((x > (BORDER_OFFSET + i * X_OFFSET) - IMAGE_SIZE / 2)
                    && (x < (BORDER_OFFSET + i * X_OFFSET) + IMAGE_SIZE / 2))
                {
                    itemAtPoint = mImageList[i];
                    break;
                }
            }

            return itemAtPoint;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            bool controlSelect = Control.ModifierKeys == Keys.Control;
            bool shiftSelect = Control.ModifierKeys == Keys.Shift;
            System.Drawing.Point cp = panel1.PointToClient(new System.Drawing.Point(Control.MousePosition.X, Control.MousePosition.Y));

            for (int i = 0; i < mPictureBoxes.Count; i++)
            {
                if ((cp.X > mPictureBoxes[i].Location.X) && (cp.X < mPictureBoxes[i].Location.X + IMAGE_SIZE) &&
                    (cp.Y < mPictureBoxes[i].Location.Y+IMAGE_SIZE))
                {
                    if (!mSelectedIndices.Contains(i))
                    {
                        if (controlSelect)
                        {
                            mSelectedIndices.Add(i);
                        }
                        else if (shiftSelect)
                        {
                            if (mLastSelected >= 0)
                            {
                                mSelectedIndices.Clear();
                                if (mLastSelected > i)
                                {
                                    for (int index = i; index <= mLastSelected; index++)
                                    {
                                        mSelectedIndices.Add(index);
                                    }
                                }
                                else //if (mLastSelected < i)
                                {
                                    for (int index = mLastSelected; index <= i; index++)
                                    {
                                        mSelectedIndices.Add(index);
                                    }
                                }
                            }
                        }
                        else
                        {
                            mSelectedIndices.Clear();
                            mSelectedIndices.Add(i);
                        }
                        mLastSelected = i;
                        drawRectangles();
                    }
                    return;
                }
            }

            mLastSelected = -1;
            mSelectedIndices.Clear();
            drawRectangles();
        }

        public void ClearSelections()
        {
            mLastSelected = -1;
            mSelectedIndices.Clear();
            drawRectangles();
        }

        private void shiftPictureBoxes()
        {
            for (int i = 0; i < mPictureBoxes.Count; i++)
            {
                mPictureBoxes[i].Location = new Point(BORDER_OFFSET + (i - 1) * X_OFFSET, BORDER_OFFSET);
            }
        }

        private void drawRectangles()
        {
            System.Drawing.Graphics g = panel1.CreateGraphics();
            
            for (int i = 0; i < mPictureBoxes.Count; i++)
            {
                Rectangle rect = new Rectangle(mPictureBoxes[i].Location.X, mPictureBoxes[i].Location.Y, IMAGE_SIZE, IMAGE_SIZE);
                Pen rectPen = new Pen(Color.Blue);
                rectPen.Width = 3;
                if (!mSelectedIndices.Contains(i))
                {
                    rectPen.Color = Color.LightPink;
                }
                g.DrawRectangle(rectPen, rect);
                rectPen.Dispose();
            }
        }
    }
}
