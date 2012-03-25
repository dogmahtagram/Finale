using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FNA;
using FNA.Components;
using FNA.Graphics;
using ScriptEditor;
using FNA.Managers;

namespace FNA.AnimationEditor
{
    public partial class AnimationEditorForm : Form
    {
        private string mContentDirectory = string.Empty;
        private string mOpenImageFilename = string.Empty;
        private string mAnimationProjectFilename = string.Empty;
        private System.Drawing.Image mSpriteSheetImage = null;

        private List<Bitmap> mKeyframeImageList = new List<Bitmap>();
        private Dictionary<string, Animation> mAnimations = null;
        private Animation mCurrentAnimation;
        private RotationComponent.CardinalDirections mCurrentDirection;

        private int mGridWidth;
        private int mGridHeight;
        private int mSpriteSheetWidth;
        private int mSpriteSheetHeight;
        private Microsoft.Xna.Framework.Rectangle mSelectionRect;

        bool mMultdirChanged;
        bool mFlippingChanged;
        private string mCurrentDirectionText = string.Empty;
        
        private int mCurrentAnimationKeyFrame = 0;
        private int mTotalFrameCount = 0;
        private int mCurrentKeyFrameFrame = 0;
        private int FRAME_DURATION_MS = 33; 
        private Timer mFrameTimer = new Timer();

        private TextEntryDialog mTextEntryDialog;
        ScriptEditorSuite mScriptEditorSuite;

        private bool mSuppressChange;

        public AnimationEditorForm()
        {
            InitializeComponent();

            mScriptEditorSuite = new ScriptEditorSuite();
            mTabControl.TabPages[1].Controls.Add(mScriptEditorSuite);

            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Disable the form until a project is created or opened
            mTabControl.Enabled = false;
            mKeyframePanel.Enabled = false;
            mScriptEditorSuite.Enabled = false;

            mWidthComboBox.SelectedIndex = 3;
            mHeightComboBox.SelectedIndex = 3;
            mGridWidth = int.Parse(mWidthComboBox.SelectedItem.ToString());
            mGridHeight = int.Parse(mHeightComboBox.SelectedItem.ToString());

            mSelectionRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);

            mTextEntryDialog = new TextEntryDialog();
            mTextEntryDialog.Message = string.Empty;

            mAddKeyframeButton.Enabled = false;
            mRemoveKeyframeButton.Enabled = false;

            mFrameTimer.Interval = FRAME_DURATION_MS;
            mFrameTimer.Tick += new EventHandler(advanceFrame);

            mMultdirChanged = false;
            mFlippingChanged = false;
            mMultidirCheckBox.Enabled = false;
            mDirectionComboBox.Enabled = false;
            mFlippingCheckBox.Enabled = false;
            mSuppressChange = false;

            mMultiSelectLabel.Visible = false;

            mImageView.Panel.MouseClick += new MouseEventHandler(mImageView_MouseClick);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.A | Keys.Control))
            {
                if (mAddKeyframeButton.Enabled)
                {
                    mAddFrameButton_Click(null, null);
                }
            }
            else if (keyData == (Keys.D | Keys.Control))
            {
                if (mRemoveKeyframeButton.Enabled)
                {
                    mRemoveFrameButton_Click(null, null);
                }
            }
            else if (keyData == Keys.Space)
            {
                mAnimationPictureBox_Click(null, null);
            }
            else
            {
                return false;
            }

            return true;
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fDialog = new SaveFileDialog();
            fDialog.Title = "Create FNA Animation Project";
            fDialog.Filter = ".fnAnim Files|*.fnAnim";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                mAnimationProjectFilename = fDialog.FileName.ToString();
                if (mAnimationProjectFilename != string.Empty)
                {
                    if (System.IO.File.Exists(mAnimationProjectFilename) == false)
                    {
                        System.IO.File.Create(mAnimationProjectFilename);
                    }

                    mCurrentAnimation = null;
                    mAnimations = new Dictionary<string, Animation>();
                    mTabControl.Enabled = true;
                    mKeyframePanel.Enabled = true;

                    mContentDirectory = mAnimationProjectFilename;
                    mContentDirectory = mContentDirectory.Remove(mContentDirectory.IndexOf("\\Content\\"));

                    mAnimationNameComboBox.Items.Clear();

                    for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
                    {
                        mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick -= new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                    }
                    mKeyframeImageList.Clear();
                    mImageView.Clear();
                }
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open FNA Animation Project";
            fDialog.Filter = ".fnAnim Files|*.fnAnim";

            string projectFilename = string.Empty;
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                projectFilename = fDialog.FileName.ToString();
            }

            if (projectFilename != string.Empty)
            {
                string currentDirectory = projectFilename;
                int pathEnd = currentDirectory.LastIndexOf("\\");
                mScriptEditorSuite.LoadScripts(currentDirectory.Remove(pathEnd + 1) + "nodes.fnNodes");

                StopAnimation();

                // Deserialize the fnAnim file to get all the saved Animations
                Dictionary<string, Animation> animationsTemp = new Dictionary<string, Animation>();
                if (DeserializeAnimations(projectFilename, ref animationsTemp))
                {
                    for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
                    {
                        mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick -= new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                    }
                    mKeyframeImageList.Clear();
                    mImageView.Clear();
                    
                    mAnimationNameComboBox.Items.Clear();
                    mDirectionComboBox.Items.Clear();

                    mAnimationProjectFilename = projectFilename;
                    mContentDirectory = mAnimationProjectFilename;
                    mContentDirectory = mContentDirectory.Remove(mContentDirectory.IndexOf("\\Content\\"));

                    mAnimations = animationsTemp;
                    foreach (KeyValuePair<string, Animation> ani in mAnimations)
                    {
                        mAnimationNameComboBox.Items.Add(ani.Key);
                    }

                    mTabControl.Enabled = true;
                    mKeyframePanel.Enabled = true;
                }
            }
        }

        private bool DeserializeAnimations(string filename, ref Dictionary<string, Animation> animDictionary)
        {
            //if (File.Exists(filename))
            try
            {
                FileStream fnAnimFile = new FileStream(filename, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(fnAnimFile);

                try
                {
                    int numAnimations = reader.ReadInt32();
                    for (int index = 0; index < numAnimations; index++)
                    {
                        //string animationName = reader.ReadString();
                        Animation anim = (Animation)formatter.Deserialize(fnAnimFile);
                        animDictionary.Add(anim.Name, anim);
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize fnAnim file. " + e.Message);
                    return false;
                }
                finally
                {
                    fnAnimFile.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid fnAnim file loaded. " + e.ToString());
                return false;
            }

            return true;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int numFrames;
            if (mMultidirCheckBox.Checked)
            {
                int frameCount;
                numFrames = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.N].Count;
                frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.NE].Count;
                if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nNorthEast differs."); return; }
                frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.E].Count;
                if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nEast differs."); return; }
                frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.SE].Count;
                if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nSouthEast differs."); return; }
                frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.S].Count;
                if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nSouth differs."); return; }
                
                if (mFlippingCheckBox.Checked == false)
                {
                    frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.SW].Count;
                    if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nSouthWest differs."); return; }
                    frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.W].Count;
                    if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nWest differs."); return; }
                    frameCount = mCurrentAnimation.Frames[RotationComponent.CardinalDirections.NW].Count;
                    if (frameCount != numFrames) { MessageBox.Show("Cannot Save!\nAll directions must have an equal number of frames.\nNorthWest differs."); return; }
                }
            }
            else // Animation is NOT multi-directional
            {
            }
            
            // Export the entire Dictionary of Animations by serialization
            Stream stream = File.Create(mAnimationProjectFilename);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            int numAnimations = mAnimations.Count;
            writer.Write(numAnimations);                    
            foreach (KeyValuePair<string,Animation> pair in mAnimations)
            {
                formatter.Serialize(stream, pair.Value);
            }
            stream.Close();
        }

        private void loadSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mImageView.ImageCount > 0)
            {
                MessageBox.Show("Do not mix sprite sheets within an Animation!");
            }
            else
            {
                OpenFileDialog fDialog = new OpenFileDialog();
                fDialog.Title = "Open Image File";
                fDialog.Filter = "PNG Files|*.png";
                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    mOpenImageFilename = fDialog.FileName.ToString();
                }

                if (mOpenImageFilename != string.Empty)
                {
                    mSpriteSheetImage = Image.FromFile(mOpenImageFilename);
                    mSpriteSheetWidth = mSpriteSheetImage.Width;
                    mSpriteSheetHeight = mSpriteSheetImage.Height;

                    mPictureBox.Picture = mOpenImageFilename;

                    if (mCurrentAnimation != null)
                    {
                        if (mCurrentAnimation.TextureName == null)
                        {
                            SetTextureName(mOpenImageFilename);
                        }
                    }
                }
            }
        }

        private void mExportButton_Click(object sender, EventArgs e)
        {
            exportToolStripMenuItem_Click(sender, e);
        }

        private void StopAnimation()
        {
            mFrameTimer.Stop();
            mCurrentKeyFrameFrame = 0;
            mCurrentAnimationKeyFrame = 0;
            mAnimationPictureBox.Image = null;
        }

        private void SetTextureName(string filename)
        {
            if (filename != null)
            {
                mCurrentAnimation.TextureName = filename.Substring(filename.IndexOf("\\Content\\") + 9);
            }
            else
            {
                MessageBox.Show("Image filename is null!");
            }
        }

        private void mAnimationNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopAnimation();

            mCurrentAnimation = mAnimations[mAnimationNameComboBox.Text];

            if (mCurrentAnimation.TextureName != null)
            {
                string filename = mContentDirectory + "\\Content\\" + mCurrentAnimation.TextureName;
                mSpriteSheetImage = Image.FromFile(filename);
                mSpriteSheetWidth = mSpriteSheetImage.Width;
                mSpriteSheetHeight = mSpriteSheetImage.Height;
                mPictureBox.Picture = filename;
            }
            else if (mOpenImageFilename != "")
            {
                //MessageBox.Show("Sprite sheet filename is NULL for animation: " + mCurrentAnimation.Name);
                SetTextureName(mOpenImageFilename);
            }
            else
            {
                loadSpriteSheetToolStripMenuItem_Click(null, null);
            }

            if (mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.N))
            {
                // Check if flipping is enabled
                if (mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] == RotationComponent.CardinalDirections.SW)
                {
                    if (mMultidirCheckBox.Checked == false) mSuppressChange = true;
                    mMultidirCheckBox.Checked = true;
                    if (mFlippingCheckBox.Checked) mSuppressChange = true;
                    mFlippingCheckBox.Checked = false;
                }
                else
                {
                    // Selected Animation is multi-directional, without flipping
                    if (mMultidirCheckBox.Checked == false) mSuppressChange = true;
                    mMultidirCheckBox.Checked = true;
                    if (mFlippingCheckBox.Checked == false) mSuppressChange = true;
                    mFlippingCheckBox.Checked = true;
                }
                mMultidirCheckBox.Enabled = true;
                mFlippingCheckBox.Enabled = true;
                SetDirections();
            }
            else if (mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.NONE))
            {
                // Selected Animation is one-directional
                if (mFlippingCheckBox.Checked) mSuppressChange = true;
                mFlippingCheckBox.Checked = false;
                mFlippingCheckBox.Enabled = false;
                if (mMultidirCheckBox.Checked) mSuppressChange = true;
                mMultidirCheckBox.Checked = false;
                mMultidirCheckBox.Enabled = true;
                SetDirections();
            }
            else // Selected animation is new/undefined
            {
                if (mFlippingCheckBox.Checked) mSuppressChange = true;
                mFlippingCheckBox.Checked = false;
                mFlippingCheckBox.Enabled = false;
                if (mMultidirCheckBox.Checked) mSuppressChange = true;
                mMultidirCheckBox.Checked = false;
                mMultidirCheckBox.Enabled = true;
                
                SetDirections();
            }

            if (mDirectionComboBox.SelectedIndex == 0)
            {
                // This is taken care of when the direction is changed (x->0)
                mKeyframeImageList.Clear();
                Bitmap imageToCopy = new Bitmap(mSpriteSheetImage);
                KeyFrameDictionary kfd = mCurrentAnimation.Frames[mCurrentDirection];
                for (int i = 0; i < kfd.Values.Count; i++)
                {
                    System.Drawing.Rectangle frameRect = new System.Drawing.Rectangle();
                    frameRect.X = (int)(kfd[i].U * mSpriteSheetWidth);
                    frameRect.Y = (int)(kfd[i].V * mSpriteSheetHeight);
                    frameRect.Width = (int)(kfd[i].Width * mSpriteSheetWidth);
                    frameRect.Height = (int)(kfd[i].Height * mSpriteSheetHeight);
                    Bitmap croppedImage = imageToCopy.Clone(frameRect, imageToCopy.PixelFormat);
                    mKeyframeImageList.Add(croppedImage);
                }

                mImageView.Clear();
                for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
                {
                    mImageView.AddImage(mKeyframeImageList[ii]);
                    mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick += new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                }
                SetTotalFrameCount();
            }
            else
            {
                mDirectionComboBox.SelectedIndex = 0;
            }

            mAddKeyframeButton.Enabled = true;

            mScriptEditorSuite.SetActiveScript(mCurrentAnimation.AnimationScripts, mCurrentAnimation.Name);
            mScriptEditorSuite.Enabled = true;
        }

        private void SetDirections()
        {
            mDirectionComboBox.Items.Clear();

            // Remove all Keyframes when switching between multi-directional and not
            if (mMultdirChanged)
            {
                mCurrentAnimation.Frames.Clear();
            }
            else if (mFlippingCheckBox.Checked && mFlippingChanged)
            {
                mCurrentAnimation.Frames.Remove(RotationComponent.CardinalDirections.SW);
                mCurrentAnimation.Frames.Remove(RotationComponent.CardinalDirections.W);
                mCurrentAnimation.Frames.Remove(RotationComponent.CardinalDirections.NW);
            }

            mMultdirChanged = false;
            mFlippingChanged = false;

            if (mMultidirCheckBox.Checked)
            {
                mDirectionComboBox.Items.Add("North");
                mDirectionComboBox.Items.Add("NorthEast");
                mDirectionComboBox.Items.Add("East");
                mDirectionComboBox.Items.Add("SouthEast");
                mDirectionComboBox.Items.Add("South");
                if (mFlippingCheckBox.Checked == false)
                {
                    mDirectionComboBox.Items.Add("SouthWest");
                    mDirectionComboBox.Items.Add("West");
                    mDirectionComboBox.Items.Add("NorthWest");
                }

                if (!mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.N))
                {
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.N] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.NE] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.E] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.SE] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.S] = new KeyFrameDictionary();
                }
                if ((mFlippingCheckBox.Checked == false) && (!mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.SW)))
                {
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.SW] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.W] = new KeyFrameDictionary();
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.NW] = new KeyFrameDictionary();
                }

                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.N] = RotationComponent.CardinalDirections.N;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NE] = RotationComponent.CardinalDirections.NE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.E] = RotationComponent.CardinalDirections.E;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SE] = RotationComponent.CardinalDirections.SE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.S] = RotationComponent.CardinalDirections.S;
                if (mFlippingCheckBox.Checked)
                {
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SE;
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.E;
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NE;
                }
                else
                {
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SW;
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.W;
                    mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NW;
                }

                mDirectionComboBox.Enabled = true;
            }
            else
            {
                mDirectionComboBox.Items.Add("None");

                if (!mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.NONE))
                {
                    mCurrentAnimation.Frames[RotationComponent.CardinalDirections.NONE] = new KeyFrameDictionary();
                }

                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.N] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NE] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.E] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SE] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.S] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.NONE;
                mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NONE;

                mDirectionComboBox.Enabled = false;
            }

            mDirectionComboBox.SelectedIndex = 0;
        }

        private void mMultidirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //StopAnimation();

            if (mSuppressChange)
            {
                mSuppressChange = false;
                return;
            }

            if ((mCurrentAnimation == null) || (mCurrentAnimation.Frames == null))
            {
                return;
            }

            //if (mCurrentAnimation.Frames[GetDirectionFromString(mDirectionComboBox.Text)].Count > 0)
            //{
                DialogResult dialogResult = MessageBox.Show("This will delete all existing Frames for this Animation.\nAre you sure you want to do this?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    mSuppressChange = true;
                    mMultidirCheckBox.Checked = !mMultidirCheckBox.Checked;
                    return;
                }
                //else if (dialogResult == DialogResult.Yes)
                //{
                //    // ...
                //}
            //}

            mMultdirChanged = true;
            if (mMultidirCheckBox.Checked)
            {
                //SetDirections(true);
                if (mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.SW))
                {
                    mFlippingCheckBox.Checked = false;
                    mFlippingCheckBox.Enabled = false;
                }
                else
                {
                    mFlippingCheckBox.Checked = true;
                    mFlippingCheckBox.Enabled = true;
                }
            }
            else // Box was unchecked
            {
                //SetDirections(false);
                mFlippingCheckBox.Checked = false;
                mFlippingCheckBox.Enabled = false;
            }
        }

        private void mFlippingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StopAnimation();

            if (mSuppressChange)
            {
                mSuppressChange = false;
                return;
            }

            if ((mCurrentAnimation == null) || (mCurrentAnimation.Frames == null))
            {
                return;
            }

            if (mCurrentAnimation.Frames.ContainsKey(RotationComponent.CardinalDirections.SW))
            {
                DialogResult dialogResult = MessageBox.Show("This will delete all existing Frames for SW, W, and NW.\nAre you sure you want to do this?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    mSuppressChange = true;
                    mFlippingCheckBox.Checked = !mFlippingCheckBox.Checked;
                    return;
                }
            }

            mFlippingChanged = true;
            SetDirections();
        }

        private void SetTotalFrameCount()
        {
            mTotalFrameCount = 0;
            for (int i = 0; i < mKeyframeImageList.Count; i++)
            {
                mTotalFrameCount += mCurrentAnimation.FrameDurations[i];
            }
            mTotalFrameCountBox.Text = mTotalFrameCount.ToString();
            mTotalTimeTextBox.Text = (mTotalFrameCount * 0.0333).ToString();
        }

        private void mAddFrameButton_Click(object sender, EventArgs e)
        {
            if (mSpriteSheetImage != null)
            {
                mSelectionRect.X = mPictureBox.SelectionRect.X;
                mSelectionRect.Y = mPictureBox.SelectionRect.Y;
                mSelectionRect.Width = mPictureBox.SelectionRect.Width;
                mSelectionRect.Height = mPictureBox.SelectionRect.Height;

                if ((mSelectionRect.Width > 0) && (mSelectionRect.Height > 0))
                {
                    StopAnimation();

                    Bitmap imageToCopy = new Bitmap(mSpriteSheetImage);
                    if (mSelectionRect.Width > mGridWidth)
                    {
                        if (mSelectionRect.Height > mGridHeight)
                        {
                            MessageBox.Show("Select only one row or column.");
                            return;
                        }

                        int numCells = mSelectionRect.Width / mGridWidth;
                        for (int i = 0; i < numCells; i++)
                        {
                            System.Drawing.Rectangle cellRect = new System.Drawing.Rectangle(mPictureBox.SelectionRect.X + (i * mGridWidth), mPictureBox.SelectionRect.Y, mGridWidth, mGridHeight);
                            FloatRectangle cellFloatRect = new FloatRectangle();

                            cellFloatRect.U = cellRect.X / (float)mSpriteSheetWidth;
                            cellFloatRect.V = cellRect.Y / (float)mSpriteSheetHeight;
                            cellFloatRect.Width = (float)cellRect.Width / mSpriteSheetWidth;
                            cellFloatRect.Height = (float)cellRect.Height / mSpriteSheetHeight;
                            
                            if (mMultidirCheckBox.Checked)
                            {
                                mCurrentAnimation.AddFrame(GetDirectionFromString(mDirectionComboBox.Text),
                                    cellFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[GetDirectionFromString(mDirectionComboBox.Text)]].Count);
                            }
                            else
                            {
                                mCurrentAnimation.AddFrame(RotationComponent.CardinalDirections.NONE,
                                    cellFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NONE]].Count);
                            }

                            Bitmap croppedImage = imageToCopy.Clone(cellRect, imageToCopy.PixelFormat);
                            mKeyframeImageList.Add(croppedImage);
                            mImageView.AddImage(mKeyframeImageList[mKeyframeImageList.Count - 1]);
                            mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick += new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                            if (!mCurrentAnimation.FrameDurations.ContainsKey(mKeyframeImageList.Count - 1))
                            {
                                mCurrentAnimation.FrameDurations.Add(mCurrentAnimation.FrameDurations.Count, 1);
                            }
                        }
                    }
                    else if (mSelectionRect.Height > mGridHeight)
                    {
                        if (mSelectionRect.Width > mGridWidth)
                        {
                            MessageBox.Show("Select only one row or column.");
                            return;
                        }

                        int numCells = mSelectionRect.Height / mGridHeight;
                        for (int i = 0; i < numCells; i++)
                        {
                            System.Drawing.Rectangle cellRect = new System.Drawing.Rectangle(mPictureBox.SelectionRect.X, mPictureBox.SelectionRect.Y + (i * mGridHeight), mGridWidth, mGridHeight);
                            FloatRectangle cellFloatRect = new FloatRectangle();
                            cellFloatRect.U = cellRect.X / (float)mSpriteSheetWidth;
                            cellFloatRect.V = cellRect.Y / (float)mSpriteSheetHeight;
                            cellFloatRect.Width = (float)cellRect.Width / mSpriteSheetWidth;
                            cellFloatRect.Height = (float)cellRect.Height / mSpriteSheetHeight;

                            if (mMultidirCheckBox.Checked)
                            {
                                mCurrentAnimation.AddFrame(GetDirectionFromString(mDirectionComboBox.Text),
                                    cellFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[GetDirectionFromString(mDirectionComboBox.Text)]].Count);
                            }
                            else
                            {
                                mCurrentAnimation.AddFrame(RotationComponent.CardinalDirections.NONE,
                                    cellFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NONE]].Count);
                            }

                            Bitmap croppedImage = imageToCopy.Clone(cellRect, imageToCopy.PixelFormat);
                            mKeyframeImageList.Add(croppedImage);
                            mImageView.AddImage(mKeyframeImageList[mKeyframeImageList.Count - 1]);
                            mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick += new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                            if (!mCurrentAnimation.FrameDurations.ContainsKey(mKeyframeImageList.Count - 1))
                            {
                                mCurrentAnimation.FrameDurations.Add(mCurrentAnimation.FrameDurations.Count, 1);
                            }
                        }
                    }
                    else
                    {
                        FloatRectangle selectionFloatRect = new FloatRectangle();
                        selectionFloatRect.U = mSelectionRect.X / (float)mSpriteSheetWidth;
                        selectionFloatRect.V = mSelectionRect.Y / (float)mSpriteSheetHeight;
                        selectionFloatRect.Width = (float)mSelectionRect.Width / mSpriteSheetWidth;
                        selectionFloatRect.Height = (float)mSelectionRect.Height / mSpriteSheetHeight;

                        if (mMultidirCheckBox.Checked)
                        {
                            mCurrentAnimation.AddFrame(GetDirectionFromString(mDirectionComboBox.Text),
                                selectionFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[GetDirectionFromString(mDirectionComboBox.Text)]].Count);
                        }
                        else
                        {
                            mCurrentAnimation.AddFrame(RotationComponent.CardinalDirections.NONE,
                                selectionFloatRect, mCurrentAnimation.Frames[mCurrentAnimation.DirectionMap[RotationComponent.CardinalDirections.NONE]].Count);
                        }

                        Bitmap croppedImage = imageToCopy.Clone(mPictureBox.SelectionRect, imageToCopy.PixelFormat);
                        mKeyframeImageList.Add(croppedImage);
                        mImageView.AddImage(mKeyframeImageList[mKeyframeImageList.Count - 1]);
                        mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick += new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                        if (!mCurrentAnimation.FrameDurations.ContainsKey(mKeyframeImageList.Count-1))
                        {
                            mCurrentAnimation.FrameDurations.Add(mCurrentAnimation.FrameDurations.Count, 1);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Make a selection in the SpriteSheet first!");
                }
            }
            else
            {
                MessageBox.Show("Load a SpriteSheet first!");
            }
        }
        
        private void mRemoveFrameButton_Click(object sender, EventArgs e)
        {
            StopAnimation();

            int numSelected = mImageView.SelectedIndices.Count;
            int indexCnt = 0;
            int[] selIndices = new int[numSelected];

            Image[] listSelections = new Image[numSelected];
            Image[] imageSelections = new Image[numSelected];

            for (int i = 0; i < numSelected; i++)
            {
                selIndices[indexCnt++] = mImageView.SelectedIndices[i];
            }

            for (int i = 0; i < selIndices.Length; i++)
            {
                mImageView.PictureBoxes[i].MouseClick -= new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
                listSelections[i] = mImageView.GetImage(selIndices[i]);
                imageSelections[i] = mKeyframeImageList[selIndices[i]];
            }

            // Remove the Animation object Keyframes at the selected indices
            for (int i = selIndices.Length - 1; i >= 0; i--)
            {
                mCurrentAnimation.Frames[mCurrentDirection].Remove(selIndices[i]);
                //mCurrentAnimation.Frames[GetDirectionFromString(mDirectionComboBox.Text)][selIndices[i]] = null;

                bool removeDur = true;
                foreach (RotationComponent.CardinalDirections dir in mCurrentAnimation.Frames.Keys)
                {
                    if (mCurrentAnimation.Frames[dir].Count - 1 >= mImageView.SelectedIndices[i])
                    {
                        removeDur = false;
                        break;
                    }
                }
                if (removeDur)
                {
                    // Remove the FrameDuration for the selected frame by shifting others down
                    for (int j = mImageView.SelectedIndices[i]; j < mCurrentAnimation.FrameDurations.Count - 1; j++)
                    {
                        mCurrentAnimation.FrameDurations[j] = mCurrentAnimation.FrameDurations[j + 1];
                    }
                    mCurrentAnimation.FrameDurations.Remove(mCurrentAnimation.FrameDurations.Count - 1);
                }
            }

            // Shift leftover frame keys down to maintain 0-N order
            int[] keys = mCurrentAnimation.Frames[mCurrentDirection].Keys.ToArray();
            for (int fr = 0; fr < mCurrentAnimation.Frames[mCurrentDirection].Count; fr++)
            {
                if (keys[fr] != fr)
                {
                    mCurrentAnimation.Frames[mCurrentDirection].Add(fr, mCurrentAnimation.Frames[mCurrentDirection][keys[fr]]);
                    mCurrentAnimation.Frames[mCurrentDirection].Remove(keys[fr]);
                }
            }

            // Remove the items at the selected indices (by object, not index)
            foreach (Bitmap li in listSelections)
            {
                mImageView.RemoveImage(li);
            }
            foreach (Bitmap img in imageSelections)
            {
                mKeyframeImageList.Remove(img);
            }

            mImageView.SelectedIndices.Clear();
            mRemoveKeyframeButton.Enabled = false;
            mFrameDurationTextBox.Enabled = false;
            mFrameDurationTextBox.Text = "-";
            mMotionChannelComboBox.Enabled = false;
            mMotionChannelComboBox.SelectedIndex = 0;
            mImageView.Panel.Refresh();
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
            if (mImageView.ImageCount > 0)
            {
                if (mFrameTimer.Enabled == false)
                {
                    mFrameTimer.Start();
                }
                else
                {
                    StopAnimation();
                }
            }
            else
            {
                MessageBox.Show("Add some frames first!");
            }
        }

        private void advanceFrame(Object sender, EventArgs e)
        {
            mCurrentKeyFrameFrame++;
            if (mCurrentKeyFrameFrame > mCurrentAnimation.FrameDurations[mCurrentAnimationKeyFrame])
            {
                mCurrentAnimationKeyFrame++;
                if (mCurrentAnimationKeyFrame >= mKeyframeImageList.Count)
                {
                    mCurrentAnimationKeyFrame = 0;
                }

                mAnimationPictureBox.Image = mKeyframeImageList[mCurrentAnimationKeyFrame];

                mCurrentKeyFrameFrame = 0;
            }
        }

        private void mAddAnimationButton_Click(object sender, EventArgs e)
        {
            mTextEntryDialog.ShowDialog();

            if ((mTextEntryDialog.DialogResult == DialogResult.OK) && (mTextEntryDialog.Message != string.Empty))
            {
                if (mAnimations.ContainsKey(mTextEntryDialog.Message))
                {
                    MessageBox.Show("Animation '" + mTextEntryDialog.Message + "' already exists.");
                }
                else
                {
                    mCurrentAnimation = new Animation();
                    mCurrentAnimation.Name = mTextEntryDialog.Message;
                    mAnimations.Add(mTextEntryDialog.Message, mCurrentAnimation);

                    if (mFlippingCheckBox.Checked) mSuppressChange = true;
                    mFlippingCheckBox.Checked = false;
                    mFlippingCheckBox.Enabled = false;
                    if (mMultidirCheckBox.Checked) mSuppressChange = true;
                    mMultidirCheckBox.Checked = false;
                    mMultidirCheckBox.Enabled = true;                    
                    SetDirections();

                    mAnimationNameComboBox.Items.Add(mTextEntryDialog.Message);
                    mAnimationNameComboBox.SelectedIndex = mAnimationNameComboBox.Items.Count-1;
                }

                mTextEntryDialog.Message = string.Empty;
            }
        }

        private RotationComponent.CardinalDirections GetDirectionFromString(string text)
        {
            RotationComponent.CardinalDirections dir = RotationComponent.CardinalDirections.NONE;

            switch (text)
            {
                case "North":
                    dir = RotationComponent.CardinalDirections.N;
                    break;
                case "NorthEast":
                    dir = RotationComponent.CardinalDirections.NE;
                    break;
                case "East":
                    dir = RotationComponent.CardinalDirections.E;
                    break;
                case "SouthEast":
                    dir = RotationComponent.CardinalDirections.SE;
                    break;
                case "South":
                    dir = RotationComponent.CardinalDirections.S;
                    break;
                case "SouthWest":
                    if (!mFlippingCheckBox.Checked) dir = RotationComponent.CardinalDirections.SW;
                    break;
                case "West":
                    if (!mFlippingCheckBox.Checked) dir = RotationComponent.CardinalDirections.W;
                    break;
                case "NorthWest":
                    if (!mFlippingCheckBox.Checked) dir = RotationComponent.CardinalDirections.NW;
                    break;
                default:
                    break;
            }

            return dir;
        }

        private void mDirectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopAnimation();
            
            // Clear the ImageView to be populated by the newly selected direction
            for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
            {
                mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick -= new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
            }
            mKeyframeImageList.Clear();
            mImageView.Clear();

            // Populate the KeyFrame icon ListView with the selected direction's KeyFrames
            for (int i = 0; i < mCurrentAnimation.Frames[GetDirectionFromString(mDirectionComboBox.Text)].Keys.Count; i++)
            {
                FloatRectangle floatRect = mCurrentAnimation.Frames[GetDirectionFromString(mDirectionComboBox.Text)][i];
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                rect.Width = (int)(floatRect.Width * mSpriteSheetWidth);
                rect.Height = (int)(floatRect.Height * mSpriteSheetHeight);
                rect.X = (int)(floatRect.U * mSpriteSheetWidth);
                rect.Y = (int)(floatRect.V * mSpriteSheetHeight);
                
                Bitmap imageToCopy = new Bitmap(mSpriteSheetImage);
                Bitmap croppedImage = imageToCopy.Clone(rect, imageToCopy.PixelFormat);
                mKeyframeImageList.Add(croppedImage);
            }

            for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
            {
                mImageView.AddImage(mKeyframeImageList[ii]);
                mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick += new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
            }

            mCurrentDirectionText = mDirectionComboBox.Text;
            mCurrentDirection = GetDirectionFromString(mCurrentDirectionText);

            mFrameDurationTextBox.Enabled = false;
            mFrameDurationTextBox.Text = "-";
            mMotionChannelComboBox.Enabled = false;
            mMotionChannelComboBox.SelectedIndex = 0;

            SetTotalFrameCount();
        }

        private void mRemoveAnimationButton_Click(object sender, EventArgs e)
        {
            StopAnimation();
                
            string name = mAnimationNameComboBox.Text;
            mAnimationNameComboBox.Items.Remove(name);
            mAnimationNameComboBox.SelectedItem = null;

            mCurrentAnimation = null;
            mAnimations.Remove(name);

            for (int ii = 0; ii < mKeyframeImageList.Count; ii++)
            {
                mImageView.PictureBoxes[mImageView.PictureBoxes.Count - 1].MouseClick -= new System.Windows.Forms.MouseEventHandler(imageViewKeyframe_Click);
            }
            mKeyframeImageList.Clear();
            mImageView.Clear();

            mDirectionComboBox.Items.Clear();
            mDirectionComboBox.Enabled = false;
            mMultidirCheckBox.Enabled = false;
            mFlippingCheckBox.Enabled = false;
            mMultiSelectLabel.Visible = false;
            mScriptEditorSuite.ClearActiveScript();
            mScriptEditorSuite.Enabled = false;
        }

        private void mFrameDurationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (mSuppressChange)
            {
                mSuppressChange = false;
                return;
            }

            if (mFrameDurationTextBox.Text != "-")
            {
                if (mFrameDurationTextBox.Text.Contains('.'))
                {
                    MessageBox.Show("Frame duration must be a whole number.");
                    if (mImageView.SelectedIndices.Count == 1)
                    {
                        mFrameDurationTextBox.Text = mCurrentAnimation.FrameDurations[mImageView.SelectedIndices[0]].ToString();
                    }
                    else
                    {
                        int duration = mCurrentAnimation.FrameDurations[mImageView.SelectedIndices[0]];
                        foreach (int sel in mImageView.SelectedIndices)
                        {
                            int dur = mCurrentAnimation.FrameDurations[sel];
                            if (dur != duration)
                            {
                                mFrameDurationTextBox.Text = "-";
                                break;
                            }
                            else
                            {
                                mFrameDurationTextBox.Text = duration.ToString();
                            }
                        }
                    }
                    return;
                }

                foreach (int i in mImageView.SelectedIndices)
                {
                    mCurrentAnimation.FrameDurations[i] = Convert.ToInt32(mFrameDurationTextBox.Text);
                }

                SetTotalFrameCount();
            }
        }

        private void mFrameDurAddButton_Click(object sender, EventArgs e)
        {
            foreach (int i in mImageView.SelectedIndices)
            {
                mCurrentAnimation.FrameDurations[i] += 1;
            }
            SetTotalFrameCount();
        }

        private void mFrameDurSubButton_Click(object sender, EventArgs e)
        {
            foreach (int i in mImageView.SelectedIndices)
            {
                mCurrentAnimation.FrameDurations[i] -= 1;
            }
            SetTotalFrameCount();
        }

        private void mImageView_MouseClick(object sender, MouseEventArgs e)
        {
            //if (mImageView.SelectedIndices.Count == 0)
            //{
                mMultiSelectLabel.Visible = false;
                mFrameDurationTextBox.Enabled = false;
                mFrameDurationTextBox.Text = "-";
                mMotionChannelComboBox.Enabled = false;
                mMotionChannelComboBox.SelectedIndex = 0;
                mRemoveKeyframeButton.Enabled = false;

                if (mCurrentAnimation != null)
                {
                    mScriptEditorSuite.SetActiveScript(mCurrentAnimation.AnimationScripts, mCurrentAnimation.Name);
                    mScriptEditorSuite.Enabled = true;
                }
            //}
        }

        private void imageViewKeyframe_Click(object sender, MouseEventArgs e)
        {
            if (mImageView.SelectedIndices.Count > 0)
            {
                if (mImageView.SelectedIndices.Count == 1)
                {
                    mSuppressChange = true;
                    mFrameDurationTextBox.Text = mCurrentAnimation.FrameDurations[mImageView.SelectedIndices[0]].ToString();
                    mSuppressChange = false;
                    mScriptEditorSuite.SetActiveScript(mCurrentAnimation.FrameScripts[mImageView.SelectedIndices[0]],
                                                        mCurrentAnimation.Name,
                                                        mImageView.SelectedIndices[0]);
                    mScriptEditorSuite.Enabled = true;
                    mMultiSelectLabel.Visible = false;
                }
                else
                {
                    int duration = mCurrentAnimation.FrameDurations[mImageView.SelectedIndices[0]];
                    mSuppressChange = true;
                    mFrameDurationTextBox.Text = duration.ToString();
                    mSuppressChange = false;
                    foreach (int sel in mImageView.SelectedIndices)
                    {
                        int dur = mCurrentAnimation.FrameDurations[sel];
                        if (dur != duration)
                        {
                            mFrameDurationTextBox.Text = "-";
                            break;
                        }
                    }

                    mMultiSelectLabel.Visible = true;
                }

                mFrameDurationTextBox.Enabled = true;
                mMotionChannelComboBox.Enabled = true;
                mMotionChannelComboBox.SelectedIndex = 0;
                mRemoveKeyframeButton.Enabled = true;
            }
        }

        private void mFrameLeftButton_Click(object sender, EventArgs e)
        {
            List<int> selected = new List<int>(mImageView.SelectedIndices.Count);
            for (int index = 0; index < mImageView.SelectedIndices.Count; index++)
            {
                selected.Add(mImageView.SelectedIndices[0]);
            }

            // Check if no items are selected in the ListView control
            if (selected.Count == 0)
            {
                return;
            }

            // Check if user is trying to move the first Keyframe left
            if ((selected.Count == 1) && (selected[0] == 0))
            {
                return;
            }

            // Only allow moving 1 item at a time. TEMPORARY fix?
            if (selected.Count > 1)
            {
                return;
            }

            StopAnimation();

            // Swap each selected item with the one before it in the ImageView
            for (int i = 0; i < selected.Count; i++)
            {
                if (selected[i] > 0)
                {
                    // Swap ImageView images
                    mImageView.SwapImages(selected[i], selected[i] - 1);

                    // Swap Keyframe images for use in the Animation preview
                    Bitmap tempKeyframeImage = (Bitmap)mKeyframeImageList[selected[i]].Clone();
                    mKeyframeImageList[selected[i]] = (Bitmap)mKeyframeImageList[selected[i]-1].Clone();
                    mKeyframeImageList[selected[i] - 1] = tempKeyframeImage;

                    // Swap frame durations
                    int tempDuration = mCurrentAnimation.FrameDurations[selected[i]];
                    mCurrentAnimation.FrameDurations[selected[i]] = mCurrentAnimation.FrameDurations[selected[i] - 1];
                    mCurrentAnimation.FrameDurations[selected[i] - 1] = tempDuration;

                    // Swap rectangles
                    FloatRectangle tempRect = mCurrentAnimation.Frames[mCurrentDirection][selected[i]];
                    mCurrentAnimation.Frames[mCurrentDirection][selected[i]] = mCurrentAnimation.Frames[mCurrentDirection][selected[i] - 1];
                    mCurrentAnimation.Frames[mCurrentDirection][selected[i] - 1] = tempRect;

                    mCurrentAnimation.MoveFrameScript(selected[i], -1);
                }
            }
        }

        private void mFrameRightButton_Click(object sender, EventArgs e)
        {
            List<int> selected = new List<int>(mImageView.SelectedIndices.Count);
            for (int index = 0; index < mImageView.SelectedIndices.Count; index++)
            {
                selected.Add(mImageView.SelectedIndices[0]);
            }

            // Check if no items are selected in the ListView control
            if (selected.Count == 0)
            {
                return;
            }

            // Check if user is trying to move the first Keyframe left
            if ((selected.Count == 1) && (selected[0] == mImageView.ImageCount-1))
            {
                return;
            }

            // Only allow moving 1 item at a time. TEMPORARY fix?
            if (selected.Count > 1)
            {
                return;
            }

            StopAnimation();

            // Swap each selected item with the one after it in the ImageView
            for (int i = 0; i < selected.Count; i++)
            {
                if (selected[i] < mImageView.ImageCount)
                {
                    // Swap ImageView images
                    mImageView.SwapImages(selected[i], selected[i] + 1);

                    // Swap Keyframe images for use in the Animation preview
                    Bitmap tempKeyframeImage = (Bitmap)mKeyframeImageList[selected[i]].Clone();
                    mKeyframeImageList[selected[i]] = (Bitmap)mKeyframeImageList[selected[i] + 1].Clone();
                    mKeyframeImageList[selected[i] + 1] = tempKeyframeImage;

                    // Swap frame durations
                    int tempDuration = mCurrentAnimation.FrameDurations[selected[i]];
                    mCurrentAnimation.FrameDurations[selected[i]] = mCurrentAnimation.FrameDurations[selected[i] + 1];
                    mCurrentAnimation.FrameDurations[selected[i] + 1] = tempDuration;

                    // Swap rectangles
                    FloatRectangle tempRect = mCurrentAnimation.Frames[mCurrentDirection][selected[i]];
                    mCurrentAnimation.Frames[mCurrentDirection][selected[i]] = mCurrentAnimation.Frames[mCurrentDirection][selected[i] + 1];
                    mCurrentAnimation.Frames[mCurrentDirection][selected[i] + 1] = tempRect;

                    mCurrentAnimation.MoveFrameScript(selected[i], 1);
                }
            }
        }

        private void mMotionChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mMotionChannelComboBox.SelectedIndex == 0)
            {
                mMotionDeltaXBox.Enabled = false;
                mMotionDeltaYBox.Enabled = false;
            }
            else
            {
                Dictionary<int, Dictionary<int, Vector2>> motions = mCurrentAnimation.FrameMotionDeltas;
                if (!motions.Keys.Contains(mImageView.SelectedIndices[0]))
                {
                    motions[mImageView.SelectedIndices[0]] = new Dictionary<int, Vector2>();
                    motions[mImageView.SelectedIndices[0]][mMotionChannelComboBox.SelectedIndex-1] = new Vector2(0f, 0f);
                }

                Vector2 mdv = Vector2.Zero;
                if (motions[mImageView.SelectedIndices[0]].ContainsKey(mMotionChannelComboBox.SelectedIndex - 1))
                {
                    mdv = motions[mImageView.SelectedIndices[0]][mMotionChannelComboBox.SelectedIndex - 1];
                }

                //mSuppressChange = true;
                mMotionDeltaXBox.Value = (decimal)mdv.X;
                mMotionDeltaYBox.Value = (decimal)mdv.Y;
                mMotionDeltaXBox.Enabled = true;
                mMotionDeltaYBox.Enabled = true;
            }            
        }

        private void mMotionChannelComboBox_EnabledChanged(object sender, EventArgs e)
        {
            if (mMotionChannelComboBox.Enabled == false)
            {
                mMotionDeltaXBox.Enabled = false;
                mMotionDeltaYBox.Enabled = false;
            }
        }

        private void mMotionDeltaBox_ValueChanged(object sender, EventArgs e)
        {
            if (mSuppressChange)
            {
                mSuppressChange = false;
                return;
            }

            Vector2 mdv = new Vector2((float)mMotionDeltaXBox.Value, (float)mMotionDeltaYBox.Value);
            foreach (int index in mImageView.SelectedIndices)
            {
                mCurrentAnimation.ChangeFrameMotionDelta(index, mMotionChannelComboBox.SelectedIndex - 1, mdv);
            }
        }
    }
}
