using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using XnaInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FNA;
using FNA.Components;
using FNA.Components.Cameras;
using FNA.Core;
using FNA.Graphics;
using FNA.Interfaces;
using FNA.Managers;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Effects;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Rendering;

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace FNA
{
    /// <summary>
    /// This is the base class for a game in the FNA engine which inherits an XNA Game.
    /// </summary>
    public class Game_cl : Microsoft.Xna.Framework.Game
    {
        /************************************************************************
         * Sunburn variables
         *************************************************************************/
        /// 
        protected readonly LightingSystemManager mLightingSystemManager;
        /// 
        protected readonly SceneInterface mSceneInterface;

        /// <summary>
        /// 
        /// </summary>
        public SceneInterface SceneInterface
        {
            get
            {
                return mSceneInterface;
            }
        }

        /// 
        protected readonly SceneState mSceneState;

        /// <summary>
        /// Gets the SceneState for this game.
        /// </summary>
        public SceneState SceneState
        {
            get
            {
                return mSceneState;
            }
        }

        /// 
        protected SceneEnvironment mEnvironment;
        /// 
        protected SpriteManager mSpriteManager;

        /// <summary>
        /// 
        /// </summary>
        public SpriteManager SpriteManager
        {
            get
            {
                return mSpriteManager;
            }
        }

        /// 
        protected FrameBuffers mFrameBuffers;
        /// 
        protected SpriteContainer mStaticSceneSprites;

        /************************************************************************
         * Sunburn camera variables
         ************************************************************************/
        //protected Matrix mView;
        //protected Matrix mProjection;

        /// <summary>
        /// 
        /// </summary>
        protected struct SceneEffect
        {
            /// 
            public BaseRenderableEffect effect;
            /// 
            public Vector2 size;
            /// 
            public Vector2 position;
            /// 
            public float depth;
            ///
            public float rotation;
        };
        /// 
        protected List<SceneEffect> mStaticSceneEffects;

        // Scene constants that control the number of world units visible
        // in the view and the number of units wide the world is.
        /// 
        protected float mViewWidth = 10.0f;
        /// 
        protected int mWorldSize = 10;

        /************************************************************************
         * TODO:
         * Add in ability to add multiple point lights.
         * Add in ability to edit light properties.
         * 
         * Jerad Dunn   -   2011/09/28
         *************************************************************************/
        //private Entity mAmbientLight;
        //private List<Entity> mDirectionalLights;
        //private List<Entity> mPointLights;

        /************************************************************************
         * TODO:
         * We still need to figure out entity management. For now, I'm thinking
         * that the world editor could hold its own entity list, that way it can 
         * add/remove/change objects that it owns. When you save or load an edited
         * world, that's when the editor's list gets transferred over to the active
         * world or saved world file.
         * 
         * Edit: The game, which acts as a world editor in a certain state, will
         * have a Scene object loaded that contains all the lights, entities, etc.
         * Jay Sternfield   -   2011/11/05
         * 
         * Jerad Dunn   -   2011/09/28
         ************************************************************************/
        //List<Entity> mEntities = new List<Entity>();

        /// <summary>
        /// 
        /// </summary>
        protected FNA.Core.Scene_cl mScene;

        /// <summary>
        /// 
        /// </summary>
        public FNA.Core.Scene_cl Scene
        {
            get
            {
                return mScene;
            }
            set
            {
                mScene = value;
            }
        }

        /************************************************************************/
        /* HACK - Trying to copy Vision here with a way to tell if we're in the 
         * game vs in a tool. This static bool is set to true in the constructor
         * for Game.
         * 
         * Larsson Burch - 10/01/2011
        /************************************************************************/
        /// <summary>
        /// Whether the engine is currently in play the game mode.
        /// </summary>
        protected static bool mIsPlayingGame = false;

        /// <summary>
        /// Returns whether the engine is currently in playing the game mode.
        /// </summary>
        public static bool IsPlayingGame
        {
            get
            {
                return mIsPlayingGame;
            }
        }

        /// <summary>
        /// The static instance of the base FNA game.
        /// </summary>
        private static Game_cl mBaseInstance;

        /// <summary>
        /// Gets the static instance of the base FNA game.
        /// </summary>
        public static Game_cl BaseInstance
        {
            get
            {
                return mBaseInstance;
            }
        }

        /// <summary>
        /// A list of IManagers associated with the FNA game which provide access and distribute update calls to other
        /// parts of the FNA engine.
        /// </summary>
        protected List<BaseManager_cl> mManagers;

        /// <summary>
        /// A random number generated associated with our FNA game.
        /// </summary>
        private Random mRandom;

        /// <summary>
        /// Get the random number generator for this game.
        /// </summary>
        public Random Random
        {
            get
            {
                return mRandom;
            }
        }

        /// <summary>
        /// The FNA timer associated with this game.
        /// </summary>
        private FNA.Core.Timer_cl mTimer;

        /// <summary>
        /// Gets the Timer that this game uses.
        /// </summary>
        public FNA.Core.Timer_cl Timer
        {
            get
            {
                return mTimer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Entity_cl mCameraEntity;

        /// <summary>
        /// 
        /// </summary>
        public Entity_cl Camera
        {
            get
            {
                return mCameraEntity;
            }
        }
#if WORLD_EDITOR
        /// 
        protected WorldEditor_cl mWorldEditor;

        /// <summary>
        /// 
        /// </summary>
        public WorldEditor_cl WorldEditor
        {
            get
            {
                return mWorldEditor;
            }
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        protected SpriteFont mDebugFont;

        /// <summary>
        /// 
        /// </summary>
        public SpriteFont DebugFont
        {
            get
            {
                return mDebugFont;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int mWindowWidth = 1280;
        /// <summary>
        /// 
        /// </summary>
        public int WindowWidth
        {
            get
            {
                return mWindowWidth;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int mWindowHeight = 800;
        /// <summary>
        /// 
        /// </summary>
        public int WindowHeight
        {
            get
            {
                return mWindowHeight;
            }
        }

        /// <summary>
        /// Returns the window handle.
        /// Can be used make a Form for adding WinForms controls.
        /// </summary>
        public IntPtr WindowHandle
        {
            get
            {
                return this.Window.Handle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Game_cl()
            : base()
        {
            mGraphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            // Required for lighting system.
            Components.Add(new SplashScreenGameComponent(this, mGraphics));

            // Create the lighting system.
            mLightingSystemManager = new LightingSystemManager(Services, Content);

            mSceneState = new SceneState();
            mEnvironment = new SceneEnvironment();

            mSceneInterface = new SceneInterface(mGraphics);
            mSceneInterface.CreateDefaultManagers(false);
            mSceneInterface.AddManager(new ObjectManager_cl());
            
            // Create the sprite manager used to create and organize sprite containers for 2D rendering.
            mSpriteManager = new SpriteManager(mGraphics);
            mSceneInterface.AddManager(mSpriteManager);

            mFrameBuffers = new FrameBuffers(mGraphics, DetailPreference.Low, DetailPreference.Low);
            mSceneInterface.ResourceManager.AssignOwnership(mFrameBuffers);
            mStaticSceneEffects = new List<SceneEffect>();
                                    
            //mView = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            //mProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            mCameraEntity = new Entity_cl();

#if WORLD_EDITOR
            mWorldEditor = new WorldEditor_cl();         
#endif

            mManagers = new List<BaseManager_cl>();
            mRandom = new Random();
            mTimer = new FNA.Core.Timer_cl();

            mBaseInstance = this;
            mIsPlayingGame = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            
            mDebugFont = Content.Load<SpriteFont>("Fonts\\DebugFont");

            PositionComponent_cl cameraPosition = new PositionComponent_cl(mCameraEntity, 0, 0);

            /************************************************************************
             * HACK:
             * We're using Sunburn now, so the camera's getting changed to the SunburnCameraComponent_cl.
             * Is this really a hack? At what point do we say Sunburn is official, or do we always want
             * backward compatibility with the original FNA?
             *
             * Larsson Burch - 2011/11/11 - 10:07
             ************************************************************************/
            SunburnCameraComponent_cl cameraComponent = new SunburnCameraComponent_cl(mCameraEntity);
            cameraComponent.ViewWidth = 30.0f;

            mManagers.Add(RenderableManager_cl.Instance);
            mManagers.Add(TriggerManager_cl.Instance);
            mManagers.Add(CameraManager_cl.Instance);
            mManagers.Add(PartyManager_cl.Instance);

            this.IsMouseVisible = true;

#if WORLD_EDITOR            
            mManagers.Add(EditingManager_cl.Instance);
#endif

            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            // Create the static scenery container.
            mStaticSceneSprites = mSpriteManager.CreateSpriteContainer();
            
            mStaticSceneSprites.Begin();
            for (int x = 0; x < mStaticSceneEffects.Count; x++)
            {
                // Note: closest layer depth to the camera is 0
                mStaticSceneSprites.Add(mStaticSceneEffects[x].effect,
                                        mStaticSceneEffects[x].size,
                                        mStaticSceneEffects[x].position,
                                        mStaticSceneEffects[x].rotation,
                                        mStaticSceneEffects[x].depth);
            }
            mStaticSceneSprites.End();
            
            // Submit the static scenery container.
            mSceneInterface.ObjectManager.Submit(mStaticSceneSprites);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UnloadContent()
        {
            mSceneInterface.Unload();
            mLightingSystemManager.Unload();
            mEnvironment = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            mTimer.Update(gameTime);

            UpdateManagers();

            XnaInput.KeyboardState keyState = InputManager_cl.Instance.KeyState;
            XnaInput.MouseState currentMouseState = InputManager_cl.Instance.MouseState;
            XnaInput.MouseState lastMouseState = InputManager_cl.Instance.LastMouseState;
            XnaInput.GamePadState gamepadState1 = InputManager_cl.Instance.GetGamePadState(PlayerIndex.One);

            /************************************************************************
             * HACK:
             * The following allows the game to exit.
             * We do not necessarily want Escape to do this, or we should at least
             * pop up a confirmation dialog for the Release version.
             *
             * Jay Sternfield	-	2011/11/08
             ************************************************************************/
            if ((InputManager_cl.Instance.WasKeyPressed(XnaInput.Keys.Escape)) ||
                (gamepadState1.Buttons.Back == XnaInput.ButtonState.Pressed))
            {
                this.Exit();
            }

#if WORLD_EDITOR
            else if (InputManager_cl.Instance.WasKeyPressed(XnaInput.Keys.P) && !mWorldEditor.DialogOpen)
            {
                if (GameStateManager_cl.State == GameStateManager_cl.GameState.GAME_STATE_EDITOR)
                {
                    mWorldEditor.Deinitialize();

                    Timer.Paused = false;

                    GameStateManager_cl.State = GameStateManager_cl.GameState.GAME_STATE_INGAME;
                }
                else
                {
                    Timer.Paused = true;
                    
                    mWorldEditor.Initialize();

                    GameStateManager_cl.State = GameStateManager_cl.GameState.GAME_STATE_EDITOR;
                }
            }
#endif

            switch (GameStateManager_cl.State)
            {
#if WORLD_EDITOR
                case GameStateManager_cl.GameState.GAME_STATE_EDITOR:
                    mWorldEditor.Update(gameTime);
                    break;
#endif
                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates all the managers.
        /// </summary>
        private void UpdateManagers()
        {
            InputManager_cl.Instance.Update();

            PhysicsManager_cl.Instance.Update();

            ComponentManager_cl.Instance.PreUpdate();            
            ComponentManager_cl.Instance.Update();

            foreach (BaseManager_cl manager in mManagers)
            {
                manager.PreUpdate();
            }

            foreach (BaseManager_cl manager in mManagers)
            {
                manager.Update();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            // Check to see if the splash screen is finished.
            if (!SplashScreenGameComponent.DisplayComplete)
            {
                base.Draw(gameTime);
                return;
            }

            // Setup the scene state using the player position and the number of world
            // units that should be visible in the view (viewWidth).
            SunburnCameraComponent_cl camera = (SunburnCameraComponent_cl)FNA.Managers.CameraManager_cl.Instance.ActiveCamera;
            mSceneState.BeginFrameRendering(new Vector2(-camera.Position2D.X, camera.Position2D.Y), camera.ViewWidth, GraphicsDevice.Viewport.AspectRatio, gameTime, mEnvironment, mFrameBuffers, true);

            // Render the scene.
            mSceneInterface.BeginFrameRendering(mSceneState);
            // Add custom rendering that should occur before the scene is rendered.
            mSceneInterface.RenderManager.Render();
            // Add custom rendering that should occur after the scene is rendered.
            mSceneInterface.EndFrameRendering();
            mSceneState.EndFrameRendering();

            /************************************************************************
             * TODO:
             * Draw Renderable manager first?
             *
             * Jay Sternfield	-	2011/12/05
             ************************************************************************/
            foreach (BaseManager_cl manager in mManagers)
            {
                manager.Draw();
            }

            // Draw components with IDrawAble second so that debug draw overlays the rest.
            ComponentManager_cl.Instance.Draw();

            base.Draw(gameTime);

            switch (GameStateManager_cl.State)
            {
#if WORLD_EDITOR
                case GameStateManager_cl.GameState.GAME_STATE_EDITOR:
                    mWorldEditor.DrawEditorGUI();
                    break;
#endif
                default:
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        protected GraphicsDeviceManager mGraphics;

        /// <summary>
        /// 
        /// </summary>
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return mGraphics;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected SpriteBatch mSpriteBatch;

        /// <summary>
        /// 
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return mSpriteBatch;
            }
        }

        /// <summary>
        /// Attempt to set the display mode to the desired resolution.  Iterates through the display
        /// capabilities of the default graphics adapter to determine if the graphics adapter supports the
        /// requested resolution.  If so, the resolution is set and the function returns true.  If not,
        /// no change is made and the function returns false.
        /// </summary>
        /// <param name="fullScreen">True if you wish to go to Full Screen, false for Windowed Mode.</param>
        protected bool InitializeGraphics(bool fullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (fullScreen == false)
            {
                if ((mWindowWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) &&
                    (mWindowHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    mGraphics.PreferredBackBufferWidth = mWindowWidth;
                    mGraphics.PreferredBackBufferHeight = mWindowHeight;
                    mGraphics.IsFullScreen = fullScreen;
                    mGraphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate thorugh the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == mWindowWidth) && (dm.Height == mWindowHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        mGraphics.PreferredBackBufferWidth = mWindowWidth;
                        mGraphics.PreferredBackBufferHeight = mWindowHeight;
                        mGraphics.IsFullScreen = fullScreen;
                        mGraphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
