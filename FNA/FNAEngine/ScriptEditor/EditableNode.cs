using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FNA.Scripts;

namespace ScriptEditor
{
    public class EditableNode : TableLayoutPanel
    {
        private Button mEditButton;
        public Button EditButton
        {
            get
            {
                return mEditButton;
            }
            set
            {
                mEditButton = value;
            }
        }

        private ComboBox mNodeSelector;
        public ComboBox NodeSelector
        {
            get
            {
                return mNodeSelector;
            }
        }

        private INode mCurrentNode;
        public INode CurrentNode
        {
            get
            {
                return mCurrentNode;
            }
            set
            {
                mCurrentNode = value;
            }
        }

        public EditableNode()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Name = "Editable Node";
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)));
            this.RowCount = 1;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Height = 32;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            mEditButton = new System.Windows.Forms.Button();
            mEditButton.Name = "Edit Node Button";
            mEditButton.Text = "Edit";
            mEditButton.UseVisualStyleBackColor = true;

            mNodeSelector = new System.Windows.Forms.ComboBox();
            mNodeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            mNodeSelector.FormattingEnabled = true;
            mNodeSelector.Name = "Node Selector";
            mNodeSelector.Dock = DockStyle.Fill;

            Controls.Add(mNodeSelector, 0, 0);
            Controls.Add(mEditButton, 1, 0);
        }
    }
}
