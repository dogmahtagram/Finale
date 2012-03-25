using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using FNA.Components;
using FNA.Core;

namespace FNA.Scripts
{
    /// <summary>
    /// ToggleMotionDeltaScriptNode is a ScriptNode that toggles on and off motion delta for a specific channel based on a boolean input.
    /// </summary>
    [Serializable]
    public class ToggleMotionDeltaScriptNode : ScriptNode
    {
        /// <summary>
        /// The input bool that this node uses when processing.
        /// </summary>
        private bool mMotionDeltaOn;

        private bool mDefaultValue;

        /// <summary>
        /// 
        /// </summary>
        public bool DefaultValue
        {
            get
            {
                return mDefaultValue;
            }
            set
            {
                mDefaultValue = value;
            }
        }

        /// <summary>
        /// The channel of motion delta that this node cares about.
        /// </summary>
        private int mMotionDeltaChannel;

        /// <summary>
        /// 
        /// </summary>
        public int MotionDeltaChannel
        {
            get
            {
                return mMotionDeltaChannel;
            }
            set
            {
                mMotionDeltaChannel = value;
            }
        }

        /// <summary>
        /// Default constructor - sets up known variables for this ScriptNode.
        /// </summary>
        public ToggleMotionDeltaScriptNode()
        {
            mInputType = InputTypes.INPUT_BOOL;
            mOutputType = OutputTypes.OUTPUT_VOID;
            mMotionDeltaChannel = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ToggleMotionDeltaScriptNode(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mMotionDeltaChannel = info.GetInt32("MotionDeltaChannel");
            mDefaultValue = info.GetBoolean("DefaultValue");
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
            info.AddValue("MotionDeltaChannel", mMotionDeltaChannel);
            info.AddValue("DefaultValue", mDefaultValue);
        }

        /// <summary>
        /// Process node is the main script method which performs the function of a ScriptNode.
        /// </summary>
        /// <param name="entity">entity is the entity of whos components are relevant to the current script.</param>
        /// <param name="next">next is the next ScriptNode in the current script of whos input we will set with the current node's output.</param>
        public override void ProcessNode(Entity_cl entity, string next)
        {
            AnimatedComponent_cl animated = (AnimatedComponent_cl)(entity.GetComponentOfType(typeof(AnimatedComponent_cl)));
            animated.SetUseMotionDelta(mMotionDeltaOn, mMotionDeltaChannel);
        }

        /// <summary>
        /// Sets the input value of this node. This method is called by previous nodes who pass in their output.
        /// </summary>
        /// <param name="input">input is the object which we will cast as this node's input.</param>
        public override void SetInputValue(object input)
        {
            mMotionDeltaOn = Convert.ToBoolean(input);
        }
    }
}
