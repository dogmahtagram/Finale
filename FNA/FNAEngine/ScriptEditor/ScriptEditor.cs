using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.ObjectModel;

using FNA.Managers;
using FNA.Scripts;
using FNA.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FNA.Components;

namespace ScriptEditor
{
    public partial class ScriptEditor : Form
    {
        private struct FrameSelection
        {
            public int Frame;

            public override string ToString()
            {
                if(Frame == -1)
                {
                    return "ALL FRAMES";
                }
                else
                {
                    return Frame.ToString();
                }
            }
        }

        private ScriptEditorSuite mScriptEditor;
        private Script mAnimationScript = new Script();

        private List<Animation> mAnimations = new List<Animation>();

        public ScriptEditor()
        {
            mScriptEditor = new ScriptEditorSuite();

            InitializeComponent();
            this.mScriptEditingPanel.Controls.Add(mScriptEditor);
            mScriptEditor.HideEditables();
        }

        private void LoadFAPClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFAPDialog = new OpenFileDialog();
            openFAPDialog.Filter = "FAP File|*.fap";
            openFAPDialog.Title = "Load a FAP";
            openFAPDialog.ShowDialog();

            if (openFAPDialog.FileName != null)
            {
                mAnimations = AnimatedComponent_cl.LoadAnimationFile(openFAPDialog.FileName);
            }

            foreach (Animation anim in mAnimations)
            {
                mAnimationSelector.Items.Add(anim);
            }
        }

        private void AnimationSelectorChanged(object sender, EventArgs e)
        {
            mFrameSelector.Items.Clear();

            for (int frame = -1; frame <= ((Animation)mAnimationSelector.SelectedItem).NumFrames() - 1; frame++)
            {
                FrameSelection frameSelection = new FrameSelection();
                frameSelection.Frame = frame;
                mFrameSelector.Items.Add(frameSelection);
            }

            mScriptEditor.HideEditables();
        }

        private void FrameSelectionChanged(object sender, EventArgs e)
        {
            int frameIndex = ((FrameSelection)((ComboBox)sender).SelectedItem).Frame;
            if (frameIndex == -1)
            {
                mScriptEditor.SetActiveScript(((Animation)mAnimationSelector.SelectedItem).AnimationScripts, ((Animation)mAnimationSelector.SelectedItem).ToString());
            }
            else
            {
                if (((Animation)mAnimationSelector.SelectedItem).FrameScripts.ContainsKey(frameIndex))
                {
                    mScriptEditor.SetActiveScript(((Animation)mAnimationSelector.SelectedItem).FrameScripts[frameIndex], ((Animation)mAnimationSelector.SelectedItem).ToString(), frameIndex);
                }
                else
                {
                    mScriptEditor.SetActiveScript(new Script(), ((Animation)mAnimationSelector.SelectedItem).ToString(), frameIndex);
                }
            }

            mScriptEditor.ShowEditables();
        }

        private void SaveFAPClicked(object sender, EventArgs e)
        {
            SaveFileDialog saveFAPDialog = new SaveFileDialog();
            saveFAPDialog.Filter = "FAP File|*.fap";
            saveFAPDialog.Title = "Save a FAP";
            saveFAPDialog.ShowDialog();

            if (saveFAPDialog.FileName != null)
            {
                Stream stream = File.Create(saveFAPDialog.FileName);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryWriter writer = new BinaryWriter(stream);

                int numAnimations = mAnimations.Count;
                writer.Write(numAnimations);
                foreach (Animation anim in mAnimations)
                {
                    formatter.Serialize(stream, anim);
                }

                stream.Close();
            }
        }
    }
}
