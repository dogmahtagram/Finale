using System;

using Microsoft.Xna.Framework;

using FNA.Core;

namespace FNA.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public class HitBoxTrigger_cl : BaseTriggerComponent_cl
    {
        /// <summary>
        /// 
        /// </summary>
        protected HitBoxTrigger_cl() : base()
        {
        }

        /// <summary>
        /// Constructor that assigns the new Component to an Entity.
        /// </summary>
        /// <param name="parent">The parent Entity for this Component.</param>
        public HitBoxTrigger_cl(Entity_cl parent)
            : base(parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
            /************************************************************************
             * TODO:
             *  Check collision with all valid Entities.
             *  Valid entities contain PhysicsComponents that are in the same collision group.
             *
             * Jay Sternfield	-	2011/12/03
             ************************************************************************/
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Trigger()
        {
            Console.WriteLine("hit box triggered!");
        }
    }
}
