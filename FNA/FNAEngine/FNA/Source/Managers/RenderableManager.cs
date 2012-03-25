using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FNA.Core;
using FNA.Components;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderableManager_cl : BaseManager_cl
    {
#region Variables, Structs, Enumerations, and Type Definitions
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        private static readonly RenderableManager_cl sInstance = new RenderableManager_cl();
        
        /// <summary>
        /// 
        /// </summary>
        public static RenderableManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

#region Buffer Member Variables
        /************************************************************************
         * TODO:
         * Add in specular to this shiz
         *
         * Jerad Dunn - 2011/11/05 - 17:57
         ************************************************************************/
        /// <summary>
        /// The buffer which holds Color information and Specular Intensity for the game world.
        /// (R,G,B) = color
        /// A = Specular Intensity (eventually)
        /// </summary>
        private RenderTarget2D mColorBuffer;

        /// <summary>
        /// The buffer which holds Normals and Specular Power for the game world.
        /// (R,G,B) = normal (mapped from [-1,1] to [0,1]
        /// A = Specular Power (eventually)
        /// </summary>
        private RenderTarget2D mNormalBuffer;

        /// <summary>
        /// The buffer which holds depth information for the game world.
        /// </summary>
        private RenderTarget2D mDepthBuffer;
        
        /// <summary>
        /// 
        /// </summary>
        public RenderTarget2D DepthBuffer
        {
            get
            {
                return mDepthBuffer;
            }
        }

        /// <summary>
        /// The buffer which holds Lighting Information (and eventually Specular Information) for the game world.
        /// (R,G,B) = light intensity and color information
        /// A = Specular Information (eventually)
        /// </summary>
        private RenderTarget2D mLightingBuffer;

        /// <summary>
        /// The buffer which holds Position Information for the game world visible on the screen.
        /// (R,G,B) = position offset information (relative to the screen)
        /// </summary>
        private RenderTarget2D mPositionBuffer;
#endregion

#region Effect Member Variables
        private Effect mClearBufferEffect;
        private Effect mAmbientLightEffect;
        private Effect mDirectionalLightEffect;
        private Effect mPointLightEffect;
        private Effect mCombineBuffersEffect;
#endregion

#region Quad Rendering Members
        private VertexPositionTexture[] mVertices;
        private short[] mIndexBuffer;
#endregion

        /// <summary>
        /// A procedural 'texture' used when writing Point Light information to the Light Buffer.
        /// </summary>
        private Texture2D mPointLightTexture;

        /// <summary>
        /// Used to map screen-space coordinates to texels in post-processing effects.
        /// For us, this would be for lighting, to be specific.
        /// </summary>
        private Vector2 mHalfPixel;
        
        /// <summary>
        /// 
        /// </summary>
        public Vector2 HalfPixel
        {
            get
            {
                return mHalfPixel;
            }
        }

        // All of the RenderableComponents within the game world
        List<RenderableComponent_cl> mActiveComponents = new List<RenderableComponent_cl>();
        /************************************************************************
         * TODO:
         * Change this from a list to some sort of partitioning system (using a Grid or Quadtree...leaning towards Grid)
         * After doing that, change all of the registration methods to place each component within their respective sectors and add a method
         * to return all of the RenderableComponents within a certain area (viewable world area perhaps?)
         *
         * Jerad Dunn - 2011/09/25 - 17:48
         ************************************************************************/
#endregion

#region Functions
        /// <summary>
        /// Constructor.
        /// </summary>
        private RenderableManager_cl()
        {
            // Just null everything out
            mColorBuffer = null;
            mNormalBuffer = null;
            mDepthBuffer = null;
            mLightingBuffer = null;
            mPositionBuffer = null;

            mClearBufferEffect = null;
            mAmbientLightEffect = null;
            mDirectionalLightEffect = null;
            mPointLightEffect = null;
            mCombineBuffersEffect = null;

            mPointLightTexture = null;
            
            mVertices = null;
            mIndexBuffer = null;
        }

        /// <summary>
        /// Adds a Component to this manager's list of all RenderableComponents.
        /// </summary>
        /// <param name="c">The Component to add.</param>
        public void RegisterComponent(RenderableComponent_cl c)
        {
            mActiveComponents.Add(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void UnregisterComponent(RenderableComponent_cl c)
        {
            mActiveComponents.Remove(c);
        }

#region IManager Implementation
        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
        }

        /// <summary>
        /// Updates all RenderableComponents registered with this manager.
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// Draws the current scene.
        /// </summary>
        public override void Draw()
        {
            /************************************************************************
             * HACK:
             * Bypassing FNA rendering for stated reasons.
             *
             * Larsson Burch - 2011/11/09 - 13:06
             ************************************************************************/
            foreach (RenderableComponent_cl c in mActiveComponents)
            {
                c.Draw();
            }

            /************************************************************************
             * TODO:
             * Clean up this code to look more like the other version...at least this stuff works for now...
             *
             * Jerad Dunn - 2011/11/05 - 18:42
             ************************************************************************/
            //SetGBuffer();

            //ClearDepthBuffer();
            //GenerateDepthMap();

            //Game_cl.BaseInstance.GraphicsDevice.SetRenderTargets(mColorBuffer, mNormalBuffer, mPositionBuffer);

            //ClearColorAndNormalBuffers();
            //GenerateGBuffer();

            //ResolveGBuffer();
            //ApplyLighting();

            /************************************************************************
             * TODO:
             * Add in debug functionality to view all of the buffers on-screen
             *
             * Jerad Dunn - 2011/11/06 - 19:07
             ************************************************************************/
            //Game_cl.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, null, null);
            //Game_cl.BaseInstance.SpriteBatch.Begin();
            //Game_cl.BaseInstance.SpriteBatch.Draw(mLightingBuffer, Vector2.Zero, Color.White);
            //Game_cl.BaseInstance.SpriteBatch.End();
        }
#endregion

#region Initialization
        /// <summary>
        /// Responsible for initializing everything required for render in FNA.
        /// If no width and height are passed, then the buffers will be the same size as the backbuffers.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Initialize(int width = 0, int height = 0)
        {
            LoadEffects();
            AllocateGBuffer(width, height);

            // Set up the half-pixel transformation
            mHalfPixel.X = 0.5f / (float)Game_cl.BaseInstance.WindowWidth;
            mHalfPixel.Y = 0.5f / (float)Game_cl.BaseInstance.WindowHeight;

            // Create a 1x1 'texture' to use when 'drawing' Point Lights
            mPointLightTexture = new Texture2D(Game_cl.BaseInstance.GraphicsDevice, 1, 1);
            mPointLightTexture.SetData(new Color[] { Color.White });

            // Setting up the member variables needed to render fullscreen quads
            mVertices = new VertexPositionTexture[]
                        {
                            new VertexPositionTexture(Vector3.Zero, Vector2.One),
                            new VertexPositionTexture(Vector3.Zero, new Vector2(0, 1)),
                            new VertexPositionTexture(Vector3.Zero, Vector2.Zero),
                            new VertexPositionTexture(Vector3.Zero, new Vector2(1, 0))
                        };
            mIndexBuffer = new short[] { 0, 1, 2, 2, 3, 0 };
        }

        /// <summary>
        /// Loads all of the Effect files used by the RenderableManager
        /// </summary>
        private void LoadEffects()
        {
            mClearBufferEffect = Game_cl.BaseInstance.Content.Load<Effect>("Effects\\FNAClearGBuffer");
            mAmbientLightEffect = Game_cl.BaseInstance.Content.Load<Effect>("Effects\\FNAAmbientLight");
            mDirectionalLightEffect = Game_cl.BaseInstance.Content.Load<Effect>("Effects\\FNADirectionalLight");
            mPointLightEffect = Game_cl.BaseInstance.Content.Load<Effect>("Effects\\FNAPointLight");
            mCombineBuffersEffect = Game_cl.BaseInstance.Content.Load<Effect>("Effects\\FNACombineBuffers");
        }

        /// <summary>
        /// Creates all of the buffers necessary for deferred rendering (color, normal, depth, lighting, and position).
        /// If no width and height are passed, then the buffers will be the same size as the backbuffers.
        /// </summary>
        /// <param name="width">The width of the buffers, in pixels.</param>
        /// <param name="height">The height of the buffers, in pixels.</param>       
        private void AllocateGBuffer(int width, int height)
        {
            if (width == 0 && height == 0)
            {
                width = Game_cl.BaseInstance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                height = Game_cl.BaseInstance.GraphicsDevice.PresentationParameters.BackBufferHeight;
            }

            // Create the color render target
            mColorBuffer = new RenderTarget2D(Game_cl.BaseInstance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            // Create the normal render target
            mNormalBuffer = new RenderTarget2D(Game_cl.BaseInstance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);

            // Create floating point depth render target
            mDepthBuffer = new RenderTarget2D(Game_cl.BaseInstance.GraphicsDevice, width, height, false, SurfaceFormat.Single, DepthFormat.Depth24);

            // Create lighting render target
            mLightingBuffer = new RenderTarget2D(Game_cl.BaseInstance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);

            // Create position render target
            mPositionBuffer = new RenderTarget2D(Game_cl.BaseInstance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        }
#endregion

#region Buffer Methods and Stuff
        /// <summary>
        /// 
        /// </summary>
        public void SetGBuffer()
        {
            Game_cl.BaseInstance.GraphicsDevice.SetRenderTargets(mColorBuffer, mNormalBuffer, mDepthBuffer);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResolveGBuffer()
        {
            Game_cl.BaseInstance.GraphicsDevice.SetRenderTargets(null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDepthBuffer()
        {
            Game_cl.BaseInstance.GraphicsDevice.BlendState = BlendState.Opaque;
            ApplyFullscreenEffect(mClearBufferEffect, "ClearDepthBuffer");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearColorAndNormalBuffers()
        {
            ApplyFullscreenEffect(mClearBufferEffect, "ClearColorNormalBuffers");
        }

        /// <summary>
        /// 'Renders' the scene in order to construct the depth map
        /// </summary>
        private void GenerateDepthMap()
        {
            // Draw all of the possible obstructors
            /************************************************************************
             * TODO:
             * Draw only those components that could possibly be visible within the current camera view (obviously done after the partitioning thing is done)
             *
             * Jerad Dunn - 2011/09/25 - 19:00
             ************************************************************************/
            foreach (RenderableComponent_cl c in mActiveComponents)
            {
                //c.Draw(Graphics.Sprite_cl.DrawMode.GENERATE_DEPTH_BUFFER);
            }
        }

        private void GenerateGBuffer()
        {
            /************************************************************************
             * TODO:
             * Draw only those components that could possibly be visible within the current camera view (obviously done after the partitioning thing is done)
             *
             * Jerad Dunn - 2011/09/25 - 19:00
             ************************************************************************/
            foreach (RenderableComponent_cl c in mActiveComponents)
            {
                //c.Draw(Graphics.Sprite_cl.DrawMode.GENERATE_G_BUFFER);
            }
        }
#endregion

        /// <summary>
        /// Applies an Effect to the entire screen area.
        /// In the case that a technique name is not provided, the first available technique will be used.
        /// </summary>
        /// <param name="effect">The Effect to apply to the screen area</param>
        /// <param name="technique">The name of the technique within the Effect to use</param>
        private void ApplyFullscreenEffect(Effect effect, string technique = null)
        {
            if (technique != null)
            {
                effect.CurrentTechnique = effect.Techniques[technique];
            }
            else
            {
                effect.CurrentTechnique = effect.Techniques[0];
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                RenderScreenSpaceQuad(-1 * Vector2.One, Vector2.One);
            }

        }

        /************************************************************************
         * TODO:
         * Add in proper references
         *
         * Jerad Dunn - 2011/11/05 - 19:04
         ************************************************************************/
        /// <summary>
        /// Renders a quad using screen-space coordinates.  
        /// Currently only used when applying fullscreen post-processing effects.
        /// Modeled after _____'s implementation. (INSERT LINK HERE)
        /// </summary>
        /// <param name="min">The minimum bounds of the quad to render</param>
        /// <param name="max">The maximum bounds of the quad to render</param>
        private void RenderScreenSpaceQuad(Vector2 min, Vector2 max)
        {
            mVertices[0].Position.X = max.X;
            mVertices[0].Position.Y = min.Y;

            mVertices[1].Position.X = min.X;
            mVertices[1].Position.Y = min.Y;

            mVertices[2].Position.X = min.X;
            mVertices[2].Position.Y = max.Y;

            mVertices[3].Position.X = max.X;
            mVertices[3].Position.Y = max.Y;

            Game_cl.BaseInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, mVertices, 0, 4, mIndexBuffer, 0, 2);
        }

        /// <summary>
        /// Remove all the renderable component references from the manager. Used when loading a new scene.
        /// </summary>
        public void ClearRenderables()
        {
            foreach (RenderableComponent_cl renderable in mActiveComponents)
            {
                renderable.Sprite.DeInitSunburnStuff();
            }
            mActiveComponents.Clear();
        }
#endregion
    }
}
