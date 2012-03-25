using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Components;
using FNA.Core;
using FNA.Graphics;
using FNA.Managers;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace FNA.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerEntity_cl : Entity_cl // need interfaces?
    {
        private static int TRIGGER_DEFAULT_SIZE = 1;
        
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return ((PositionComponent_cl)GetComponentOfType(typeof(PositionComponent_cl))).Position3D;
            }
            set
            {
                ((PositionComponent_cl)GetComponentOfType(typeof(PositionComponent_cl))).SetPosition3D(value);
            }
        }

        private Body mPhysicsBody;
        private Fixture mPhysicsFixture;

        /// <summary>
        /// The type of physics object on this physics component.
        /// </summary>
        private PhysicsComponent_cl.PhysicsObjectType mPhysicsType;

        /// <summary>
        /// Gets or sets the physics object type.
        /// </summary>
        [System.ComponentModel.Description("The shape of the Physics object for collision.")]
        public PhysicsComponent_cl.PhysicsObjectType PhysicsType
        {
            get
            {
                return mPhysicsType;
            }
            set
            {
                mPhysicsType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("The size of the Trigger.\nRectangle: X=Width, Y=Height\nCircle: X=Radius.")]
        public Vector2 Size
        {
            set
            {
                ((RenderableComponent_cl)GetComponentOfType(typeof(RenderableComponent_cl))).Sprite.Size = new Vector2(value.X, value.Y);
            }
        }

        private bool mDisableAfterTrigger;

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("Whether to disable after being triggered once.")]
        public bool DisableAfterTrigger
        {
            get
            {
                return mDisableAfterTrigger;
            }
            set
            {
                mDisableAfterTrigger = value;
            }
        }

        private bool mEnabled;

        private float mCooldownTime;
        
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("How long to wait before triggering again. Note this is ignored if Disable is true.")]
        public float CooldownTime
        {
            get
            {
                return mCooldownTime;
            }
            set
            {
                mCooldownTime = value;
            }
        }

        private float mCooldownTimer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="disable"></param>
        /// <param name="cooldown"></param>
        public TriggerEntity_cl(string type, bool disable = false, float cooldown = 0.0f)
            : base()
        {
            mDisableAfterTrigger = disable;
            mCooldownTime = cooldown;
            mCooldownTimer = 0.0f;

            AddComponent(new PositionComponent_cl(this));

            AddComponent(new RenderableComponent_cl(this, "whitePixel"));
            ((RenderableComponent_cl)GetComponentOfType(typeof(RenderableComponent_cl))).Sprite.Size = new Vector2(TRIGGER_DEFAULT_SIZE, TRIGGER_DEFAULT_SIZE);

            mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);
            SetShape(PhysicsComponent_cl.PhysicsObjectType.RECTANGLE);

            /************************************************************************
             * TODO:
             * Allow multiple trigger types.
             * Ex: Sound and DamageHealth components on a single TriggerEntity
             *
             * Jay Sternfield	-	2011/12/05
             ************************************************************************/
            AddTriggerOfType(type);

            TriggerManager_cl.Instance.AddTrigger(this);

            mEnabled = true;
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected TriggerEntity_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mPhysicsType = (PhysicsComponent_cl.PhysicsObjectType)info.GetValue("PhysicsType", typeof(PhysicsComponent_cl.PhysicsObjectType));
            mDisableAfterTrigger = info.GetBoolean("Disable");
            mCooldownTime = info.GetSingle("Cooldown");
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

            info.AddValue("PhysicsType", mPhysicsType);
            info.AddValue("Disable", mDisableAfterTrigger);
            info.AddValue("Cooldown", mCooldownTime);
        }

        /// <summary>
        /// 
        /// </summary>
        ~TriggerEntity_cl()
        {
            //TriggerManager_cl.Instance.QueueRemove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void AddTriggerOfType(string type)
        {
            TriggerManager_cl.TRIGGER_TYPES typeEnumerated = (TriggerManager_cl.TRIGGER_TYPES)Enum.Parse(typeof(TriggerManager_cl.TRIGGER_TYPES), type, true);

            switch (typeEnumerated)
            {
                case TriggerManager_cl.TRIGGER_TYPES.HIT_BOX_TRIGGER:
                    AddComponent(new HitBoxTrigger_cl(this));
                    break;
                default:
                    throw new Exception("Invalid trigger type");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreUpdate()
        {
            /************************************************************************
             * TODO:
             * Check the cooldown timer to check if this Trigger should be re-enabled.
             *
             * Jay Sternfield	-	2011/12/03
             ************************************************************************/
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if ((mEnabled == false) && (mDisableAfterTrigger == false))
            {
                mCooldownTimer += FNA.Game_cl.BaseInstance.Timer.ElapsedSeconds;
                if (mCooldownTimer > mCooldownTime)
                {
                    mCooldownTimer = 0.0f;
                    mEnabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            /************************************************************************
             * TODO:
             * Draw debug info.
             *
             * Jay Sternfield	-	2011/12/03
             ************************************************************************/

            // The trigger shape is drawn through its RenderableComponent
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (BaseTriggerComponent_cl triggerComponent in componentList.Where(component => component.Enabled && typeof(BaseTriggerComponent_cl).IsAssignableFrom(component.GetType())))
                {
                    triggerComponent.Trigger();
                }
            }

            mEnabled = false;
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        public void SetShape(PhysicsComponent_cl.PhysicsObjectType shape)
        {
            if (shape == mPhysicsType)
            {
                return;
            }

            /************************************************************************
             * HACK:
             * All shapes should be supported
             *
             * Jay Sternfield	-	2011/12/03
             ************************************************************************/
            if (shape != PhysicsComponent_cl.PhysicsObjectType.RECTANGLE && shape != PhysicsComponent_cl.PhysicsObjectType.CIRCLE)
            {
                throw new Exception("Trigger shape must be Rectangle or Circle.");
            }

            Vector2 size = ((RenderableComponent_cl)GetComponentOfType(typeof(RenderableComponent_cl))).Sprite.Size;

            mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);

            switch (shape)
            {
                case PhysicsComponent_cl.PhysicsObjectType.RECTANGLE:
                    mPhysicsFixture = FixtureFactory.AttachRectangle(size.X, size.Y, 0f, Vector2.Zero, mPhysicsBody);
                    break;
                case PhysicsComponent_cl.PhysicsObjectType.CIRCLE:
                    mPhysicsFixture = FixtureFactory.AttachCircle(size.X, 0f, mPhysicsBody);
                    break;
                default:
                    throw new Exception("Invalid trigger shape.");
            }

            mPhysicsType = shape;
            mPhysicsBody.BodyType = BodyType.Static;
            mPhysicsBody.IsSensor = true;
            mPhysicsBody.CollisionGroup = 100;
            mPhysicsBody.OnCollision += OnCollision;
        }
    }
}
