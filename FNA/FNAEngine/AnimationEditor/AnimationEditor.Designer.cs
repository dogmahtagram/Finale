namespace FNA
{
    partial class AnimationEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.mAnimationTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.mFrameTimeButton1 = new System.Windows.Forms.RadioButton();
            this.mTotalTimeNumberBox = new System.Windows.Forms.NumericUpDown();
            this.mFrameTimeButton2 = new System.Windows.Forms.RadioButton();
            this.mTimePerFrameNumberBox = new System.Windows.Forms.NumericUpDown();
            this.mFPSNumberBox = new System.Windows.Forms.NumericUpDown();
            this.mPlayStopLabel = new System.Windows.Forms.Label();
            this.deleteLabel = new System.Windows.Forms.Label();
            this.ctrlALabel = new System.Windows.Forms.Label();
            this.mAnimationPictureBox = new System.Windows.Forms.PictureBox();
            this.mGridOffsetNumber = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.mRemoveFrameButton = new System.Windows.Forms.Button();
            this.mAddFrameButton = new System.Windows.Forms.Button();
            this.mHeightComboBox = new System.Windows.Forms.ComboBox();
            this.mGridSizeLinkButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mWidthComboBox = new System.Windows.Forms.ComboBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mFramePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mFrameTimeButton3 = new System.Windows.Forms.RadioButton();
            this.mPictureBox = new ScrollingPictureBox();
            this.mFrameListView = new DragDropListView();
            this.panel1.SuspendLayout();
            this.mAnimationTimeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTotalTimeNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTimePerFrameNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mFPSNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mAnimationPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridOffsetNumber)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.mFramePropertyGrid);
            this.panel1.Controls.Add(this.mAnimationTimeGroupBox);
            this.panel1.Controls.Add(this.deleteLabel);
            this.panel1.Controls.Add(this.ctrlALabel);
            this.panel1.Controls.Add(this.mPictureBox);
            this.panel1.Controls.Add(this.mFrameListView);
            this.panel1.Controls.Add(this.mGridOffsetNumber);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.mRemoveFrameButton);
            this.panel1.Controls.Add(this.mAddFrameButton);
            this.panel1.Controls.Add(this.mHeightComboBox);
            this.panel1.Controls.Add(this.mGridSizeLinkButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.mWidthComboBox);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(888, 710);
            this.panel1.TabIndex = 0;
            // 
            // mAnimationTimeGroupBox
            // 
            this.mAnimationTimeGroupBox.Controls.Add(this.mFrameTimeButton1);
            this.mAnimationTimeGroupBox.Controls.Add(this.mFrameTimeButton3);
            this.mAnimationTimeGroupBox.Controls.Add(this.mTotalTimeNumberBox);
            this.mAnimationTimeGroupBox.Controls.Add(this.mFrameTimeButton2);
            this.mAnimationTimeGroupBox.Controls.Add(this.mTimePerFrameNumberBox);
            this.mAnimationTimeGroupBox.Controls.Add(this.mFPSNumberBox);
            this.mAnimationTimeGroupBox.Location = new System.Drawing.Point(566, 179);
            this.mAnimationTimeGroupBox.Name = "mAnimationTimeGroupBox";
            this.mAnimationTimeGroupBox.Size = new System.Drawing.Size(256, 100);
            this.mAnimationTimeGroupBox.TabIndex = 33;
            this.mAnimationTimeGroupBox.TabStop = false;
            this.mAnimationTimeGroupBox.Text = "Animation Timing";
            // 
            // mFrameTimeButton1
            // 
            this.mFrameTimeButton1.AutoSize = true;
            this.mFrameTimeButton1.Location = new System.Drawing.Point(6, 21);
            this.mFrameTimeButton1.Name = "mFrameTimeButton1";
            this.mFrameTimeButton1.Size = new System.Drawing.Size(166, 21);
            this.mFrameTimeButton1.TabIndex = 30;
            this.mFrameTimeButton1.TabStop = true;
            this.mFrameTimeButton1.Text = "Time Per Frame (ms):";
            this.mFrameTimeButton1.UseVisualStyleBackColor = true;
            // 
            // mTotalTimeNumberBox
            // 
            this.mTotalTimeNumberBox.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.mTotalTimeNumberBox.Location = new System.Drawing.Point(178, 47);
            this.mTotalTimeNumberBox.Maximum = new decimal(new int[] {
            1316134902,
            2328,
            0,
            0});
            this.mTotalTimeNumberBox.Name = "mTotalTimeNumberBox";
            this.mTotalTimeNumberBox.Size = new System.Drawing.Size(70, 22);
            this.mTotalTimeNumberBox.TabIndex = 28;
            this.mTotalTimeNumberBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mTotalTimeNumberBox.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.mTotalTimeNumberBox.ValueChanged += new System.EventHandler(this.mTotalTimeNumberBox_ValueChanged_1);
            // 
            // mFrameTimeButton2
            // 
            this.mFrameTimeButton2.AutoSize = true;
            this.mFrameTimeButton2.Location = new System.Drawing.Point(6, 48);
            this.mFrameTimeButton2.Name = "mFrameTimeButton2";
            this.mFrameTimeButton2.Size = new System.Drawing.Size(132, 21);
            this.mFrameTimeButton2.TabIndex = 31;
            this.mFrameTimeButton2.TabStop = true;
            this.mFrameTimeButton2.Text = "Total Time (ms):";
            this.mFrameTimeButton2.UseVisualStyleBackColor = true;
            // 
            // mTimePerFrameNumberBox
            // 
            this.mTimePerFrameNumberBox.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.mTimePerFrameNumberBox.Location = new System.Drawing.Point(178, 21);
            this.mTimePerFrameNumberBox.Maximum = new decimal(new int[] {
            1316134902,
            2328,
            0,
            0});
            this.mTimePerFrameNumberBox.Name = "mTimePerFrameNumberBox";
            this.mTimePerFrameNumberBox.Size = new System.Drawing.Size(70, 22);
            this.mTimePerFrameNumberBox.TabIndex = 7;
            this.mTimePerFrameNumberBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mTimePerFrameNumberBox.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.mTimePerFrameNumberBox.ValueChanged += new System.EventHandler(this.mTimePerFrameNumberBox_ValueChanged);
            // 
            // mFPSNumberBox
            // 
            this.mFPSNumberBox.Location = new System.Drawing.Point(178, 75);
            this.mFPSNumberBox.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.mFPSNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mFPSNumberBox.Name = "mFPSNumberBox";
            this.mFPSNumberBox.Size = new System.Drawing.Size(70, 22);
            this.mFPSNumberBox.TabIndex = 8;
            this.mFPSNumberBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mFPSNumberBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.mFPSNumberBox.ValueChanged += new System.EventHandler(this.mFPSNumberBox_ValueChanged);
            // 
            // mPlayStopLabel
            // 
            this.mPlayStopLabel.AutoSize = true;
            this.mPlayStopLabel.Location = new System.Drawing.Point(931, 323);
            this.mPlayStopLabel.Name = "mPlayStopLabel";
            this.mPlayStopLabel.Size = new System.Drawing.Size(237, 17);
            this.mPlayStopLabel.TabIndex = 0;
            this.mPlayStopLabel.Text = "Click to Play/Stop Animation (Space)";
            // 
            // deleteLabel
            // 
            this.deleteLabel.AutoSize = true;
            this.deleteLabel.Location = new System.Drawing.Point(800, 678);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(59, 17);
            this.deleteLabel.TabIndex = 27;
            this.deleteLabel.Text = "(Delete)";
            // 
            // ctrlALabel
            // 
            this.ctrlALabel.AutoSize = true;
            this.ctrlALabel.Location = new System.Drawing.Point(16, 678);
            this.ctrlALabel.Name = "ctrlALabel";
            this.ctrlALabel.Size = new System.Drawing.Size(56, 17);
            this.ctrlALabel.TabIndex = 26;
            this.ctrlALabel.Text = "(Ctrl+A)";
            // 
            // mAnimationPictureBox
            // 
            this.mAnimationPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.mAnimationPictureBox.Location = new System.Drawing.Point(917, 64);
            this.mAnimationPictureBox.Name = "mAnimationPictureBox";
            this.mAnimationPictureBox.Size = new System.Drawing.Size(256, 256);
            this.mAnimationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mAnimationPictureBox.TabIndex = 24;
            this.mAnimationPictureBox.TabStop = false;
            this.mAnimationPictureBox.Click += new System.EventHandler(this.mAnimationPictureBox_Click);
            // 
            // mGridOffsetNumber
            // 
            this.mGridOffsetNumber.Location = new System.Drawing.Point(686, 130);
            this.mGridOffsetNumber.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.mGridOffsetNumber.Name = "mGridOffsetNumber";
            this.mGridOffsetNumber.Size = new System.Drawing.Size(69, 22);
            this.mGridOffsetNumber.TabIndex = 4;
            this.mGridOffsetNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(563, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Grid Offset:";
            // 
            // mRemoveFrameButton
            // 
            this.mRemoveFrameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mRemoveFrameButton.Image = global::AnimationEditor.Properties.Resources.x32;
            this.mRemoveFrameButton.Location = new System.Drawing.Point(803, 627);
            this.mRemoveFrameButton.Name = "mRemoveFrameButton";
            this.mRemoveFrameButton.Size = new System.Drawing.Size(48, 48);
            this.mRemoveFrameButton.TabIndex = 10;
            this.mRemoveFrameButton.TabStop = false;
            this.mRemoveFrameButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mRemoveFrameButton.UseVisualStyleBackColor = true;
            this.mRemoveFrameButton.Click += new System.EventHandler(this.mRemoveFrameButton_Click);
            // 
            // mAddFrameButton
            // 
            this.mAddFrameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAddFrameButton.Image = global::AnimationEditor.Properties.Resources.plus32;
            this.mAddFrameButton.Location = new System.Drawing.Point(19, 627);
            this.mAddFrameButton.Name = "mAddFrameButton";
            this.mAddFrameButton.Size = new System.Drawing.Size(48, 48);
            this.mAddFrameButton.TabIndex = 9;
            this.mAddFrameButton.TabStop = false;
            this.mAddFrameButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mAddFrameButton.UseVisualStyleBackColor = true;
            this.mAddFrameButton.Click += new System.EventHandler(this.mAddFrameButton_Click);
            // 
            // mHeightComboBox
            // 
            this.mHeightComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mHeightComboBox.FormattingEnabled = true;
            this.mHeightComboBox.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128"});
            this.mHeightComboBox.Location = new System.Drawing.Point(686, 81);
            this.mHeightComboBox.Name = "mHeightComboBox";
            this.mHeightComboBox.Size = new System.Drawing.Size(69, 26);
            this.mHeightComboBox.TabIndex = 2;
            this.mHeightComboBox.SelectedIndexChanged += new System.EventHandler(this.mHeightComboBox_SelectedIndexChanged);
            this.mHeightComboBox.TextChanged += new System.EventHandler(this.mHeightComboBox_TextChanged);
            // 
            // mGridSizeLinkButton
            // 
            this.mGridSizeLinkButton.Location = new System.Drawing.Point(769, 54);
            this.mGridSizeLinkButton.Name = "mGridSizeLinkButton";
            this.mGridSizeLinkButton.Size = new System.Drawing.Size(50, 47);
            this.mGridSizeLinkButton.TabIndex = 3;
            this.mGridSizeLinkButton.Text = "=";
            this.mGridSizeLinkButton.UseVisualStyleBackColor = true;
            this.mGridSizeLinkButton.Click += new System.EventHandler(this.mGridSizeLinkButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(636, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(636, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Width";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(560, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grid Size:";
            // 
            // mWidthComboBox
            // 
            this.mWidthComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mWidthComboBox.FormattingEnabled = true;
            this.mWidthComboBox.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128"});
            this.mWidthComboBox.Location = new System.Drawing.Point(686, 51);
            this.mWidthComboBox.Name = "mWidthComboBox";
            this.mWidthComboBox.Size = new System.Drawing.Size(69, 26);
            this.mWidthComboBox.TabIndex = 1;
            this.mWidthComboBox.SelectedIndexChanged += new System.EventHandler(this.mWidthComboBox_SelectedIndexChanged);
            this.mWidthComboBox.TextChanged += new System.EventHandler(this.mWidthComboBox_TextChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(566, 482);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(166, 21);
            this.radioButton1.TabIndex = 30;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Time Per Frame (ms):";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(566, 542);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(158, 21);
            this.radioButton3.TabIndex = 32;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Frames Per Second:";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1188, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSpriteSheetToolStripMenuItem,
            this.exportAnimationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadSpriteSheetToolStripMenuItem
            // 
            this.loadSpriteSheetToolStripMenuItem.Name = "loadSpriteSheetToolStripMenuItem";
            this.loadSpriteSheetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadSpriteSheetToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.loadSpriteSheetToolStripMenuItem.Text = "Load Sprite Sheet";
            this.loadSpriteSheetToolStripMenuItem.Click += new System.EventHandler(this.loadSpriteSheetToolStripMenuItem_Click);
            // 
            // exportAnimationToolStripMenuItem
            // 
            this.exportAnimationToolStripMenuItem.Name = "exportAnimationToolStripMenuItem";
            this.exportAnimationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportAnimationToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.exportAnimationToolStripMenuItem.Text = "Export Animation";
            // 
            // mFramePropertyGrid
            // 
            this.mFramePropertyGrid.Location = new System.Drawing.Point(553, 301);
            this.mFramePropertyGrid.Name = "mFramePropertyGrid";
            this.mFramePropertyGrid.Size = new System.Drawing.Size(319, 270);
            this.mFramePropertyGrid.TabIndex = 2;
            this.mFramePropertyGrid.TabStop = false;
            // 
            // mFrameTimeButton3
            // 
            this.mFrameTimeButton3.AutoSize = true;
            this.mFrameTimeButton3.Location = new System.Drawing.Point(6, 75);
            this.mFrameTimeButton3.Name = "mFrameTimeButton3";
            this.mFrameTimeButton3.Size = new System.Drawing.Size(158, 21);
            this.mFrameTimeButton3.TabIndex = 32;
            this.mFrameTimeButton3.TabStop = true;
            this.mFrameTimeButton3.Text = "Frames Per Second:";
            this.mFrameTimeButton3.UseVisualStyleBackColor = true;
            // 
            // mPictureBox
            // 
            this.mPictureBox.BackColor = System.Drawing.Color.White;
            this.mPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mPictureBox.Border = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mPictureBox.Location = new System.Drawing.Point(3, 51);
            this.mPictureBox.Name = "mPictureBox";
            this.mPictureBox.Picture = "";
            this.mPictureBox.Size = new System.Drawing.Size(537, 537);
            this.mPictureBox.TabIndex = 25;
            this.mPictureBox.TabStop = false;
            // 
            // mFrameListView
            // 
            this.mFrameListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.mFrameListView.AllowDrop = true;
            this.mFrameListView.AllowRowReorder = true;
            this.mFrameListView.Location = new System.Drawing.Point(73, 600);
            this.mFrameListView.Name = "mFrameListView";
            this.mFrameListView.Size = new System.Drawing.Size(724, 107);
            this.mFrameListView.TabIndex = 23;
            this.mFrameListView.TabStop = false;
            this.mFrameListView.UseCompatibleStateImageBehavior = false;
            this.mFrameListView.SelectedIndexChanged += new System.EventHandler(this.mFrameListView_SelectedIndexChanged);
            // 
            // AnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 735);
            this.Controls.Add(this.mPlayStopLabel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mAnimationPictureBox);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AnimationEditor";
            this.Text = "FNA Animation Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.mAnimationTimeGroupBox.ResumeLayout(false);
            this.mAnimationTimeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTotalTimeNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTimePerFrameNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mFPSNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mAnimationPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridOffsetNumber)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox mWidthComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mGridSizeLinkButton;
        private System.Windows.Forms.ComboBox mHeightComboBox;
        private System.Windows.Forms.NumericUpDown mFPSNumberBox;
        private System.Windows.Forms.NumericUpDown mTimePerFrameNumberBox;
        private System.Windows.Forms.Button mAddFrameButton;
        private System.Windows.Forms.Button mRemoveFrameButton;
        private System.Windows.Forms.NumericUpDown mGridOffsetNumber;
        private System.Windows.Forms.Label label4;
        private DragDropListView mFrameListView;
        private System.Windows.Forms.PictureBox mAnimationPictureBox;
        private System.Windows.Forms.Label mPlayStopLabel;
        private ScrollingPictureBox mPictureBox;
        private System.Windows.Forms.Label deleteLabel;
        private System.Windows.Forms.Label ctrlALabel;
        private System.Windows.Forms.NumericUpDown mTotalTimeNumberBox;
        private System.Windows.Forms.RadioButton mFrameTimeButton2;
        private System.Windows.Forms.RadioButton mFrameTimeButton1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.GroupBox mAnimationTimeGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAnimationToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid mFramePropertyGrid;
        private System.Windows.Forms.RadioButton mFrameTimeButton3;
    }
}

