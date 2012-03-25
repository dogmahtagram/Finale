using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA;
using FNA.Core;
using FNA.Managers;

namespace FNA.Components
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TigerAIControllerComponent_cl : CharacterControllerComponent_cl, ISerializable
    {
        private List<Entity_cl> mPlayerRefs;
        private Vector2 mTargetPos;
        private float mAttackDistance = 32.0f;

        /// <summary>
        /// The speed at which a tiger walks.
        /// </summary>
        private static float mSpeed = 0.2f;

        /// <summary>
        /// 
        /// </summary>
        public float Speed
        {
            get
            {
                return mSpeed;
            }
            set
            {
                mSpeed = value;
            }
        }

        /// <summary>
        /// Constructor which calls the overloaded constructor on the base class.
        /// </summary>
        /// <param name="parent">parent is the parent Entity of this component.</param>
        public TigerAIControllerComponent_cl(Entity_cl parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected TigerAIControllerComponent_cl(SerializationInfo info, StreamingContext context)
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
        /// Constructor which calls the overloaded constructor on the base class.
        /// </summary>
        /// <param name="parent">parent is the parent Entity of this component.</param>
        /// <param name="players"></param>
        public TigerAIControllerComponent_cl(Entity_cl parent, List<Entity_cl> players)
            : base(parent)
        {
            mPlayerRefs = players;
        }

        /// <summary>
        /// Adds a player to this component's list of references.
        /// </summary>
        /// <param name="e">The player reference.</param>
        public void AddPlayerRef(Entity_cl e)
        {
            mPlayerRefs.Add(e);
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
            PhysicsComponent_cl physics = ((PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl)));
            AnimatedComponent_cl animated = ((AnimatedComponent_cl)mParentEntity.GetComponentOfType(typeof(AnimatedComponent_cl)));

            // Get position of nearest player
            float minDistance = float.MaxValue;
            PositionComponent_cl positionComponent = ((PositionComponent_cl)(mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))));
            Vector2 parentPos = positionComponent.Position2D;
            foreach (Entity_cl entity in mPlayerRefs)
            {
                Vector2 playerPos = ((PositionComponent_cl)(entity.GetComponentOfType(typeof(PositionComponent_cl)))).Position2D;
                float distance = (playerPos - parentPos).Length();

                if (distance < minDistance)
                {
                    mTargetPos = playerPos;
                    minDistance = distance;
                }
            }

            if (mAttackDistance < minDistance)
            {
                // Apply force to move in that direction
                Vector2 movementDir = mTargetPos - parentPos;
                Vector2.Normalize(movementDir);

                ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).SetPosition2D(((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).Position2D.X + movementDir.X * mSpeed * Game_cl.BaseInstance.Timer.ElapsedSeconds,
                                                                                                ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).Position2D.Y + movementDir.Y * mSpeed * 0.57735f * Game_cl.BaseInstance.Timer.ElapsedSeconds);

                // Play the appropriate animation
                float x = movementDir.X;
                float y = movementDir.Y;

                if (x < 0)
                {
                    if (y < 0) animated.QueueAnimation("tigerWalk");
                    else if (y > 0) animated.QueueAnimation("tigerWalk");
                    else animated.QueueAnimation("tigerWalk");
                }

                else if (x > 0)
                {
                    if (y < 0) animated.QueueAnimation("tigerWalk");
                    else if (y > 0) animated.QueueAnimation("tigerWalk");
                    else animated.QueueAnimation("tigerWalk");
                }

                else // x==0
                {
                    if (y < 0) animated.QueueAnimation("tigerWalk");
                    else if (y > 0) animated.QueueAnimation("tigerWalk");
                    else animated.QueueAnimation("tigerIdle");
                }
            }
            else
            {
                animated.QueueAnimation("tigerAttack");
            }
            //CHECKING HEALTH
            if (((HealthComponent_cl)mParentEntity.GetComponentOfType(typeof(HealthComponent_cl))).Health <= 0)
            {

            }
        }
    }
}
