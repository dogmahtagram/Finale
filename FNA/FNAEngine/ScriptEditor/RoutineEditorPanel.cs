using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using FNA.Core;
using FNA.Scripts;
using FNA.Managers;

namespace ScriptEditor
{
    public class RoutineEditorPanel : Panel
    {
        private Button mRemoveNodeButton;
        private Button mAddNodeButton;
        private Button mMoveNodeDownButton;
        private Button mMoveNodeUpButton;
        private GroupBox mRoutineNodesGroup;
        private Panel mRoutineNodesPanel;
        private TextBox mRoutineName;
        private Label mRoutineNameLabel;

        private string mFnNodesPath;
        public string FnNodesPath
        {
            get
            {
                return mFnNodesPath;
            }
            set
            {
                mFnNodesPath = value;
            }
        }

        private Dictionary<int, EditableNode> mRoutineNodes = new Dictionary<int, EditableNode>();
        private KeyValuePair<int, EditableNode> mActiveNode;
        public KeyValuePair<int, EditableNode> ActiveNode
        {
            get
            {
                return mActiveNode;
            }
        }

        private ScriptRoutine mActiveRoutine;

        public RoutineEditorPanel()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Name = "Routine Editor Panel";
            this.BorderStyle = BorderStyle.Fixed3D;
            this.Dock = DockStyle.Fill;

            mRoutineName = new TextBox();
            mRoutineName.Location = new Point(12, 20);
            mRoutineName.Size = new Size(130, 23);
            mRoutineName.TextAlign = HorizontalAlignment.Center;
            mRoutineName.TextChanged += new EventHandler(RoutineNameChanged);

            mRoutineNameLabel = new Label();
            mRoutineNameLabel.Size = new Size(130, 18);
            mRoutineNameLabel.Location = new Point(12, 0);
            mRoutineNameLabel.Text = "Routine Name";
            mRoutineNameLabel.TextAlign = ContentAlignment.MiddleCenter;

            mMoveNodeDownButton = new Button();
            mMoveNodeDownButton.Location = new System.Drawing.Point(96, 42);
            mMoveNodeDownButton.Name = "mMoveNodeDownButton";
            mMoveNodeDownButton.Size = new System.Drawing.Size(24, 24);
            mMoveNodeDownButton.TabIndex = 8;
            mMoveNodeDownButton.Text = "v";
            mMoveNodeDownButton.UseVisualStyleBackColor = true;
            mMoveNodeDownButton.Click += new System.EventHandler(IncreaseNodeIndex);

            mMoveNodeUpButton = new Button();
            mMoveNodeUpButton.Location = new System.Drawing.Point(72, 42);
            mMoveNodeUpButton.Name = "mMoveNodeUpButton";
            mMoveNodeUpButton.Size = new System.Drawing.Size(24, 24);
            mMoveNodeUpButton.TabIndex = 7;
            mMoveNodeUpButton.Text = "^";
            mMoveNodeUpButton.UseVisualStyleBackColor = true;
            mMoveNodeUpButton.Click += new System.EventHandler(DecreaseNodeIndex);

            mRemoveNodeButton = new Button();
            mRemoveNodeButton.Location = new System.Drawing.Point(24, 42);
            mRemoveNodeButton.Name = "mRemoveNodeButton";
            mRemoveNodeButton.Size = new System.Drawing.Size(24, 24);
            mRemoveNodeButton.TabIndex = 6;
            mRemoveNodeButton.Text = "-";
            mRemoveNodeButton.UseVisualStyleBackColor = true;
            mRemoveNodeButton.Click += new EventHandler(RemoveNodeClicked);

            mAddNodeButton = new Button();
            mAddNodeButton.Location = new System.Drawing.Point(0, 42);
            mAddNodeButton.Name = "mAddNodeButton";
            mAddNodeButton.Size = new System.Drawing.Size(24, 24);
            mAddNodeButton.TabIndex = 5;
            mAddNodeButton.Text = "+";
            mAddNodeButton.UseVisualStyleBackColor = true;
            mAddNodeButton.Click += new EventHandler(AddNodeClicked);

            mRoutineNodesGroup = new System.Windows.Forms.GroupBox();
            mRoutineNodesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((
                System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom)));
            mRoutineNodesGroup.Location = new System.Drawing.Point(3, 78);
            mRoutineNodesGroup.Name = "mScriptNodesGroup";
            mRoutineNodesGroup.TabIndex = 0;
            mRoutineNodesGroup.TabStop = false;
            mRoutineNodesGroup.Text = "Routine Nodes";

            mRoutineNodesPanel = new Panel();
            mRoutineNodesPanel.Dock = DockStyle.Fill;
            mRoutineNodesPanel.AutoScroll = true;
            mRoutineNodesGroup.Controls.Add(mRoutineNodesPanel);

            Controls.Add(mRoutineNameLabel);
            Controls.Add(mRoutineName);
            Controls.Add(mRoutineNodesGroup);
            Controls.Add(mRemoveNodeButton);
            Controls.Add(mAddNodeButton);
            Controls.Add(mMoveNodeDownButton);
            Controls.Add(mMoveNodeUpButton);
        }

        private void AddNodeClicked(object sender, EventArgs e)
        {
            AddScriptNode();
        }

        private void AddScriptNode(INode node = null, string nodeName = "New Node")
        {
            EditableNode scriptNode = new EditableNode();
            scriptNode.NodeSelector.SelectionChangeCommitted += new System.EventHandler(NodeDropDownChanged);
            scriptNode.NodeSelector.Click += new EventHandler(NodeClicked);
            scriptNode.EditButton.Click += new EventHandler(NodeClicked);
            scriptNode.NodeSelector.Text = nodeName;

            scriptNode.NodeSelector.Items.Add("New Node");
            foreach (ScriptNode managerNode in ScriptNodeManager_cl.Instance.ScriptNodes.Values)
            {
                scriptNode.NodeSelector.Items.Add(managerNode.Name);
            }

            if (node != null)
            {
                scriptNode.CurrentNode = node; 
            }

            mActiveNode = new KeyValuePair<int, EditableNode>(mRoutineNodes.Count, scriptNode);
            SetActiveNode(mActiveNode.Value);

            mRoutineNodes.Add(mRoutineNodes.Count, scriptNode);
            mRoutineNodesPanel.Controls.Add(scriptNode);
            scriptNode.Width = mRoutineNodesPanel.Width;
            scriptNode.NodeSelector.SelectedItem = nodeName;

            OrderNodes();
        }

        public void RoutineNameChanged(object sender, EventArgs e)
        {
            Control[] routineControls = Parent.Controls.Find("Script Editor Panel", false);
            if (routineControls.Length > 0)
            {
                ((ScriptEditorPanel)routineControls[0]).ActiveRoutine.Button.Text = mRoutineName.Text;
            }

            mActiveRoutine.Name = mRoutineName.Text;
        }

        private void RemoveNodeClicked(object sender, EventArgs e)
        {
            if (mActiveNode.Value != null)
            {
                for (int index = mActiveNode.Key + 1; index < mRoutineNodes.Count; index++)
                {
                    mRoutineNodes[index - 1] = mRoutineNodes[index];
                }
                mRoutineNodes.Remove(mRoutineNodes.Count - 1);
                mRoutineNodesPanel.Controls.Remove(mActiveNode.Value);

                mActiveNode = new KeyValuePair<int, EditableNode>(0, null);

                OrderNodes();
            }
        }

        private void OrderNodes()
        {
            foreach (KeyValuePair<int, EditableNode> pair in mRoutineNodes)
            {
                pair.Value.Location = new System.Drawing.Point(0, pair.Key * 32);
            }
        }

        private void DecreaseNodeIndex(object sender, EventArgs e)
        {
            if (mActiveNode.Value != null && mActiveNode.Key != 0)
            {
                EditableNode temp = mRoutineNodes[mActiveNode.Key - 1];
                mRoutineNodes[mActiveNode.Key - 1] = mActiveNode.Value;
                mRoutineNodes[mActiveNode.Key] = temp;
                mActiveNode = new KeyValuePair<int,EditableNode>(mActiveNode.Key - 1, mActiveNode.Value);
            }

            OrderNodes();
        }

        private void IncreaseNodeIndex(object sender, EventArgs e)
        {
            if (mActiveNode.Value != null && mActiveNode.Key != mRoutineNodes.Count - 1)
            {
                EditableNode temp = mRoutineNodes[mActiveNode.Key + 1];
                mRoutineNodes[mActiveNode.Key + 1] = mActiveNode.Value;
                mRoutineNodes[mActiveNode.Key] = temp;
                mActiveNode = new KeyValuePair<int, EditableNode>(mActiveNode.Key + 1, mActiveNode.Value);
            }

            OrderNodes();
        }

        public void SetActiveRoutine(ScriptRoutine routine)
        {
            this.BackColor = Color.LightBlue;

            mRoutineNodesPanel.Controls.Clear();
            mRoutineNodes.Clear();

            mActiveRoutine = routine;

            mRoutineNodes = new Dictionary<int, EditableNode>();
            for (int nodeIndex = 0; nodeIndex < routine.Count; nodeIndex++)
            {
                INode nodeCopy = ExtensionFunctions_cl.DeepCopy<INode>(ScriptNodeManager_cl.Instance.GetNode(routine[nodeIndex]));
                AddScriptNode(nodeCopy, routine[nodeIndex]);
            }

            foreach (EditableNode node in mRoutineNodes.Values)
            {
                SetActiveNode(node);
                break;
            }

            mRoutineName.Text = mActiveRoutine.Name;
        }

        public void SetActiveNode(EditableNode node)
        {
            Control[] nodeEditorControls = Parent.Controls.Find("Node Editor Panel", false);
            if (nodeEditorControls.Length > 0)
            {
                ((NodeEditorPanel)nodeEditorControls[0]).SetActiveNode(node.CurrentNode);
            }
            HighlightActiveEditableNode(node);
        }

        private void NodeDropDownChanged(object sender, EventArgs e)
        {
            EditableNode node = ((EditableNode)((ComboBox)sender).Parent);
            if ((string)node.NodeSelector.SelectedItem != "New Node")
            {
                node.CurrentNode = ExtensionFunctions_cl.DeepCopy<INode>(ScriptNodeManager_cl.Instance.GetNode((string)node.NodeSelector.SelectedItem));
            }
            SetActiveNode(node);
            HighlightActiveEditableNode(node);

            RoutineChangesApplied();
        }

        private void NodeClicked(object sender, EventArgs e)
        {
            EditableNode node = ((EditableNode)((Control)sender).Parent);
            if ((string)node.NodeSelector.SelectedItem != "New Node")
            {
                node.CurrentNode = ExtensionFunctions_cl.DeepCopy<INode>(ScriptNodeManager_cl.Instance.GetNode((string)node.NodeSelector.SelectedItem));
            }

            SetActiveNode(node);
            foreach (KeyValuePair<int, EditableNode> nodePair in mRoutineNodes)
            {
                if (nodePair.Value == node)
                {
                    mActiveNode = nodePair;
                }
            }
            HighlightActiveEditableNode(node);

            RefreshNodeList(((EditableNode)((Control)sender).Parent).NodeSelector);
        }

        private void RefreshNodeList(ComboBox selector, string selection = "")
        {
            string previousSelected = "";
            if (selection == "")
            {
                previousSelected = (string)selector.SelectedItem;
            }
            else
            {
                previousSelected = selection;
            }

            selector.Items.Clear();
            selector.Items.Add("New Node");
            foreach (ScriptNode managerNode in ScriptNodeManager_cl.Instance.ScriptNodes.Values)
            {
                selector.Items.Add(managerNode.Name);
            }
            if (selector.Items.Contains(previousSelected))
            {
                selector.SelectedItem = previousSelected;
            }
        }

        private void HighlightActiveEditableNode(EditableNode activeNode)
        {
            foreach (KeyValuePair<int, EditableNode> nodePair in mRoutineNodes)
            {
                nodePair.Value.BackColor = Color.LightGray;
                if (nodePair.Value == activeNode)
                {
                    mActiveNode = nodePair;
                }
            }

            if (activeNode != null)
            {
                activeNode.BackColor = Color.LightGreen;
            }
        }

        public void Clean()
        {
            foreach (EditableNode node in mRoutineNodes.Values)
            {
                mRoutineNodesPanel.Controls.Remove(node);
            }
            mRoutineNodes.Clear();
            this.BackColor = Color.WhiteSmoke;
        }

        public void AcknowledgeNodeEditor()
        {
            Control[] nodeEditorControls = Parent.Controls.Find("Node Editor Panel", false);
            if (nodeEditorControls.Length > 0)
            {
                ((NodeEditorPanel)nodeEditorControls[0]).ApplyChangesButton.Click += new EventHandler(NodeChangesApplied);
            }
        }

        private void NodeChangesApplied(object sender, EventArgs e)
        {
            ScriptNodeManager_cl.Instance.SerializeNodes(mFnNodesPath, true);

            /************************************************************************
             * TODO:
             * This first condition stops it from breaking if you hit apply with no node selected.
             * Do we need the second condition still?
             *
             * Larsson Burch - 2011/11/15 - 17:43
             ************************************************************************/
            if (mActiveNode.Value != null && mActiveNode.Value.CurrentNode != null)
            {
                RefreshNodeList(mActiveNode.Value.NodeSelector, mActiveNode.Value.CurrentNode.GetName());
            }

            RoutineChangesApplied();
        }

        private void RoutineChangesApplied()
        {
            mActiveRoutine.Clear();
            foreach (EditableNode node in mRoutineNodes.Values)
            {
                if (((string)node.NodeSelector.SelectedItem) != "New Node")
                {
                    mActiveRoutine.Add((string)node.NodeSelector.SelectedItem);
                }
            }
        }
    }
}
