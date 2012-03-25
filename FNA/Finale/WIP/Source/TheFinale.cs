using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FNA;
using FNA.Core;
using FNA.Components;
using FNA.Components.Cameras;
using FNA.Managers;
using FNA.Scripts;

using TheFinale.Components;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Effects;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Rendering;
using FNA.Graphics;


namespace TheFinale
{    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TheFinale : FNA.Game_cl
    {
        //private Vector2 textureScale;

        private static TheFinale mInstance;
        public static TheFinale Instance
        {
            get
            {
                return mInstance;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TheFinale() : base()
        {
            mInstance = this;
            GameStateManager_cl.State = GameStateManager_cl.GameState.GAME_STATE_LOADING;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            if (InitializeGraphics(false) == false)
            {
                this.Exit();
            }
                        
            base.Initialize();

            /************************************************************************
             * TODO:
             * Add initialization of game stuff like a menu.
             * 
             * Jay Sternfield   -   2011/11/05
             ************************************************************************/

            InitializeScene();

            GameStateManager_cl.State = GameStateManager_cl.GameState.GAME_STATE_INGAME;
        }

        protected override void LoadContent()
        {
            /************************************************************************/
            /* JACOB LOADING SHIT LOL                                                 */
            /************************************************************************/
            
            /************************************************************************/
            
            /************************************************************************
             * HACK TODO:
             * Load the scene effects in from some sort of data file.
             *
             * Jay Sternfield	-	2011/11/09
             ************************************************************************/
            /// Add static scene effects.
            SceneEffect effect = new SceneEffect();
            effect.effect = Content.Load<BaseRenderableEffect>("Materials/Forward/concept7");
            effect.size = new Vector2(750, 482f);
            effect.position = Vector2.Zero;
            effect.depth = 400.0f;
            effect.rotation = MathHelper.Pi;
            mStaticSceneEffects.Add(effect);

            //int center = mWorldSize / 2;
            //for (int x = 0; x < mWorldSize; x++)
            //{
            //    for (int y = 0; y < mWorldSize; y++)
            //    {
            //        SceneEffect effect = new SceneEffect();
            //        effect.effect = Content.Load<BaseRenderableEffect>("Materials/Forward/tile_floor");
            //        effect.size = Vector2.One;
            //        effect.position = new Vector2(x-center, y-center);
            //        effect.depth = 1.0f;
            //        mStaticSceneEffects.Add(effect);
            //    }
            //}

            base.LoadContent();

            //SynapseGaming.LightingSystem.Rendering.Scene rig = Content.Load<SynapseGaming.LightingSystem.Rendering.Scene>("Sunburn/light_rig");
            //mSceneInterface.Submit(rig);

            
            mEnvironment = Content.Load<SceneEnvironment>("Sunburn/scene_environment");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload all unmanaged content.
        /// </summary>
        protected override void UnloadContent()
        {
            /************************************************************************
             * TODO:
             * Unload any non ContentManager content here.
             ************************************************************************/
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /************************************************************************
             * TODO:
             * Add game-specific update logic here.
             * Jay Sternfield	-	2011/11/04
             ************************************************************************/

            switch (GameStateManager_cl.State)
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            switch (GameStateManager_cl.State)
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// Initialized for testing.
        /// </summary>
        private void InitializeScene()
        {
            mScene = new Scene_cl();

            ComponentManager_cl.Instance.PreUpdate();

            FNA.Components.InputComponent_cl cameraInput = new FNA.Components.InputComponent_cl(mCameraEntity);
            mCameraEntity.RemoveComponent(mCameraEntity.GetComponentOfType(typeof(SunburnCameraComponent_cl)));

            PositionComponent_cl position = (PositionComponent_cl)mCameraEntity.GetComponentOfType(typeof(PositionComponent_cl));
            position.SetPosition3D(new Vector3(0, 0, 0));

            TheFinaleCameraComponent_cl cameraComponent = new TheFinaleCameraComponent_cl(mCameraEntity);
            cameraComponent.ViewWidth = 20.0f;
            cameraComponent.FarPlane = 100.0f;
            CameraManager_cl.Instance.ActiveCamera = (CameraComponent_cl)mCameraEntity.GetComponentOfType(typeof(CameraComponent_cl));

            /************************************************************************
             * HACK:
             * Use the AmbientLightComponent to set up the scene's ambient light.
             *
             * Jay Sternfield	-	2011/12/07
             ************************************************************************/
            mScene.SetAmbientLight(Color.White.ToVector3(), 0.15f); 
            SynapseGaming.LightingSystem.Lights.AmbientLight ambient = new SynapseGaming.LightingSystem.Lights.AmbientLight();
            ambient.Depth = 0.0f;
            ambient.DiffuseColor = new Vector3(1f);
            ambient.Intensity = 1f;
            mSceneInterface.LightManager.Submit(ambient);

            RenderableManager_cl.Instance.Initialize(mWindowWidth, mWindowHeight);


            //// texture to vertices - terrain test
            //Entity_cl floor = new Entity_cl();
            //RenderableComponent floorRenderable = new RenderableComponent(floor);
            //floorRenderable.LoadSpriteContent("terrain");
            //floorRenderable.Sprite.Rectangle = new FloatRectangle(0, 0, 1, 1);

            //PositionComponent floorPosition = new PositionComponent(floor);
            //PhysicsComponent floorPhysics = new PhysicsComponent(floor);

            //floorPhysics.IsStatic = true;
            //floorPhysics.InitializePhysics("Terrain");
            //floorPhysics.SetCollisionGroup((short)1);

            //textureScale.X = floorPhysics._polygonTexture.Width / 64;   // this is converting display to sim units
            //textureScale.Y = floorPhysics._polygonTexture.Height / 64;
            //floorRenderable.Sprite.Size = textureScale;
            

            //// Scarfellina
            //Entity_cl scarfellina = PrefabManager_cl.Instance.LoadPrefab("C:/dev2/Cohort7-Public/Venture/FSS/Finale/WIP/Content/Prefabs/scarfellina.prefab"); // easier to test hardcoded
            //PositionComponent scarfellinaPosition = (PositionComponent)scarfellina.GetComponentOfType(typeof(PositionComponent));
            //PhysicsComponent scarfellinaPhysics = (PhysicsComponent)scarfellina.GetComponentOfType(typeof(PhysicsComponent));
            //RenderableComponent scarfellinaRenderable = (RenderableComponent)scarfellina.GetComponentOfType(typeof(RenderableComponent));

            //scarfellinaRenderable.Sprite.Rectangle = new FloatRectangle(0, 0, 1, 1); // does this matter for animated entities?
            //scarfellinaRenderable.Sprite.Size = new Vector2(2, 2);

            //scarfellinaPhysics.Position = new Vector2(0, 200);
            //scarfellinaPhysics.SetCollisionGroup((short)1);
            //mScene.AddEntity(scarfellina);
        }
    }
}
