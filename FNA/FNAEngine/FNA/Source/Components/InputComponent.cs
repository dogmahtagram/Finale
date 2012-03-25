using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FNA.Managers;
using FNA.Core;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// A component that handles input queries from other components and interfaces with the InputManager
    /// </summary>
    [Serializable]
    public class InputComponent_cl : Component_cl, ISerializable, IComponent, IPreUpdateAble, IExclusiveComponent
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public ISite Site
        {
            // return our "site" which connects back to us to expose our tagged methods
            get { return new DesignerVerbSite_cl(this); }
            set { throw new NotImplementedException(); }
        }

        System.Windows.Forms.ComboBox mKeySelection;
        System.Windows.Forms.Form mAddInputForm;
        System.Windows.Forms.ComboBox mButtonSelection;
        System.Windows.Forms.TextBox mNameBox;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public void AddInput()
        {
            mAddInputForm = new System.Windows.Forms.Form();
            mNameBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button addInputButton = new System.Windows.Forms.Button();

            mAddInputForm.Size = new System.Drawing.Size(384, 128);

            mNameBox.Location = new System.Drawing.Point(0, 0);
            mNameBox.Name = "mInputName";
            mNameBox.Size = new System.Drawing.Size(128, 20);
            mNameBox.Text = "[enter input name]";
            mNameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            mAddInputForm.Controls.Add(mNameBox);

            mKeySelection = new System.Windows.Forms.ComboBox();
            mKeySelection.Location = new System.Drawing.Point(128, 0);
            mKeySelection.Name = "mKeySelection";
            mKeySelection.Size = new System.Drawing.Size(128, 20);
            mKeySelection.TabIndex = 0;
            mKeySelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                mKeySelection.Items.Add(key);
            }

            mButtonSelection = new System.Windows.Forms.ComboBox();
            mButtonSelection.Location = new System.Drawing.Point(256, 0);
            mButtonSelection.Name = "mButtonSelection";
            mButtonSelection.Size = new System.Drawing.Size(128, 20);
            mButtonSelection.TabIndex = 0;
            mButtonSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                mButtonSelection.Items.Add(button);
            }

            mAddInputForm.Controls.Add(mButtonSelection);
            mAddInputForm.Controls.Add(mKeySelection);

            addInputButton.Location = new System.Drawing.Point(144, 32);
            addInputButton.Name = "mAddInputButton";
            addInputButton.Size = new System.Drawing.Size(96, 20);
            addInputButton.Text = "Add Input";
            addInputButton.UseVisualStyleBackColor = true;
            addInputButton.Click += new EventHandler(AddInputClicked);

            mAddInputForm.Controls.Add(addInputButton);

            mAddInputForm.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddInputClicked(object sender, EventArgs e)
        {
            if (mNameBox.Text != "[enter input name]")
            {
                if (mButtonSelection.SelectedItem != null)
                {
                    AddButton(mNameBox.Text, (Buttons)mButtonSelection.SelectedItem);
                }
                if (mKeySelection.SelectedItem != null && (Keys)mKeySelection.SelectedItem != Keys.None)
                {
                    AddKey(mNameBox.Text, (Keys)mKeySelection.SelectedItem);
                }
            }

            mAddInputForm.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // never called in this specific context with the PropertyGrid
            // but just reference the required Disposed event to avoid warnings
            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        /// <summary>
        /// A dictionary of all Keys this component cares about with their string names.
        /// </summary>
        private Dictionary<string, Keys> mKeys;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Keys> ActiveKeys
        {
            get
            {
                return mKeys;
            }
        }

        /// <summary>
        /// A dictionary of all Buttons this component cares about with their string names.
        /// </summary>
        private Dictionary<string, Buttons> mButtons;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Buttons> ActiveButtons
        {
            get
            {
                return mButtons;
            }
        }

        /// <summary>
        /// Dead zone for the thumbstick - fuck, should change this
        /// </summary>
        public const float mThumbStickDeadZone = 0.05f;

        private GamePadState mCurrentGamepadState;

        /// <summary>
        /// 
        /// </summary>
        public GamePadState CurrentGamepadState
        {
            set
            {
                mCurrentGamepadState = value;
            }
        }

        private GamePadState mLastGamePadState;

        /// <summary>
        /// 
        /// </summary>
        public GamePadState LastGamePadState
        {
            set
            {
                mLastGamePadState = value;
            }
        }

        /// <summary>
        /// The player index to associate a specific input device with this component.
        /// </summary>
        private PlayerIndex mPlayerIndex;

        /// <summary>
        /// 
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get
            {
                return mPlayerIndex;
            }
            set
            {
                mPlayerIndex = value;
                mCurrentGamepadState = InputManager_cl.Instance.GetGamePadState(mPlayerIndex);
                mLastGamePadState = InputManager_cl.Instance.GetLastGamePadState(mPlayerIndex);
            }
        }

        private bool mUseInputManager = true;

        /// <summary>
        /// 
        /// </summary>
        public bool UseInputManager
        {
            get
            {
                return mUseInputManager;
            }
            set
            {
                mUseInputManager = value;
            }
        }

        /// <summary>
        /// Constructor with an entity.
        /// </summary>
        /// <param name="parent"></param>
        public InputComponent_cl(Entity_cl parent)
            : base(parent)
        {
            mKeys = new Dictionary<string, Keys>();
            mButtons = new Dictionary<string, Buttons>();

            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected InputComponent_cl(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            List<KeyValuePair<string, Keys>> keyPairs = (List<KeyValuePair<string, Keys>>)(info.GetValue("KeyPairs", typeof(List<KeyValuePair<string, Keys>>)));
            List<KeyValuePair<string, Buttons>> buttonPairs = (List<KeyValuePair<string, Buttons>>)(info.GetValue("ButtonPairs", typeof(List<KeyValuePair<string, Buttons>>)));

            mKeys = new Dictionary<string, Keys>();
            foreach (KeyValuePair<string, Keys> pair in keyPairs)
            {
                mKeys.Add(pair.Key, pair.Value);
            }

            mButtons = new Dictionary<string, Buttons>();
            foreach (KeyValuePair<string, Buttons> pair in buttonPairs)
            {
                mButtons.Add(pair.Key, pair.Value);
            }

            mPlayerIndex = (PlayerIndex)Enum.Parse(typeof(PlayerIndex), info.GetString("PlayerIndex"));
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            List<KeyValuePair<string, Keys>> keyPairs = mKeys.ToList<KeyValuePair<string, Keys>>();
            info.AddValue("KeyPairs", keyPairs);

            List<KeyValuePair<string, Buttons>> buttonPairs = mButtons.ToList<KeyValuePair<string, Buttons>>();
            info.AddValue("ButtonPairs", buttonPairs);

            info.AddValue("PlayerIndex", mPlayerIndex.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        public void AddKey(string name, Keys key)
        {
            mKeys.Add(name, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="button"></param>
        public void AddButton(string name, Buttons button)
        {
            mButtons.Add(name, button);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreUpdate()
        {
            if (mUseInputManager == true)
            {
                mCurrentGamepadState = InputManager_cl.Instance.GetGamePadState(mPlayerIndex);
                mLastGamePadState = InputManager_cl.Instance.GetLastGamePadState(mPlayerIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsKeyUp(string input)
        {
            return InputManager_cl.Instance.KeyState.IsKeyUp(mKeys[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsKeyDown(string input)
        {
            return InputManager_cl.Instance.KeyState.IsKeyDown(mKeys[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyUp(string input)
        {
            return InputManager_cl.Instance.LastKeyState.IsKeyUp(mKeys[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyDown(string input)
        {
            return InputManager_cl.Instance.LastKeyState.IsKeyDown(mKeys[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyPressedThisFrame(string input)
        {
            return (InputManager_cl.Instance.LastKeyState.IsKeyUp(mKeys[input]) && InputManager_cl.Instance.KeyState.IsKeyDown(mKeys[input]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyReleasedThisFrame(string input)
        {
            return (InputManager_cl.Instance.LastKeyState.IsKeyDown(mKeys[input]) && InputManager_cl.Instance.KeyState.IsKeyUp(mKeys[input]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsKeyHeld(string input)
        {
            // fuck - currently there's no delay factored in here, TODO!
            return (InputManager_cl.Instance.LastKeyState.IsKeyDown(mKeys[input]) && InputManager_cl.Instance.KeyState.IsKeyDown(mKeys[input]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 RightStickPosition()
        {
            return new Vector2(mCurrentGamepadState.ThumbSticks.Right.X, mCurrentGamepadState.ThumbSticks.Right.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 LeftStickPosition()
        {
            return new Vector2(mCurrentGamepadState.ThumbSticks.Left.X, mCurrentGamepadState.ThumbSticks.Left.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsButtonDown(string input)
        {
            return mCurrentGamepadState.IsButtonDown(mButtons[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsButtonUp(string input)
        {
            return mCurrentGamepadState.IsButtonUp(mButtons[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasButtonDown(string input)
        {
            return mLastGamePadState.IsButtonDown(mButtons[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasButtonUp(string input)
        {
            return mLastGamePadState.IsButtonUp(mButtons[input]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasButtonPressedThisFrame(string input)
        {
            return (mLastGamePadState.IsButtonUp(mButtons[input]) && mCurrentGamepadState.IsButtonDown(mButtons[input]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasButtonReleasedThisFrame(string input)
        {
            return (mLastGamePadState.IsButtonDown(mButtons[input]) && mCurrentGamepadState.IsButtonUp(mButtons[input]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsButtonHeld(string input)
        {
            // fuck - currently there's no delay factored in here, TODO!
            return (mLastGamePadState.IsButtonDown(mButtons[input]) && mCurrentGamepadState.IsButtonDown(mButtons[input]));
        }
    }
}
