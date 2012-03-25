using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework.Input;

using FNA.Core;
using FNA.Managers;
using FNA.Components.Cameras;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// PositionComponent holds the position of 
    /// </summary>
    [Serializable]
    public class PositionComponent_cl : Component_cl, ISerializable, IExclusiveComponent, IUpdateAble
    {
        private Vector3 mPosition;

        /// <summary>
        /// Returns the position as a vector in 3-space.
        /// </summary>
        [Browsable(false)]
        public Vector3 Position3D
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        /// <summary>
        /// Returns the position as a vector in 2-space.
        /// </summary>
        [Browsable(false)]
        public Vector2 Position2D
        {
            get
            {
                return new Vector2(mPosition.X, mPosition.Y);
            }
        }

        private float mRotation = 0;

        /// <summary>
        /// Gets or sets the rotation of this component.
        /// </summary>
        public float Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                mRotation = value;
            }
        }

        /// <summary>
        /// Constructor with no specified default position.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        public PositionComponent_cl(Entity_cl parent)
            : base(parent)
        {
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        /// <param name="x">Initial X position.</param>
        /// <param name="y">Initial Y position.</param>
        /// <param name="z">Initial Z position.</param>
        public PositionComponent_cl(Entity_cl parent, float x, float y, float z = 0.0f)
            : base(parent)
        {
            mPosition = new Vector3(x, y, z);
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected PositionComponent_cl(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            mPosition = new Vector3(info.GetSingle("X"), info.GetSingle("Y"), info.GetSingle("Z"));
            mRotation = info.GetSingle("Rotation");
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("X", mPosition.X);
            info.AddValue("Y", mPosition.Y);
            info.AddValue("Z", mPosition.Z);
            info.AddValue("Rotation", mRotation);
        }

        /// <summary>
        /// Sets the X and Y coordinates of the position.
        /// </summary>
        /// <param name="x">The new X coordinate of the position</param>
        /// <param name="y">The new Y coordinate of the position</param>
        public void SetPosition2D(float x, float y)
        {
            mPosition.X = x;
            mPosition.Y = y;
        }

        /// <summary>
        /// Sets the X and Y coordinates of the position
        /// </summary>
        /// <param name="position">Vector2 containing the new X and Y coordinates for the position</param>
        public void SetPosition2D(Vector2 position)
        {
            SetPosition2D(position.X, position.Y);
        }

        /// <summary>
        /// Sets the X, Y, and Z coordinates of the position.
        /// </summary>
        /// <param name="x">The new X coordinate of the position</param>
        /// <param name="y">The new Y coordinate of the position</param>
        /// <param name="z">The new Z coordinate of the position</param>
        public void SetPosition3D(float x, float y, float z)
        {
            mPosition.X = x;
            mPosition.Y = y;
            mPosition.Z = z;
        }

        /// <summary>
        /// Sets the X, Y, and Z coordinates of the position.
        /// </summary>
        /// <param name="position">Vector3 containing the new X, Y, and Z coordinates for the position</param>
        public void SetPosition3D(Vector3 position)
        {
            SetPosition3D(position.X, position.Y, position.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {

            // When running with world editor, allow the player to move objects with the mouse while paused
#if WORLD_EDITOR
            if (mParentEntity.ActiveForEditing == true)
            {
                if (InputManager_cl.Instance.MouseState.LeftButton == ButtonState.Pressed && InputManager_cl.Instance.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (InputManager_cl.Instance.IsKeyDown(Keys.W) || InputManager_cl.Instance.IsKeyDown(Keys.S))
                    {
                        if (Game_cl.BaseInstance.WorldEditor.DepthSnappingOn() == true)
                        {
                            int nearestSnap = (int)(mPosition.Z / PhysicsComponent_cl.mSnappingResolution) * PhysicsComponent_cl.mSnappingResolution;
                            if (InputManager_cl.Instance.WasKeyPressedThisFrame(Keys.S))
                            {
                                SetPosition3D(new Vector3(mPosition.X, mPosition.Y, nearestSnap - PhysicsComponent_cl.mSnappingResolution));
                            }
                            else if (InputManager_cl.Instance.WasKeyPressedThisFrame(Keys.W))
                            {
                                SetPosition3D(new Vector3(mPosition.X, mPosition.Y, nearestSnap + PhysicsComponent_cl.mSnappingResolution));
                            }
                        }
                        else
                        {
                            if (InputManager_cl.Instance.IsKeyDown(Keys.S))
                            {
                                SetPosition3D(new Vector3(mPosition.X, mPosition.Y, mPosition.Z - PhysicsComponent_cl.mDepthMoveSpeed * Game_cl.BaseInstance.Timer.ElapsedSeconds));
                            }
                            else if (InputManager_cl.Instance.IsKeyDown(Keys.W))
                            {
                                SetPosition3D(new Vector3(mPosition.X, mPosition.Y, mPosition.Z + PhysicsComponent_cl.mDepthMoveSpeed * Game_cl.BaseInstance.Timer.ElapsedSeconds));
                            }
                        }
                    }
                    if (InputManager_cl.Instance.IsKeyDown(Keys.A) || InputManager_cl.Instance.IsKeyDown(Keys.D))
                    {
                        if (InputManager_cl.Instance.IsKeyDown(Keys.A))
                        {
                            mRotation -= 0.01f;
                        }
                        else if (InputManager_cl.Instance.IsKeyDown(Keys.D))
                        {
                            mRotation += 0.01f;
                        }

                        PhysicsComponent_cl physicsComponent = (PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl));
                        if (physicsComponent != null)
                        {
                            physicsComponent.SetRotation(-mRotation);
                        }
                    }
                    else
                    {
                        /************************************************************************
                         * HACK:
                         * We want objects in the distance to smoothly follow mouse movement, so we
                         * must modify the mouse world delta according to the object's depth.
                         *
                         * Larsson Burch - 2011/11/29 - 18:16
                         ************************************************************************/
                        // Get the mouse screen position
                        Vector2 mouseFNAScreenPosition = InputManager_cl.Instance.GetFNAMouseScreenPosition();

                        Vector3 finalPosition = ((SunburnCameraComponent_cl)CameraManager_cl.Instance.ActiveCamera).GetWorldPosition(mouseFNAScreenPosition/* + EditingManager_cl.Instance.ActiveEntityClickOffset*/, mPosition.Z);

                        PhysicsComponent_cl physicsComponent = (PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl));
                        if (physicsComponent != null)
                        {
                            physicsComponent.SetPosition(finalPosition);
                        }
                        else
                        {
                            mPosition = finalPosition;
                        }
                    }
                }
            }
#endif
        }
    }
}
