// The editing manager exist only when you build in Debug wEditor mode.
#if WORLD_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Xna.Framework;

using FNA.Components;
using FNA.Components.Cameras;
using FNA.Graphics;
using FNA.Interfaces;
using FNA.Core;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Effects;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Rendering;

namespace FNA.Managers
{
    /// <summary>
    /// The editing manager controls the editing of entities and components within the game.
    /// </summary>
    public sealed class EditingManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The current entity that is active for editing.
        /// </summary>
        private Entity_cl mActiveEntity;

        /************************************************************************
         * HACK:
         * Figure out a better way to pass around information about the active entity.
         * This was done so that the physics component could query the screen position
         * of an entity to determine user inputted forces.
         *
         * Larsson Burch - 2011/11/29 - 18:36
         ************************************************************************/
        /// <summary>
        /// The screen position of the active entity in FNA units.
        /// </summary>
        private Vector2 mActiveEntityClickOffset;

        /// <summary>
        /// Gets the screen position of the active entity.
        /// </summary>
        public Vector2 ActiveEntityClickOffset
        {
            get
            {
                return mActiveEntityClickOffset;
            }
        }

        /// <summary>
        /// Whether editing is currently enabled in FNA.
        /// </summary>
        private bool mEditingEnabled;

        /// <summary>
        /// Sets whether editing is enabled for FNA.
        /// </summary>
        public bool EditingEnabled
        {
            set
            {
                mEditingEnabled = value;

                // If we're turning editing off, make sure that we unset the active entity.
                if (mEditingEnabled == false && mActiveEntity != null)
                {
                    mActiveEntity.ActiveForEditing = false;
                    mActiveEntity = null;
                }
            }
        }

        /// <summary>
        /// The static instance of this class.
        /// </summary>
        private static readonly EditingManager_cl mInstance = new EditingManager_cl();

        /// <summary>
        /// Accessor for the static instance.
        /// </summary>
        public static EditingManager_cl Instance
        {
            get
            {
                return mInstance;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private EditingManager_cl()
        {
        }

        /// <summary>
        /// Watch the mouse and determine whether we need to change the active entity.
        /// </summary>
        public override void Update()
        {
            if (mEditingEnabled && (Game_cl.BaseInstance.WorldEditor.DialogOpen == false))
            {
                // If the mouse is clicked this frame, try and set the active entity according to the mouse position.
                if (InputManager_cl.Instance.WasLeftMouseClicked())
                {
                   SelectEntityAtMouse();
                   Game_cl.BaseInstance.WorldEditor.SelectObject(mActiveEntity);
                }
                else if (InputManager_cl.Instance.WasRightMouseClicked())
                {
                    SelectEntityAtMouse();
                    if (mActiveEntity != null)
                    {
                        Game_cl.BaseInstance.WorldEditor.ViewObjectProperties(mActiveEntity);
                    }
                }
            }
        }

        /// <summary>
        /// If an entity is found at the position the mouse is clicked, it gets set as active for editing.
        /// </summary>
        public void SelectEntityAtMouse()
        {
            // Get the mouse screen position
            Vector2 mouseFNAScreenPosition = InputManager_cl.Instance.GetFNAMouseScreenPosition();

            if (Game_cl.BaseInstance.WorldEditor.MouseInEditorPanel(mouseFNAScreenPosition) == false)
            {
                return;
            }

            // Deactivate the previously active entity.
            if (mActiveEntity != null)
            {
                //mActiveEntity.ActiveForEditing = false;
            }

            // Remove the reference to any selected Entity
            mActiveEntity = null;

            // Keep track of the position of the found entity so that we can compare depth and pick the closest one.
            PositionComponent_cl foundEntityPosition = null;

            // Get information about how wide the near and far planes are.
            SceneState state = Game_cl.BaseInstance.SceneState;
            Vector3[] frustumCorners = new Vector3[8];
            state.ViewFrustum.GetCorners(frustumCorners);
            float nearPlaneHalfWidth = (frustumCorners[0].X - frustumCorners[1].X) / 2;
            float nearPlaneHalfHeight = (frustumCorners[1].Y - frustumCorners[2].Y) / 2;

            // Go through all the scene entities and try to select one.
            foreach (Entity_cl sceneEntity in Game_cl.BaseInstance.Scene.Entities)
            {
                // Get the position component of the current entity and the position relative to the camera.
                PositionComponent_cl sceneEntityPosition = (PositionComponent_cl)sceneEntity.GetComponentOfType(typeof(PositionComponent_cl));
                Vector3 entityViewedPosition = sceneEntityPosition.Position3D;

                /************************************************************************
                 * HACK:
                 * 
                 *
                 * Larsson Burch - 2011/11/29 - 20:36
                 ************************************************************************/
                InputManager_cl.Instance.GetMouseWorldPosition(entityViewedPosition.Z);

                // Because Sunburn likes x to be flipped, we have to do different operations for x and y here.
                entityViewedPosition.X -= CameraManager_cl.Instance.ActiveCamera.Position.X;
                entityViewedPosition.Y -= CameraManager_cl.Instance.ActiveCamera.Position.Y;
                entityViewedPosition.Z -= CameraManager_cl.Instance.ActiveCamera.Position.Z;

                // The two magic numbers here are the slopes of the view frustum sides on x and y.
                // This gets us the half width and half height of the view plane at the depth of the current entity.
                float currentDepthHalfWidth = 0.2f * entityViewedPosition.Z + nearPlaneHalfWidth;
                float currentDepthHalfHeight = 0.125f * entityViewedPosition.Z + nearPlaneHalfHeight;

                RenderableComponent_cl renderable = (RenderableComponent_cl)sceneEntity.GetComponentOfType(typeof(RenderableComponent_cl));

                // We must get the clickable width and height of the current entity based on its sprite size.
                // This corrects for the object being smaller when at a greater depth.
                float clickableWidth = renderable.Sprite.Size.X / 2 * nearPlaneHalfWidth / currentDepthHalfWidth;
                float clickableHeight = renderable.Sprite.Size.Y / 2 * nearPlaneHalfHeight / currentDepthHalfHeight;

                // Use the ratios of the current depth size to near plane size to figure out the near plane position of the current entity.
                Vector2 entityScreenPosition = new Vector2(entityViewedPosition.X * nearPlaneHalfWidth / currentDepthHalfWidth, entityViewedPosition.Y * nearPlaneHalfHeight / currentDepthHalfHeight);

                // clickOffset is the near plane distance we are clicking from the entity's position.
                Vector2 clickOffset = entityScreenPosition - mouseFNAScreenPosition;

                // If we are clicking within the bounds of the clickable area for this entity.
                if (Math.Abs(clickOffset.X) < clickableWidth && Math.Abs(clickOffset.Y) < clickableHeight)
                {
                    if (foundEntityPosition != null)
                    {
                        if (sceneEntityPosition.Position3D.Z < foundEntityPosition.Position3D.Z && sceneEntityPosition.Position3D.Z >= CameraManager_cl.Instance.ActiveCamera.Position.Z)
                        {
                            // Deactivate the last active entity.
                            //mActiveEntity.ActiveForEditing = false;

                            // Set the new active entity.
                            mActiveEntity = sceneEntity;
                            foundEntityPosition = (PositionComponent_cl)sceneEntity.GetComponentOfType(typeof(PositionComponent_cl));
                            //mActiveEntity.ActiveForEditing = true;
                            mActiveEntityClickOffset = clickOffset;
                        }
                    }
                    else if(sceneEntityPosition.Position3D.Z >= CameraManager_cl.Instance.ActiveCamera.Position.Z)
                    {
                        // Set the new active entity.
                        mActiveEntity = sceneEntity;
                        foundEntityPosition = (PositionComponent_cl)sceneEntity.GetComponentOfType(typeof(PositionComponent_cl));
                        //mActiveEntity.ActiveForEditing = true;
                        mActiveEntityClickOffset = clickOffset;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableActiveEntity()
        {
            if (mActiveEntity != null)
            {
                //mActiveEntity.ActiveForEditing = false;
                mActiveEntity = null;
            }
        }
    }
}
#endif
