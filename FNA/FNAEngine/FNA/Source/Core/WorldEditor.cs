// The world editor exists only when you build in Debug wEditor mode.
#if WORLD_EDITOR

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaInput = Microsoft.Xna.Framework.Input;

using FNA.Components;
using FNA.Managers;
using FNA.Triggers;

namespace FNA.Core
{
    /************************************************************************
     * HACK:
     * The World Editor should be a stand-alone program that takes the DLL of
     * your game and provides functionality based on that.
     * For example: the editor gives a list of Trigger types based on those
     * created in your own game.
     * Right now the FNA project just contains all those Trigger types.
     *
     * Jay Sternfield	-	2011/12/05
     ************************************************************************/

    /// <summary>
    /// Contains the elements and functionality of a World Editor.
    /// Game contains an instance of this class.
    /// </summary>
    public class WorldEditor_cl
    {
        ///
        protected string mOpenSceneFile;

        /// 
        protected bool mShowEditorGUI;

        ///
        protected Vector2 mLastClickScreenCoordinates;
        /// 
        protected Vector3 mLastClickWorldCoordinates;
        /// 
        protected Vector2 mMouseCursorWorldCoordinates;
        /// 
        protected MenuStrip mEditorMenuStrip;
        ///
        protected StatusStrip mEditorStatusStrip;
        ///
        protected ToolStripStatusLabel mEditorStatusLabel;
        ///
        protected ToolStripMenuItem mMenuFile;
        ///
        protected ToolStripMenuItem mMenuPanels;
        ///
        protected ToolStripMenuItem mMenuView;
        ///
        protected ToolStripMenuItem mMenuItemNewScene;
        ///
        protected ToolStripMenuItem mMenuItemOpenScene;
        ///
        protected ToolStripMenuItem mMenuItemSaveScene;
        ///
        protected ToolStripMenuItem mMenuItemObjectPanel;
        ///
        protected ToolStripMenuItem mMenuItemPrefabPanel;
        ///
        protected ToolStripMenuItem mMenuItemDepthSnapping;
        /// 
        protected ContextMenuStrip mEditorContextMenu;
        ///
        protected ToolStripMenuItem mContextItemLoadPrefab;
        ///
        protected ToolStripMenuItem mContextItemAddTrigger;
        ///
        protected ToolStripMenuItem mContextItemEditComponents;
        ///
        protected ToolStripMenuItem mContextItemAddParticle;

        /// The panel that contains the list of Entities
        protected Panel mObjectPanel;
        /// The hierarchical view of the Scene's Entities.
        protected TreeView mObjectPanelTree;
        /// Allows the Object Panel to be resized
        protected Splitter mObjectPanelSplitter;

        /// The panel that contains the list of Entities
        protected Panel mPrefabPanel;
        /// The hierarchical view of the Scene's Entities.
        protected TreeView mPrefabPanelTree;
        /// Allows the Prefab Panel to be resized
        protected Splitter mPrefabPanelSplitter;

        List<string> mTriggerTypes;
        
        /// Flag for when the Object Panel is being resized
        protected bool mMouseResizing;

        /// <summary>
        /// 
        /// </summary>
        private enum PANEL_TYPE
        {
            INVALID = 0,
            OBJECT,
            PREFAB
        };

        private PANEL_TYPE mActivePanelType = PANEL_TYPE.INVALID;

        /// A list of references to all the Entities in the Scene
        protected Dictionary<string, Entity_cl> mObjectList;

        /// 
        protected bool mDialogOpen = false;
        /// <summary>
        /// 
        /// </summary>
        public bool DialogOpen
        {
            get
            {
                return mDialogOpen;
            }
        }

        /// 
        protected bool mDraggingPrefab = false;
        
        /// <summary>
        /// Gets whether depth snapping is turned on.
        /// </summary>
        /// <returns>true if snapping to depth is enabled.</returns>
        public bool DepthSnappingOn()
        {
            return mMenuItemDepthSnapping.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        public WorldEditor_cl()
        {
            mLastClickScreenCoordinates = Vector2.Zero;
            mLastClickWorldCoordinates = Vector3.Zero;
            mMouseCursorWorldCoordinates = Vector2.Zero;

            mObjectList = new Dictionary<string, Entity_cl>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            InitializeGUI();

            // Fill the Object Panel with the scene's objects
            foreach (Entity_cl entity in Game_cl.BaseInstance.Scene.Entities)
            {
                AddObjectToList(entity);
            }

            // Fill the Prefab Panel with existing prefabs
            string dir = Game_cl.BaseInstance.Content.RootDirectory;
            dir += "\\Prefabs\\";
            string[] prefabFilePaths = Directory.GetFiles(@dir, "*.fnPrefab");
            for (int i = 0; i < prefabFilePaths.Length; i++)
            {
                prefabFilePaths[i] = Path.GetFileNameWithoutExtension(prefabFilePaths[i]);
                mPrefabPanelTree.Nodes.Add(prefabFilePaths[i]);
                mPrefabPanelTree.ItemDrag += new ItemDragEventHandler(mPrefabPanelTree_ItemDrag);

                /************************************************************************
                 * TODO:
                 * Allow for sub-folders and recursively populate the tree.
                 *
                 * Jay Sternfield	-	2011/12/03
                 ************************************************************************/
            }

            // Fill the list of Trigger types
            mTriggerTypes = new List<string>();
            foreach (string type in Enum.GetNames(typeof(TriggerManager_cl.TRIGGER_TYPES)))
            {
                mTriggerTypes.Add(type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InitializeGUI()
        {
            EditingManager_cl.Instance.EditingEnabled = true;

            mShowEditorGUI = true;

            // Get the game window as a WinForm Form class
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;

            windowForm.MouseUp += new MouseEventHandler(windowForm_MouseUp);
            windowForm.MouseLeave += new EventHandler(windowForm_MouseLeave);
            windowForm.MouseEnter += new EventHandler(windowForm_MouseEnter);
            
            mEditorMenuStrip = new MenuStrip();
            mEditorMenuStrip.Dock = DockStyle.Top;
            mEditorMenuStrip.SuspendLayout();
            mEditorStatusStrip = new StatusStrip();
            mEditorStatusStrip.SuspendLayout();
            mEditorStatusLabel = new ToolStripStatusLabel("Welcome to the World Editor!");
            mMenuFile = new ToolStripMenuItem();
            mMenuPanels = new ToolStripMenuItem();
            mMenuView = new ToolStripMenuItem();
            mMenuItemNewScene = new ToolStripMenuItem();
            mMenuItemOpenScene = new ToolStripMenuItem();
            mMenuItemSaveScene = new ToolStripMenuItem();
            mMenuItemObjectPanel = new ToolStripMenuItem();
            mMenuItemPrefabPanel = new ToolStripMenuItem();
            mMenuItemDepthSnapping = new ToolStripMenuItem();
            
            //mEditorMenuStrip.Dock = System.Windows.Forms.DockStyle.Top;
            mEditorMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mMenuFile,
                mMenuPanels,
                mMenuView});
            mEditorMenuStrip.Location = new System.Drawing.Point(0, 0);
            mEditorMenuStrip.Name = "mEditorMenuStrip";
            mEditorMenuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            mEditorMenuStrip.Size = new System.Drawing.Size(windowForm.Width, 32);
            mEditorMenuStrip.TabIndex = 1;
            mEditorMenuStrip.Text = "mEditorMenuStrip";

            mEditorStatusStrip.Dock = DockStyle.Bottom;
            mEditorStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mEditorStatusLabel });
            mEditorStatusStrip.TabStop = false;
            mEditorStatusLabel.Size = new Size(mEditorStatusStrip.Width, mEditorStatusStrip.Height);

            mMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mMenuItemNewScene, mMenuItemOpenScene, mMenuItemSaveScene});
            mMenuFile.Name = "mMenuFile";
            mMenuFile.Size = new System.Drawing.Size(40, 20);
            mMenuFile.Text = "File";
            mMenuPanels.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mMenuItemObjectPanel, mMenuItemPrefabPanel});
            mMenuPanels.Name = "mMenuPanels";
            mMenuPanels.Size = new System.Drawing.Size(40, 20);
            mMenuPanels.Text = "Panels";

            mMenuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mMenuItemDepthSnapping});
            mMenuView.Name = "mMenuView";
            mMenuView.Size = new System.Drawing.Size(40, 20);
            mMenuView.Text = "View";

            mMenuItemNewScene.Name = "mMenuItemNewScene";
            mMenuItemNewScene.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            mMenuItemNewScene.Size = new System.Drawing.Size(200, 22);
            mMenuItemNewScene.Text = "New Scene";
            mMenuItemNewScene.Click += new System.EventHandler(mMenuItemNewScene_Click);
            mMenuItemOpenScene.Name = "mMenuItemOpenScene";
            mMenuItemOpenScene.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            mMenuItemOpenScene.Size = new System.Drawing.Size(200, 22);
            mMenuItemOpenScene.Text = "Open Scene";
            mMenuItemOpenScene.Click += new System.EventHandler(mMenuItemOpenScene_Click);
            mMenuItemSaveScene.Name = "mMenuItemSaveScene";
            mMenuItemSaveScene.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            mMenuItemSaveScene.Size = new System.Drawing.Size(200, 22);
            mMenuItemSaveScene.Text = "Save Scene";
            mMenuItemSaveScene.Click += new System.EventHandler(mMenuItemSaveScene_Click);
            mMenuItemObjectPanel.Size = new System.Drawing.Size(200, 22);
            mMenuItemObjectPanel.Text = "Object Panel";
            mMenuItemObjectPanel.Checked = true;
            mMenuItemObjectPanel.CheckOnClick = true;
            mMenuItemObjectPanel.Click += new System.EventHandler(mMenuItemObjectPanel_Click);
            mMenuItemPrefabPanel.Size = new System.Drawing.Size(200, 22);
            mMenuItemPrefabPanel.Text = "Prefab Panel";
            mMenuItemPrefabPanel.Checked = true;
            mMenuItemPrefabPanel.CheckOnClick = true;
            mMenuItemPrefabPanel.Click += new System.EventHandler(mMenuItemPrefabPanel_Click);
            mMenuItemDepthSnapping.Size = new System.Drawing.Size(200, 22);
            mMenuItemDepthSnapping.Text = "Snap to Depth";
            mMenuItemDepthSnapping.Checked = false;
            mMenuItemDepthSnapping.CheckOnClick = true;
            mMenuItemDepthSnapping.Click += new System.EventHandler(mMenuItemSnapping_Click);
            
            mEditorContextMenu = new ContextMenuStrip(new System.ComponentModel.Container());
            /************************************************************************
             * TODO:
             * Add images to the context menu items.
             *
             * Jay Sternfield	-	2011/11/16
             ************************************************************************/
            mContextItemLoadPrefab = new ToolStripMenuItem("Load Prefab");
            mContextItemAddTrigger = new ToolStripMenuItem("Add Trigger");
            mContextItemAddParticle = new ToolStripMenuItem("Add Particle");
            //mContextItemEditComponents = new ToolStripMenuItem("Edit Components");
            mContextItemLoadPrefab.Click += new System.EventHandler(mContextItemLoadPrefab_Click);
            mContextItemAddTrigger.Click += new System.EventHandler(mContextItemAddTrigger_Click);
            mContextItemAddParticle.Click += new System.EventHandler(mContextItemAddParticle_Click);
            //mContextItemAddParticle.Click += new System.EventHandler(mContextItemAddParticle_Click);
            //mContextItemAddParticle.Click += new System.EventHandler(mContextItemAddParticle_Click);
            mEditorContextMenu.SuspendLayout();
            mEditorContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                mContextItemLoadPrefab, mContextItemAddTrigger, mContextItemAddParticle});
            mEditorContextMenu.Name = "mEditorContextMenu";
            mEditorContextMenu.Size = new System.Drawing.Size(200, 28);
            mEditorContextMenu.MouseLeave += new EventHandler(mEditorContextMenu_MouseLeave);

            mObjectPanel = new Panel();
            mObjectPanel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            mObjectPanel.Size = new System.Drawing.Size(240, FNA.Game_cl.BaseInstance.WindowHeight - 22);
            mObjectPanel.Dock = DockStyle.Left;
            Panel titlePanel = new Panel();
            titlePanel.Dock = DockStyle.Top;
            titlePanel.Size = new System.Drawing.Size(mObjectPanel.Width, 22);
            Label titleLabel = new Label();
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.Text = "Object Panel";
            titlePanel.Controls.Add(titleLabel);
            mObjectPanelTree = new TreeView();
            mObjectPanelTree.Dock = DockStyle.Fill;
            //mObjectPanelTree.AllowDrop = true;
            mObjectPanelTree.HideSelection = false;
            mObjectPanelTree.DoubleClick += new EventHandler(mObjectPanelTree_DoubleClick);
            mObjectPanelTree.AfterSelect += new TreeViewEventHandler(mObjectPanelTree_AfterSelect);
            //Splitter midSplitter = new Splitter();
            //midSplitter.BackColor = System.Drawing.Color.DarkGray;
            //midSplitter.Dock = DockStyle.Bottom;
            mObjectPanelSplitter = new Splitter();
            mObjectPanelSplitter.Dock = DockStyle.Right;
            mObjectPanelSplitter.SplitPosition = mObjectPanel.Width - 4;
            mObjectPanelSplitter.MouseDown += new MouseEventHandler(mObjectPanelSplitter_MouseDown);
            mObjectPanelSplitter.MouseUp += new MouseEventHandler(mObjectPanelSplitter_MouseUp);
            
            mObjectPanel.Controls.Add(mObjectPanelTree);
            mObjectPanel.Controls.Add(titlePanel); 
            //mObjectPanel.Controls.Add(midSplitter);
            mObjectPanel.Controls.Add(mObjectPanelSplitter);

            mPrefabPanel = new Panel();
            mPrefabPanel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            mPrefabPanel.Size = new System.Drawing.Size(240, FNA.Game_cl.BaseInstance.WindowHeight - 22);
            mPrefabPanel.Dock = DockStyle.Right;
            mPrefabPanelTree = new TreeView();
            mPrefabPanelTree.Dock = DockStyle.Fill;
            //mPrefabPanelTree.AllowDrop = true;
            mPrefabPanelTree.HideSelection = false;
            mPrefabPanelTree.MouseDown += new MouseEventHandler(mPrefabPanelTree_MouseDown);

            titlePanel = new Panel();
            titlePanel.Dock = DockStyle.Top;
            titlePanel.Size = new System.Drawing.Size(mPrefabPanel.Width, 22);
            titleLabel = new Label();
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.Text = "Prefab Panel";
            titlePanel.Controls.Add(titleLabel);

            mPrefabPanelSplitter = new Splitter();
            mPrefabPanelSplitter.Dock = DockStyle.Left;
            mPrefabPanelSplitter.SplitPosition = 4;
            mPrefabPanelSplitter.MouseDown += new MouseEventHandler(mPrefabPanelSplitter_MouseDown);
            mPrefabPanelSplitter.MouseUp += new MouseEventHandler(mPrefabPanelSplitter_MouseUp);

            mPrefabPanel.Controls.Add(mPrefabPanelTree);
            mPrefabPanel.Controls.Add(titlePanel);
            mPrefabPanel.Controls.Add(mPrefabPanelSplitter);

            windowForm.Controls.Add(mEditorMenuStrip);
            windowForm.Controls.Add(mEditorStatusStrip);
            windowForm.Controls.Add(mObjectPanel);
            windowForm.Controls.Add(mPrefabPanel);

            windowForm.MainMenuStrip = mEditorMenuStrip;
            windowForm.ContextMenuStrip = mEditorContextMenu;

            mEditorMenuStrip.ResumeLayout(false);
            mEditorMenuStrip.PerformLayout();
            mEditorStatusStrip.ResumeLayout(false);
            mEditorStatusStrip.PerformLayout();
            mEditorContextMenu.ResumeLayout(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deinitialize()
        {
            DeinitializeGUI();

            mObjectList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DeinitializeGUI()
        {
            mShowEditorGUI = false;
            
            mMenuItemNewScene.Click -= new System.EventHandler(mMenuItemNewScene_Click);
            mMenuItemOpenScene.Click -= new System.EventHandler(mMenuItemOpenScene_Click);
            mMenuItemSaveScene.Click -= new System.EventHandler(mMenuItemSaveScene_Click);

            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            
            windowForm.Controls.Remove(mEditorMenuStrip);
            windowForm.Controls.Remove(mEditorStatusStrip);
            mEditorMenuStrip = null;
            windowForm.ContextMenuStrip = null;
            mEditorContextMenu = null;

            if (windowForm.Controls.Contains(mObjectPanel))
            {
                windowForm.Controls.Remove(mObjectPanel);
            }
            mObjectPanel = null;

            if (windowForm.Controls.Contains(mPrefabPanel))
            {
                windowForm.Controls.Remove(mPrefabPanel);
            }
            mPrefabPanel = null;

            windowForm = null;

            EditingManager_cl.Instance.EditingEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            XnaInput.MouseState currentMouseState = InputManager_cl.Instance.MouseState;

            if (mMouseResizing && (currentMouseState.X > 4))
            {
                Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;

                switch (mActivePanelType)
                {
                    case PANEL_TYPE.INVALID:
                        break;
                    case PANEL_TYPE.OBJECT:
                        mObjectPanel.Size = new System.Drawing.Size(currentMouseState.X, 0);
                        mObjectPanelSplitter.SplitPosition = currentMouseState.X - 4;
                        break;
                    case PANEL_TYPE.PREFAB:
                        mPrefabPanel.Size = new System.Drawing.Size(windowForm.Width - currentMouseState.X, 0);
                        mPrefabPanelSplitter.SplitPosition = currentMouseState.X - 4;
                        break;
                    default:
                        break;
                }
                
                return;
            }
        }

        /// <summary>
        /// Draws the GUI associated with the world editor.
        /// </summary>
        public void DrawEditorGUI()
        {
            //Game_cl.BaseInstance.SpriteBatch.Begin();

            //if (mShowEditorGUI)
            //{
                //string camInfo = "Camera Position: " + CameraManager_cl.Instance.ActiveCamera.Position.ToString();
                //mSpriteBatch.DrawString(BaseInstance.DebugFont, camInfo, new Vector2(220, 0), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

                //string mouseInfo = "Mouse Position: " + mMouseCursorWorldCoordinates.ToString();
                //mSpriteBatch.DrawString(BaseInstance.DebugFont, mouseInfo, new Vector2(220, 10), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            //}

            //Game_cl.BaseInstance.SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (mDraggingPrefab)
            {
                Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;

                if ((InputManager_cl.Instance.MouseState.X > mObjectPanel.Size.Width) &&
                    (InputManager_cl.Instance.MouseState.X < windowForm.Width - mPrefabPanel.Size.Width))                    
                {
                    string filePath = Game_cl.BaseInstance.Content.RootDirectory + "\\Prefabs\\";
                    filePath += mPrefabPanelTree.SelectedNode.Text + ".fnPrefab";
                    Entity_cl prefab = PrefabManager_cl.Instance.LoadPrefab(filePath);
                    Vector3 mouseWorldPosition = InputManager_cl.Instance.GetMouseWorldPosition();

                    // If the prefab has a physics component, use that to set the position, else just set the position component directly.
                    PhysicsComponent_cl physics = (PhysicsComponent_cl)prefab.GetComponentOfType(typeof(PhysicsComponent_cl));
                    if (physics != null)
                    {
                        physics.SetPosition(mouseWorldPosition);
                    }
                    else
                    {
                        PositionComponent_cl position = (PositionComponent_cl)prefab.GetComponentOfType(typeof(PositionComponent_cl));
                        position.SetPosition3D(mouseWorldPosition);
                    }

                    Game_cl.BaseInstance.Scene.AddEntity(prefab);

                    AddObjectToList(prefab);
                }

                mDraggingPrefab = false;
                windowForm.Cursor = Cursors.Arrow;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowForm_MouseLeave(object sender, EventArgs e)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            windowForm.ContextMenuStrip = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowForm_MouseEnter(object sender, EventArgs e)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            windowForm.ActiveControl = null;
            windowForm.ContextMenuStrip = mEditorContextMenu;             
        }

        /// <summary>
        /// Function to call when the New Scene menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemNewScene_Click(object sender, EventArgs e)
        {
            /************************************************************************
             * TODO:
             * Check if changes have been made to the scene, and prompt user to save.
             *
             * Jay Sternfield	-	2011/11/16
             ************************************************************************/

            SaveFileDialog fDialog = new SaveFileDialog();
            fDialog.Title = "Create FNA Animation Project";
            fDialog.Filter = ".fnAnim Files|*.fnAnim";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                if (fDialog.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(fDialog.FileName) == false)
                    {
                        System.IO.File.Create(fDialog.FileName);
                    }

                    ComponentManager_cl.Clear();
                    Game_cl.BaseInstance.Scene.Clear();
                }
            }
        }

        /// <summary>
        /// Function to call when the Open Scene menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemOpenScene_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "FNA Scene Files (*.fnScene)|*.fnScene";
            dialog.Title = "Select Scene to Load...";

            if ((dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                && (dialog.FileName != ""))
            {
                mOpenSceneFile = dialog.FileName;

                ComponentManager_cl.Clear();
                RenderableManager_cl.Instance.ClearRenderables();
                PhysicsManager_cl.Instance.ClearPhysics();

                Game_cl.BaseInstance.Scene = null;
                Game_cl.BaseInstance.Scene = SceneManager_cl.LoadScene(mOpenSceneFile);

                // Fill the Object Panel with the scene's objects
                foreach (Entity_cl entity in Game_cl.BaseInstance.Scene.Entities)
                {
                    AddObjectToList(entity);
                }
            }
        }

        /// <summary>
        /// Function to call when the Save Scene menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemSaveScene_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "FNA Scene Files (*.fnScene)|*.fnScene";
            dialog.Title = "Select Scene File...";

            if ((dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                && (dialog.FileName != ""))
            {
                mOpenSceneFile = dialog.FileName;
                SceneManager_cl.SaveScene(mOpenSceneFile, Game_cl.BaseInstance.Scene);
            }
        }

        /// <summary>
        /// Function to call when the Object Panel menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemObjectPanel_Click(object sender, EventArgs e)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            if (windowForm.Controls.Contains(mObjectPanel))
            {
                //mObjectPanelTree.MouseDoubleClick -= new MouseEventHandler(mObjectPanelTree_Click);
                windowForm.Controls.Remove(mObjectPanel);
            }
            else
            {
                windowForm.Controls.Add(mObjectPanel);
                //mObjectPanelTree.MouseDoubleClick += new MouseEventHandler(mObjectPanelTree_Click);
            }
        }

        /// <summary>
        /// Function to call when the Prefab Panel menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemPrefabPanel_Click(object sender, EventArgs e)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            if (windowForm.Controls.Contains(mPrefabPanel))
            {
                windowForm.Controls.Remove(mPrefabPanel);
            }
            else
            {
                windowForm.Controls.Add(mPrefabPanel);             
            }
        }

        /// <summary>
        /// Function to call when the snapping menu option is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuItemSnapping_Click(object sender, EventArgs e)
        {
        }

        private void mEditorContextMenu_MouseLeave(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Opens an input box to rename the selected object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjectPanelTree_DoubleClick(object sender, EventArgs e)
        {
            //if (mObjectPanelTree.SelectedNode != null)
            //{
            //    Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            //    windowForm.Enabled = false; 
            //    mDialogOpen = true;

            //    TreeNode selectedNode = mObjectPanelTree.SelectedNode;
            //    string name = selectedNode.Text;
            //    string oldName = name;
            //    Entity_cl entity = mObjectList[name];

            //    if (FNA.GUI.InputDialog_cl.Show("Object Name", "", ref name) == DialogResult.OK)
            //    {
            //        if (name != oldName)
            //        {
            //            selectedNode.Text = name;
            //            mObjectList.Remove(oldName);
            //            mObjectList.Add(name, entity);
            //            entity.Name = name;
            //        }
            //    }

            //    mDialogOpen = false;
            //    windowForm.Enabled = true;
            //    windowForm.ActiveControl = null;
            //    windowForm.ContextMenuStrip = mEditorContextMenu;
            //}
        }

        /// <summary>
        /// Opens an input box to rename the selected object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjectPanelTree_AfterSelect(object sender, EventArgs e)
        {
            foreach (Entity_cl entity in mObjectList.Values)
            {
                entity.ActiveForEditing = false;
            }

            if (mObjectPanelTree.SelectedNode != null)
            {
                mObjectList[mObjectPanelTree.SelectedNode.Text].ActiveForEditing = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mPrefabPanelTree_MouseDown(object sender, MouseEventArgs e)
        {
            mObjectPanelTree.SelectedNode = null;
            EditingManager_cl.Instance.DisableActiveEntity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mPrefabPanelTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (mPrefabPanelTree.SelectedNode != null)
            {
                Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
                windowForm.Cursor = Cursors.Cross;
                mDraggingPrefab = true;
            }
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjectPanelSplitter_MouseDown(object sender, MouseEventArgs e)
        {
            mMouseResizing = true;
            mActivePanelType = PANEL_TYPE.OBJECT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjectPanelSplitter_MouseUp(object sender, MouseEventArgs e)
        {
            mMouseResizing = false;
            mActivePanelType = PANEL_TYPE.INVALID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mPrefabPanelSplitter_MouseDown(object sender, MouseEventArgs e)
        {
            mMouseResizing = true;
            mActivePanelType = PANEL_TYPE.PREFAB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mPrefabPanelSplitter_MouseUp(object sender, MouseEventArgs e)
        {
            mMouseResizing = false;
            mActivePanelType = PANEL_TYPE.INVALID;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mContextItemLoadPrefab_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPrefabDialog = new OpenFileDialog();
            openPrefabDialog.Filter = "Prefab File|*.fnPrefab";
            openPrefabDialog.Title = "Load a Prefab";
            openPrefabDialog.ShowDialog();

            if (openPrefabDialog.FileName != "")
            {
                Entity_cl prefab = PrefabManager_cl.Instance.LoadPrefab(openPrefabDialog.FileName);
                Vector3 mouseWorldPosition = InputManager_cl.Instance.GetMouseWorldPosition();

                // If the prefab has a physics component, use that to set the position, else just set the position component directly.
                PhysicsComponent_cl physics = (PhysicsComponent_cl)prefab.GetComponentOfType(typeof(PhysicsComponent_cl));
                if(physics != null)
                {
                    physics.SetPosition(mouseWorldPosition);
                }
                else
                {
                    PositionComponent_cl position = (PositionComponent_cl)prefab.GetComponentOfType(typeof(PositionComponent_cl));
                    position.SetPosition3D(mouseWorldPosition);
                }

                Game_cl.BaseInstance.Scene.AddEntity(prefab);

                AddObjectToList(prefab);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mContextItemAddTrigger_Click(object sender, EventArgs e)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            windowForm.Enabled = false;
            mDialogOpen = true;

            string name = "";
            Vector3 mouseWorldPosition = InputManager_cl.Instance.GetMouseWorldPosition();

            if (FNA.GUI.DropDown_cl.Show("Trigger Type", "Select the Trigger type:", mTriggerTypes, ref name) == DialogResult.OK)
            {
                if (name != "")
                {
                    TriggerEntity_cl trigger = new TriggerEntity_cl(name);

                    trigger.Position = mouseWorldPosition;
                    trigger.Name = name;

                    Game_cl.BaseInstance.Scene.AddEntity(trigger);

                    AddObjectToList(trigger);
                }
            }

            mDialogOpen = false;
            windowForm.Enabled = true;
            windowForm.ActiveControl = null;
            windowForm.ContextMenuStrip = mEditorContextMenu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mContextItemAddParticle_Click(object sender, EventArgs e)
        {
            Vector2 particlelocation;
            string particleName;

            XnaInput.MouseState ms = XnaInput.Mouse.GetState();
            particlelocation = new Vector2(ms.X, ms.Y);

            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "XML files (*.xml)|*.xml";
            dialog.Title = "Select Particle File...";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                particleName = Path.GetFileNameWithoutExtension(dialog.FileName);
                particleName = "Particles/" + particleName;

                AddObjectToList(PartyManager_cl.Instance.CreateNewParticle(particleName, particlelocation));
            }
        }

        /// <summary>
        /// Add an Entity to the list displayed in the Object Panel.
        /// </summary>
        /// <param name="obj"></param>
        public void AddObjectToList(Entity_cl obj)
        {
            // If there is not already an entity of this same name, add this one.
            if (mObjectList.ContainsKey(obj.Name) == false)
            {
                mObjectList.Add(obj.Name, obj);
            }
            else // Append the next unused int to the end of the name, and add to the object list.
            {
                int suffix = 0;
                while (mObjectList.ContainsKey(obj.Name + suffix))
                {
                    suffix++;
                }

                obj.Name += suffix;
                mObjectList.Add(obj.Name, obj);
            }

            TreeNode node = new TreeNode(obj.Name);
            
            mObjectPanelTree.Nodes.Add(node);
        }

        /// <summary>
        /// Remove an Entity from the list displayed in the Object Panel.
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObjectFromList(Entity_cl obj)
        {
            mObjectList.Remove(obj.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SelectObject(Entity_cl entity)
        {
            if ((entity == null) || (!entity.ActiveForEditing))
            {
                mObjectPanelTree.SelectedNode = null;
                return;
            }

            for (int i = 0; i < mObjectPanelTree.Nodes.Count; i++)
            {
                if (mObjectPanelTree.Nodes[i].Text == entity.Name)
                {
                    mObjectPanelTree.SelectedNode = mObjectPanelTree.Nodes[i];
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MouseInEditorPanel(Vector2 pos)
        {
            Form windowForm = Control.FromHandle(FNA.Game_cl.BaseInstance.WindowHandle) as Form;
            
            if (windowForm.Controls.Contains(mObjectPanel))
            {
                if (InputManager_cl.Instance.MouseState.X < mObjectPanel.Size.Width)
                {
                    return false;
                }
            }

            if (windowForm.Controls.Contains(mPrefabPanel))
            {
                if (InputManager_cl.Instance.MouseState.X > (windowForm.Width - mPrefabPanel.Size.Width))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void ViewObjectProperties(Entity_cl entity)
        {
            SelectObject(entity);
            TreeNode selectedNode = mObjectPanelTree.SelectedNode;
            string oldName = entity.Name;
            
            mDialogOpen = true;
            FNA.GUI.PropertyDialog_cl.Show(entity.Name, ref entity);
            mDialogOpen = false;

            if (entity.Name != oldName)
            {
                selectedNode.Text = entity.Name;
                mObjectList.Remove(oldName);
                mObjectList.Add(entity.Name, entity);
            }
        }
    }
}

#endif
