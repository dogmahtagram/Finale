using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FNA.Core;
using FNA.Scripts;
using FNA.Managers;

namespace FNA.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public class Trigger : ITrigger
    {
        /// <summary>
        /// 
        /// </summary>
        protected Vector2 mPosition;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 mVelocity;

        /// <summary>
        /// 
        /// </summary>
        protected float mLifeLeft;

        /// <summary>
        /// 
        /// </summary>
        public bool mExists;

        /// <summary>
        /// 
        /// </summary>
        public AffectFlags Affect = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="duration"></param>
        /// <param name="flags"></param>
        public Trigger(Vector2 position, float duration = float.MaxValue, AffectFlags flags = 0)
        {
            mPosition = position;
            mVelocity = Vector2.Zero;
            mExists = true;
            Affect = flags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="duration"></param>
        /// <param name="flags"></param>
        public Trigger(Vector2 position, Vector2 velocity, float duration = float.MaxValue, AffectFlags flags = 0)
        {
            mPosition = position;
            mVelocity = velocity;
            mExists = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void PreUpdate()
        {
            mPosition += mVelocity * Game.BaseInstance.Timer.ElapsedMilliseconds;

            mLifeLeft -= Game.BaseInstance.Timer.ElapsedMilliseconds;
            if (mLifeLeft < 0.0f)
            {
                RemoveSelf();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RemoveSelf()
        {
            mExists = false;
            TriggerManager_cl.Instance.QueueRemove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// Process node is the main script method which performs the function of a ScriptNode.
        /// </summary>
        /// <param name="entity">entity is the entity of whos components are relevant to the current script.</param>
        /// <param name="next">next is the next ScriptNode in the current script of whos input we will set with the current node's output.</param>
        public virtual void ProcessNode(Entity_cl entity, INode next)
        {
            TriggerManager_cl.Instance.AddTrigger(this);
        }
    }
}
