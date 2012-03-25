using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FNA
{
    public partial class AnimationEditor : Form
    {
        private string mOpenFilename = "";
        private Image mSpriteSheetImage = null;

        private List<Keyframe> mSpriteFrames = new List<Keyframe>();

        private int mGridWidth;
        private int mGridHeight;
        private System.Drawing.Rectangle mSelectionRect;

        private int mCurrentAnimationFrame = 0;
        private int mFrameDuration;
        private Timer mFrameTimer = new Timer();

        public AnimationEditor()
        {
            InitializeComponent();

            mWidthComboBox.SelectedIndex = 5;
            mHeightComboBox.SelectedIndex = 5;
            mGridWidth = int.Parse(mWidthComboBox.SelectedItem.ToString());
            mGridHeight = int.Parse(mHeightComboBox.SelectedItem.ToString());

            mSelectionRect = new System.Drawing.Rectangle(0, 0, 0, 0);

            mFrameTimer.Tick += new EventHandler(advanceFrame);
            mFrameDuration = (int)mTimePerFrameNumberBox.Value;

            mAnimationTimeGroupBox.Enabled = false; 
            mFrameTimeButton1.CheckedChanged += new EventHandler(frameTimeButton_CheckedChanged);
            mFrameTimeButton2.CheckedChanged += new EventHandler(frameTimeButton_CheckedChanged);
            mFrameTimeButton3.CheckedChanged += new EventHandler(frameTimeButton_CheckedChanged);
            mFrameTimeButton1.Checked = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.A | Keys.Control))
            {
                mAddFrameButton_Click(null, null);
            }
            else if (keyData == Keys.Delete)
            {
                mRemoveFrameButton_Click(null, null);
            }
            else
            {
                return false;
            }

            return true;
        }

        private void loadSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open Image File";
            fDialog.Filter = "PNG Files|*.png";
            //fDialog.InitialDirectory = @"C:\";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                mOpenFilename = fDialog.FileName.ToString();
            }

            if (mOpenFilename != "")
            {
                mSpriteSheetImage = Image.FromFile(mOpenFilename);
                mPictureBox.Picture = mOpenFilename;
            }
        }

        private void mAddFrameButton_Click(object sender, EventArgs e)
        {
            if (mSpriteSheetImage != null)
            {
                if (mFrameTimer.Enabled)
                {
                    mFrameTimer.Stop();
                }

                Bitmap imageToCopy = new Bitmap(mSpriteSheetImage);
                mSelectionRect = mPictureBox.SelectionRect;

                if ((mSelectionRect.Width > 256) || (mSelectionRect.Height > 256))
                {
                    MessageBox.Show("Selection width and height must be < 256");
                }
                else if ((mSelectionRect.Width > 0) && (mSelectionRect.Height > 0))
                {
                    //string filename = System.IO.Path.GetFileName(mOpenFilename);
                    //int index = filename.LastIndexOf('.');
                    //filename = filename.Remove(index);
                    
                    Keyframe k = new Keyframe();
                    k.Rect = new Microsoft.Xna.Framework.Rectangle(
                        mSelectionRect.X, mSelectionRect.Y, mSelectionRect.Width, mSelectionRect.Height);
                    k.FrameDuration = 1;
                    k.MotionDelta = new Vector2(0f, 0f);
                    mSpriteFrames.Add(k);

                    Bitmap croppedImage = imageToCopy.Clone(mSelectionRect, imageToCopy.PixelFormat);
                    Image newImage = (Image)croppedImage;
                    mFrameListView.LargeImageList.Images.Add(newImage);

                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = mFrameListView.LargeImageList.Images.Count - 1;
                    mFrameListView.Items.Add(item);

                    mAnimationTimeGroupBox.Enabled = true;
                    var checkedButton = mAnimationTimeGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    if (checkedButton == mFrameTimeButton1)
                    {
                        mTotalTimeNumberBox.Value = mTimePerFrameNumberBox.Value * mFrameListView.Items.Count;
                    }
                    else if (checkedButton == mFrameTimeButton2)
                    {
                        mTimePerFrameNumberBox.Value = mTotalTimeNumberBox.Value / mFrameListView.Items.Count;
                    }
                    else if (checkedButton == mFrameTimeButton3)
                    {

                    }
                }
            }
            else
            {
                MessageBox.Show("Load a SpriteSheet first!");
            }
        }
        
        private void mRemoveFrameButton_Click(object sender, EventArgs e)
        {
            if (mFrameTimer.Enabled)
            {
                mFrameTimer.Stop();
            }

            int[] selIndices = new int[mFrameListView.SelectedItems.Count];
            if (selIndices.Length > 0)
            {
                int indexCnt = 0;
                int numSelected = mFrameListView.SelectedItems.Count;
                ListViewItem[] sel = new ListViewItem[mFrameListView.SelectedItems.Count];

                for (int i = 0; i < numSelected; i++)
                {
                    sel[i] = mFrameListView.SelectedItems[i];
                    selIndices[indexCnt] = mFrameListView.SelectedItems[i].Index;
                    indexCnt++;
                }

                // Remove the ListView item at the selected indices
                foreach (ListViewItem i in sel)
                {
                    mFrameListView.Items.Remove(i);
                }

                //for (int i = 0; i <= numSelected - 1; i++)
                //{
                //    sel[i] = mFrameListView.SelectedItems[i];
                //    selIndices[indexCnt] = mFrameListView.SelectedItems[i].Index;
                //    indexCnt++;
                //}

                //// Remove the image item at the selected indices
                //foreach (ListViewItem i in sel)
                //{
                //    mFrameListView.Items.Remove(i);
                //}

                mAnimationPictureBox.Image = null;
                if (mFrameListView.Items.Count > 0)
                {
                    mCurrentAnimationFrame = 0;
                }
                else
                {
                    mAnimationTimeGroupBox.Enabled = false;
                }
            }
        }

        private void mWidthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mGridWidth = int.Parse(mWidthComboBox.SelectedItem.ToString());

            if (mGridSizeLinkButton.Text == "=")
            {
                mHeightComboBox.SelectedIndex = mWidthComboBox.SelectedIndex;
            }

            mPictureBox.Grid(mGridWidth, mGridHeight);
        }

        private void mWidthComboBox_TextChanged(object sender, EventArgs e)
        {
            mGridWidth = int.Parse(mWidthComboBox.Text);

            if (mGridSizeLinkButton.Text == "=")
            {
                mHeightComboBox.Text = mWidthComboBox.Text;
            }

            mPictureBox.Grid(mGridWidth, mGridHeight);
        }

        private void mHeightComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mGridHeight = int.Parse(mHeightComboBox.SelectedItem.ToString());

                if (mGridSizeLinkButton.Text == "=")
                {
                    mWidthComboBox.SelectedIndex = mHeightComboBox.SelectedIndex;
                }

                mPictureBox.Grid(mGridWidth, mGridHeight);
            }
            catch (System.Exception ex)
            {
                // A non-number was entered
                MessageBox.Show(ex.ToString());
            }
        }

        private void mHeightComboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                mGridHeight = int.Parse(mHeightComboBox.Text);
                if (mGridSizeLinkButton.Text == "=")
                {
                    mWidthComboBox.Text = mHeightComboBox.Text;
                }

                mPictureBox.Grid(mGridWidth, mGridHeight);
            }
            catch (System.Exception ex)
            {
                // A non-number was entered
                MessageBox.Show(ex.ToString());
            }
        }

        private void mGridSizeLinkButton_Click(object sender, EventArgs e)
        {
            if (mGridSizeLinkButton.Text == "=") mGridSizeLinkButton.Text = "x";
            else mGridSizeLinkButton.Text = "=";
        }

        private void mAnimationPictureBox_Click(object sender, EventArgs e)
        {
            if (mFrameListView.Items.Count > 0)
            {
                if (mFrameTimer.Enabled == false)
                {
                    mFrameTimer.Interval = mFrameDuration;
                    mFrameTimer.Start();
                }
                else
                {
                    mFrameTimer.Stop();
                }
            }
            else
            {
                MessageBox.Show("Add some frames first!");
            }
        }

        private void advanceFrame(Object sender, EventArgs e)
        {
            mCurrentAnimationFrame++;
            if (mCurrentAnimationFrame >= mFrameListView.Items.Count)
            {
                mCurrentAnimationFrame = 0;
            }

            //mAnimationPictureBox.Image = mFrameImageList.Images[mCurrentAnimationFrame];
        }

        private void frameTimeButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton selected = (RadioButton)sender;
                if (selected == mFrameTimeButton1)
                {
                    mTimePerFrameNumberBox.Enabled = true;
                    mTotalTimeNumberBox.Enabled = false;
                    mFPSNumberBox.Enabled = false;
                }
                else if (selected == mFrameTimeButton2)
                {
                    mTimePerFrameNumberBox.Enabled = false;
                    mTotalTimeNumberBox.Enabled = true;
                    mFPSNumberBox.Enabled = false;
                }
                else if (selected == mFrameTimeButton3)
                {
                    mTimePerFrameNumberBox.Enabled = false;
                    mTotalTimeNumberBox.Enabled = false;
                    mFPSNumberBox.Enabled = true;
                }
            }
        }

        private void mTimePerFrameNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if ((int)mTimePerFrameNumberBox.Value < 1)
            {
                mTimePerFrameNumberBox.Value = 1;
            }

            var checkedButton = mAnimationTimeGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (checkedButton == mFrameTimeButton1)
            {
                mTotalTimeNumberBox.Value = mTimePerFrameNumberBox.Value *mFrameListView.Items.Count;
                mFrameDuration = (int)mTimePerFrameNumberBox.Value;
                mFrameTimer.Interval = mFrameDuration;
            }
        }

        private void mTotalTimeNumberBox_ValueChanged_1(object sender, EventArgs e)
        {
            if ((int)mTotalTimeNumberBox.Value < 1)
            {
                mTotalTimeNumberBox.Value = 1;
            }
            
            var checkedButton = mAnimationTimeGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (checkedButton == mFrameTimeButton2)
            {
                mTimePerFrameNumberBox.Value = mTotalTimeNumberBox.Value / mFrameListView.Items.Count;
                mFrameDuration = (int)mTimePerFrameNumberBox.Value;
                mFrameTimer.Interval = mFrameDuration;
            }
        }

        private void mFPSNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if ((double)mTotalTimeNumberBox.Value < .0001)
            {
                mTotalTimeNumberBox.Value = (decimal)(.0001);
            }

            var checkedButton = mAnimationTimeGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (checkedButton == mFrameTimeButton3)
            {
                mTimePerFrameNumberBox.Value = mFrameListView.Items.Count / mFPSNumberBox.Value;
                mTotalTimeNumberBox.Value = mTimePerFrameNumberBox.Value * mFrameListView.Items.Count;
                mFrameDuration = (int)mTimePerFrameNumberBox.Value;
                mFrameTimer.Interval = mFrameDuration;
            }
        }

        private void mFrameListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Keyframe> selectedKeys = new List<Keyframe>();
            foreach (ListViewItem i in mFrameListView.SelectedItems)
            {
                selectedKeys.Add(mSpriteFrames[i.Index]);
            }
            mFramePropertyGrid.SelectedObjects = selectedKeys.ToArray();
        }
    }
}
