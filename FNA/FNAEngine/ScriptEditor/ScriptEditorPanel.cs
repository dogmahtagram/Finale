using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using FNA.Scripts;

namespace ScriptEditor
{
    public class ScriptEditorPanel : Panel
    {
        public struct RoutineButton
        {
            public ScriptRoutine Routine;
            public Button Button;
        }

        private Button mMoveRoutineDownButton;
        private Button mMoveRoutineUpButton;
        private Button mRemoveRoutineButton;
        private Button mAddRoutineButton;
        private Label mScriptNameLabel;

        private TextBox mScriptName;
        public TextBox ScriptName
        {
            get
            {
                return mScriptName;
            }
            set
            {
                mScriptName = value;
            }
        }

        private GroupBox mScriptRoutinesGroup;
        private Panel mScriptRoutinesPanel;

        private Dictionary<int, RoutineButton> mRoutines = new Dictionary<int, RoutineButton>();

        private KeyValuePair<int, RoutineButton> mActiveRoutine;
        public RoutineButton ActiveRoutine
        {
            get
            {
                return mActiveRoutine.Value;
            }
        }

        private Script mActiveScript;

        public ScriptEditorPanel()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.Name = "Script Editor Panel";
            this.BorderStyle = BorderStyle.Fixed3D;
            this.Dock = DockStyle.Fill;

            mScriptName = new TextBox();
            mScriptName.Location = new Point(12, 20);
            mScriptName.Size = new Size(130, 23);
            mScriptName.TextAlign = HorizontalAlignment.Center;
            mScriptName.ReadOnly = true;

            mScriptNameLabel = new Label();
            mScriptNameLabel.Size = new Size(130, 18);
            mScriptNameLabel.Location = new Point(12, 0);
            mScriptNameLabel.Text = "Script Name";
            mScriptNameLabel.TextAlign = ContentAlignment.MiddleCenter;

            mAddRoutineButton = new System.Windows.Forms.Button();
            mAddRoutineButton.Location = new System.Drawing.Point(0, 42);
            mAddRoutineButton.Name = "mAddRoutineButton";
            mAddRoutineButton.Size = new System.Drawing.Size(24, 24);
            mAddRoutineButton.TabIndex = 1;
            mAddRoutineButton.Text = "+";
            mAddRoutineButton.UseVisualStyleBackColor = true;
            mAddRoutineButton.Click += new EventHandler(AddRoutineClicked);

            mRemoveRoutineButton = new System.Windows.Forms.Button();
            mRemoveRoutineButton.Location = new System.Drawing.Point(24, 42);
            mRemoveRoutineButton.Name = "mRemoveRoutineButton";
            mRemoveRoutineButton.Size = new System.Drawing.Size(24, 24);
            mRemoveRoutineButton.TabIndex = 2;
            mRemoveRoutineButton.Text = "-";
            mRemoveRoutineButton.UseVisualStyleBackColor = true;
            mRemoveRoutineButton.Click += new EventHandler(RemoveRoutineClicked);

            mMoveRoutineUpButton = new System.Windows.Forms.Button();
            mMoveRoutineUpButton.Location = new System.Drawing.Point(72, 42);
            mMoveRoutineUpButton.Name = "mMoveRoutineUpButton";
            mMoveRoutineUpButton.Size = new System.Drawing.Size(24, 24);
            mMoveRoutineUpButton.TabIndex = 3;
            mMoveRoutineUpButton.Text = "^";
            mMoveRoutineUpButton.UseVisualStyleBackColor = true;
            mMoveRoutineUpButton.Click += new EventHandler(DecreaseRoutineIndex);

            mMoveRoutineDownButton = new System.Windows.Forms.Button();
            mMoveRoutineDownButton.Location = new System.Drawing.Point(96, 42);
            mMoveRoutineDownButton.Name = "mMoveRoutineDownButton";
            mMoveRoutineDownButton.Size = new System.Drawing.Size(24, 24);
            mMoveRoutineDownButton.TabIndex = 4;
            mMoveRoutineDownButton.Text = "v";
            mMoveRoutineDownButton.UseVisualStyleBackColor = true;
            mMoveRoutineDownButton.Click += new EventHandler(IncreaseRoutineIndex);

            mScriptRoutinesGroup = new System.Windows.Forms.GroupBox();
            mScriptRoutinesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((
                System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom)));
            mScriptRoutinesGroup.Location = new System.Drawing.Point(3, 78);
            mScriptRoutinesGroup.Name = "mScriptRoutinesGroup";
            mScriptRoutinesGroup.TabIndex = 0;
            mScriptRoutinesGroup.TabStop = false;
            mScriptRoutinesGroup.Text = "Script Routines";

            mScriptRoutinesPanel = new Panel();
            mScriptRoutinesPanel.Dock = DockStyle.Fill;
            mScriptRoutinesPanel.AutoScroll = true;
            mScriptRoutinesGroup.Controls.Add(mScriptRoutinesPanel);

            this.Controls.Add(mScriptName);
            this.Controls.Add(mScriptNameLabel);
            this.Controls.Add(mScriptRoutinesGroup);
            this.Controls.Add(mAddRoutineButton);
            this.Controls.Add(mRemoveRoutineButton);
            this.Controls.Add(mMoveRoutineUpButton);
            this.Controls.Add(mMoveRoutineDownButton);
        }

        private void OrderRoutines()
        {
            foreach (KeyValuePair<int, RoutineButton> pair in mRoutines)
            {
                pair.Value.Button.Location = new System.Drawing.Point(0, pair.Key * 24);
            }
        }

        private void RemoveRoutineClicked(object sender, EventArgs e)
        {
            if (mActiveRoutine.Key != -1)
            {
                for (int index = mActiveRoutine.Key + 1; index < mRoutines.Count; index++)
                {
                    mRoutines[index - 1] = mRoutines[index];
                }
                mScriptRoutinesPanel.Controls.Remove(mActiveRoutine.Value.Button);
                mRoutines.Remove(mRoutines.Count - 1);
                mActiveScript.Remove(mActiveRoutine.Value.Routine);

                mActiveRoutine = new KeyValuePair<int, RoutineButton>(-1, mActiveRoutine.Value);

                OrderRoutines();
            }
        }

        private void AddRoutineClicked(object sender, EventArgs e)
        {
            string newName = "New Script Routine";
            ScriptRoutine routine = new ScriptRoutine();
            routine.Name = "New Script Routine";
            mActiveScript.Add(routine);

            AddRoutine(newName, routine);
        }

        private void AddRoutine(string name, ScriptRoutine routine)
        {
            Button button = new Button();
            button.Text = name;
            button.Click += new EventHandler(RoutineButtonSelected);
            button.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)));

            RoutineButton routineButton = new RoutineButton();
            routineButton.Button = button;
            routineButton.Routine = routine;

            mActiveRoutine = new KeyValuePair<int, RoutineButton>(mRoutines.Count, routineButton);
            SetActiveRoutine();

            mRoutines.Add(mRoutines.Count, routineButton);
            button.Width = mScriptRoutinesPanel.Width;
            mScriptRoutinesPanel.Controls.Add(button);

            OrderRoutines();
        }

        private void DecreaseRoutineIndex(object sender, EventArgs e)
        {
            if (mActiveRoutine.Key != -1 && mActiveRoutine.Key != 0)
            {
                RoutineButton temp = mRoutines[mActiveRoutine.Key - 1];
                mRoutines[mActiveRoutine.Key - 1] = mActiveRoutine.Value;
                mRoutines[mActiveRoutine.Key] = temp;
                mActiveRoutine = new KeyValuePair<int, RoutineButton>(mActiveRoutine.Key - 1, mActiveRoutine.Value);
            }

            OrderRoutines();
        }

        private void IncreaseRoutineIndex(object sender, EventArgs e)
        {
            if (mActiveRoutine.Key != -1 && mActiveRoutine.Key != mRoutines.Count - 1)
            {
                RoutineButton temp = mRoutines[mActiveRoutine.Key + 1];
                mRoutines[mActiveRoutine.Key + 1] = mActiveRoutine.Value;
                mRoutines[mActiveRoutine.Key] = temp;
                mActiveRoutine = new KeyValuePair<int, RoutineButton>(mActiveRoutine.Key + 1, mActiveRoutine.Value);
            }

            OrderRoutines();
        }

        private void RoutineButtonSelected(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, RoutineButton> routineButtonPair in mRoutines)
            {
                if (routineButtonPair.Value.Button == (Button)sender)
                {
                    mActiveRoutine = routineButtonPair;
                    SetActiveRoutine();
                    break;
                }
            }
        }

        private void HighlightActiveButton(RoutineButton activeButton)
        {
            foreach (RoutineButton button in mRoutines.Values)
            {
                button.Button.BackColor = Color.LightGray;
            }
            activeButton.Button.BackColor = Color.LightBlue;
        }

        private void SetActiveRoutine()
        {
            Control[] routineControls = Parent.Controls.Find("Routine Editor Panel", false);
            if (routineControls.Length > 0)
            {
                ((RoutineEditorPanel)routineControls[0]).SetActiveRoutine(mActiveRoutine.Value.Routine);
            }

            HighlightActiveButton(mActiveRoutine.Value);
        }

        public void SetActiveScript(Script script, string scriptName)
        {
            mScriptName.Text = scriptName;
            mActiveScript = script;

            mRoutines.Clear();
            mScriptRoutinesPanel.Controls.Clear();

            foreach (ScriptRoutine routine in mActiveScript)
            {
                AddRoutine(routine.Name, routine);
            }

            foreach (KeyValuePair<int, RoutineButton> routinePair in mRoutines)
            {
                mActiveRoutine = routinePair;
                mActiveRoutine.Value.Button.Select();
                SetActiveRoutine();
                break;
            }
        }

        public void Clean()
        {
            foreach (RoutineButton button in mRoutines.Values)
            {
                mScriptRoutinesPanel.Controls.Remove(button.Button);
            }
            mRoutines.Clear();
        }
    }
}
