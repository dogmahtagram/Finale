using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FNA.Managers;
using FNA.Graphics;
using FNA.Scripts;
using System.Collections.ObjectModel;

namespace ScriptEditor
{
    public class ScriptEditorSuite : TableLayoutPanel
    {
        RoutineEditorPanel mRoutineEditor;
        ScriptEditorPanel mScriptEditor;
        NodeEditorPanel mNodeEditor;

        private ObservableCollection<ScriptNode> mScriptNodes = new ObservableCollection<ScriptNode>();

        private Dictionary<int, ComboBox> mComboBoxes = new Dictionary<int, ComboBox>();
        private Dictionary<Button, List<string>> mScriptButtonMap = new Dictionary<Button, List<string>>();

        public ScriptEditorSuite() : base()
        {
            mRoutineEditor = new RoutineEditorPanel();
            mScriptEditor = new ScriptEditorPanel();
            mNodeEditor = new NodeEditorPanel();

            Initialize();
        }

        public void Initialize()
        {
            this.Name = "Script Editor Suite";
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.Anchor = ((System.Windows.Forms.AnchorStyles)((
            //    System.Windows.Forms.AnchorStyles.Top
            //    | System.Windows.Forms.AnchorStyles.Left
            //    | System.Windows.Forms.AnchorStyles.Right
            //    | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Location = new System.Drawing.Point(3, 16);
            this.RowCount = 1;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            this.Controls.Add(mScriptEditor, 0, 0);
            this.Controls.Add(mRoutineEditor, 1, 0);
            this.Controls.Add(mNodeEditor, 2, 0);

            mRoutineEditor.AcknowledgeNodeEditor();

            //LoadScripts();
        }

        /// <summary>
        /// Sets the active script that the script editor will be editing.
        /// </summary>
        /// <param name="script">script is the Script object that we will edit</param>
        /// <param name="animationName">animationName is the name to display in the editor so we know what script we're editing</param>
        /// <param name="frameIndex">frameIndex is the number to tag onto the animation name for specific frames</param>
        public void SetActiveScript(Script script, string animationName, int frameIndex = -1)
        {
            string scriptName = animationName;
            if(frameIndex != -1)
            {
                scriptName += frameIndex.ToString();
            }

            if (script.Count > 0)
            {
                Control[] scriptEditorControls = Controls.Find("Script Editor Panel", false);
                if (scriptEditorControls.Length > 0)
                {
                    ((ScriptEditorPanel)scriptEditorControls[0]).SetActiveScript(script, scriptName);
                }
            }
            else
            {
                Control[] scriptEditorControls = Controls.Find("Script Editor Panel", false);
                if (scriptEditorControls.Length > 0)
                {
                    ((ScriptEditorPanel)scriptEditorControls[0]).SetActiveScript(script, scriptName);
                    ((ScriptEditorPanel)scriptEditorControls[0]).Clean();
                    ((ScriptEditorPanel)scriptEditorControls[0]).ScriptName.Text = scriptName;
                }

                Control[] routineEditorControls = Controls.Find("Routine Editor Panel", false);
                if (routineEditorControls.Length > 0)
                {
                    ((RoutineEditorPanel)routineEditorControls[0]).Clean();
                }

                Control[] nodeEditorControls = Controls.Find("Node Editor Panel", false);
                if (nodeEditorControls.Length > 0)
                {
                    ((NodeEditorPanel)nodeEditorControls[0]).Clean();
                }
            }
        }

        public void ClearActiveScript()
        {
            Control[] scriptEditorControls = Controls.Find("Script Editor Panel", false);
            if (scriptEditorControls.Length > 0)
            {
                ((ScriptEditorPanel)scriptEditorControls[0]).ScriptName.Text = string.Empty;
            }
        }

        public void LoadScripts()
        {
            OpenFileDialog openFnNodesDialog = new OpenFileDialog();
            openFnNodesDialog.Filter = "fnNodes File|*.fnNodes";
            openFnNodesDialog.Title = "Select the .fnNodes file";
            openFnNodesDialog.ShowDialog();

            if (openFnNodesDialog.FileName != null)
            {

                Control[] routineEditorControls = Controls.Find("Routine Editor Panel", false);
                if (routineEditorControls.Length > 0)
                {
                    ((RoutineEditorPanel)routineEditorControls[0]).FnNodesPath = openFnNodesDialog.FileName;
                }

                ScriptNodeManager_cl.Instance.DeserializeNodes(openFnNodesDialog.FileName, true);
                foreach (ScriptNode node in ScriptNodeManager_cl.Instance.ScriptNodes.Values)
                {
                    mScriptNodes.Add(node);
                }
            }
        }

        public void LoadScripts(string path)
        {
            Control[] routineEditorControls = Controls.Find("Routine Editor Panel", false);
            if (routineEditorControls.Length > 0)
            {
                ((RoutineEditorPanel)routineEditorControls[0]).FnNodesPath = path;
            }

            ScriptNodeManager_cl.Instance.DeserializeNodes(path, true);
            foreach (ScriptNode node in ScriptNodeManager_cl.Instance.ScriptNodes.Values)
            {
                mScriptNodes.Add(node);
            }
        }

        public void HideEditables()
        {
            mScriptEditor.Hide();
            mNodeEditor.Hide();
            mRoutineEditor.Hide();
        }

        public void ShowEditables()
        {
            mScriptEditor.Show();
            mNodeEditor.Show();
            mRoutineEditor.Show();
        }
    }
}
