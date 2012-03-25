namespace ScriptEditor
{
    partial class ScriptEditor
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
            this.button1 = new System.Windows.Forms.Button();
            this.mAnimationSelector = new System.Windows.Forms.ComboBox();
            this.mFrameSelector = new System.Windows.Forms.ComboBox();
            this.loadFAPButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mScriptEditingPanel = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save FAP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveFAPClicked);
            // 
            // mAnimationSelector
            // 
            this.mAnimationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mAnimationSelector.FormattingEnabled = true;
            this.mAnimationSelector.Location = new System.Drawing.Point(19, 39);
            this.mAnimationSelector.Name = "mAnimationSelector";
            this.mAnimationSelector.Size = new System.Drawing.Size(121, 21);
            this.mAnimationSelector.TabIndex = 1;
            this.mAnimationSelector.SelectionChangeCommitted += new System.EventHandler(this.AnimationSelectorChanged);
            // 
            // mFrameSelector
            // 
            this.mFrameSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mFrameSelector.FormattingEnabled = true;
            this.mFrameSelector.Location = new System.Drawing.Point(160, 39);
            this.mFrameSelector.Name = "mFrameSelector";
            this.mFrameSelector.Size = new System.Drawing.Size(103, 21);
            this.mFrameSelector.TabIndex = 2;
            this.mFrameSelector.SelectionChangeCommitted += new System.EventHandler(this.FrameSelectionChanged);
            // 
            // loadFAPButton
            // 
            this.loadFAPButton.Location = new System.Drawing.Point(12, 12);
            this.loadFAPButton.Name = "loadFAPButton";
            this.loadFAPButton.Size = new System.Drawing.Size(75, 23);
            this.loadFAPButton.TabIndex = 3;
            this.loadFAPButton.Text = "Load FAP";
            this.loadFAPButton.UseVisualStyleBackColor = true;
            this.loadFAPButton.Click += new System.EventHandler(this.LoadFAPClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Animation Selection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Frame Selection";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.mAnimationSelector);
            this.groupBox1.Controls.Add(this.mFrameSelector);
            this.groupBox1.Location = new System.Drawing.Point(93, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 70);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current FAP";
            // 
            // mScriptEditingPanel
            // 
            this.mScriptEditingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mScriptEditingPanel.Location = new System.Drawing.Point(12, 88);
            this.mScriptEditingPanel.Name = "mScriptEditingPanel";
            this.mScriptEditingPanel.Size = new System.Drawing.Size(860, 462);
            this.mScriptEditingPanel.TabIndex = 7;
            this.mScriptEditingPanel.TabStop = false;
            this.mScriptEditingPanel.Text = "Script Editing Area";
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.mScriptEditingPanel);
            this.Controls.Add(this.loadFAPButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "ScriptEditor";
            this.Text = "FNA Script Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox mAnimationSelector;
        private System.Windows.Forms.ComboBox mFrameSelector;
        private System.Windows.Forms.Button loadFAPButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox mScriptEditingPanel;



    }
}

