using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA;
using FNA.Core;

namespace FNA.Components
{
    /// <summary>
    /// An extension of the BaseLightComponent which represents a directional light source within FNA.
    /// </summary>
    [Serializable]
    public class DirectionalLightComponent : BaseLightComponent, ISerializable
    {
        private Vector3 mDirection;
        ///
        public Vector3 Direction
        {
            get
            {
                return mDirection;
            }
            set
            {
                mDirection = value;                
                mDirection.Normalize();
            }
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">the parent Entity of this component.</param>        
        /// <param name="direction">the direction of this DirectionalLightComponent.</param>     
        public DirectionalLightComponent(Entity_cl parent, Vector3 direction) : base(parent)
        {            
            mDirection = direction;
            mDirection.Normalize();
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected DirectionalLightComponent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mDirection = (Vector3)(info.GetValue("Direction", typeof(Vector3)));
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Direction", mDirection);
        }

        /// <summary>
        /// The PreUpdate method will handle anything that needs to take place before the Update.
        /// </summary>
        public override void PreUpdate()
        {

        }

        /// <summary>
        /// Main update loop.
        /// </summary>
        public override void Update()
        {

        }
    }
}
