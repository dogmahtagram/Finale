using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FNA.Physics;

namespace FNA.Graphics
{
    /************************************************************************
     * TODO:
     * Enable simple Sprite rendering without lighting
     *
     * Jerad Dunn - 2011/09/27 - 20:08
     ************************************************************************/
    /************************************************************************
     * TODO:
     * Clean this up and align it with the updated version
     *
     * Jerad Dunn - 2011/11/05 - 20:12
     ************************************************************************/
    /// <summary>
    /// 
    /// </summary>
    public class Sprite_cl
    {
#region Variables, Structs, Enumerations, and Type Definitions
        /// <summary>
        /// States the current drawing mode for this Sprite
        /// </summary>
        public enum DrawMode
        {
            /// <summary>
            /// Draw to create the depth buffer
            /// </summary>
            GENERATE_DEPTH_BUFFER,

            /// <summary>
            /// Draw to populate the G-buffer
            /// </summary>
            GENERATE_G_BUFFER,

            /// <summary>
            /// Fancy rendering withing lighting applied
            /// </summary>
            DRAW_WITH_LIGHTING,

            /// <summary>
            /// Display the Sprite's normals
            /// </summary>
            DRAW_NORMALS,

            /// <summary>
            /// Display the Sprite's offsets
            /// </summary>
            DRAW_OFFSETS,

            /// <summary>
            /// Regular Sprite rendering with no lighting effects
            /// </summary>
            DRAW_DEFAULT
        }

        /// <summary>
        /// States the mapping type for this particular Sprite
        /// </summary>
        private enum MappingType
        {
            /// <summary>
            /// Offset and Normal maps are procedurally generated
            /// </summary>
            PROCEDURALLY_GENERATED,

            /// <summary>
            /// Offset and Normal maps are provided by the user
            /// </summary>
            PROVIDED
        }
        
        /************************************************************************
         * TODO:
         * Figure out what we want to do about texture/content management, and apply that thinking here...
         *
         * Jerad Dunn - 2011/11/05 - 20:11
         ************************************************************************/
        /// <summary>
        /// The diffuse texture for this Sprite
        /// </summary>
        private Texture2D mDiffuseTexture;

        /************************************************************************
         * HACK:
         * With the current prefab tool, there is never a chance for the animated component
         * to update the texture on the sprite, therefore we need to provide a default value
         * such that it can still be serialized/deserialized.
         *
         * Larsson Burch - 2011/11/10 - 10:08
         ************************************************************************/
        /// <summary>
        /// The string name of the diffuse texture, used for serialization of the renderable component.
        /// </summary>
        private string mDiffuseTextureName = "Burberry";

        /// <summary>
        /// Gets the diffuse texture name.
        /// </summary>
        public string DiffuseTextureName
        {
            get
            {
                return mDiffuseTextureName;
            }
        }
        
        /// <summary>
        /// The normal map for this Sprite
        /// </summary>
        private Texture2D mNormalTexture;
        
        /// <summary>
        /// The offset map for this Sprite
        /// </summary>
        private Texture2D mOffsetTexture;

        /// <summary>
        /// The active Effect for this Sprite
        /// </summary>
        private Effect mEffect;

        private Effect mGenerateDepthBufferEffect;
        private Effect mGenerateGBufferEffect;

        /// <summary>
        /// The current MappingType for this particular Sprite
        /// </summary>
        private MappingType mMappingType;

        /// <summary>
        /// The value (in world units) that the offset map gets multiplied by for conversion of a pixel to world units
        /// </summary>
        private float mOffsetScalar;

        /// <summary>
        /// 
        /// </summary>
        public float OffsetScalar
        {
            get
            {
                return mOffsetScalar;
            }
            set
            {
                mOffsetScalar = value;
            }
        }

        /// <summary>
        /// Width of this Sprite / Screen Width.  Used for depth buffer access.
        /// </summary>
        private float mWidthScale;

        /// <summary>
        /// Height of this Sprite / Screen Height. Used for depth buffer access.
        /// </summary>
        private float mHeightScale;

        /************************************************************************
         * HACK:
         * With the current prefab tool, there is never a chance for the animated component
         * to update the rectangle on the sprite, therefore we need to provide a default value
         * such that it can still be serialized/deserialized.
         *
         * Larsson Burch - 2011/11/10 - 10:08
         ************************************************************************/
        /// <summary>
        /// The Rectangle used to render a specific sprite from the sprite sheet
        /// </summary>
        private Rectangle mRectangle = new Rectangle(0, 0, 2, 2);

        /// <summary>
        /// 
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return mRectangle;
            }
            set
            {
                mRectangle = value;

                if (FNA.Game.IsPlayingGame == true)
                {
                    // Update the width and height scales accordingly
                    mWidthScale = mRectangle.Width / (float)Game.BaseInstance.WindowWidth;
                    mHeightScale = mRectangle.Height / (float)Game.BaseInstance.WindowHeight;
                }
            }
        }

        /// <summary>
        /// States whether or not the Sprite should be flipped when it is rendered.
        /// </summary>
        private bool mFlipped;

        /// <summary>
        /// 
        /// </summary>
        public bool Flipped
        {
            get
            {
                return mFlipped;
            }
            set
            {
                mFlipped = value;
            }
        }
#endregion

#region Functions
        /// <summary>
        /// Loads the textures with the provided asset names and sets them to be this Sprite's color, normal, and offset maps.
        /// Calling this method sets this Sprite's mapping type as MappingType.PROVIDED
        /// </summary>
        /// <param name="colorTexture">The color texture of this Sprite</param>
        /// <param name="normalTexture">The normal map of this Sprite</param>
        /// <param name="offsetTexture">The offset map of this Sprite</param>
        public void LoadContent(string colorTexture, string normalTexture, string offsetTexture)
        {
            mDiffuseTextureName = colorTexture;

            // We were given the maps, so this won't be procedural
            mMappingType = MappingType.PROVIDED;

            mDiffuseTexture = Game.BaseInstance.Content.Load<Texture2D>(colorTexture);
            mNormalTexture = Game.BaseInstance.Content.Load<Texture2D>(normalTexture);
            mOffsetTexture = Game.BaseInstance.Content.Load<Texture2D>(offsetTexture);

            mEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNASpriteRender");
            mGenerateDepthBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNACreateDepthBuffer");
            mGenerateGBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNARenderGBuffer");
        }

        /// <summary>
        /// Loads the texture with the provided asset name and sets it to be this Sprite's color map.
        /// Calling this method sets this Sprite's mapping type as MappingType.PROCEDURALLY_GENERATED
        /// </summary>
        /// <param name="colorTexture">The color texture of this Sprite</param>
        public void LoadContent(string colorTexture)
        {
            mDiffuseTextureName = colorTexture;

            // We were not given any maps, so we must procedurally generate them
            mMappingType = MappingType.PROCEDURALLY_GENERATED;

            mDiffuseTexture = Game.BaseInstance.Content.Load<Texture2D>(colorTexture);
            mNormalTexture = null;
            mOffsetTexture = null;

            mEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNASpriteRender");
            mGenerateDepthBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNACreateDepthBuffer");
            mGenerateGBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNARenderGBuffer");
        }

        /// <summary>
        /// Loads the texture with the provided asset name and sets it to be this Sprite's color map.
        /// Calling this method sets this Sprite's mapping type as MappingType.PROCEDURALLY_GENERATED
        /// </summary>
        /// <param name="colorTexture">The color texture of this Sprite</param>
        public void LoadContent(Texture2D colorTexture)
        {
            mDiffuseTextureName = colorTexture.Name;
            // We were not given any maps, so we must procedurally generate them
            mMappingType = MappingType.PROCEDURALLY_GENERATED;

            mDiffuseTexture = colorTexture;
            mNormalTexture = null;
            mOffsetTexture = null;

            mEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNASpriteRender");
            mGenerateDepthBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNACreateDepthBuffer");
            mGenerateGBufferEffect = Game.BaseInstance.Content.Load<Effect>("Effects\\FNARenderGBuffer");
        }

        /************************************************************************
         * HACK:
         * Implementing a bypass of FNA rendering so that we can get Farseer working
         * well with FNA world units in a standard XNA spritebatch draw call.
         *
         * Larsson Burch - 2011/11/09 - 13:00
         ************************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void DrawStandardXNA(Vector2 position)
        {
            Game.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Vector3 cameraPosition = FNA.Managers.CameraManager_cl.Instance.ActiveCamera.Position;
            Vector2 screenPosition = new Vector2((int)(ConvertUnits_cl.ToDisplayUnits(position.X - (cameraPosition.X/* - ConvertUnits_cl.ToSimUnits(FNA.Game.BaseInstance.WindowWidth / 2)*/)) - mRectangle.Width / 2), -(int)(ConvertUnits_cl.ToDisplayUnits(position.Y - (cameraPosition.Y/* + ConvertUnits_cl.ToSimUnits(FNA.Game.BaseInstance.WindowHeight / 2)*/)) + mRectangle.Height / 2));
            Game.BaseInstance.SpriteBatch.Draw(mDiffuseTexture, screenPosition, mRectangle, Color.White);
            Game.BaseInstance.SpriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        public static void DrawStandardXNAStatic(Vector2 position, Texture2D texture, Rectangle rectangle)
        {
            DrawStandardXNAStatic(position, texture, rectangle, 0, Vector2.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        public static void DrawStandardXNAStatic(Vector2 position, Texture2D texture, Rectangle rectangle, float rotation, Vector2 origin)
        {
            Game.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Vector3 cameraPosition = FNA.Managers.CameraManager_cl.Instance.ActiveCamera.Position;
            Vector2 screenPosition = new Vector2((int)(ConvertUnits_cl.ToDisplayUnits(position.X - (cameraPosition.X - ConvertUnits_cl.ToSimUnits(FNA.Game.BaseInstance.WindowWidth / 2))) - rectangle.Width / 2), -(int)(ConvertUnits_cl.ToDisplayUnits(position.Y - (cameraPosition.Y + ConvertUnits_cl.ToSimUnits(FNA.Game.BaseInstance.WindowHeight / 2))) + rectangle.Height / 2));
            Game.BaseInstance.SpriteBatch.Draw(texture, screenPosition, rectangle, Color.White, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            Game.BaseInstance.SpriteBatch.End();
        }

        /************************************************************************
         * TODO:
         * Optimize, optimize, optimize!
         *
         * Jerad Dunn - 2011/09/25 - 20:16
         ************************************************************************/
        /************************************************************************
         * TODO:
         * Team up with Larsson and get that whole 'flipped' shit going
         *
         * Jerad Dunn - 2011/11/05 - 20:16
         ************************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="position"></param>
        public void Draw(DrawMode mode, Vector2 position)
        {
            //string techniqueName = "";

            switch (mode)
            {
                case DrawMode.GENERATE_DEPTH_BUFFER:
                    {
                        //techniqueName = "CreateDepthMap";
                        DrawToDepthBuffer(position);
                        break;
                    }
                case DrawMode.GENERATE_G_BUFFER:
                    {
                        //techniqueName = "DisplayNormals";
                        GenerateGBuffer(position);
                        break;
                    }
                case DrawMode.DRAW_OFFSETS:
                    {
                        //techniqueName = "DisplayOffsets";
                        break;
                    }
                case DrawMode.DRAW_WITH_LIGHTING:
                    {
                        //techniqueName = "LitRender";
                        break;
                    }
                /************************************************************************
                 * TODO:
                 * Add in plain Sprite rendering functionality sans lighting effects
                 *
                 * Jerad Dunn - 2011/09/27 - 20:19
                 ************************************************************************/
                case DrawMode.DRAW_DEFAULT:
                    {
                        //techniqueName = "LitRender";
                        break;
                    }
                default:
                    {
                        // throw an exception?
                        break;
                    }
            }

            //if (mMappingType == MappingType.PROCEDURALLY_GENERATED)
            //{
            //    techniqueName += "Procedural";
            //}

            //if (mode == DrawMode.DRAW_WITH_LIGHTING)
            //{
            //    techniqueName += "1";
            //}

            //mEffect.CurrentTechnique = mEffect.Techniques[techniqueName];

            //mEffect.Parameters["CameraTransformationMatrix"].SetValue(Managers.CameraManager.Instance.ActiveCamera.TransformMatrix);

            //mEffect.Parameters["DiffuseTexture"].SetValue(mDiffuseTexture);
            //mEffect.Parameters["OffsetTexture"].SetValue(mOffsetTexture);

            //mEffect.Parameters["SpriteOrigin"].SetValue(new Vector3(position.X, position.Y, 0));
            
            //************************************************************************
            // * HACK:
            // * This exists only until we pass the offset scalar to the Sprite
            // *
            // * Jerad Dunn - 2011/10/06 - 20:20
            // ************************************************************************/
            ////mEffect.Parameters["SpriteOffsetScalar"].SetValue(mOffsetScalar);
            //mEffect.Parameters["SpriteOffsetScalar"].SetValue(2.0f);

            //mEffect.Parameters["CameraInfo"].SetValue(new Vector4(Managers.CameraManager.Instance.ActiveCamera.Position, Managers.CameraManager.Instance.ActiveCamera.FarPlane));

            //if (mode != DrawMode.GENERATE_DEPTH_BUFFER)
            //{
            //    Vector2 screenCoords = Managers.CameraManager.Instance.ActiveCamera.ConvertToPixelSpace(new Vector3(position.X, position.Y, 0));
            //    screenCoords.X = (screenCoords.X - (float)mRectangle.Width / 2) / (float)Game.BaseInstance.WindowWidth;
            //    screenCoords.Y = (screenCoords.Y - (float)mRectangle.Height) / (float)Game.BaseInstance.WindowHeight;
            //    mEffect.Parameters["SpriteScreenCoord"].SetValue(screenCoords);

            //    mEffect.Parameters["SpriteWidthCoordScale"].SetValue(mWidthScale);
            //    mEffect.Parameters["SpriteHeightCoordScale"].SetValue(mHeightScale);

            //    mEffect.Parameters["NormalTexture"].SetValue(mNormalTexture);
            //    mEffect.Parameters["DepthTexture"].SetValue((Texture2D)Managers.RenderableManager_cl.Instance.DepthBuffer);

            //    if (mode == DrawMode.DRAW_WITH_LIGHTING)
            //    {
            //        EffectParameter rectangleParameter = mEffect.Parameters["Rectangle"];
            //        rectangleParameter.StructureMembers["Boundaries"].SetValue(new Vector4((float)mRectangle.Left / (float)mDiffuseTexture.Width, (float)mRectangle.Right / (float)mDiffuseTexture.Width,
            //                                                                               (float)mRectangle.Top / (float)mDiffuseTexture.Height, (float)mRectangle.Bottom / (float)mDiffuseTexture.Height));
                    
            //        EffectParameter ambientLightParameter = mEffect.Parameters["AmbientLight"];
            //        ambientLightParameter.StructureMembers["Color"].SetValue(Managers.LightManager_cl.Instance.ActiveAmbientLight.ShaderStruct.Color);

            //        EffectParameter directionalLightParameter = mEffect.Parameters["DirectionalLight"];
            //        //directionalLightParameter.StructureMembers["Color"].SetValue(Managers.LightManager_cl.Instance.ActiveDirectionalLight.ShaderStruct.Color);
            //        //directionalLightParameter.StructureMembers["Direction"].SetValue(Managers.LightManager_cl.Instance.ActiveDirectionalLight.ShaderStruct.Direction);

            //        EffectParameterCollection pointLights = mEffect.Parameters["PointLights"].Elements;
            //        EffectParameterCollection pointLightParameters = pointLights[0].StructureMembers;
            //        pointLightParameters["Position"].SetValue(Managers.LightManager_cl.Instance.PointLights[0].ShaderStruct.Position);
            //        pointLightParameters["Color"].SetValue(Managers.LightManager_cl.Instance.PointLights[0].ShaderStruct.Color);
            //    }
            //}


            //if (mode == DrawMode.GENERATE_DEPTH_BUFFER)
            //{
            //    Game.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, mEffect);
            //}
            //else
            //{
            //    Game.BaseInstance.SpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, mEffect);
            //}

            //Game.BaseInstance.SpriteBatch.Draw(mDiffuseTexture, new Vector2(position.X * (float)Game.BaseInstance.SqueetrePixelWidth - (float)mRectangle.Width / 2, position.Y * (float)Game.BaseInstance.SqueetrePixelHeight - (float)mRectangle.Height), mRectangle, Color.White);
            //Game.BaseInstance.SpriteBatch.End();
        }

        private void DrawToDepthBuffer(Vector2 position)
        {
            string techniqueName = "CreateDepthBuffer";

            if (mMappingType == MappingType.PROCEDURALLY_GENERATED)
            {
                techniqueName += "Procedural";
            }

            mGenerateDepthBufferEffect.CurrentTechnique = mGenerateDepthBufferEffect.Techniques[techniqueName];

            mGenerateDepthBufferEffect.Parameters["CameraTransformationMatrix"].SetValue(Managers.CameraManager_cl.Instance.ActiveCamera.TransformMatrix);

            mGenerateDepthBufferEffect.Parameters["DiffuseTexture"].SetValue(mDiffuseTexture);
            mGenerateDepthBufferEffect.Parameters["OffsetTexture"].SetValue(mOffsetTexture);

            mGenerateDepthBufferEffect.Parameters["SpriteOrigin"].SetValue(new Vector3(position.X, position.Y, 0));

            /************************************************************************
             * HACK:
             * This exists only until we pass the offset scalar to the Sprite
             *
             * Jerad Dunn - 2011/10/06 - 20:20
             ************************************************************************/
            //mGenerateDepthBufferEffect.Parameters["SpriteOffsetScalar"].SetValue(mOffsetScalar);
            mGenerateDepthBufferEffect.Parameters["SpriteOffsetScalar"].SetValue(2.0f);

            mGenerateDepthBufferEffect.Parameters["CameraInfo"].SetValue(new Vector4(Managers.CameraManager_cl.Instance.ActiveCamera.Position, Managers.CameraManager_cl.Instance.ActiveCamera.FarPlane));

            EffectParameter spriteRectangleParameter = mGenerateDepthBufferEffect.Parameters["SpriteRectangle"];
            spriteRectangleParameter.StructureMembers["Boundaries"].SetValue(new Vector4(mRectangle.Left, mRectangle.Right, mRectangle.Top, mRectangle.Bottom));

            Game.BaseInstance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, mGenerateDepthBufferEffect);
            Game.BaseInstance.SpriteBatch.Draw(mDiffuseTexture, new Vector2(position.X * (float)Game.BaseInstance.SqueetrePixelWidth - (float)mRectangle.Width / 2, position.Y * (float)Game.BaseInstance.SqueetrePixelHeight - (float)mRectangle.Height), mRectangle, Color.White);
            Game.BaseInstance.SpriteBatch.End();
        }

        private void GenerateGBuffer(Vector2 position)
        {
            string techniqueName = "GenerateGBuffer";

            if (mMappingType == MappingType.PROCEDURALLY_GENERATED)
            {
                techniqueName += "Procedural";
            }

            mGenerateGBufferEffect.CurrentTechnique = mGenerateGBufferEffect.Techniques[techniqueName];

            mGenerateGBufferEffect.Parameters["CameraTransformationMatrix"].SetValue(Managers.CameraManager_cl.Instance.ActiveCamera.TransformMatrix);

            mGenerateGBufferEffect.Parameters["DiffuseTexture"].SetValue(mDiffuseTexture);
            mGenerateGBufferEffect.Parameters["OffsetTexture"].SetValue(mOffsetTexture);

            mGenerateGBufferEffect.Parameters["SpriteOrigin"].SetValue(new Vector3(position.X, position.Y, 0));

            /************************************************************************
             * HACK:
             * This exists only until we pass the offset scalar to the Sprite
             *
             * Jerad Dunn - 2011/10/06 - 20:20
             ************************************************************************/
            //mGenerateGBufferEffect.Parameters["SpriteOffsetScalar"].SetValue(mOffsetScalar);
            mGenerateGBufferEffect.Parameters["SpriteOffsetScalar"].SetValue(2.0f);

            mGenerateGBufferEffect.Parameters["CameraInfo"].SetValue(new Vector4(Managers.CameraManager_cl.Instance.ActiveCamera.Position, Managers.CameraManager_cl.Instance.ActiveCamera.FarPlane));

            Vector2 screenCoords = Managers.CameraManager_cl.Instance.ActiveCamera.ConvertToPixelSpace(new Vector3(position.X, position.Y, 0));
            screenCoords.X = (screenCoords.X - (float)mRectangle.Width / 2) / (float)Game.BaseInstance.WindowWidth;
            screenCoords.Y = (screenCoords.Y - (float)mRectangle.Height) / (float)Game.BaseInstance.WindowHeight;
            mGenerateGBufferEffect.Parameters["SpriteScreenCoord"].SetValue(screenCoords);

            mGenerateGBufferEffect.Parameters["SpriteWidthCoordScale"].SetValue(mWidthScale);
            mGenerateGBufferEffect.Parameters["SpriteHeightCoordScale"].SetValue(mHeightScale);

            EffectParameter spriteRectangleParameter = mGenerateGBufferEffect.Parameters["SpriteRectangle"];
            spriteRectangleParameter.StructureMembers["Boundaries"].SetValue(new Vector4(mRectangle.Left, mRectangle.Right, mRectangle.Top, mRectangle.Bottom));

            mGenerateGBufferEffect.Parameters["NormalTexture"].SetValue(mNormalTexture);
            mGenerateGBufferEffect.Parameters["DepthTexture"].SetValue((Texture2D)Managers.RenderableManager_cl.Instance.DepthBuffer);

            EffectParameter screenBoundsParameter = mGenerateGBufferEffect.Parameters["ScreenBounds"];
            Vector3 min = Managers.CameraManager_cl.Instance.ActiveCamera.ConvertToWorldCoordinates(Vector2.Zero);
            Vector3 max = Managers.CameraManager_cl.Instance.ActiveCamera.ConvertToWorldCoordinates(new Vector2(Game.BaseInstance.WindowWidth, Game.BaseInstance.WindowHeight));
            screenBoundsParameter.StructureMembers["Boundaries"].SetValue(new Vector4(min.X, max.X, min.Y, max.Y));

            mGenerateGBufferEffect.Parameters["MaximumZValue"].SetValue(4.0f);

            Game.BaseInstance.SpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, mGenerateGBufferEffect);
            Game.BaseInstance.SpriteBatch.Draw(mDiffuseTexture, new Vector2(position.X * (float)Game.BaseInstance.SqueetrePixelWidth - (float)mRectangle.Width / 2, position.Y * (float)Game.BaseInstance.SqueetrePixelHeight - (float)mRectangle.Height), mRectangle, Color.White);
            Game.BaseInstance.SpriteBatch.End();
        }
#endregion
    }
}
