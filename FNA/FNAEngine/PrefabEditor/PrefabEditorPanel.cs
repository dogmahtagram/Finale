using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using FNA;
using FNA.Components;
using FNA.Core;
using FNA.Managers;
using System.Reflection;

namespace PrefabEditor
{
    public class PrefabEditorPanel : Panel
    {
        private Panel mComponentsPanel;
        private Panel mComponentPropertiesPanel;
        private PropertyGrid mComponentProperties;

        private Button mAddComponentButton;
        private Button mRemoveComponentButton;
        private Button mLoadPrefabButton;
        private Button mNewPrefabButton;
        private Button mSavePrefabButton;

        private Entity_cl mEntity;
        private Button mActiveComponentButton;
        private ComponentNames mPreviousComponentType;
        private Label mComponentTypeLabel;

        private ComboBox mComponentTypeSelection = new ComboBox();
        private List<Button> mComponentButtons = new List<Button>();
        private Dictionary<string, Component_cl> mComponents = new Dictionary<string, Component_cl>();

        private string mComponentBaseAssembly = "FNA.Components.";

        private enum ComponentNames
        {
            AnimatedComponent_cl,
            HealthComponent_cl,
            InputComponent_cl,
            PhysicsComponent_cl,
            PlayerControllerComponent_cl,
            PositionComponent_cl,
            RenderableComponent_cl,
            RotationComponent,
            TigerAIControllerComponent_cl,
            UNSPECIFIED_COMPONENT,
        }

        public PrefabEditorPanel()
            : base()
        {
            mComponentsPanel = new Panel();
            mComponentPropertiesPanel = new Panel();
            mComponentProperties = new PropertyGrid();
            mAddComponentButton = new Button();
            mRemoveComponentButton = new Button();
            mLoadPrefabButton = new Button();
            mNewPrefabButton = new Button();
            mSavePrefabButton = new Button();
            mComponentTypeLabel = new Label();
        }

        public void Initialize()
        {
            Size = new System.Drawing.Size(600, 440);

            mComponentsPanel.AutoScroll = true;
            mComponentsPanel.Location = new System.Drawing.Point(0, 20);
            mComponentsPanel.Name = "mComponentsPanel";
            mComponentsPanel.Size = new System.Drawing.Size(200, 440);

            mComponentPropertiesPanel.Location = new System.Drawing.Point(200, 0);
            mComponentPropertiesPanel.Name = "mComponentPropertiesPanel";
            mComponentPropertiesPanel.Size = new System.Drawing.Size(400, 440);

            mComponentProperties.Location = new System.Drawing.Point(0, 48);
            mComponentProperties.Name = "mNodeProperties";
            mComponentProperties.Size = new System.Drawing.Size(400, 400);

            mLoadPrefabButton.Location = new System.Drawing.Point(64, 0);
            mLoadPrefabButton.Name = "mLoadPrefabButton";
            mLoadPrefabButton.Size = new System.Drawing.Size(64, 20);
            mLoadPrefabButton.Text = "Load";
            mLoadPrefabButton.UseVisualStyleBackColor = true;
            mLoadPrefabButton.MouseUp += new System.Windows.Forms.MouseEventHandler(LoadPrefabClicked);

            mNewPrefabButton.Location = new System.Drawing.Point(0, 0);
            mNewPrefabButton.Name = "mNewPrefabButton";
            mNewPrefabButton.Size = new System.Drawing.Size(64, 20);
            mNewPrefabButton.Text = "New";
            mNewPrefabButton.UseVisualStyleBackColor = true;
            mNewPrefabButton.MouseUp += new System.Windows.Forms.MouseEventHandler(NewPrefabClicked);

            mSavePrefabButton.Location = new System.Drawing.Point(128, 0);
            mSavePrefabButton.Name = "mSavePrefabButton";
            mSavePrefabButton.Size = new System.Drawing.Size(64, 20);
            mSavePrefabButton.Text = "Save";
            mSavePrefabButton.UseVisualStyleBackColor = true;
            mSavePrefabButton.MouseUp += new System.Windows.Forms.MouseEventHandler(SavePrefabClicked);

            mAddComponentButton.Location = new System.Drawing.Point(0, 0);
            mAddComponentButton.Name = "mAddComponentButton";
            mAddComponentButton.Size = new System.Drawing.Size(20, 20);
            mAddComponentButton.Text = "+";
            mAddComponentButton.UseVisualStyleBackColor = true;
            mAddComponentButton.MouseUp += new System.Windows.Forms.MouseEventHandler(AddComponentClicked);

            mRemoveComponentButton.Location = new System.Drawing.Point(20, 0);
            mRemoveComponentButton.Name = "mRemoveComponentButton";
            mRemoveComponentButton.Size = new System.Drawing.Size(20, 20);
            mRemoveComponentButton.Text = "-";
            mRemoveComponentButton.UseVisualStyleBackColor = true;
            mRemoveComponentButton.MouseUp += new System.Windows.Forms.MouseEventHandler(RemoveComponentClicked);

            mComponentTypeLabel.Location = new System.Drawing.Point(100, 0);
            mComponentTypeLabel.Size = new System.Drawing.Size(200, 20);
            mComponentTypeLabel.Text = "Component Type";
            mComponentTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            mComponentTypeSelection.Location = new System.Drawing.Point(100, 24);
            mComponentTypeSelection.Size = new System.Drawing.Size(200, 20);
            mComponentTypeSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            mComponentTypeSelection.SelectedIndexChanged += new System.EventHandler(ComponentTypeChanged);
            mComponentTypeSelection.SelectedIndexChanged += new System.EventHandler(ComponentTypeClicked);

            Controls.Add(mLoadPrefabButton);
            Controls.Add(mSavePrefabButton);
            Controls.Add(mNewPrefabButton);
            Controls.Add(mComponentsPanel);
            Controls.Add(mComponentPropertiesPanel);

            mComponentsPanel.Controls.Add(mAddComponentButton);
            mComponentsPanel.Controls.Add(mRemoveComponentButton);

            mComponentPropertiesPanel.Controls.Add(mComponentProperties);
            mComponentPropertiesPanel.Controls.Add(mComponentTypeSelection);
            mComponentPropertiesPanel.Controls.Add(mComponentTypeLabel);
            mComponentPropertiesPanel.Hide();

            mComponentsPanel.Hide();
            mComponentProperties.Hide();
        }

        private void LoadPrefabClicked(object sender, EventArgs e)
        {
            DeserializePrefab();
        }

        private void NewPrefabClicked(object sender, EventArgs e)
        {
            mEntity = new Entity_cl();
            mComponentProperties.Show();
            mComponentsPanel.Show();
        }

        private void SavePrefabClicked(object sender, EventArgs e)
        {
            SerializePrefab();
        }

        private void ComponentTypeClicked(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                mPreviousComponentType = (ComponentNames)((ComboBox)sender).SelectedItem;
            }
        }

        private void AddComponentClicked(object sender, EventArgs e)
        {
            Button newComponent = new Button();
            newComponent.Location = new System.Drawing.Point(0, 0);
            newComponent.Size = new System.Drawing.Size(200, 20);
            newComponent.Text = "UNSPECIFIED_COMPONENT";
            newComponent.UseVisualStyleBackColor = true;
            newComponent.Click += new EventHandler(ComponentClicked);

            mActiveComponentButton = newComponent;

            mPreviousComponentType = ComponentNames.UNSPECIFIED_COMPONENT;

            mComponentButtons.Add(newComponent);
            mComponentsPanel.Controls.Add(newComponent);

            OrderComponents();
        }

        private void RemoveComponentClicked(object sender, EventArgs e)
        {
            if (mActiveComponentButton != null)
            {
                mComponentButtons.Remove(mActiveComponentButton);
                mComponentsPanel.Controls.Remove(mActiveComponentButton);
                mActiveComponentButton = null;

                mComponents.Remove(mPreviousComponentType.ToString());

                OrderComponents();
            }
        }

        private void ComponentTypeChanged(object sender, EventArgs e)
        {
            if ((ComponentNames)((ComboBox)sender).SelectedItem != ComponentNames.UNSPECIFIED_COMPONENT)
            {
                if (((ComponentNames)((ComboBox)sender).SelectedItem).ToString() != mActiveComponentButton.Text)
                {
                    if (mComponents.ContainsKey(((ComboBox)sender).SelectedItem.ToString()) == false)
                    {
                        Assembly assembly = typeof(Component_cl).Assembly;
                        string componentName = mComponentBaseAssembly + ((ComboBox)sender).SelectedItem.ToString();

                        Component_cl newComponent = (Component_cl)Activator.CreateInstance(assembly.GetType(componentName), mEntity);
                        this.mComponentProperties.SelectedObject = newComponent;

                        if (mPreviousComponentType != ComponentNames.UNSPECIFIED_COMPONENT)
                        {
                            mComponents.Remove(mPreviousComponentType.ToString());
                        }
                        mComponents.Add(((ComboBox)sender).SelectedItem.ToString(), newComponent);
                        mActiveComponentButton.Text = ((ComponentNames)((ComboBox)sender).SelectedItem).ToString();
                    }
                    else
                    {
                        ((ComboBox)sender).SelectedItem = mPreviousComponentType;
                    }
                }
            }
        }

        private void ComponentClicked(object sender, EventArgs e)
        {
            Button box = (Button)sender;
            mActiveComponentButton = box;

            if (mComponents.ContainsKey(((Button)sender).Text))
            {
                mComponentProperties.SelectedObject = mComponents[((Button)sender).Text];
                mComponentTypeSelection.Text = ((Button)sender).Text;
            }
            else
            {
                mComponentTypeSelection.Text = "";
                mComponentProperties.SelectedObject = null;
            }

            mPreviousComponentType = (ComponentNames)Enum.Parse(typeof(ComponentNames), box.Text);

            mComponentTypeSelection.Items.Clear();
            foreach (ComponentNames name in Enum.GetValues(typeof(ComponentNames)))
            {
                mComponentTypeSelection.Items.Add(name);
            }

            if (mComponents.ContainsKey(((Button)sender).Text))
            {
                string selectedTypeName = mComponents[((Button)sender).Text].GetType().Name;
                mComponentTypeSelection.SelectedIndex = mComponentTypeSelection.FindStringExact(selectedTypeName);
            }
            else
            {
                mComponentTypeSelection.SelectedIndex = (int)ComponentNames.UNSPECIFIED_COMPONENT;
            }

            mComponentPropertiesPanel.Show();
        }

        private void OrderComponents()
        {
            int yOffset = 20;

            foreach (Button box in mComponentButtons)
            {
                box.Location = new System.Drawing.Point(0, yOffset);
                yOffset += 20;
            }
        }

        public void SerializePrefab()
        {
            SaveFileDialog savePrefabDialog = new SaveFileDialog();
            savePrefabDialog.Filter = "Prefab File|*.fnPrefab";
            savePrefabDialog.Title = "Save a Prefab";
            savePrefabDialog.ShowDialog();

            if (savePrefabDialog.FileName != "")
            {
                Stream stream = File.Create(savePrefabDialog.FileName);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryWriter writer = new BinaryWriter(stream);

                formatter.Serialize(stream, mEntity);
                stream.Close();
            }
        }

        public void DeserializePrefab()
        {
            OpenFileDialog loadPrefabDialog = new OpenFileDialog();
            loadPrefabDialog.Filter = "Prefab File|*.fnPrefab";
            loadPrefabDialog.Title = "Save a Prefab";
            loadPrefabDialog.ShowDialog();

            if (loadPrefabDialog.FileName != "")
            {
                mEntity = PrefabManager_cl.Instance.LoadPrefab(loadPrefabDialog.FileName);
                mComponents.Clear();

                foreach (Button button in mComponentButtons)
                {
                    mComponentsPanel.Controls.Remove(button);
                }
                mComponentButtons.Clear();

                foreach (List<Component_cl> componentList in mEntity.Components.Values)
                {
                    foreach (Component_cl component in componentList)
                    {
                        mComponents.Add(component.ToString(), component);
                    }
                }

                foreach (Component_cl component in mComponents.Values)
                {
                    Button newComponent = new Button();
                    newComponent.Location = new System.Drawing.Point(0, 0);
                    newComponent.Size = new System.Drawing.Size(200, 20);
                    newComponent.Text = component.ToString();
                    newComponent.UseVisualStyleBackColor = true;
                    newComponent.Click += new EventHandler(ComponentClicked);

                    mActiveComponentButton = newComponent;

                    mComponentButtons.Add(newComponent);
                    mComponentsPanel.Controls.Add(newComponent);
                }

                OrderComponents();

                mComponentProperties.Show();
                mComponentsPanel.Show();
            }
        }
    }
}
