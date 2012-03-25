using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FNA;
using FNA.Components;
using FNA.Core;
using FNA.Managers;
using FNA.Scripts;

namespace FNA.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public class HitBox : Trigger
    {
        private Rectangle hitRect;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="duration"></param>
        /// <param name="flags"></param>
        public HitBox(Rectangle rect, float duration = float.MaxValue, AffectFlags flags = 0)
            : base(new Vector2(rect.X,rect.Y), duration, flags)
        {
            hitRect = rect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="velocity"></param>
        /// <param name="duration"></param>
        /// <param name="flags"></param>
        public HitBox(Rectangle rect, Vector2 velocity, float duration = float.MaxValue, AffectFlags flags = 0)
            : base(new Vector2(rect.X,rect.Y), velocity, duration, flags)
        {
            hitRect = rect;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
            
            foreach (CharacterControllerComponent character in ComponentManager_cl.GetEnabledComponents(typeof(CharacterControllerComponent)))
            {
                Vector2 pos = ((Components.PositionComponent)character.Parent.GetComponentOfType(typeof(PositionComponent))).Position2D;
                if (hitRect.Contains(new Point((int)pos.X,(int)pos.Y)))
                {
                    ((HealthComponent)character.Parent.GetComponentOfType(typeof(HealthComponent))).AddHealth(-10);
                }
            }
            
            base.PreUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="next"></param>
        public override void ProcessNode(Entity_cl entity, INode next)
        {
            base.ProcessNode(entity, next);
        }
    }
}
