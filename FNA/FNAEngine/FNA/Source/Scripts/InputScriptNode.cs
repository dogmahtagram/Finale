using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework.Input;

using FNA.Components;
using FNA.Core;
using FNA.Managers;

namespace FNA.Scripts
{
    /// <summary>
    /// The InputScriptNode is allows scripts to query the state of an InputComponent and pass the result on to the next node.
    /// </summary>
    [Serializable()]
    public class InputScriptNode : ScriptNode
    {
        /// <summary>
        /// The input string as set by the previous node or the tool.
        /// </summary>
        private string mInput;

        /// <summary>
        /// 
        /// </summary>
        public string Input
        {
            get
            {
                return mInput;
            }
            set
            {
                mInput = value;
            }
        }

        /// <summary>
        /// The type of input action that this script processes.
        /// </summary>
        private InputScriptType mScriptType;

        /// <summary>
        /// 
        /// </summary>
        public InputScriptType ScriptType
        {
            get
            {
                return mScriptType;
            }
            set
            {
                mScriptType = value;
                if (mScriptType < InputScriptType.END_OF_BOOL_RETURN_TYPES)
                {
                    mOutputType = ScriptNode.OutputTypes.OUTPUT_BOOL;
                }
                else if (mScriptType < InputScriptType.END_OF_FLOAT_RETURN_TYPES)
                {
                    mOutputType = ScriptNode.OutputTypes.OUTPUT_FLOAT;
                }
                else
                {
                    mOutputType = ScriptNode.OutputTypes.OUTPUT_VECTOR2;
                }
            }
        }

        /// <summary>
        /// The possible input actions that this script node can process.
        /// </summary>
        public enum InputScriptType
        {
            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_DOWN,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_UP,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_WAS_DOWN,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_WAS_UP,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_PRESSED_THIS_FRAME,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_RELEASED_THIS_FRAME,

            /// <summary>
            /// 
            /// </summary>
            TYPE_BUTTON_HELD,


            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_DOWN,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_UP,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_WAS_DOWN,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_WAS_UP,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_PRESSED_THIS_FRAME,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_RELEASED_THIS_FRAME,

            /// <summary>
            /// 
            /// </summary>
            TYPE_KEY_HELD,

            /// <summary>
            /// 
            /// </summary>
            TYPE_LEFT_STICK_ACTIVE,

            /// <summary>
            /// 
            /// </summary>
            TYPE_RIGHT_STICK_ACTIVE,

            /// <summary>
            /// 
            /// </summary>
            END_OF_BOOL_RETURN_TYPES,

            /// <summary>
            /// 
            /// </summary>
            TYPE_LEFT_STICK_MAGNITUDE,

            /// <summary>
            /// 
            /// </summary>
            TYPE_RIGHT_STICK_MAGNITUDE,

            /// <summary>
            /// 
            /// </summary>
            END_OF_FLOAT_RETURN_TYPES,

            /// <summary>
            /// 
            /// </summary>
            TYPE_LEFT_STICK_POSITION,

            /// <summary>
            /// 
            /// </summary>
            TYPE_RIGHT_STICK_POSITION,
        }

        /// <summary>
        /// Default constructor - sets up known script node variables.
        /// </summary>
        public InputScriptNode() : base()
        {
            mInputType = ScriptNode.InputTypes.INPUT_VOID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InputScriptNode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            mScriptType = (InputScriptType)Enum.Parse(typeof(InputScriptType), info.GetString("ScriptType"));
            mInput = info.GetString("InitialValue");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ScriptType", mScriptType.ToString());
            info.AddValue("InitialValue", mInput);
        }

        /// <summary>
        /// Processes the appropriate query to the input component.
        /// </summary>
        /// <param name="entity">entity is the relevant entity of whos components we will query.</param>
        /// <param name="next">next is the next script node in this script of whos input value we will set with our output.</param>
        public override void ProcessNode(Entity_cl entity, string next)
        {
            InputComponent_cl input = (InputComponent_cl)(entity.GetComponentOfType(typeof(InputComponent_cl)));
            object output;
            INode nextNode = ScriptNodeManager_cl.Instance.GetNode(next);

            switch (mScriptType)
            {
                case InputScriptType.TYPE_BUTTON_DOWN:
                    output = input.IsButtonDown(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_UP:
                    output = input.IsButtonUp(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_WAS_DOWN:
                    output = input.WasButtonDown(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_WAS_UP:
                    output = input.WasButtonUp(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_PRESSED_THIS_FRAME:
                    output = input.WasButtonPressedThisFrame(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_RELEASED_THIS_FRAME:
                    output = input.WasButtonReleasedThisFrame(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_BUTTON_HELD:
                    output = input.IsButtonHeld(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_DOWN:
                    output = input.IsKeyDown(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_UP:
                    output = input.IsKeyUp(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_WAS_DOWN:
                    output = input.WasKeyDown(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_WAS_UP:
                    output = input.WasKeyUp(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_PRESSED_THIS_FRAME:
                    output = input.WasKeyPressedThisFrame(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_RELEASED_THIS_FRAME:
                    output = input.WasKeyReleasedThisFrame(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_KEY_HELD:
                    output = input.IsKeyHeld(mInput);
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_LEFT_STICK_MAGNITUDE:
                    output = input.LeftStickPosition().Length();
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_LEFT_STICK_POSITION:
                    output = input.LeftStickPosition();
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_LEFT_STICK_ACTIVE:
                    output = input.LeftStickPosition().Length() > InputComponent_cl.mThumbStickDeadZone;
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_RIGHT_STICK_MAGNITUDE:
                    output = input.RightStickPosition().Length();
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_RIGHT_STICK_POSITION:
                    output = input.RightStickPosition();
                    nextNode.SetInputValue(output);
                    break;

                case InputScriptType.TYPE_RIGHT_STICK_ACTIVE:
                    output = input.LeftStickPosition().Length() > InputComponent_cl.mThumbStickDeadZone;
                    nextNode.SetInputValue(output);
                    break;
            }
        }

        /// <summary>
        /// Sets the input string that this script node will use to query the input component with.
        /// </summary>
        /// <param name="input">input is the object which we are assigning as this node's input.</param>
        public override void SetInputValue(object input)
        {
            mInput = Convert.ToString(input);
        }
    }
}
