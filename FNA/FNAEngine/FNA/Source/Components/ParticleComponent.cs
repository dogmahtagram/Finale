using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using FNA.Core;
using FNA.Interfaces;


using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace FNA.Components
{
    /// <summary>
    /// Put an explanation for your component here.
    /// </summary>
    [Serializable]
    public class ParticleComponent_cl : Component_cl, ISerializable
    {
        // Put your member variables, enums, etc... here.

        /// <summary>
        /// 
        /// </summary>
        public ParticleEffect particle;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 particlelocation;






        #region Constructors / Serialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="location"></param>
        public ParticleComponent_cl(Entity_cl parent, Vector2 location)
            : base(parent)
        {
            particle = new ParticleEffect();
            particlelocation = location;
        }









        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected ParticleComponent_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }














        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// 
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Add data to the serialization context here.
        }
        #endregion

        // Place additional functions below here.
    }
}