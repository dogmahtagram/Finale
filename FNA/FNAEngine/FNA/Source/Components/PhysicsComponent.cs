using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;

using SynapseGaming.LightingSystem.Core;

using FNA.Core;
using FNA.Managers;
using FNA.Interfaces;
using FNA.Components.Cameras;
using FNA.Graphics;


namespace FNA.Components
{
    /// <summary>
    /// The physics component ties a physics object into our component system allowing it to interact with animated components, position components, other physics components and more.
    /// The physics object that a physics component owns is the connection between FNA and the Farseer physics engine.
    /// </summary>
    [Serializable]
    public class PhysicsComponent_cl : Component_cl, ISerializable, IExclusiveComponent, IInitializeAble, IDrawAble
    {
        private Dictionary<Vector3, SunburnSprite_cl> mDebugShapeMap = new Dictionary<Vector3, SunburnSprite_cl>();

        /// <summary>
        /// The resolution at which objects snap their depth to.
        /// </summary>
        public static int mSnappingResolution = 20;

#if WORLD_EDITOR
        /// <summary>
        /// The speed at which we smoothly move through depth
        /// </summary>
        public static int mDepthMoveSpeed = 20;
#endif 

        /// <summary>
        /// The different types of physics objects.
        /// </summary>
        public enum PhysicsObjectType : int
        {
            ///
            INVALID = 0,
            ///
            RECTANGLE,
            ///
            CIRCLE,
            ///
            CAPSULE,
            ///
            FROM_TEXTURE,
            ///
            PLAYER,
        };

        /// <summary>
        /// The type of physics object on this physics component.
        /// </summary>
        private PhysicsObjectType mPhysicsType = PhysicsObjectType.RECTANGLE;

        /// <summary>
        /// Gets or sets the physics object type.
        /// </summary>
        public PhysicsObjectType PhysicsType
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
        /// The offset vector from the physics object to the position component.
        /// </summary>
        private Vector2 mPhysicsObjectOffset = Vector2.Zero;

        /// <summary>
        /// Gets or sets the offset vector between physics object and position component.
        /// </summary>
        public Vector2 PhysicsObjectOffset
        {
            get
            {
                return mPhysicsObjectOffset;
            }
            set
            {
                mPhysicsObjectOffset = value;
            }
        }

        /************************************************************************/
        /* Texture to Vertices                                                  */
        /************************************************************************/

        private static float mTextureToVerticesScale = 4 / 256.0f;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 mTextureToVerticesOrigin;

        /// <summary>
        /// 
        /// </summary>
        public Texture2D mPolygonTexture;
        

        /************************************************************************/

        private Body mPhysicsBody;

        /// <summary>
        /// 
        /// </summary>
        public Body PhysicsBody
        {
            get
            {
                return mPhysicsBody;
            }
        }

        private Fixture mPhysicsFixture;

        /*Detects when the player is about to land */
        private Fixture mFallingFixture;

        private PositionComponent_cl mPlayerPosition;

        private string mCollisionTexture = "";

        /// <summary>
        /// Gets or sets the collision texture.
        /// </summary>
        public string CollisionTexture
        {
            get
            {
                return mCollisionTexture;
            }
            set
            {
                mCollisionTexture = value;
            }
        }

        private float mWidth = 1;

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get
            {
                return mWidth;
            }
            set
            {
                mWidth = value;
            }
        }

        private float mHeight = 1;

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
            }
        }

        private bool mIsStatic = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return mIsStatic;
            }
            set
            {
                mIsStatic = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 currentVelocity
        {
            get
            {
                return mPhysicsBody.LinearVelocity;
            }
            set
            {
                mPhysicsBody.LinearVelocity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private float mMaxVelocity = 4.0f;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public float MaxVelocity
        {
            get
            {
                return mMaxVelocity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 mVelocity = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 Velocity
        {
            get
            {
                return mVelocity;
            }
            set
            {
                mVelocity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 mForce = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 Force
        {
            get
            {
                return mForce;
            }
            set
            {
                mForce = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 mAddedPosition = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 AddedPosition
        {
            get
            {
                return mAddedPosition;
            }
            set
            {
                mAddedPosition = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 mImpulse = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 Impulse
        {
            get
            {
                return mImpulse;
            }
            set
            {
                mImpulse = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 mJumpImpulse = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 JumpImpulse
        {
            get
            {
                return mJumpImpulse;
            }
            set
            {
                mJumpImpulse = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLanding = false;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool IsLanding
        {
            get
            {
                return mIsLanding;
            }

            set
            {
                mIsLanding = value;
            }
        }


        private bool mIsTerrain = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsTerrain
        {
            get
            {
                return mIsTerrain;
            }
            set
            {
                mIsTerrain = value;
            }
        }

        private float mAccelerationFromFriction = 1.5f;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public float AccelerationFromFriction
        {
            get
            {
                return mAccelerationFromFriction;
            }
            set
            {
                mAccelerationFromFriction = value;
            }
        }

        /// <summary>
        /// Get or set the physics object's position.
        /// </summary>
        [Browsable(false)]
        public Vector2 Position
        {
            get
            {
                return mPhysicsBody.Position;
            }
            set
            {
                mPhysicsBody.Position = value;
            }
        }

        private short mCollisionGroup = 0;

        /// <summary>
        /// The collision group that is set on this object by default.
        /// </summary>
        public short CollisionGroup
        {
            get
            {
                return mCollisionGroup;
            }
            set
            {
                mCollisionGroup = value;
            }
        }

        bool Fixture_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (mPhysicsBody.LinearVelocity.Y < 0.001f && mIsLanding == false)
            {
                mIsLanding = true;
                return true;
            }

            else
            {
                return false;
            }
        }


        private List<Vertices> points = new List<Vertices>();

        /************************************************************************
         * TODO:
         * For all constructors, put a check to see if there is already a position
         * component and, if not, create one.
         *
         * Larsson Burch - 2011/11/11 - 11:46
         ************************************************************************/

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this component.</param>
        public PhysicsComponent_cl(Entity_cl parent) : base(parent)
        {
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected PhysicsComponent_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            mIsStatic = info.GetBoolean("IsStatic");
            mWidth = info.GetSingle("Width");
            mHeight = info.GetSingle("Height");
            mCollisionGroup = info.GetInt16("CollisionGroup");
            mCollisionTexture = info.GetString("CollisionTexture");
            mPhysicsType = (PhysicsObjectType)info.GetInt32("PhysicsType");
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

            info.AddValue("IsStatic", mIsStatic);
            info.AddValue("Width", mWidth);
            info.AddValue("Height", mHeight);
            info.AddValue("CollisionGroup", mCollisionGroup);
            info.AddValue("CollisionTexture", mCollisionTexture);
            info.AddValue("PhysicsType", mPhysicsType);
        }

        /// <summary>
        /// Update this Component.
        /// </summary>
        public virtual void Update()
        {
            PositionComponent_cl position = ((PositionComponent_cl)(mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))));

            Vector2 newPosition = mPhysicsBody.Position;

            newPosition.X -= mPhysicsObjectOffset.X;
            newPosition.Y -= mPhysicsObjectOffset.Y;
            position.SetPosition2D(newPosition);

            // Position - ignores collision and gravity when moving
            if (mAddedPosition.Length() > 0)
            {
                mPhysicsBody.Position = mPhysicsBody.Position + mAddedPosition;
                mAddedPosition = Vector2.Zero;
            }

            // Velocity
            if (mVelocity.Length() > 0)
            {
                mPhysicsBody.LinearVelocity = mVelocity;
                mVelocity = Vector2.Zero;
            }

            // Constant Force
            if (mForce.Length() > 0)
            {
                // When applying Force, the added Velocity is Time dependent
                mPhysicsBody.ApplyForce(mForce);
                mForce = Vector2.Zero;

                // Cap velocity
                if (Math.Abs(mPhysicsBody.LinearVelocity.X) > mMaxVelocity)
                {
                    Vector2 velocityVector = mPhysicsBody.LinearVelocity;
                    if (velocityVector.X > 0)
                    {
                        velocityVector.X = mMaxVelocity;
                    }
                    else
                    {
                        velocityVector.X = -mMaxVelocity;
                    }
                    mPhysicsBody.LinearVelocity = velocityVector;
                }
            }

            // Impulse
            if (mImpulse.Length() > 0)
            {
                mPhysicsBody.Friction = 0.4f;
                // Applying Impulse, Velocity is affected instantaneously
                //mImpulse.Y = 0.0f; // kill vertical movement for now
                mPhysicsBody.ApplyLinearImpulse(mImpulse);
                mImpulse = Vector2.Zero;

                // Cap velocity
                if (Math.Abs(mPhysicsBody.LinearVelocity.X) > mMaxVelocity)
                {
                    Vector2 velocityVector = mPhysicsBody.LinearVelocity;
                    if (velocityVector.X > 0)
                    {
                        velocityVector.X = mMaxVelocity;
                    }
                    else
                    {
                        velocityVector.X = -mMaxVelocity;
                    }
                    mPhysicsBody.LinearVelocity = velocityVector;
                }
            }
            else if (mPhysicsBody.LinearVelocity.Length() > 0.0f)
            {
                mPhysicsBody.Friction = 0.95f;
            }

            // Jump Impulse
            if (mJumpImpulse.Length() > 0)
            {
                mPhysicsBody.ApplyLinearImpulse(mJumpImpulse);
                mJumpImpulse = Vector2.Zero;
            }
        }

        /// <summary>
        /// Sets the position of the physics object, position component, and updates the collision group.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            mPhysicsBody.Position = new Vector2(position.X, position.Y);

            PositionComponent_cl positionComponent = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));

            // Subtract the offset vector to get the correct position component position.
            position.X -= mPhysicsObjectOffset.X;
            position.Y -= mPhysicsObjectOffset.Y;

            positionComponent.Position3D = position;

            // The collision group is determined by the snapping resolution. We add a magic number to the end to
            // allow collision groups before depth zero that are not negative numbers.
            short collisionGroup = (short)((position.Z + mSnappingResolution / 2) / mSnappingResolution + 100);

            /************************************************************************
             * TODO:
             * Put a check here to see if we need to set the collision group.
             * Keep in mind, it must happen at least once.
             *
             * Larsson Burch - 2011/12/02 - 12:30
             ************************************************************************/
            SetCollisionGroup(collisionGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            if (Game_cl.IsPlayingGame == true)
            {
                InitializePhysics(mPhysicsType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializePhysics(PhysicsObjectType type)
        {
            PositionComponent_cl positionComponent = ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl)));

            switch(type)
            {
                case PhysicsObjectType.RECTANGLE:
                    mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);
                    mPhysicsFixture = FixtureFactory.AttachRectangle(mWidth, mHeight, 10, Vector2.Zero, mPhysicsBody);
                    mPhysicsBody.BodyType = mIsStatic ? BodyType.Static : BodyType.Dynamic;
                    mPhysicsFixture.Restitution = 0.0f;
                    mPhysicsFixture.Friction = 0.5f;

                    mPhysicsBody.AngularDamping = 1000.0f;

                    SetPosition(positionComponent.Position3D);
                    break;

                case PhysicsObjectType.CIRCLE:
                    mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);
                    mPhysicsFixture = FixtureFactory.AttachCircle(mHeight / 2, 10, mPhysicsBody);
                    mPhysicsBody.BodyType = mIsStatic ? BodyType.Static : BodyType.Dynamic;
                    mPhysicsFixture.Restitution = 0.0f;
                    mPhysicsFixture.Friction = 0.5f;

                    SetPosition(positionComponent.Position3D);
                    break;

                case PhysicsObjectType.CAPSULE:
                    mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);
                    mPhysicsFixture = FixtureFactory.AttachCircle(mHeight / 6, 100, mPhysicsBody);// .AttachRectangle(mWidth, mHeight, 10, Vector2.Zero, mPhysicsBody);
                    mPhysicsBody.BodyType = mIsStatic ? BodyType.Static : BodyType.Dynamic;
                    mPhysicsFixture.Restitution = 0.0f;
                    mPhysicsFixture.Friction = 0.5f;

                    //JointFactory.CreateFixedRevoluteJoint(PhysicsManager_cl.Instance.PhysicsWorld, mPhysicsBody, Vector2.Zero, Vector2.Zero);

                    mPhysicsObjectOffset = new Vector2(0, -2 * mHeight / 6);

                    mPhysicsBody.AngularDamping = 1000.0f;

                    SetPosition(positionComponent.Position3D);
                    break;

                case PhysicsObjectType.FROM_TEXTURE:
                    mIsTerrain = true;

                    CreatePolygonFromTexture(mCollisionTexture);

                    RenderableComponent_cl renderable = (RenderableComponent_cl)mParentEntity.GetComponentOfType(typeof(RenderableComponent_cl));
                    renderable.Sprite.Size = new Vector2(mPolygonTexture.Width, mPolygonTexture.Height) * mTextureToVerticesScale;

                    SetPosition(positionComponent.Position3D);
                    break;

                case PhysicsObjectType.PLAYER:
                    Vector2 fixtureOffset = new Vector2(0, -0.20f);
                    float fixtureDensity = 40.0f / (mWidth * mHeight); //we want a mass of 40kg

                    mPhysicsBody = BodyFactory.CreateBody(PhysicsManager_cl.Instance.PhysicsWorld);
                    mPhysicsFixture = FixtureFactory.AttachRectangle(mWidth, mHeight, fixtureDensity, fixtureOffset /2, mPhysicsBody);
                    mPhysicsBody.BodyType = mIsStatic ? BodyType.Static : BodyType.Dynamic;
                    mPhysicsFixture.Restitution = 0.0f;
                    mPhysicsFixture.Friction = 0.5f;

                    SunburnSprite_cl rectangleSprite = new SunburnSprite_cl();
                    rectangleSprite.InitSunburnStuff();
                    rectangleSprite.LoadContent("debugRectangle");
                    rectangleSprite.Rectangle = new FloatRectangle(0, 0, 1, 1);
                    rectangleSprite.Size = new Vector2(mWidth, mHeight);
                    mDebugShapeMap.Add(new Vector3(0, -0.1f, -0.1f), rectangleSprite);

                    mPhysicsBody.Inertia = 9999999999.0f; // resist rotation

                    SetPosition(positionComponent.Position3D);

                    /*Adding the falling sensor here */
                    mFallingFixture = FixtureFactory.AttachRectangle(mWidth, 1.75f, 0, new Vector2(0, -2.5f), mPhysicsBody);
                    mFallingFixture.IsSensor = true;

                    mFallingFixture.OnCollision += Fixture_OnCollision;
                    mFallingFixture.UserData = mParentEntity;

                    SunburnSprite_cl rectangleSprite2 = new SunburnSprite_cl();
                    rectangleSprite2.InitSunburnStuff();
                    rectangleSprite2.LoadContent("debugRectangle");
                    rectangleSprite2.Rectangle = new FloatRectangle(0, 0, 1, 1);
                    rectangleSprite2.Size = new Vector2(mWidth, 1.75f);
                    mDebugShapeMap.Add(new Vector3(0, -2.5f, -0.1f), rectangleSprite2);


                    /*focus camera to player */
                    mPlayerPosition = ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl)));
                    SunburnCameraComponent_cl camera = (SunburnCameraComponent_cl)CameraManager_cl.Instance.ActiveCamera;
                    camera.FocusOnPlayer(mPlayerPosition);


                    break;
                               
                default:
                    break;
            }

            /************************************************************************
             * HACK:
             * Setting the physics rotation off of the position component's rotation.
             *
             * Larsson Burch - 2011/12/02 - 13:29
             ************************************************************************/
            mPhysicsBody.Rotation = -positionComponent.Rotation;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreatePolygonFromTexture(string texture)
        {
            // need to decide how we want to choose texture - read material file?
            mPolygonTexture = Game_cl.BaseInstance.Content.Load<Texture2D>("Textures\\" + texture);
            uint[] data = new uint[mPolygonTexture.Width * mPolygonTexture.Height];
            mPolygonTexture.GetData(data);

            // Create a polygon from our texture, and simplify the resulting vertices according to the specified accuracy.
            Vertices textureVertices = PolygonTools.CreatePolygon(data, mPolygonTexture.Width, false);
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 20f);

            // Flip all the y values for FNA and sunburn and reorder the list
            // to remain clockwise.
            Vertices reorderedVertices = new Vertices();
            for (int vertIndex = textureVertices.Count - 1; vertIndex >= 0; vertIndex--)
            {
                Vector2 flippedVertex = textureVertices[vertIndex];
                flippedVertex.Y = -flippedVertex.Y;

                reorderedVertices.Add(flippedVertex);
            }
            textureVertices = reorderedVertices;

            // get centroid
            Vector2 centroid = -textureVertices.GetCentroid();

            Vector2 textureCenter = new Vector2(-mPolygonTexture.Width / 2, mPolygonTexture.Height / 2);
            textureVertices.Translate(ref textureCenter);

            // Get a list of the vertices and scale them from pixel space to FNA space.
            List<Vertices> vertexList = BayazitDecomposer.ConvexPartition(textureVertices);
            Vector2 vertScale = new Vector2(mTextureToVerticesScale);
            foreach (Vertices vertices in vertexList)
            {
                vertices.Scale(ref vertScale);
            }

            mPhysicsBody = BodyFactory.CreateCompoundPolygon(PhysicsManager_cl.Instance.PhysicsWorld, vertexList, 1f, (mIsStatic ? BodyType.Static : BodyType.Dynamic));
            mTextureToVerticesOrigin = new Vector2(mPolygonTexture.Width / 2, mPolygonTexture.Height / 2);
            mPhysicsBody.Friction = 0.2f;

            //for debug
            points = vertexList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(float rotation)
        {
            mPhysicsBody.Rotation = rotation;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            /************************************************************************
             * TODO:
             * Make this optional, put in some kind of debug draw boolean.
             *
             * Larsson Burch - 2011/11/10 - 13:38
             ************************************************************************/
            PositionComponent_cl position = ((PositionComponent_cl)(mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))));

            if (InputManager_cl.Instance.WasKeyPressedThisFrame(Keys.LeftControl))
            {
                foreach (KeyValuePair<Vector3, SunburnSprite_cl> pair in mDebugShapeMap)
                {
                    pair.Value.AddSpriteObject();
                }
            }
            else if (InputManager_cl.Instance.WasKeyReleasedThisFrame(Keys.LeftControl))
            {
                foreach (KeyValuePair<Vector3, SunburnSprite_cl> pair in mDebugShapeMap)
                {
                    pair.Value.RemoveSpriteObject();
                }
            }
            else if (InputManager_cl.Instance.IsKeyDown(Keys.LeftControl))
            {
                foreach (KeyValuePair<Vector3, SunburnSprite_cl> pair in mDebugShapeMap)
                {
                    pair.Value.Draw(position.Position3D + pair.Key, false, -mPhysicsBody.Rotation);
                }
            }

            //FNA.Game.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //Vector2 position = ((PositionComponent)mParentEntity.GetComponentOfType(typeof(PositionComponent))).Position2D;
            Vector3 cameraPosition = FNA.Managers.CameraManager_cl.Instance.ActiveCamera.Position;
            //Vector2 screenPosition = new Vector2((int)(ConvertUnits_cl.ToDisplayUnits(position.X - (cameraPosition.X)) - mWidth / 2), -(int)(ConvertUnits_cl.ToDisplayUnits(position.Y - (cameraPosition.Y)) + mHeight / 2));
            //Game.BaseInstance.SpriteBatch.Draw(FNA.Game.BaseInstance.Content.Load<Texture2D>("Textures\\red"), new Rectangle((int)screenPosition.X, (int)screenPosition.Y, (int)mWidth, (int)mHeight), null, new Color(1.0f, 1.0f, 1.0f, 0.01f));
            //FNA.Game.BaseInstance.SpriteBatch.End();

            
            //print vertices
            //SpriteBatch mSpriteBatch = FNA.Game.BaseInstance.SpriteBatch;
            //mSpriteBatch.Begin();
            //string samples = "o";

            //foreach (Vertices vertices in points)
            //{
            //    foreach (Vector2 pos in vertices)
            //    {
            //        Vector2 testPos = new Vector2((FNA.Game.BaseInstance.WindowWidth / 2) + ConvertUnits_cl.ToDisplayUnits(pos.X + cameraPosition.X), (FNA.Game.BaseInstance.WindowHeight / 2) - ConvertUnits_cl.ToDisplayUnits(pos.Y - cameraPosition.Y));
            //        mSpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, samples, testPos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
            //    }
            //}

            //mSpriteBatch.End();
        }

        /// <summary>
        /// Set the collision group for this physics body.
        /// </summary>
        /// <param name="group">The collision group that this body should be in.</param>
        public void SetCollisionGroup(short group)
        {
            mCollisionGroup = group;
            mPhysicsBody.CollidesWith = Category.None;
            mPhysicsBody.CollisionGroup = group;
        }
    }
}
