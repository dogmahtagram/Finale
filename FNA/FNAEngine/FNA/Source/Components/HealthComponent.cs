using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using FNA.Core;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// A component to handle the health of an Entity.
    /// </summary>
    [Serializable]
    public class HealthComponent_cl : Component_cl, ISerializable, IExclusiveComponent
    {
        /// <summary>
        /// The current health.
        /// </summary>
        private float mCurrentHealth = 100.0f;

        /// <summary>
        /// 
        /// </summary>
        public float Health
        {
            get
            {
                return mCurrentHealth;
            }
            set
            {
                mCurrentHealth = value;
            }
        }

        /// <summary>
        /// The maximum health.
        /// </summary>
        private float mMaxHealth = 100.0f;

        /// <summary>
        /// 
        /// </summary>
        public float MaxHealth
        {
            get
            {
                return mMaxHealth;
            }
            set
            {
                mMaxHealth = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        public HealthComponent_cl(Entity_cl parent)
            : base(parent)
        {
            mCurrentHealth = mMaxHealth;

            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        /// <param name="max">Max and initial health.</param>
        public HealthComponent_cl(Entity_cl parent, float max)
            : base(parent)
        {
            mMaxHealth = max;
            mCurrentHealth = mMaxHealth;

            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected HealthComponent_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mCurrentHealth = info.GetSingle("CurrentHealth");
            mMaxHealth = info.GetSingle("MaxHealth");
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

            info.AddValue("CurrentHealth", mCurrentHealth);
            info.AddValue("MaxHealth", mMaxHealth);
        }

        /// <summary>
        /// Add the given amount of health to the current health.
        /// A negative amount subtracts health.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        public void AddHealth(float amount)
        {
            mCurrentHealth += amount;
        }
    }
}
