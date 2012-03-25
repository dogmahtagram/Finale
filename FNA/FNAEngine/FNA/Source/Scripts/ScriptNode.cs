using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using FNA;
using FNA.Core;

namespace FNA.Scripts
{
    /// <summary>
    /// ScriptNode is an abstract class that provides the base notion of a node to build scripts with.
    /// </summary>
    public class ScriptNode : ISerializable, INode, IComparable
    {
        /// <summary>
        /// The possible types of input for a ScriptNode.
        /// </summary>
        public enum InputTypes
        {
            /// <summary>
            /// 
            /// </summary>
            INPUT_VOID,

            /// <summary>
            /// 
            /// </summary>
            INPUT_INT,

            /// <summary>
            /// 
            /// </summary>
            INPUT_FLOAT,

            /// <summary>
            /// 
            /// </summary>
            INPUT_BOOL,

            /// <summary>
            /// 
            /// </summary>
            INPUT_STRING
        }

        /// <summary>
        /// The possible types of output for a ScriptNode.
        /// </summary>
        public enum OutputTypes
        {
            /// <summary>
            /// 
            /// </summary>
            OUTPUT_VOID,

            /// <summary>
            /// 
            /// </summary>
            OUTPUT_INT,

            /// <summary>
            /// 
            /// </summary>
            OUTPUT_FLOAT,

            /// <summary>
            /// 
            /// </summary>
            OUTPUT_BOOL,

            /// <summary>
            /// 
            /// </summary>
            OUTPUT_VECTOR2
        }

        /// <summary>
        /// Whether this script node requires input for its Play method
        /// </summary>
        protected bool mInputRequired = false;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool InputRequired
        {
            get
            {
                return mInputRequired;
            }
        }

        /// <summary>
        /// The unique name associated with this node which is used to access it by any scripts that use this specific node.
        /// </summary>
        private string mName;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        /// <summary>
        /// The input type for this ScriptNode instance.
        /// </summary>
        protected InputTypes mInputType;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public InputTypes InputType
        {
            get
            {
                return mInputType;
            }
            set
            {
                mInputType = value;
            }
        }

        /// <summary>
        /// The output type for this ScriptNode instance.
        /// </summary>
        protected OutputTypes mOutputType;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public OutputTypes OutputType
        {
            get
            {
                return mOutputType;
            }
            set
            {
                mOutputType = value;
            }
        }

        /// <summary>
        /// Process node is the main script method which performs the function of a ScriptNode.
        /// </summary>
        /// <param name="entity">entity is the entity of whos components are relevant to the current script.</param>
        /// <param name="next">next is the next ScriptNode in the current script of whos input we will set with the current node's output.</param>
        public virtual void ProcessNode(Entity_cl entity, string next)
        {

        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScriptNode()
        {

        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected ScriptNode(SerializationInfo info, StreamingContext context)
        {
            mName = info.GetString("NodeName");
            mInputType = (ScriptNode.InputTypes)Enum.Parse(typeof(ScriptNode.InputTypes), info.GetString("InputType"));
            mOutputType = (ScriptNode.OutputTypes)Enum.Parse(typeof(ScriptNode.OutputTypes), info.GetString("OutputType"));
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NodeName", mName);
            info.AddValue("InputType", mInputType.ToString());
            info.AddValue("OutputType", mOutputType.ToString());
        }

        /// <summary>
        /// Sets the input value of this node. This method is called by previous nodes who pass in their output.
        /// </summary>
        /// <param name="input">input is the object which we will cast as this node's input.</param>
        public virtual void SetInputValue(object input)
        {

        }

        /// <summary>
        /// A method to return just the name of the node as its string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return mName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is ScriptNode)
            {
                ScriptNode node = (ScriptNode)obj;
                return mName.CompareTo(node.Name);
            }
            else
            {
                throw new ArgumentException("");
            }
        }
    }
}
