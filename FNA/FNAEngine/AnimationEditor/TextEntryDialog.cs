using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace FNA.AnimationEditor
{
    public class TextEntryDialog : Form
    {
        private Container mContainer = null;
        private Button mOKButton;
        private Label mLabel;
        private Button mCancelButton;
        private TextBox mFormationNameBox;

        public TextEntryDialog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private string mMessage;
        public string Message
        {
            get
            {
                return mMessage;
            }
            set
            {
                mMessage = value;
                mFormationNameBox.Text = mMessage;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mContainer != null)
                {
                    mContainer.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mLabel = new System.Windows.Forms.Label();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mFormationNameBox = new System.Windows.Forms.TextBox();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mLabel
            // 
            this.mLabel.Location = new System.Drawing.Point(9, 9);
            this.mLabel.Name = "mLabel";
            this.mLabel.Size = new System.Drawing.Size(288, 28);
            this.mLabel.TabIndex = 1;
            this.mLabel.Text = "Type in the name of the new Animation:";
            // 
            // mOKButton
            // 
            this.mOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mOKButton.Enabled = false;
            this.mOKButton.Location = new System.Drawing.Point(12, 65);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(116, 28);
            this.mOKButton.TabIndex = 2;
            this.mOKButton.Text = "OK";
            this.mOKButton.Click += new System.EventHandler(this.mOKButton_Click);
            // 
            // mFormationNameBox
            // 
            this.mFormationNameBox.Location = new System.Drawing.Point(12, 37);
            this.mFormationNameBox.Name = "mFormationNameBox";
            this.mFormationNameBox.Size = new System.Drawing.Size(285, 22);
            this.mFormationNameBox.TabIndex = 0;
            this.mFormationNameBox.TextChanged += new System.EventHandler(this.mFormationNameBox_TextChanged);
            // 
            // mCancelButton
            // 
            this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mCancelButton.Location = new System.Drawing.Point(181, 65);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(116, 28);
            this.mCancelButton.TabIndex = 3;
            this.mCancelButton.Text = "Cancel";
            // 
            // TextEntryDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(309, 101);
            this.ControlBox = false;
            this.Controls.Add(this.mCancelButton);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mLabel);
            this.Controls.Add(this.mFormationNameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextEntryDialog";
            this.Text = "New Animation Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected void mOKButton_Click(object sender, System.EventArgs e)
        {
            mMessage = mFormationNameBox.Text;
        }

        private void mFormationNameBox_TextChanged(object sender, EventArgs e)
        {
            if (mFormationNameBox.Text.Length > 0)
            {
                mOKButton.Enabled = true;
            }
            else
            {
                mOKButton.Enabled = false;
            }
        }
    }
}
