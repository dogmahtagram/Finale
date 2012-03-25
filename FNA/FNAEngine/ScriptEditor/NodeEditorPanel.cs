using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using FNA.Scripts;
using FNA.Managers;
using System.Reflection;

namespace ScriptEditor
{
    public class NodeEditorPanel : Panel
    {
        private Button mApplyChangesButton;
        public Button ApplyChangesButton
        {
            get
            {
                return mApplyChangesButton;
            }
        }

        private ComboBox mNodeTypeSelector;

        private PropertyGrid mNodeProperties;

        private INode mActiveNode;

        public NodeEditorPanel()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Name = "Node Editor Panel";
            this.BorderStyle = BorderStyle.Fixed3D;
            this.Dock = DockStyle.Fill;

            mApplyChangesButton = new Button();
            mApplyChangesButton.Text = "Apply Changes";
            mApplyChangesButton.Location = new System.Drawing.Point(230, 1);
            mApplyChangesButton.Size = new System.Drawing.Size(96, 23);
            mApplyChangesButton.Click += new EventHandler(ApplyChanges);

            mNodeProperties = new PropertyGrid();
            mNodeProperties.Location = new System.Drawing.Point(191, 26);
            mNodeProperties.Name = "mNodeProperties";
            mNodeProperties.Dock = DockStyle.Fill;

            mNodeTypeSelector = new System.Windows.Forms.ComboBox();
            mNodeTypeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            mNodeTypeSelector.FormattingEnabled = true;
            mNodeTypeSelector.Location = new System.Drawing.Point(90, 2);
            mNodeTypeSelector.Name = "Node Type Selector";
            mNodeTypeSelector.Size = new System.Drawing.Size(136, 22);

            mNodeTypeSelector.Items.Add("[Select Node Type]");
            mNodeTypeSelector.Items.Add("InputScriptNode");
            mNodeTypeSelector.Items.Add("PlayAnimationScriptNode");
            mNodeTypeSelector.Items.Add("ToggleMotionDeltaScriptNode");
            mNodeTypeSelector.Items.Add("ToggleActionCheckNode");
            mNodeTypeSelector.SelectedIndex = 0;
            mNodeTypeSelector.SelectionChangeCommitted += new EventHandler(NodeTypeChanged);

            Controls.Add(mApplyChangesButton);
            Controls.Add(mNodeTypeSelector);
            Controls.Add(mNodeProperties);
        }

        private void ApplyChanges(object sender, EventArgs e)
        {
            if (mNodeProperties.SelectedObject != null)
            {
                if (ScriptNodeManager_cl.Instance.ScriptNodes.ContainsKey(((INode)mNodeProperties.SelectedObject).GetName()))
                {
                    ScriptNodeManager_cl.Instance.ScriptNodes[((INode)mNodeProperties.SelectedObject).GetName()] = (INode)mNodeProperties.SelectedObject;
                }
                else
                {
                    ScriptNodeManager_cl.Instance.RegisterNode((INode)mNodeProperties.SelectedObject);
                }
            }
        }

        public void SetActiveNode(INode node)
        {
            mActiveNode = node;
            mNodeProperties.SelectedObject = mActiveNode;
            mNodeProperties.BackColor = Color.LightGreen;
        }

        public void Clean()
        {
            mNodeProperties.SelectedObject = null;
            mNodeProperties.BackColor = Color.WhiteSmoke;
        }

        public void NodeTypeChanged(object sender, EventArgs e)
        {
            string selectedType = (string)((ComboBox)sender).SelectedItem;
            if (selectedType == "[Select Node Type]")
            {
                mActiveNode = null;
                mNodeProperties.SelectedObject = null;
            }
            else
            {
                Assembly assembly = typeof(InputScriptNode).Assembly;

                string typeName = "FNA.Scripts." + selectedType;
                ScriptNode newNode = (ScriptNode)Activator.CreateInstance(assembly.GetType(typeName));
                mActiveNode = newNode;
                mNodeProperties.SelectedObject = mActiveNode;
            }

            Control[] routineControls = Parent.Controls.Find("Routine Editor Panel", false);
            if (routineControls.Length > 0)
            {
                ((RoutineEditorPanel)routineControls[0]).ActiveNode.Value.CurrentNode = mActiveNode;
            }
        }
    }
}
