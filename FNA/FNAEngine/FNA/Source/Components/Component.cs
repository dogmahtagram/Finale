using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using FNA.Core;
using FNA.Managers;

namespace FNA.Components
{
    /// <summary>
    /// Base component class for FNA.
    /// </summary>
    [Serializable]
    public class Component_cl : ISerializable
    {
        /// <summary>
        /// The parent entity that this component resides on.
        /// </summary>
        protected Entity_cl mParentEntity;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Entity_cl Parent
        {
            get
            {
                return mParentEntity;
            }
            set
            {
                mParentEntity = value;
            }
        }

        /// <summary>
        /// Whether this component is enabled and gets updated/drawn
        /// </summary>
        protected bool mEnabled = true;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool Enabled
        {
            get
            {
                return mEnabled;
            }
            set
            {
                mEnabled = value;
            }
        }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        protected Component_cl()
        {
        }

        /// <summary>
        /// Constructor that assigns the new Component to an Entity.
        /// </summary>
        /// <param name="parent">The parent Entity for this Component.</param>
        protected Component_cl(Entity_cl parent)
        {
            mParentEntity = parent;

            ComponentManager_cl.PreAddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected Component_cl(SerializationInfo info, StreamingContext context)
        {
            mEnabled = info.GetBoolean("Enabled");
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
            info.AddValue("Enabled", mEnabled);
        }

        /// <summary>
        /// Assign the ownership of this component to the specified entity and register with the component manager.
        /// </summary>
        /// <param name="parent">parent is the entity which owns this component.</param>
        public virtual void AssignOwnership(Entity_cl parent)
        {
            mParentEntity = parent;
            mParentEntity.AddComponent(this);

            ComponentManager_cl.PreAddComponent(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string typeString = GetType().ToString();
            string[] typeNameParts = typeString.Split('.');
            return typeNameParts[typeNameParts.Length - 1];
        }
    }
}
