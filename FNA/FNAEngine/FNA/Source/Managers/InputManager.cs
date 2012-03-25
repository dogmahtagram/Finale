using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SynapseGaming.LightingSystem.Core;

using FNA.Components;
using FNA.Components.Cameras;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class InputManager_cl : BaseManager_cl
    {
        private static readonly InputManager_cl sInstance = new InputManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static InputManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        List<InputComponent_cl> mActiveComponents = new List<InputComponent_cl>();

        private Dictionary<PlayerIndex, GamePadState> mGamePadStates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GamePadState GetGamePadState(PlayerIndex index)
        {
            return mGamePadStates[index];
        }

        private Dictionary<PlayerIndex, GamePadState> mLastGamePadStates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GamePadState GetLastGamePadState(PlayerIndex index)
        {
            return mLastGamePadStates[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        public void SetGamePadState(PlayerIndex index, GamePadState state)
        {
            mGamePadStates[index] = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        public void SetLastGamePadState(PlayerIndex index, GamePadState state)
        {
            mGamePadStates[index] = state;
        }

        private KeyboardState mKeyState;

        /// <summary>
        /// 
        /// </summary>
        public KeyboardState KeyState
        {
            get
            {
                return mKeyState;
            }
        }

        private KeyboardState mLastKeyState;

        /// <summary>
        /// 
        /// </summary>
        public KeyboardState LastKeyState
        {
            get
            {
                return mLastKeyState;
            }
        }

        private MouseState mMouseState;

        /// <summary>
        /// 
        /// </summary>
        public MouseState MouseState
        {
            get
            {
                return mMouseState;
            }
        }

        private MouseState mLastMouseState;

        /// <summary>
        /// 
        /// </summary>
        public MouseState LastMouseState
        {
            get
            {
                return mLastMouseState;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public InputManager_cl()
        {
            mGamePadStates = new Dictionary<PlayerIndex, GamePadState>();
            mLastGamePadStates = new Dictionary<PlayerIndex, GamePadState>();

            mGamePadStates.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular));
            mGamePadStates.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.Circular));
            mGamePadStates.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three, GamePadDeadZone.Circular));
            mGamePadStates.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four, GamePadDeadZone.Circular));

            mLastGamePadStates.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular));
            mLastGamePadStates.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.Circular));
            mLastGamePadStates.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three, GamePadDeadZone.Circular));
            mLastGamePadStates.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four, GamePadDeadZone.Circular));

            mKeyState = Keyboard.GetState();
            mMouseState = Mouse.GetState();

            mLastKeyState = mKeyState;
            mLastMouseState = mMouseState;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            mLastKeyState = mKeyState;
            mLastMouseState = mMouseState;
            mLastGamePadStates[PlayerIndex.One] = mGamePadStates[PlayerIndex.One];
            mLastGamePadStates[PlayerIndex.Two] = mGamePadStates[PlayerIndex.Two];
            mLastGamePadStates[PlayerIndex.Three] = mGamePadStates[PlayerIndex.Three];
            mLastGamePadStates[PlayerIndex.Four] = mGamePadStates[PlayerIndex.Four];

            mKeyState = Keyboard.GetState();
            mMouseState = Mouse.GetState();
            mGamePadStates[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
            mGamePadStates[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.Circular);
            mGamePadStates[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three, GamePadDeadZone.Circular);
            mGamePadStates[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four, GamePadDeadZone.Circular);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        }

        /// <summary>
        /// Check if key was just pressed.
        /// </summary>
        public bool WasKeyPressed(Keys key)
        {
            if (mKeyState.IsKeyDown(key) && mLastKeyState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WasLeftMouseClicked()
        {
            if ((mMouseState.LeftButton == ButtonState.Pressed) &&
                (mLastMouseState.LeftButton == ButtonState.Released))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WasRightMouseClicked()
        {
            if ((mMouseState.RightButton == ButtonState.Pressed) &&
                (mLastMouseState.RightButton == ButtonState.Released))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the delta of the mouse's position since the last frame in FNA units.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMouseWorldDelta()
        {
            float pixelToFNARatio = ((SunburnCameraComponent_cl)CameraManager_cl.Instance.ActiveCamera).PixelToFNARatio;
            Vector2 delta = new Vector2(mMouseState.X - mLastMouseState.X, -(mMouseState.Y - mLastMouseState.Y));
            delta *= pixelToFNARatio;
            return delta;
        }

        /// <summary>
        /// Gets the position of the mouse relative to the center of the near plane in FNA units.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetFNAMouseScreenPosition()
        {
            // Get information about how wide the near and far planes are.
            SceneState state = Game_cl.BaseInstance.SceneState;
            Vector3[] frustumCorners = new Vector3[8];
            state.ViewFrustum.GetCorners(frustumCorners);
            float nearPlaneHalfWidth = (frustumCorners[0].X - frustumCorners[1].X) / 2;
            float nearPlaneHalfHeight = (frustumCorners[1].Y - frustumCorners[2].Y) / 2;

            // Get the mouse position relative to the center of the screen - in pixels.
            Vector2 mouseScreenPosition = new Vector2(InputManager_cl.Instance.MouseState.X, -InputManager_cl.Instance.MouseState.Y);
            mouseScreenPosition.X -= Game_cl.BaseInstance.WindowWidth / 2;
            mouseScreenPosition.Y += Game_cl.BaseInstance.WindowHeight / 2;

            // Convert that to the position relative to the center in FNA units.
            Vector2 fnaScreenPosition = new Vector2();
            fnaScreenPosition.X = mouseScreenPosition.X * nearPlaneHalfWidth * 2.0f / Game_cl.BaseInstance.WindowWidth;
            fnaScreenPosition.Y = mouseScreenPosition.Y * nearPlaneHalfHeight * 2.0f / Game_cl.BaseInstance.WindowHeight;

            return fnaScreenPosition;
        }

        /// <summary>
        /// Uses the current sunburn camera component and the specified depth to get the current world position of the mouse.
        /// </summary>
        /// <param name="depth"></param>
        public Vector3 GetMouseWorldPosition(float depth = 0)
        {
            Vector2 screenPosition = GetFNAMouseScreenPosition();
            return ((SunburnCameraComponent_cl)CameraManager_cl.Instance.ActiveCamera).GetWorldPosition(screenPosition, depth);
        }

        /// <summary>
        /// Returns whether the specified key was pressed or not.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyPressedThisFrame(Keys input)
        {
            return (mLastKeyState.IsKeyUp(input) && mKeyState.IsKeyDown(input));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsKeyUp(Keys input)
        {
            return mKeyState.IsKeyUp(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys input)
        {
            return mKeyState.IsKeyDown(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WasKeyReleasedThisFrame(Keys input)
        {
            return (InputManager_cl.Instance.LastKeyState.IsKeyDown(input) && InputManager_cl.Instance.KeyState.IsKeyUp(input));
        }
    }
}
