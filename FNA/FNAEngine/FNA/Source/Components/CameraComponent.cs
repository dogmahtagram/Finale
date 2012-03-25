using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using FNA.Core;

namespace FNA.Components
{
    /// <summary>
    /// The CameraComponent class enables an Entity to serve as a camera within FNA.
    /// Each FNA application should extend this class to fit its needs.
    /// </summary>
    public class CameraComponent_cl : Component
    {
#region Variables, Structs, Enumerations, and Type Definitions
        /// <summary>
        /// The far plane of this Camera (in world units).
        /// </summary>
        protected float mFarPlane;
        
        /// <summary>
        /// 
        /// </summary>
        public float FarPlane
        {
            get
            {
                return mFarPlane;
            }
            set
            {
                mFarPlane = value;
            }
        }

        /// <summary>
        /// The position (in world space) of this Camera.
        /// </summary>
        /// <note>Actually references the base class's PositionComponent</note>
        public virtual Vector3 Position
        {
            get
            {
                PositionComponent component = (PositionComponent)mParentEntity.GetComponentOfType(typeof(PositionComponent));
                return component.Position3D;
            }
        }
        /// <summary>
        /// The 2D position (in world space) of this Camera.
        /// </summary>
        public Vector2 Position2D
        {
            get
            {
                PositionComponent component = (PositionComponent)mParentEntity.GetComponentOfType(typeof(PositionComponent));
                return component.Position2D;
            }
        }

        /// <summary>
        /// Projects the game world into screen coordinates.
        /// </summary>
        protected Matrix mProjectionMatrix;
        
        /// <summary>
        /// Holds the translation that objects must undergo to make the camera appear as if it is moving.
        /// </summary>
        protected Matrix mTranslationMatrix;
        
        /// <summary>
        /// Rotates the game world.
        /// </summary>
        protected Matrix mRotationMatrix;
        
        /// <summary>
        /// Scales the game world (through zooming)
        /// </summary>
        protected Matrix mScaleMatrix;

        /// <summary>
        /// Holds any additional transformation that needs to be applied to the items in the scene (i.e., you do not want objects at the same position of the camera to be centered on the screen)
        /// </summary>
        protected Matrix mScreenOffsetMatrix;

        /// <summary>
        /// The current zoom setting of this camera.
        /// </summary>
        protected float mZoom;
        
        /// <summary>
        /// 
        /// </summary>
        public float Zoom
        {
            get
            {
                return mZoom;
            }
            set
            {
                mZoom = value;
            }
        }

        /// <summary>
        /// The final matrix which performs all necessary operations (translation, rotation, scaling, etc) to get the points
        /// from world space to screen space
        /// </summary>
        protected Matrix mTransformMatrix;
        
        /// <summary>
        /// 
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                return mTransformMatrix;
            }
        }

        /// <summary>
        /// The rotation of this camera (along the Y axis) in degrees
        /// </summary>
        private float mRotation;
        
        /// <summary>
        /// 
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
#endregion

#region Functions
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        /// <note>The parent Entity MUST have a PositionComponent in order to construct this Component.</note>
        public CameraComponent_cl(Entity_cl parent) : base(parent)
        {
            // Ensure that the parent has a PositionComponent attached to it...otherwise assert
            Debug.Assert(mParentEntity.GetComponentOfType(typeof(PositionComponent)) != null, "CameraComponents can only be added to Entities with PositionComponents!");
            /************************************************************************
             * TODO:
             * Cache off and store this position component?  Is that kosher?
             *
             * Jerad Dunn - 2011/11/05 - 19:20
             ************************************************************************/
            
            // Create the projection matrix for this camera
            mProjectionMatrix = Matrix.Identity * 
                                Matrix.CreateTranslation(-0.5f, -0.5f, 0) *         // Half-pixel offset
                                Matrix.CreateOrthographicOffCenter(0, Game.BaseInstance.WindowWidth, Game.BaseInstance.WindowHeight, 0, 0, 1);

            // By default, there is no additional offset
            mScreenOffsetMatrix = Matrix.Identity;

            mParentEntity.AddComponent(this);
            mZoom = 1.0f;
            mRotation = 0.0f;

            RecalculateTransformationMatrix();
        }

        /// <summary>
        /// Update function for this CameraComponent.
        /// Specific implementations may need to override this functionality.
        /// </summary>
        public virtual void Update()
        {
            
        }

        /// <summary>
        /// Recalculates the transformation matrix that results from the CameraComponent's member variables.
        /// </summary>
        public void RecalculateTransformationMatrix()
        {
            PositionComponent component = (PositionComponent)mParentEntity.GetComponentOfType(typeof(PositionComponent));
            Vector3 position = component.Position3D;

            /************************************************************************
             * TODO:
             * Think of a better name for squeetres
             *
             * Jerad Dunn - 2011/09/27 - 19:21
             ************************************************************************/
            position.X *= -(float)Game.BaseInstance.SqueetrePixelWidth;
            position.Y *= -(float)Game.BaseInstance.SqueetrePixelHeight;
            
            mTranslationMatrix = Matrix.CreateTranslation(position);
            mRotationMatrix = Matrix.CreateRotationZ(mRotation);
            mScaleMatrix = Matrix.CreateScale(new Vector3(mZoom, mZoom, 1.0f));

            mTransformMatrix = mTranslationMatrix * mRotationMatrix * mScaleMatrix * mScreenOffsetMatrix * mProjectionMatrix;
        }

        /// <summary>
        /// Performs all of the relevant operations to convert the provided point from world space to screen space.
        /// </summary>
        /// <param name="pos">The point (in world space coordinates) to convert</param>
        /// <returns>The transformed point (in screen coordinates)</returns>
        public Vector2 ConvertToPixelSpace(Vector3 pos)
        {
            Vector2 result = Vector2.Zero;

            /************************************************************************
             * TODO:
             * Think of a better name for squeetres
             *
             * Jerad Dunn - 2011/09/27 - 19:21
             ************************************************************************/
            pos.X *= (float)Game.BaseInstance.SqueetrePixelWidth;
            pos.Y *= (float)Game.BaseInstance.SqueetrePixelHeight;

            Vector4 temp = Vector4.Transform(pos, mTranslationMatrix * mRotationMatrix * mScaleMatrix * mScreenOffsetMatrix);
            result.X = temp.X;
            result.Y = temp.Y;

            return result;
        }

        /// <summary>
        /// Performs all of the relevant operations to convert the provided point from screen space to world space.
        /// </summary>
        /// <param name="pos">The point (in screen space coordinates) to convert</param>
        /// <returns>The transformed point (in world coordinates)</returns>
        public Vector3 ConvertToWorldCoordinates(Vector2 pos)
        {
            Vector3 result = ((PositionComponent)mParentEntity.GetComponentOfType(typeof(PositionComponent))).Position3D;

            result.Z = 0.0f;

            pos.X /= (float)Game.BaseInstance.SqueetrePixelWidth;
            pos.Y /= (float)Game.BaseInstance.SqueetrePixelHeight;

            pos.X -= (float)Game.BaseInstance.WindowWidth / (float)Game.BaseInstance.SqueetrePixelWidth / 2.0f;
            pos.Y -= (float)Game.BaseInstance.WindowHeight / (float)Game.BaseInstance.SqueetrePixelHeight;

            result.X += pos.X;
            result.Y += pos.Y;

            return result;
        }
#endregion
    }
}
