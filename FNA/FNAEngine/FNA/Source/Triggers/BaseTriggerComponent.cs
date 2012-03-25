using System;

using Microsoft.Xna.Framework;

using FNA.Components;
using FNA.Core;
using FNA.Graphics;
using FNA.Managers;

namespace FNA.Triggers
{
    /// <summary>
    /// The responsibility of a Trigger Component is to set off events when the Entity containing the Component
    /// collides with other Entities. Triggers use the game's collision system (Farseer) as if they were game objects,
    /// but they differ in that they are (usually) invisible and do not react in a physical manner but instead
    /// "trigger" some event, viewing the collision as a sort of activation signal.
    /// </summary>
    public class BaseTriggerComponent_cl : Component_cl
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseTriggerComponent_cl()
        {
        }

        /// <summary>
        /// Constructor that assigns the new Component to an Entity.
        /// </summary>
        /// <param name="parent">The parent Entity for this Component.</param>
        protected BaseTriggerComponent_cl(Entity_cl parent)
            : base(parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void PreUpdate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Trigger()
        {
        }
    }
}
