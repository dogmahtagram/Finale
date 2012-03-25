namespace ScriptEditor
{
    partial class Test
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
            System.Windows.Forms.SplitContainer mScriptListEditorSplitter;
            this.mScriptListSplitter = new System.Windows.Forms.SplitContainer();
            this.mMoveRoutineDownButton = new System.Windows.Forms.Button();
            this.mMoveRoutineUpButton = new System.Windows.Forms.Button();
            this.mRemoveRoutineButton = new System.Windows.Forms.Button();
            this.mAddRoutineButton = new System.Windows.Forms.Button();
            this.mScriptRoutinesGroup = new System.Windows.Forms.GroupBox();
            this.mMoveNodeDownButton = new System.Windows.Forms.Button();
            this.mMoveNodeUpButton = new System.Windows.Forms.Button();
            this.mRemoveNodeButton = new System.Windows.Forms.Button();
            this.mAddNodeButton = new System.Windows.Forms.Button();
            this.mScriptNodesGroup = new System.Windows.Forms.GroupBox();
            this.NodeInstance = new System.Windows.Forms.Panel();
            this.NodeSelector = new System.Windows.Forms.ComboBox();
            this.NodeEditButton = new System.Windows.Forms.Button();
            mScriptListEditorSplitter = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(mScriptListEditorSplitter)).BeginInit();
            mScriptListEditorSplitter.Panel1.SuspendLayout();
            mScriptListEditorSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mScriptListSplitter)).BeginInit();
            this.mScriptListSplitter.Panel1.SuspendLayout();
            this.mScriptListSplitter.Panel2.SuspendLayout();
            this.mScriptListSplitter.SuspendLayout();
            this.mScriptNodesGroup.SuspendLayout();
            this.NodeInstance.SuspendLayout();
            this.SuspendLayout();
            // 
            // mScriptListEditorSplitter
            // 
            mScriptListEditorSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            mScriptListEditorSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            mScriptListEditorSplitter.Location = new System.Drawing.Point(0, 0);
            mScriptListEditorSplitter.Name = "mScriptListEditorSplitter";
            // 
            // mScriptListEditorSplitter.Panel1
            // 
            mScriptListEditorSplitter.Panel1.Controls.Add(this.mScriptListSplitter);
            mScriptListEditorSplitter.Size = new System.Drawing.Size(787, 441);
            mScriptListEditorSplitter.SplitterDistance = 502;
            mScriptListEditorSplitter.TabIndex = 0;
            // 
            // mScriptListSplitter
            // 
            this.mScriptListSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mScriptListSplitter.Location = new System.Drawing.Point(0, 0);
            this.mScriptListSplitter.Name = "mScriptListSplitter";
            // 
            // mScriptListSplitter.Panel1
            // 
            this.mScriptListSplitter.Panel1.Controls.Add(this.mMoveRoutineDownButton);
            this.mScriptListSplitter.Panel1.Controls.Add(this.mMoveRoutineUpButton);
            this.mScriptListSplitter.Panel1.Controls.Add(this.mRemoveRoutineButton);
            this.mScriptListSplitter.Panel1.Controls.Add(this.mAddRoutineButton);
            this.mScriptListSplitter.Panel1.Controls.Add(this.mScriptRoutinesGroup);
            // 
            // mScriptListSplitter.Panel2
            // 
            this.mScriptListSplitter.Panel2.Controls.Add(this.mMoveNodeDownButton);
            this.mScriptListSplitter.Panel2.Controls.Add(this.mMoveNodeUpButton);
            this.mScriptListSplitter.Panel2.Controls.Add(this.mRemoveNodeButton);
            this.mScriptListSplitter.Panel2.Controls.Add(this.mAddNodeButton);
            this.mScriptListSplitter.Panel2.Controls.Add(this.mScriptNodesGroup);
            this.mScriptListSplitter.Size = new System.Drawing.Size(498, 437);
            this.mScriptListSplitter.SplitterDistance = 207;
            this.mScriptListSplitter.TabIndex = 0;
            // 
            // mMoveRoutineDownButton
            // 
            this.mMoveRoutineDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mMoveRoutineDownButton.Location = new System.Drawing.Point(166, 42);
            this.mMoveRoutineDownButton.Name = "mMoveRoutineDownButton";
            this.mMoveRoutineDownButton.Size = new System.Drawing.Size(30, 23);
            this.mMoveRoutineDownButton.TabIndex = 4;
            this.mMoveRoutineDownButton.Text = "v";
            this.mMoveRoutineDownButton.UseVisualStyleBackColor = true;
            // 
            // mMoveRoutineUpButton
            // 
            this.mMoveRoutineUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mMoveRoutineUpButton.Location = new System.Drawing.Point(166, 12);
            this.mMoveRoutineUpButton.Name = "mMoveRoutineUpButton";
            this.mMoveRoutineUpButton.Size = new System.Drawing.Size(30, 23);
            this.mMoveRoutineUpButton.TabIndex = 3;
            this.mMoveRoutineUpButton.Text = "^";
            this.mMoveRoutineUpButton.UseVisualStyleBackColor = true;
            // 
            // mRemoveRoutineButton
            // 
            this.mRemoveRoutineButton.Location = new System.Drawing.Point(12, 42);
            this.mRemoveRoutineButton.Name = "mRemoveRoutineButton";
            this.mRemoveRoutineButton.Size = new System.Drawing.Size(75, 23);
            this.mRemoveRoutineButton.TabIndex = 2;
            this.mRemoveRoutineButton.Text = "Remove";
            this.mRemoveRoutineButton.UseVisualStyleBackColor = true;
            // 
            // mAddRoutineButton
            // 
            this.mAddRoutineButton.Location = new System.Drawing.Point(12, 12);
            this.mAddRoutineButton.Name = "mAddRoutineButton";
            this.mAddRoutineButton.Size = new System.Drawing.Size(75, 23);
            this.mAddRoutineButton.TabIndex = 1;
            this.mAddRoutineButton.Text = "Add";
            this.mAddRoutineButton.UseVisualStyleBackColor = true;
            // 
            // mScriptRoutinesGroup
            // 
            this.mScriptRoutinesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mScriptRoutinesGroup.Location = new System.Drawing.Point(0, 78);
            this.mScriptRoutinesGroup.Name = "mScriptRoutinesGroup";
            this.mScriptRoutinesGroup.Size = new System.Drawing.Size(204, 360);
            this.mScriptRoutinesGroup.TabIndex = 0;
            this.mScriptRoutinesGroup.TabStop = false;
            this.mScriptRoutinesGroup.Text = "Script Routines";
            // 
            // mMoveNodeDownButton
            // 
            this.mMoveNodeDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mMoveNodeDownButton.Location = new System.Drawing.Point(245, 42);
            this.mMoveNodeDownButton.Name = "mMoveNodeDownButton";
            this.mMoveNodeDownButton.Size = new System.Drawing.Size(30, 23);
            this.mMoveNodeDownButton.TabIndex = 8;
            this.mMoveNodeDownButton.Text = "v";
            this.mMoveNodeDownButton.UseVisualStyleBackColor = true;
            // 
            // mMoveNodeUpButton
            // 
            this.mMoveNodeUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mMoveNodeUpButton.Location = new System.Drawing.Point(245, 12);
            this.mMoveNodeUpButton.Name = "mMoveNodeUpButton";
            this.mMoveNodeUpButton.Size = new System.Drawing.Size(30, 23);
            this.mMoveNodeUpButton.TabIndex = 7;
            this.mMoveNodeUpButton.Text = "^";
            this.mMoveNodeUpButton.UseVisualStyleBackColor = true;
            // 
            // mRemoveNodeButton
            // 
            this.mRemoveNodeButton.Location = new System.Drawing.Point(12, 42);
            this.mRemoveNodeButton.Name = "mRemoveNodeButton";
            this.mRemoveNodeButton.Size = new System.Drawing.Size(75, 23);
            this.mRemoveNodeButton.TabIndex = 6;
            this.mRemoveNodeButton.Text = "Remove";
            this.mRemoveNodeButton.UseVisualStyleBackColor = true;
            // 
            // mAddNodeButton
            // 
            this.mAddNodeButton.Location = new System.Drawing.Point(12, 12);
            this.mAddNodeButton.Name = "mAddNodeButton";
            this.mAddNodeButton.Size = new System.Drawing.Size(75, 23);
            this.mAddNodeButton.TabIndex = 5;
            this.mAddNodeButton.Text = "Add";
            this.mAddNodeButton.UseVisualStyleBackColor = true;
            // 
            // mScriptNodesGroup
            // 
            this.mScriptNodesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mScriptNodesGroup.Controls.Add(this.NodeInstance);
            this.mScriptNodesGroup.Location = new System.Drawing.Point(3, 78);
            this.mScriptNodesGroup.Name = "mScriptNodesGroup";
            this.mScriptNodesGroup.Size = new System.Drawing.Size(284, 360);
            this.mScriptNodesGroup.TabIndex = 0;
            this.mScriptNodesGroup.TabStop = false;
            this.mScriptNodesGroup.Text = "Script Nodes";
            // 
            // NodeInstance
            // 
            this.NodeInstance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.NodeInstance.Controls.Add(this.NodeSelector);
            this.NodeInstance.Controls.Add(this.NodeEditButton);
            this.NodeInstance.Location = new System.Drawing.Point(9, 19);
            this.NodeInstance.Name = "NodeInstance";
            this.NodeInstance.Size = new System.Drawing.Size(184, 32);
            this.NodeInstance.TabIndex = 0;
            // 
            // NodeSelector
            // 
            this.NodeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NodeSelector.FormattingEnabled = true;
            this.NodeSelector.Location = new System.Drawing.Point(4, 4);
            this.NodeSelector.Name = "NodeSelector";
            this.NodeSelector.Size = new System.Drawing.Size(131, 21);
            this.NodeSelector.TabIndex = 1;
            // 
            // NodeEditButton
            // 
            this.NodeEditButton.Location = new System.Drawing.Point(141, 3);
            this.NodeEditButton.Name = "NodeEditButton";
            this.NodeEditButton.Size = new System.Drawing.Size(35, 22);
            this.NodeEditButton.TabIndex = 0;
            this.NodeEditButton.Text = "Edit";
            this.NodeEditButton.UseVisualStyleBackColor = true;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 441);
            this.Controls.Add(mScriptListEditorSplitter);
            this.Name = "Test";
            this.Text = "Test";
            mScriptListEditorSplitter.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(mScriptListEditorSplitter)).EndInit();
            mScriptListEditorSplitter.ResumeLayout(false);
            this.mScriptListSplitter.Panel1.ResumeLayout(false);
            this.mScriptListSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mScriptListSplitter)).EndInit();
            this.mScriptListSplitter.ResumeLayout(false);
            this.mScriptNodesGroup.ResumeLayout(false);
            this.NodeInstance.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mScriptListSplitter;
        private System.Windows.Forms.GroupBox mScriptRoutinesGroup;
        private System.Windows.Forms.GroupBox mScriptNodesGroup;
        private System.Windows.Forms.Button mMoveRoutineDownButton;
        private System.Windows.Forms.Button mMoveRoutineUpButton;
        private System.Windows.Forms.Button mRemoveRoutineButton;
        private System.Windows.Forms.Button mAddRoutineButton;
        private System.Windows.Forms.Button mMoveNodeDownButton;
        private System.Windows.Forms.Button mMoveNodeUpButton;
        private System.Windows.Forms.Button mRemoveNodeButton;
        private System.Windows.Forms.Button mAddNodeButton;
        private System.Windows.Forms.Panel NodeInstance;
        private System.Windows.Forms.ComboBox NodeSelector;
        private System.Windows.Forms.Button NodeEditButton;

    }
}