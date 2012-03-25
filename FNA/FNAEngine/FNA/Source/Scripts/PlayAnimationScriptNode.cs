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
    /// PlayAnimationScriptNode is a ScriptNode that plays an animation based on a boolean input.
    /// </summary>
    [Serializable]
    public class PlayAnimationScriptNode : ScriptNode
    {
        /// <summary>
        /// The boolean input for this node which will be set by the previous node.
        /// </summary>
        private bool mInput;

        /// <summary>
        /// Whether we should play the animation if the input is true or play the animation if the input is false.
        /// </summary>
        private bool mPlayIfTrue = true;

        /// <summary>
        /// 
        /// </summary>
        public bool PlayIfTrue
        {
            get
            {
                return mPlayIfTrue;
            }
            set
            {
                mPlayIfTrue = value;
            }
        }

        /// <summary>
        /// The string name of the animation that this node will play.
        /// </summary>
        private string mAnimationToQueue;

        /// <summary>
        /// 
        /// </summary>
        public string AnimationToQueue
        {
            get
            {
                return mAnimationToQueue;
            }
            set
            {
                mAnimationToQueue = value;
            }
        }

        /// <summary>
        /// The frame in the animation that this node will queue to.
        /// </summary>
        private int mFrameToQueue;

        /// <summary>
        /// 
        /// </summary>
        public int FrameToQueue
        {
            get
            {
                return mFrameToQueue;
            }
            set
            {
                mFrameToQueue = value;
            }
        }

        /// <summary>
        /// The frame in the animation that this node will queue to.
        /// </summary>
        private float mSpeedToQueue = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        public float SpeedToQueue
        {
            get
            {
                return mSpeedToQueue;
            }
            set
            {
                mSpeedToQueue = value;
            }
        }

        /// <summary>
        /// Default constructor - sets up known variables for this ScriptNode.
        /// </summary>
        public PlayAnimationScriptNode()
        {
            mInputType = ScriptNode.InputTypes.INPUT_BOOL;
            mOutputType = ScriptNode.OutputTypes.OUTPUT_VOID;
            mFrameToQueue = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PlayAnimationScriptNode(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mAnimationToQueue = info.GetString("AnimationName");
            mFrameToQueue = info.GetInt32("StartingFrame");
            mSpeedToQueue = info.GetSingle("StartingSpeed");
            mPlayIfTrue = info.GetBoolean("PlayIfTrue");
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
            info.AddValue("AnimationName", mAnimationToQueue);
            info.AddValue("StartingFrame", mFrameToQueue);
            info.AddValue("StartingSpeed", mSpeedToQueue);
            info.AddValue("PlayIfTrue", mPlayIfTrue);
        }

        /// <summary>
        /// Process node is the main script method which performs the function of a ScriptNode.
        /// </summary>
        /// <param name="entity">entity is the entity of whos components are relevant to the current script.</param>
        /// <param name="next">next is the next ScriptNode in the current script of whos input we will set with the current node's output.</param>
        public override void ProcessNode(Entity_cl entity, string next)
        {
            if (mInput == mPlayIfTrue)
            {
                AnimatedComponent_cl animated = (AnimatedComponent_cl)(entity.GetComponentOfType(typeof(AnimatedComponent_cl)));
                animated.QueueAnimation(mAnimationToQueue, mFrameToQueue, mSpeedToQueue);
            }

            mInput = false;
        }

        /// <summary>
        /// Sets the input value of this node. This method is called by previous nodes who pass in their output.
        /// </summary>
        /// <param name="input">input is the object which we will cast as this node's input.</param>
        public override void SetInputValue(object input)
        {
            mInput = Convert.ToBoolean(input);
        }
    }
}
