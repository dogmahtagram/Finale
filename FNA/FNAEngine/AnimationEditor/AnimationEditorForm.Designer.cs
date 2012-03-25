namespace FNA.AnimationEditor
{
    partial class AnimationEditorForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationEditorForm));
            this.mFrameDurationTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.mAddAnimationButton = new System.Windows.Forms.Button();
            this.mRemoveAnimationButton = new System.Windows.Forms.Button();
            this.mAnimationNameComboBox = new System.Windows.Forms.ComboBox();
            this.mExportButton = new System.Windows.Forms.Button();
            this.mDirectionComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mMotionDeltaYBox = new System.Windows.Forms.NumericUpDown();
            this.mMotionDeltaXBox = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.mMultidirCheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.mTotalTimeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mTotalFrameCountBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mGridOffsetNumber = new System.Windows.Forms.NumericUpDown();
            this.mPlayStopLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mAnimationPictureBox = new System.Windows.Forms.PictureBox();
            this.mHeightComboBox = new System.Windows.Forms.ComboBox();
            this.mGridSizeLinkButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mWidthComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAnimation = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.mAddKeyframeButton = new System.Windows.Forms.Button();
            this.mRemoveKeyframeButton = new System.Windows.Forms.Button();
            this.mFrameLeftButton = new System.Windows.Forms.Button();
            this.mMotionChannelComboBox = new System.Windows.Forms.ComboBox();
            this.mMultiSelectLabel = new System.Windows.Forms.Label();
            this.mFrameDurAddButton = new System.Windows.Forms.Button();
            this.mFrameDurSubButton = new System.Windows.Forms.Button();
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.mAnimationTab = new System.Windows.Forms.TabPage();
            this.mGridPropertiesPanel = new System.Windows.Forms.Panel();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.mFlippingCheckBox = new System.Windows.Forms.CheckBox();
            this.mScriptTab = new System.Windows.Forms.TabPage();
            this.ctrlALabel = new System.Windows.Forms.Label();
            this.deleteLabel = new System.Windows.Forms.Label();
            this.mKeyframePanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mFrameRightButton = new System.Windows.Forms.Button();
            this.mPictureBox = new ScrollingPictureBox();
            this.mImageView = new FNA.AnimationEditor.ImageView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.mMotionDeltaYBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMotionDeltaXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridOffsetNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mAnimationPictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.mTabControl.SuspendLayout();
            this.mAnimationTab.SuspendLayout();
            this.mGridPropertiesPanel.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.mKeyframePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mFrameDurationTextBox
            // 
            this.mFrameDurationTextBox.Enabled = false;
            this.mFrameDurationTextBox.Location = new System.Drawing.Point(41, 430);
            this.mFrameDurationTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mFrameDurationTextBox.Name = "mFrameDurationTextBox";
            this.mFrameDurationTextBox.Size = new System.Drawing.Size(38, 20);
            this.mFrameDurationTextBox.TabIndex = 47;
            this.mFrameDurationTextBox.Text = "-";
            this.mFrameDurationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mTooltip.SetToolTip(this.mFrameDurationTextBox, "The number of frames the selected Keyrame(s) lasts.\r\n\'-\' indicates unequal frame " +
                    "counts for multiple selected Keyframes.\r\nNote: Frame duration is shared by all d" +
                    "irections.");
            this.mFrameDurationTextBox.TextChanged += new System.EventHandler(this.mFrameDurationTextBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(14, 412);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "Frame Duration:";
            // 
            // mAddAnimationButton
            // 
            this.mAddAnimationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAddAnimationButton.Image = global::AnimationEditor.Properties.Resources.plus32;
            this.mAddAnimationButton.Location = new System.Drawing.Point(33, 17);
            this.mAddAnimationButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAddAnimationButton.Name = "mAddAnimationButton";
            this.mAddAnimationButton.Size = new System.Drawing.Size(30, 32);
            this.mAddAnimationButton.TabIndex = 44;
            this.mAddAnimationButton.TabStop = false;
            this.mAddAnimationButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mTooltip.SetToolTip(this.mAddAnimationButton, "Create a new Animation and add it to the list.");
            this.mAddAnimationButton.UseVisualStyleBackColor = true;
            this.mAddAnimationButton.Click += new System.EventHandler(this.mAddAnimationButton_Click);
            // 
            // mRemoveAnimationButton
            // 
            this.mRemoveAnimationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mRemoveAnimationButton.Image = global::AnimationEditor.Properties.Resources.x32;
            this.mRemoveAnimationButton.Location = new System.Drawing.Point(76, 17);
            this.mRemoveAnimationButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mRemoveAnimationButton.Name = "mRemoveAnimationButton";
            this.mRemoveAnimationButton.Size = new System.Drawing.Size(30, 32);
            this.mRemoveAnimationButton.TabIndex = 43;
            this.mRemoveAnimationButton.TabStop = false;
            this.mRemoveAnimationButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mTooltip.SetToolTip(this.mRemoveAnimationButton, "Remove the current Animation from the list of Animations.\r\nIt will no longer appe" +
                    "ar in the Export file.");
            this.mRemoveAnimationButton.UseVisualStyleBackColor = true;
            this.mRemoveAnimationButton.Click += new System.EventHandler(this.mRemoveAnimationButton_Click);
            // 
            // mAnimationNameComboBox
            // 
            this.mAnimationNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mAnimationNameComboBox.FormattingEnabled = true;
            this.mAnimationNameComboBox.Location = new System.Drawing.Point(107, 67);
            this.mAnimationNameComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAnimationNameComboBox.Name = "mAnimationNameComboBox";
            this.mAnimationNameComboBox.Size = new System.Drawing.Size(122, 21);
            this.mAnimationNameComboBox.TabIndex = 42;
            this.mAnimationNameComboBox.SelectedIndexChanged += new System.EventHandler(this.mAnimationNameComboBox_SelectedIndexChanged);
            // 
            // mExportButton
            // 
            this.mExportButton.Image = global::AnimationEditor.Properties.Resources.export32;
            this.mExportButton.Location = new System.Drawing.Point(169, 17);
            this.mExportButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mExportButton.Name = "mExportButton";
            this.mExportButton.Size = new System.Drawing.Size(30, 32);
            this.mExportButton.TabIndex = 41;
            this.mExportButton.TabStop = false;
            this.mExportButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mTooltip.SetToolTip(this.mExportButton, "Exports all the Animations in the \"Animation Name\" list to a file.");
            this.mExportButton.UseVisualStyleBackColor = true;
            this.mExportButton.Click += new System.EventHandler(this.mExportButton_Click);
            // 
            // mDirectionComboBox
            // 
            this.mDirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mDirectionComboBox.FormattingEnabled = true;
            this.mDirectionComboBox.Location = new System.Drawing.Point(133, 102);
            this.mDirectionComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mDirectionComboBox.Name = "mDirectionComboBox";
            this.mDirectionComboBox.Size = new System.Drawing.Size(96, 21);
            this.mDirectionComboBox.TabIndex = 39;
            this.mDirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.mDirectionComboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 129);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(181, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "----------------------------------------------------------";
            // 
            // mMotionDeltaYBox
            // 
            this.mMotionDeltaYBox.DecimalPlaces = 2;
            this.mMotionDeltaYBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.mMotionDeltaYBox.Location = new System.Drawing.Point(133, 474);
            this.mMotionDeltaYBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mMotionDeltaYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mMotionDeltaYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.mMotionDeltaYBox.Name = "mMotionDeltaYBox";
            this.mMotionDeltaYBox.Size = new System.Drawing.Size(52, 20);
            this.mMotionDeltaYBox.TabIndex = 35;
            this.mMotionDeltaYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mTooltip.SetToolTip(this.mMotionDeltaYBox, "Y movement");
            this.mMotionDeltaYBox.ValueChanged += new System.EventHandler(this.mMotionDeltaBox_ValueChanged);
            // 
            // mMotionDeltaXBox
            // 
            this.mMotionDeltaXBox.DecimalPlaces = 2;
            this.mMotionDeltaXBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.mMotionDeltaXBox.Location = new System.Drawing.Point(133, 454);
            this.mMotionDeltaXBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mMotionDeltaXBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mMotionDeltaXBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.mMotionDeltaXBox.Name = "mMotionDeltaXBox";
            this.mMotionDeltaXBox.Size = new System.Drawing.Size(52, 20);
            this.mMotionDeltaXBox.TabIndex = 34;
            this.mMotionDeltaXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mTooltip.SetToolTip(this.mMotionDeltaXBox, "X movement");
            this.mMotionDeltaXBox.ValueChanged += new System.EventHandler(this.mMotionDeltaBox_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(131, 412);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Motion Delta:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mTooltip.SetToolTip(this.label10, "Assume movement to the East.");
            // 
            // mMultidirCheckBox
            // 
            this.mMultidirCheckBox.AutoSize = true;
            this.mMultidirCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mMultidirCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mMultidirCheckBox.Location = new System.Drawing.Point(8, 95);
            this.mMultidirCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mMultidirCheckBox.Name = "mMultidirCheckBox";
            this.mMultidirCheckBox.Size = new System.Drawing.Size(122, 17);
            this.mMultidirCheckBox.TabIndex = 32;
            this.mMultidirCheckBox.Text = "Multi-Directional:";
            this.mMultidirCheckBox.UseVisualStyleBackColor = true;
            this.mMultidirCheckBox.CheckedChanged += new System.EventHandler(this.mMultidirCheckBox_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 70);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Animation Name:";
            // 
            // mTotalTimeTextBox
            // 
            this.mTotalTimeTextBox.Enabled = false;
            this.mTotalTimeTextBox.Location = new System.Drawing.Point(169, 147);
            this.mTotalTimeTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mTotalTimeTextBox.Name = "mTotalTimeTextBox";
            this.mTotalTimeTextBox.Size = new System.Drawing.Size(38, 20);
            this.mTotalTimeTextBox.TabIndex = 28;
            this.mTotalTimeTextBox.Text = "0";
            this.mTotalTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(115, 148);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Time (s):";
            // 
            // mTotalFrameCountBox
            // 
            this.mTotalFrameCountBox.Enabled = false;
            this.mTotalFrameCountBox.Location = new System.Drawing.Point(64, 147);
            this.mTotalFrameCountBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mTotalFrameCountBox.Name = "mTotalFrameCountBox";
            this.mTotalFrameCountBox.Size = new System.Drawing.Size(38, 20);
            this.mTotalFrameCountBox.TabIndex = 26;
            this.mTotalFrameCountBox.Text = "0";
            this.mTotalFrameCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 148);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Frames:";
            // 
            // mGridOffsetNumber
            // 
            this.mGridOffsetNumber.Location = new System.Drawing.Point(333, 6);
            this.mGridOffsetNumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mGridOffsetNumber.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.mGridOffsetNumber.Name = "mGridOffsetNumber";
            this.mGridOffsetNumber.Size = new System.Drawing.Size(52, 20);
            this.mGridOffsetNumber.TabIndex = 4;
            this.mGridOffsetNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mPlayStopLabel
            // 
            this.mPlayStopLabel.AutoSize = true;
            this.mPlayStopLabel.Location = new System.Drawing.Point(28, 383);
            this.mPlayStopLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.mPlayStopLabel.Name = "mPlayStopLabel";
            this.mPlayStopLabel.Size = new System.Drawing.Size(181, 13);
            this.mPlayStopLabel.TabIndex = 0;
            this.mPlayStopLabel.Text = "Click to Play/Stop Animation (Space)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(267, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Grid Offset:";
            // 
            // mAnimationPictureBox
            // 
            this.mAnimationPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.mAnimationPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mAnimationPictureBox.Location = new System.Drawing.Point(17, 173);
            this.mAnimationPictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAnimationPictureBox.Name = "mAnimationPictureBox";
            this.mAnimationPictureBox.Size = new System.Drawing.Size(193, 209);
            this.mAnimationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mAnimationPictureBox.TabIndex = 24;
            this.mAnimationPictureBox.TabStop = false;
            this.mTooltip.SetToolTip(this.mAnimationPictureBox, "Displays a preview of the Animation using the current Keyframes.");
            this.mAnimationPictureBox.Click += new System.EventHandler(this.mAnimationPictureBox_Click);
            // 
            // mHeightComboBox
            // 
            this.mHeightComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mHeightComboBox.FormattingEnabled = true;
            this.mHeightComboBox.Items.AddRange(new object[] {
            "8",
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024"});
            this.mHeightComboBox.Location = new System.Drawing.Point(146, 6);
            this.mHeightComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mHeightComboBox.Name = "mHeightComboBox";
            this.mHeightComboBox.Size = new System.Drawing.Size(53, 23);
            this.mHeightComboBox.TabIndex = 2;
            this.mHeightComboBox.SelectedIndexChanged += new System.EventHandler(this.mHeightComboBox_SelectedIndexChanged);
            this.mHeightComboBox.TextChanged += new System.EventHandler(this.mHeightComboBox_TextChanged);
            // 
            // mGridSizeLinkButton
            // 
            this.mGridSizeLinkButton.Location = new System.Drawing.Point(117, 3);
            this.mGridSizeLinkButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mGridSizeLinkButton.Name = "mGridSizeLinkButton";
            this.mGridSizeLinkButton.Size = new System.Drawing.Size(24, 26);
            this.mGridSizeLinkButton.TabIndex = 3;
            this.mGridSizeLinkButton.Text = "=";
            this.mGridSizeLinkButton.UseVisualStyleBackColor = true;
            this.mGridSizeLinkButton.Click += new System.EventHandler(this.mGridSizeLinkButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Width";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grid Size:";
            this.mTooltip.SetToolTip(this.label1, "The size of the selection grid in the below PictureBox.");
            // 
            // mWidthComboBox
            // 
            this.mWidthComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mWidthComboBox.FormattingEnabled = true;
            this.mWidthComboBox.Items.AddRange(new object[] {
            "8",
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024"});
            this.mWidthComboBox.Location = new System.Drawing.Point(61, 6);
            this.mWidthComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mWidthComboBox.Name = "mWidthComboBox";
            this.mWidthComboBox.Size = new System.Drawing.Size(53, 23);
            this.mWidthComboBox.TabIndex = 1;
            this.mWidthComboBox.SelectedIndexChanged += new System.EventHandler(this.mWidthComboBox_SelectedIndexChanged);
            this.mWidthComboBox.TextChanged += new System.EventHandler(this.mWidthComboBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 398);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "----------------------------------------------------------";
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
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuAnimation,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(792, 32);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.exportToolStripMenuItem});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // menuAnimation
            // 
            this.menuAnimation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSpriteSheetToolStripMenuItem});
            this.menuAnimation.Name = "menuAnimation";
            this.menuAnimation.Size = new System.Drawing.Size(75, 20);
            this.menuAnimation.Text = "Animation";
            // 
            // loadSpriteSheetToolStripMenuItem
            // 
            this.loadSpriteSheetToolStripMenuItem.Name = "loadSpriteSheetToolStripMenuItem";
            this.loadSpriteSheetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadSpriteSheetToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.loadSpriteSheetToolStripMenuItem.Text = "Load Sprite Sheet";
            this.loadSpriteSheetToolStripMenuItem.Click += new System.EventHandler(this.loadSpriteSheetToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // noToolStripMenuItem
            // 
            this.noToolStripMenuItem.Name = "noToolStripMenuItem";
            this.noToolStripMenuItem.Size = new System.Drawing.Size(90, 22);
            this.noToolStripMenuItem.Text = "No";
            // 
            // mAddKeyframeButton
            // 
            this.mAddKeyframeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAddKeyframeButton.Image = global::AnimationEditor.Properties.Resources.plus32;
            this.mAddKeyframeButton.Location = new System.Drawing.Point(8, 10);
            this.mAddKeyframeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAddKeyframeButton.Name = "mAddKeyframeButton";
            this.mAddKeyframeButton.Size = new System.Drawing.Size(36, 39);
            this.mAddKeyframeButton.TabIndex = 9;
            this.mAddKeyframeButton.TabStop = false;
            this.mAddKeyframeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mTooltip.SetToolTip(this.mAddKeyframeButton, "Adds the currently selected rectangle in the above PictureBox as a Keyframe.");
            this.mAddKeyframeButton.UseVisualStyleBackColor = true;
            this.mAddKeyframeButton.Click += new System.EventHandler(this.mAddFrameButton_Click);
            // 
            // mRemoveKeyframeButton
            // 
            this.mRemoveKeyframeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mRemoveKeyframeButton.Image = global::AnimationEditor.Properties.Resources.x32;
            this.mRemoveKeyframeButton.Location = new System.Drawing.Point(55, 10);
            this.mRemoveKeyframeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mRemoveKeyframeButton.Name = "mRemoveKeyframeButton";
            this.mRemoveKeyframeButton.Size = new System.Drawing.Size(36, 39);
            this.mRemoveKeyframeButton.TabIndex = 10;
            this.mRemoveKeyframeButton.TabStop = false;
            this.mRemoveKeyframeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mTooltip.SetToolTip(this.mRemoveKeyframeButton, "Removes the currently selected Keyframes.");
            this.mRemoveKeyframeButton.UseVisualStyleBackColor = true;
            this.mRemoveKeyframeButton.Click += new System.EventHandler(this.mRemoveFrameButton_Click);
            // 
            // mFrameLeftButton
            // 
            this.mFrameLeftButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mFrameLeftButton.BackgroundImage")));
            this.mFrameLeftButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mFrameLeftButton.Location = new System.Drawing.Point(1, 55);
            this.mFrameLeftButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mFrameLeftButton.Name = "mFrameLeftButton";
            this.mFrameLeftButton.Size = new System.Drawing.Size(32, 32);
            this.mFrameLeftButton.TabIndex = 49;
            this.mTooltip.SetToolTip(this.mFrameLeftButton, "Shift the selected Keyframe to the left.");
            this.mFrameLeftButton.UseVisualStyleBackColor = true;
            this.mFrameLeftButton.Click += new System.EventHandler(this.mFrameLeftButton_Click);
            // 
            // mMotionChannelComboBox
            // 
            this.mMotionChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mMotionChannelComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mMotionChannelComboBox.FormattingEnabled = true;
            this.mMotionChannelComboBox.Items.AddRange(new object[] {
            "Channel",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29"});
            this.mMotionChannelComboBox.Location = new System.Drawing.Point(133, 428);
            this.mMotionChannelComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mMotionChannelComboBox.Name = "mMotionChannelComboBox";
            this.mMotionChannelComboBox.Size = new System.Drawing.Size(71, 23);
            this.mMotionChannelComboBox.TabIndex = 49;
            this.mTooltip.SetToolTip(this.mMotionChannelComboBox, "Channel for the selected Keyframe\'s motion delta.\r\n[0-9]:     Position\r\n[10-19]: " +
                    "Velocity\r\n[20-29]: Acceleration");
            this.mMotionChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.mMotionChannelComboBox_SelectedIndexChanged);
            this.mMotionChannelComboBox.EnabledChanged += new System.EventHandler(this.mMotionChannelComboBox_EnabledChanged);
            // 
            // mMultiSelectLabel
            // 
            this.mMultiSelectLabel.AutoSize = true;
            this.mMultiSelectLabel.Location = new System.Drawing.Point(208, 412);
            this.mMultiSelectLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.mMultiSelectLabel.Name = "mMultiSelectLabel";
            this.mMultiSelectLabel.Size = new System.Drawing.Size(22, 13);
            this.mMultiSelectLabel.TabIndex = 53;
            this.mMultiSelectLabel.Text = "[M]";
            this.mTooltip.SetToolTip(this.mMultiSelectLabel, "Multiple Keyframes selected.");
            // 
            // mFrameDurAddButton
            // 
            this.mFrameDurAddButton.Location = new System.Drawing.Point(69, 453);
            this.mFrameDurAddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mFrameDurAddButton.Name = "mFrameDurAddButton";
            this.mFrameDurAddButton.Size = new System.Drawing.Size(18, 20);
            this.mFrameDurAddButton.TabIndex = 54;
            this.mFrameDurAddButton.Text = "+";
            this.mTooltip.SetToolTip(this.mFrameDurAddButton, "Add one frame to the duration of all selected Keyframes.");
            this.mFrameDurAddButton.UseVisualStyleBackColor = true;
            this.mFrameDurAddButton.Click += new System.EventHandler(this.mFrameDurAddButton_Click);
            // 
            // mFrameDurSubButton
            // 
            this.mFrameDurSubButton.Location = new System.Drawing.Point(32, 453);
            this.mFrameDurSubButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mFrameDurSubButton.Name = "mFrameDurSubButton";
            this.mFrameDurSubButton.Size = new System.Drawing.Size(18, 20);
            this.mFrameDurSubButton.TabIndex = 55;
            this.mFrameDurSubButton.Text = "-";
            this.mTooltip.SetToolTip(this.mFrameDurSubButton, "Subtract one frame to the duration of all selected Keyframes.");
            this.mFrameDurSubButton.UseVisualStyleBackColor = true;
            this.mFrameDurSubButton.Click += new System.EventHandler(this.mFrameDurSubButton_Click);
            // 
            // mTabControl
            // 
            this.mTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.mTabControl.Controls.Add(this.mAnimationTab);
            this.mTabControl.Controls.Add(this.mScriptTab);
            this.mTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTabControl.Location = new System.Drawing.Point(2, 34);
            this.mTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mTabControl.Multiline = true;
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            this.mTabControl.Size = new System.Drawing.Size(788, 484);
            this.mTabControl.TabIndex = 2;
            this.mTabControl.TabStop = false;
            // 
            // mAnimationTab
            // 
            this.mAnimationTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mAnimationTab.Controls.Add(this.mPictureBox);
            this.mAnimationTab.Controls.Add(this.mGridPropertiesPanel);
            this.mAnimationTab.Controls.Add(this.controlsPanel);
            this.mAnimationTab.Location = new System.Drawing.Point(23, 4);
            this.mAnimationTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAnimationTab.Name = "mAnimationTab";
            this.mAnimationTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mAnimationTab.Size = new System.Drawing.Size(761, 476);
            this.mAnimationTab.TabIndex = 0;
            this.mAnimationTab.Text = "Animation";
            this.mAnimationTab.UseVisualStyleBackColor = true;
            // 
            // mGridPropertiesPanel
            // 
            this.mGridPropertiesPanel.Controls.Add(this.label2);
            this.mGridPropertiesPanel.Controls.Add(this.mGridOffsetNumber);
            this.mGridPropertiesPanel.Controls.Add(this.label4);
            this.mGridPropertiesPanel.Controls.Add(this.mWidthComboBox);
            this.mGridPropertiesPanel.Controls.Add(this.mHeightComboBox);
            this.mGridPropertiesPanel.Controls.Add(this.label1);
            this.mGridPropertiesPanel.Controls.Add(this.mGridSizeLinkButton);
            this.mGridPropertiesPanel.Controls.Add(this.label3);
            this.mGridPropertiesPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mGridPropertiesPanel.Location = new System.Drawing.Point(2, 2);
            this.mGridPropertiesPanel.Name = "mGridPropertiesPanel";
            this.mGridPropertiesPanel.Size = new System.Drawing.Size(510, 46);
            this.mGridPropertiesPanel.TabIndex = 55;
            // 
            // controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.mFrameDurSubButton);
            this.controlsPanel.Controls.Add(this.mFrameDurAddButton);
            this.controlsPanel.Controls.Add(this.mAddAnimationButton);
            this.controlsPanel.Controls.Add(this.mMultiSelectLabel);
            this.controlsPanel.Controls.Add(this.mTotalTimeTextBox);
            this.controlsPanel.Controls.Add(this.label14);
            this.controlsPanel.Controls.Add(this.label9);
            this.controlsPanel.Controls.Add(this.label12);
            this.controlsPanel.Controls.Add(this.label6);
            this.controlsPanel.Controls.Add(this.mMotionChannelComboBox);
            this.controlsPanel.Controls.Add(this.mMultidirCheckBox);
            this.controlsPanel.Controls.Add(this.mFlippingCheckBox);
            this.controlsPanel.Controls.Add(this.mTotalFrameCountBox);
            this.controlsPanel.Controls.Add(this.label10);
            this.controlsPanel.Controls.Add(this.label7);
            this.controlsPanel.Controls.Add(this.label5);
            this.controlsPanel.Controls.Add(this.mMotionDeltaXBox);
            this.controlsPanel.Controls.Add(this.mFrameDurationTextBox);
            this.controlsPanel.Controls.Add(this.mMotionDeltaYBox);
            this.controlsPanel.Controls.Add(this.mPlayStopLabel);
            this.controlsPanel.Controls.Add(this.label11);
            this.controlsPanel.Controls.Add(this.label13);
            this.controlsPanel.Controls.Add(this.mDirectionComboBox);
            this.controlsPanel.Controls.Add(this.mAnimationPictureBox);
            this.controlsPanel.Controls.Add(this.mExportButton);
            this.controlsPanel.Controls.Add(this.mRemoveAnimationButton);
            this.controlsPanel.Controls.Add(this.mAnimationNameComboBox);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.controlsPanel.Location = new System.Drawing.Point(512, 2);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new System.Drawing.Size(243, 468);
            this.controlsPanel.TabIndex = 54;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(191, 476);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 52;
            this.label14.Text = "Y";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(191, 456);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 13);
            this.label12.TabIndex = 51;
            this.label12.Text = "X";
            // 
            // mFlippingCheckBox
            // 
            this.mFlippingCheckBox.AutoSize = true;
            this.mFlippingCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mFlippingCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mFlippingCheckBox.Location = new System.Drawing.Point(5, 113);
            this.mFlippingCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mFlippingCheckBox.Name = "mFlippingCheckBox";
            this.mFlippingCheckBox.Size = new System.Drawing.Size(124, 17);
            this.mFlippingCheckBox.TabIndex = 48;
            this.mFlippingCheckBox.Text = "Flipping Enabled:";
            this.mFlippingCheckBox.UseVisualStyleBackColor = true;
            this.mFlippingCheckBox.CheckedChanged += new System.EventHandler(this.mFlippingCheckBox_CheckedChanged);
            // 
            // mScriptTab
            // 
            this.mScriptTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mScriptTab.Location = new System.Drawing.Point(23, 4);
            this.mScriptTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mScriptTab.Name = "mScriptTab";
            this.mScriptTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mScriptTab.Size = new System.Drawing.Size(765, 511);
            this.mScriptTab.TabIndex = 1;
            this.mScriptTab.Text = "Scripts";
            this.mScriptTab.UseVisualStyleBackColor = true;
            // 
            // ctrlALabel
            // 
            this.ctrlALabel.AutoSize = true;
            this.ctrlALabel.Location = new System.Drawing.Point(4, 57);
            this.ctrlALabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ctrlALabel.Name = "ctrlALabel";
            this.ctrlALabel.Size = new System.Drawing.Size(41, 13);
            this.ctrlALabel.TabIndex = 26;
            this.ctrlALabel.Text = "(Ctrl+A)";
            // 
            // deleteLabel
            // 
            this.deleteLabel.AutoSize = true;
            this.deleteLabel.Location = new System.Drawing.Point(52, 57);
            this.deleteLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(42, 13);
            this.deleteLabel.TabIndex = 27;
            this.deleteLabel.Text = "(Ctrl+D)";
            // 
            // mKeyframePanel
            // 
            this.mKeyframePanel.BackColor = System.Drawing.SystemColors.Menu;
            this.mKeyframePanel.Controls.Add(this.mImageView);
            this.mKeyframePanel.Controls.Add(this.panel1);
            this.mKeyframePanel.Controls.Add(this.panel2);
            this.mKeyframePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mKeyframePanel.Location = new System.Drawing.Point(2, 522);
            this.mKeyframePanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mKeyframePanel.Name = "mKeyframePanel";
            this.mKeyframePanel.Size = new System.Drawing.Size(788, 118);
            this.mKeyframePanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mAddKeyframeButton);
            this.panel1.Controls.Add(this.deleteLabel);
            this.panel1.Controls.Add(this.mRemoveKeyframeButton);
            this.panel1.Controls.Add(this.ctrlALabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 118);
            this.panel1.TabIndex = 51;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mFrameRightButton);
            this.panel2.Controls.Add(this.mFrameLeftButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(754, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(34, 118);
            this.panel2.TabIndex = 52;
            // 
            // mFrameRightButton
            // 
            this.mFrameRightButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mFrameRightButton.BackgroundImage")));
            this.mFrameRightButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mFrameRightButton.Location = new System.Drawing.Point(1, 15);
            this.mFrameRightButton.Margin = new System.Windows.Forms.Padding(2);
            this.mFrameRightButton.Name = "mFrameRightButton";
            this.mFrameRightButton.Size = new System.Drawing.Size(32, 32);
            this.mFrameRightButton.TabIndex = 50;
            this.mTooltip.SetToolTip(this.mFrameRightButton, "Shift the selected Keyframe to the right.");
            this.mFrameRightButton.UseVisualStyleBackColor = true;
            this.mFrameRightButton.Click += new System.EventHandler(this.mFrameRightButton_Click);
            // 
            // mPictureBox
            // 
            this.mPictureBox.BackColor = System.Drawing.Color.White;
            this.mPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mPictureBox.Border = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPictureBox.Location = new System.Drawing.Point(2, 48);
            this.mPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.mPictureBox.Name = "mPictureBox";
            this.mPictureBox.Picture = "";
            this.mPictureBox.Size = new System.Drawing.Size(510, 422);
            this.mPictureBox.TabIndex = 25;
            this.mPictureBox.TabStop = false;
            // 
            // mImageView
            // 
            this.mImageView.AutoSize = true;
            this.mImageView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mImageView.Location = new System.Drawing.Point(100, 0);
            this.mImageView.Margin = new System.Windows.Forms.Padding(2);
            this.mImageView.Name = "mImageView";
            this.mImageView.Size = new System.Drawing.Size(654, 118);
            this.mImageView.TabIndex = 48;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mKeyframePanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.mTabControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(792, 642);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // AnimationEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 642);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AnimationEditorForm";
            this.Text = "FNA Animation Editor";
            ((System.ComponentModel.ISupportInitialize)(this.mMotionDeltaYBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMotionDeltaXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridOffsetNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mAnimationPictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mTabControl.ResumeLayout(false);
            this.mAnimationTab.ResumeLayout(false);
            this.mGridPropertiesPanel.ResumeLayout(false);
            this.mGridPropertiesPanel.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.mKeyframePanel.ResumeLayout(false);
            this.mKeyframePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox mWidthComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mGridSizeLinkButton;
        private System.Windows.Forms.ComboBox mHeightComboBox;
        private System.Windows.Forms.NumericUpDown mGridOffsetNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox mAnimationPictureBox;
        private System.Windows.Forms.Label mPlayStopLabel;
        private ScrollingPictureBox mPictureBox;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuAnimation;
        private System.Windows.Forms.ToolStripMenuItem loadSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mTotalFrameCountBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox mTotalTimeTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox mMultidirCheckBox;
        private System.Windows.Forms.NumericUpDown mMotionDeltaXBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown mMotionDeltaYBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox mDirectionComboBox;
        private System.Windows.Forms.Button mExportButton;
        private System.Windows.Forms.ComboBox mAnimationNameComboBox;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noToolStripMenuItem;
        private System.Windows.Forms.Button mRemoveAnimationButton;
        private System.Windows.Forms.ToolTip mTooltip;
        private System.Windows.Forms.Button mAddAnimationButton;
        private System.Windows.Forms.TextBox mFrameDurationTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.TabPage mAnimationTab;
        private System.Windows.Forms.TabPage mScriptTab;
        private System.Windows.Forms.Button mAddKeyframeButton;
        private System.Windows.Forms.Button mRemoveKeyframeButton;
        private System.Windows.Forms.Label ctrlALabel;
        private System.Windows.Forms.Label deleteLabel;
        private ImageView mImageView;
        private System.Windows.Forms.Button mFrameLeftButton;
        private System.Windows.Forms.Panel mKeyframePanel;
        private System.Windows.Forms.CheckBox mFlippingCheckBox;
        private System.Windows.Forms.ComboBox mMotionChannelComboBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label mMultiSelectLabel;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Panel mGridPropertiesPanel;
        private System.Windows.Forms.Button mFrameDurAddButton;
        private System.Windows.Forms.Button mFrameDurSubButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button mFrameRightButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

