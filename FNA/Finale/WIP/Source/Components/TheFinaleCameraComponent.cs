using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FNA;
using FNA.Components;
using FNA.Components.Cameras;
using FNA.Core;

using TheFinale;

namespace TheFinale.Components
{
    public class TheFinaleCameraComponent_cl : SunburnCameraComponent_cl
    {
        // This needs to match the offset in Maya
        private static Vector3 CAMERA_OFFSET = new Vector3(0.0f, 0.0f, 18.344f);

        /// <summary>
        /// A reference to the parent Entity's PositionComponent. REQUIRED
        /// </summary>
        private PositionComponent_cl mPositionComponent;

        /// <summary>
        /// The movement speed of the camera when WASD is pressed.
        /// </summary>
        private float mMovementSpeed = 10.0f;
        
        /// <summary>
        /// A reference to the parent Entity's InputComponent. REQUIRED
        /// </summary>
        private InputComponent_cl mInputComponent;

        /// <summary>
        /// Utility accessor to make it easier to get the exact world position of the TheFinale's CameraComponent.
        /// The 'new' keyword is to hide the implementation of the default CameraComponent's Position accessor
        /// </summary>
        public override Vector3 Position
        {
            get
            {
                return mPositionComponent.Position3D/* + CAMERA_OFFSET*/;
            }
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        /// <note>The parent Entity MUST have a PositionComponent in order to construct this Component.</note>
        /// <note>The parent Entity MUST have an InputComponent in order to construct this Component.</note>
        public TheFinaleCameraComponent_cl(Entity_cl parent) : base(parent)
        {
            // Asserting here to ensure that the TheFinaleCameraComponent has all the required Components
            // These will be compiled out of the Release build
            Debug.Assert(mParentEntity.GetComponentOfType(typeof(PositionComponent_cl)) != null, "TheFinaleCameraComponent: No PositionComponent exists on parent Entity!");
            Debug.Assert(mParentEntity.GetComponentOfType(typeof(InputComponent_cl)) != null, "TheFinaleCameraComponent: No InputComponent exists on parent Entity!");

            mPositionComponent = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            mInputComponent = (InputComponent_cl)mParentEntity.GetComponentOfType(typeof(InputComponent_cl));

            mInputComponent.AddKey("moveN", Keys.Up);
            mInputComponent.AddKey("moveW", Keys.Left);
            mInputComponent.AddKey("moveS", Keys.Down);
            mInputComponent.AddKey("moveE", Keys.Right);

            mInputComponent.AddKey("ShakeIt", Keys.N);
            

            // Items at the same position as the camera should appear at the bottom of the screen, horizontally centered
            mScreenOffsetMatrix = Matrix.CreateTranslation(new Vector3((float)Game_cl.BaseInstance.WindowWidth * 0.5f, (float)Game_cl.BaseInstance.WindowHeight, 0));

            RecalculateTransformationMatrix();
        }

        /// <summary>
        /// Update function for this CameraComponent.
        /// Specific implementations may need to override this functionality.
        /// </summary>
        public override void Update()
        {
#if WORLD_EDITOR
            if (!Game_cl.BaseInstance.WorldEditor.DialogOpen)
            {
#endif
                Vector3 displacement = Vector3.Zero;

                if (mInputComponent.IsKeyDown("moveN"))
                {
                    displacement += new Vector3(0, 1.0f, 0);
                }
                if (mInputComponent.IsKeyDown("moveE"))
                {
                    displacement += new Vector3(1.0f, 0, 0);
                }
                if (mInputComponent.IsKeyDown("moveS"))
                {
                    displacement += new Vector3(0, -1.0f, 0);
                }
                if (mInputComponent.IsKeyDown("moveW"))
                {
                    displacement += new Vector3(-1.0f, 0, 0);
                }





                if (mInputComponent.IsKeyDown("ShakeIt"))
                {
                    Shake(0.2f, 0.5f);
                }






                if (displacement.LengthSquared() != 0)
                {
                    displacement.Normalize();
                    displacement *= mMovementSpeed * Game_cl.BaseInstance.Timer.ElapsedSeconds;
                }

                mPositionComponent.SetPosition3D(mPositionComponent.Position3D + displacement);

                RecalculateTransformationMatrix();
#if WORLD_EDITOR
            }
#endif

            base.Update();
        }
    }
}
