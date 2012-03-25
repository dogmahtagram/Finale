using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Core;

namespace FNA.Components
{
    /// <summary>
    /// An extension of the BaseLightComponent which represents a ambient light source within FNA.
    /// </summary>
    [Serializable]
    public class AmbientLightComponent : BaseLightComponent, ISerializable
    {
        ///
        public float Depth { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent Entity of this component.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="color">The color of this AmbientLightComponent.</param>
        /// <param name="intensity">The intensity of the light of this AmbientLightComponent.</param>
        public AmbientLightComponent(Entity_cl parent, float depth, Vector3 color, float intensity) : base(parent)
        {
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected AmbientLightComponent(SerializationInfo info, StreamingContext context)
        {
            Depth = info.GetSingle("Depth");
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Depth", Depth);
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
