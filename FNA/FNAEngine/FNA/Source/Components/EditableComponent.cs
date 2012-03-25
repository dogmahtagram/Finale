using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using FNA.Core;
using FNA.Interfaces;


namespace FNA.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class EditableComponent_cl : Component_cl, IUpdateAble, IExclusiveComponent
    {
        #region Constructors / Serialization
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        public EditableComponent_cl(Entity_cl parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected EditableComponent_cl(SerializationInfo info, StreamingContext context)
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

            // Add data to the serialization context here.
        }
#endregion
        
        /// <summary>
        /// Run update logic for this component when in edit mode.
        /// </summary>
        public void Update()
        {
#if WORLD_EDITOR
#endif
        }
    }
}
