using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using FNA.Core;
using FNA.Managers;

namespace FNA.Components
{
    /// <summary>
    /// The character controller component is the base component for controllers which will define character specific logic.
    /// </summary>
    [Serializable]
    public class CharacterControllerComponent_cl : Component_cl, ISerializable
    {
        /// <summary>
        /// Base constructor - registers the component and adds it to the entity.
        /// </summary>
        /// <param name="parent"></param>
        public CharacterControllerComponent_cl(Entity_cl parent)
            : base(parent)
        {
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected CharacterControllerComponent_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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
        }

        /// <summary>
        /// No need for initialization as of yet.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// The PreUpdate method will handle anything that needs to take place before the Update.
        /// </summary>
        public virtual void PreUpdate()
        {

        }

        /// <summary>
        /// Main update loop method.
        /// </summary>
        public virtual void Update()
        {

        }
    }
}
