using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Core;

namespace FNA.Components
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PointLightComponent : BaseLightComponent, ISerializable
    {
        ///
        public Vector3 Position { get; set; }
        ///
        private float Radius { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">the parent Entity of this component.</param>        
        /// <param name="position">the position of this PointLightComponent.</param>
        /// <note>The parent Entity MUST have a PositionComponent in order to construct this object.</note>
        public PointLightComponent(Entity_cl parent, Vector3 position) : base(parent)
        {
            PositionComponent_cl component = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            component.SetPosition3D(position);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected PointLightComponent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Position = (Vector3)(info.GetValue("Position", typeof(Vector3)));
            Radius = info.GetSingle("Radius");
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
            info.AddValue("Position", Position);
            info.AddValue("Radius", Radius);
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
