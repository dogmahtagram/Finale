using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FNA.Components;

namespace FNA.WorldEditor
{
    public class WorldEditorCameraComponent : CameraComponent
    {
        // This needs to match the offset in Maya
        private static Vector3 CAMERA_OFFSET = new Vector3(0.0f, 0.0f, 18.344f);

        /// <summary>
        /// A reference to the parent Entity's PositionComponent. REQUIRED
        /// </summary>
        private PositionComponent mPositionComponent;
        
        /// <summary>
        /// A reference to the parent Entity's InputComponent. REQUIRED
        /// </summary>
        private InputComponent mInputComponent;

        /// <summary>
        /// Utility accessor to make it easier to get the exact world position of the WorldEditor's CameraComponent.
        /// The 'new' keyword is to hide the implementation of the default CameraComponent's Position accessor
        /// </summary>
        public override Vector3 Position
        {
            get
            {
                return mPositionComponent.Position3D + CAMERA_OFFSET;
            }
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        /// <note>The parent Entity MUST have a PositionComponent in order to construct this Component.</note>
        /// <note>The parent Entity MUST have an InputComponent in order to construct this Component.</note>
        public WorldEditorCameraComponent(Entity parent) : base(parent)
        {
            // Asserting here to ensure that the WorldEditorCameraComponent has all the required Components
            // These will be compiled out of the Release build
            Debug.Assert(mParentEntity.GetComponentOfType("Position") != null, "WorldEditorCameraComponent: No PositionComponent exists on parent Entity!");
            Debug.Assert(mParentEntity.GetComponentOfType("Input") != null, "WorldEditorCameraComponent: No InputComponent exists on parent Entity!");

            mPositionComponent = (PositionComponent)mParentEntity.GetComponentOfType("Position");
            mInputComponent = (InputComponent)mParentEntity.GetComponentOfType("Input");

            mInputComponent.AddKey("moveN", Keys.W);
            mInputComponent.AddKey("moveW", Keys.A);
            mInputComponent.AddKey("moveS", Keys.S);
            mInputComponent.AddKey("moveE", Keys.D);

            // Items at the same position as the camera should appear at the bottom of the screen, horizontally centered
            mScreenOffsetMatrix = Matrix.CreateTranslation(new Vector3((float)Game.BaseInstance.WindowWidth * 0.5f, (float)Game.BaseInstance.WindowHeight, 0));

            RecalculateTransformationMatrix();
        }

        /// <summary>
        /// Update function for this CameraComponent.
        /// Specific implementations may need to override this functionality.
        /// </summary>
        public override void Update()
        {
            Vector3 currentPos = mPositionComponent.Position3D;

            if (mInputComponent.IsKeyDown("moveN"))
            {
                mPositionComponent.SetPosition3D(currentPos + new Vector3(0.0f, -0.05f, 0.0f));
            }
            if (mInputComponent.IsKeyDown("moveE"))
            {
                mPositionComponent.SetPosition3D(currentPos + new Vector3(0.1f, 0.0f, 0.0f));
            }
            if (mInputComponent.IsKeyDown("moveS"))
            {
                mPositionComponent.SetPosition3D(currentPos + new Vector3(0.0f, 0.05f, 0.0f));
            }
            if (mInputComponent.IsKeyDown("moveW"))
            {
                mPositionComponent.SetPosition3D(currentPos + new Vector3(-0.1f, 0.0f, 0.0f));
            }

            RecalculateTransformationMatrix();
        }
    }
}
