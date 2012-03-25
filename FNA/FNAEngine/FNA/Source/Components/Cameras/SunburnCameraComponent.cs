using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using SynapseGaming.LightingSystem.Core;

using FNA.Core;
using FNA.Managers;
using Microsoft.Xna.Framework.Input;

namespace FNA.Components.Cameras
{
    /// <summary>
    /// An child of the FNA camera component which contains a view width property to be used with Sunburn's rendering calls.
    /// </summary>
    public class SunburnCameraComponent_cl : CameraComponent_cl, FNA.Interfaces.IUpdateAble
    {

        /* Camera Shake Stuff */
        private static readonly Random random = new Random();
        private bool shaking;
        private float shakeMagnitude;
        private float shakeDuration;
        private float shakeTimer;
        private Vector3 shakeOffset;

        InputComponent_cl mInputComponent;
       
        private float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="magnitude"></param>
        /// <param name="duration"></param>
        public void Shake(float magnitude, float duration)
        {
            shaking = true;
            shakeMagnitude = magnitude;
            shakeDuration = duration;
            shakeTimer = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool focusOnPlayer = false;

       // private PositionComponent_cl mFocusPositionComponent;
        private PositionComponent_cl mPlayerPosition;
        private Vector3 focus;
        private float MoveSpeed = .5f;


        /// <summary>
        /// The width of the camera's viewing are in FNA units.
        /// </summary>
        private float mViewWidth;

        /// <summary>
        /// Gets the camera view's width in FNA units.
        /// </summary>
        public float ViewWidth
        {
            get
            {
                return mViewWidth;
            }
            set
            {
                mViewWidth = Math.Max(value, 0.1f);
                mPixelToFNARatio = mViewWidth / FNA.Game_cl.BaseInstance.WindowWidth;
            }
        }

        private float mPixelToFNARatio;

        /// <summary>
        /// Get the current ratio to convert screen pixels into FNA units with the current camera.
        /// </summary>
        public float PixelToFNARatio
        {
            get
            {
                return mPixelToFNARatio;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private float mZoomTimer = 0.0f;
        
        /// <summary>
        /// 
        /// </summary>
        private float mZoomChange = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        private const float ZOOM_TIME_THRESHOLD = 0.5f;

        /// <summary>
        /// Constructor taking the component's parent entity.
        /// </summary>
        /// <param name="parent">the parent entity that this component is attached to.</param>
        public SunburnCameraComponent_cl(Entity_cl parent) : base(parent)
        {
        }

/// <summary>
/// 
/// </summary>
/// <param name="playerPosition"></param>
        public void FocusOnPlayer(PositionComponent_cl playerPosition)
        {
            mPlayerPosition = playerPosition;
            focusOnPlayer = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            /************************************************************************
             * HACK:
             * Testing dynamic zooming with some quick controls
             *
             * Larsson Burch - 2011/11/11 - 17:01
             ************************************************************************/
            float lastScrollWheelValue = FNA.Managers.InputManager_cl.Instance.LastMouseState.ScrollWheelValue;
            float thisScrollWheelValue = FNA.Managers.InputManager_cl.Instance.MouseState.ScrollWheelValue;

            PositionComponent_cl position = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            mInputComponent = (InputComponent_cl)mParentEntity.GetComponentOfType(typeof(InputComponent_cl));

            
 

            mZoomTimer += FNA.Game_cl.BaseInstance.Timer.ElapsedSeconds;
            if (mZoomTimer > ZOOM_TIME_THRESHOLD)
            {
                mZoomChange = 1.0f;
            }
            else
            {
                mZoomChange += 0.1f;
            }

            if (thisScrollWheelValue != lastScrollWheelValue)
            {
                mZoomTimer = 0.0f;

               

                if(thisScrollWheelValue < lastScrollWheelValue)
                {
                    position.SetPosition3D(position.Position3D + new Microsoft.Xna.Framework.Vector3(0, 0, -mZoomChange));
                }
                else
                {
                    position.SetPosition3D(position.Position3D + new Microsoft.Xna.Framework.Vector3(0, 0, mZoomChange));
                }
            }




            var delta = (float)FNA.Game_cl.BaseInstance.Timer.ElapsedSeconds;

            if (focusOnPlayer && FNA.Game_cl.BaseInstance.Timer.Paused == false)
            {
             
                focus.X = (mPlayerPosition.Position3D.X - position.Position3D.X) * MoveSpeed * delta;
                focus.Y = (mPlayerPosition.Position3D.Y - position.Position3D.Y) * MoveSpeed * delta;
                //focus.Z = position.Position3D.Z;

                position.SetPosition3D(position.Position3D+focus);

            }


            /* Camera Shake Update */
            if (shaking)
            {
                shakeTimer += (float)FNA.Game_cl.BaseInstance.Timer.ActualTime.ElapsedGameTime.TotalSeconds;

                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                float progress = shakeTimer / shakeDuration;

                float magnitude = shakeMagnitude * (1f - (progress * progress));

                shakeOffset = new Vector3(NextFloat(), NextFloat(), 0) * magnitude;

                position.Position3D += shakeOffset;
                

            }


            base.Update();
        }

        /// <summary>
        /// Gets the screen position in FNA units that corresponds to the specified world position.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 GetScreenPosition(Vector3 worldPosition)
        {
            Vector2 screenPosition = new Vector2();

            // Get information about how wide the near and far planes are.
            SceneState state = FNA.Game_cl.BaseInstance.SceneState;
            Vector3[] frustumCorners = new Vector3[8];
            state.ViewFrustum.GetCorners(frustumCorners);
            float nearPlaneHalfWidth = (frustumCorners[0].X - frustumCorners[1].X) / 2;
            float nearPlaneHalfHeight = (frustumCorners[1].Y - frustumCorners[2].Y) / 2;

            // Because Sunburn likes x to be flipped, we have to do different operations for x and y here.
            worldPosition.X += CameraManager_cl.Instance.ActiveCamera.Position.X;
            worldPosition.Y -= CameraManager_cl.Instance.ActiveCamera.Position.Y;
            worldPosition.Z -= CameraManager_cl.Instance.ActiveCamera.Position.Z;

            // The two magic numbers here are the slopes of the view frustum sides on x and y.
            // This gets us the half width and half height of the view plane at the depth of the current entity.
            float currentDepthHalfWidth = 0.2f * worldPosition.Z + nearPlaneHalfWidth;
            float currentDepthHalfHeight = 0.125f * worldPosition.Z + nearPlaneHalfHeight;

            // Use the ratios of the current depth size to near plane size to figure out the near plane position of the current entity.
            screenPosition = new Vector2(worldPosition.X * nearPlaneHalfWidth / currentDepthHalfWidth, worldPosition.Y * nearPlaneHalfHeight / currentDepthHalfHeight);

            return screenPosition;
        }

        /// <summary>
        /// Gets the world position that corresponds to an FNA screen position and depth.
        /// </summary>
        /// <param name="screenPosition">The position on the screen relative to the center of the near plane.</param>
        /// <param name="depth">The depth at which we want to find the corresponding world position.</param>
        /// <returns>A vector representing the world poition that corresponds to the mouse position and queried depth.</returns>
        public Vector3 GetWorldPosition(Vector2 screenPosition, float depth)
        {
            Vector3 worldPosition = new Vector3();

            // Get information about how wide the near and far planes are.
            SceneState state = FNA.Game_cl.BaseInstance.SceneState;
            Vector3[] frustumCorners = new Vector3[8];
            state.ViewFrustum.GetCorners(frustumCorners);
            float nearPlaneHalfWidth = (frustumCorners[0].X - frustumCorners[1].X) / 2;
            float nearPlaneHalfHeight = (frustumCorners[1].Y - frustumCorners[2].Y) / 2;

            // The two magic numbers here are the slopes of the view frustum sides on x and y.
            // This gets us the half width and half height of the view plane at the depth of the current entity.
            float currentDepthHalfWidth = 0.2f * (depth - CameraManager_cl.Instance.ActiveCamera.Position.Z) + nearPlaneHalfWidth;
            float currentDepthHalfHeight = 0.125f * (depth - CameraManager_cl.Instance.ActiveCamera.Position.Z) + nearPlaneHalfHeight;

            worldPosition.X = screenPosition.X * currentDepthHalfWidth / nearPlaneHalfWidth;
            worldPosition.Y = screenPosition.Y * currentDepthHalfHeight / nearPlaneHalfHeight;
            worldPosition.Z = depth;

            worldPosition.X += CameraManager_cl.Instance.ActiveCamera.Position.X;
            worldPosition.Y += CameraManager_cl.Instance.ActiveCamera.Position.Y;
            //worldPosition.Z += CameraManager_cl.Instance.ActiveCamera.Position.Z;

            return worldPosition;
        }



    }
}
