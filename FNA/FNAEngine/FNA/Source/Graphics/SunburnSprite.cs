using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Effects;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Rendering;

using FNA.Components.Cameras;

namespace FNA.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SunburnSprite_cl
    {
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        private BaseRenderableEffect mBaseRenderableEffect;

        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        private SpriteContainer mSpriteContainer;

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
        private string mBaseRenderableEffectName = "Burberry";

        /// <summary>
        /// Gets the diffuse texture name.
        /// </summary>
        public string MaterialName
        {
            get
            {
                return mBaseRenderableEffectName;
            }
            set
            {
                mBaseRenderableEffectName = value;
            }
        }

        /// <summary>
        /// The size of the sprite in FNA units.
        /// </summary>
        private Vector2 mSize = new Vector2(1, 1);

        /// <summary>
        /// Gets or sets the size of this sprite in FNA units.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return mSize;
            }
            set
            {
                mSize = value;
            }
        }

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
        private FloatRectangle mRectangle = new FloatRectangle(0, 0, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public FloatRectangle Rectangle
        {
            get
            {
                return mRectangle;
            }
            set
            {
                mRectangle = value;
            }
        }

        /// <summary>
        /// Get or set the U of this sprite's UV.
        /// </summary>
        public float U
        {
            get
            {
                return mRectangle.U;
            }
            set
            {
                mRectangle.U = value;
            }
        }

        /// <summary>
        /// Get or set the V of this sprite's UV.
        /// </summary>
        public float V
        {
            get
            {
                return mRectangle.V;
            }
            set
            {
                mRectangle.V = value;
            }
        }

        /// <summary>
        /// Get or set the width of this sprite's UV.
        /// </summary>
        public float UVWidth
        {
            get
            {
                return mRectangle.Width;
            }
            set
            {
                mRectangle.Width = value;
            }
        }

        /// <summary>
        /// Get or set the height of this sprite's UV.
        /// </summary>
        public float UVHeight
        {
            get
            {
                return mRectangle.Height;
            }
            set
            {
                mRectangle.Height = value;
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

        /// <summary>
        /// C
        /// </summary>
        public SunburnSprite_cl()
        {

        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected SunburnSprite_cl(SerializationInfo info, StreamingContext context)
        {
            mBaseRenderableEffectName = info.GetString("TextureName");
            mRectangle = (FloatRectangle)info.GetValue("Rectangle", typeof(FloatRectangle));
            mSize = (Vector2)info.GetValue("SpriteSize", typeof(Vector2));
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TextureName", mBaseRenderableEffectName);
            info.AddValue("Rectangle", mRectangle, typeof(FloatRectangle));
            info.AddValue("SpriteSize", mSize, typeof(Vector2));
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(string material)
        {
            mBaseRenderableEffect = FNA.Game_cl.BaseInstance.Content.Load<BaseRenderableEffect>("Materials/Forward/" + material);
            mBaseRenderableEffectName = material;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitSunburnStuff()
        {
            if (FNA.Game_cl.IsPlayingGame == true)
            {
                mSpriteContainer = FNA.Game_cl.BaseInstance.SpriteManager.CreateSpriteContainer();
                FNA.Game_cl.BaseInstance.SceneInterface.ObjectManager.Submit(mSpriteContainer);
            }
        }

        /// <summary>
        /// When we load a new scene and clear out all the old components, we need a method to tell Sunburn to forget
        /// about the sprites that we were previously rendering. This method does just that, and is called from the
        /// renderable manager's ClearRenderables function.
        /// </summary>
        public void DeInitSunburnStuff()
        {
            /************************************************************************
             * TODO:
             * Are both of these calls necessary? Will Unload do everything we need?
             *
             * Larsson Burch - 2011/11/15 - 14:02
             ************************************************************************/
            FNA.Game_cl.BaseInstance.SceneInterface.ObjectManager.Remove(mSpriteContainer);
            //FNA.Game_cl.BaseInstance.SpriteManager.Unload();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="flipped"></param>
        /// <param name="rotation"></param>
        public void Draw(Vector3 position, bool flipped = false, float rotation = 0)
        {
            Draw(position, new Vector2(0, 0), flipped, rotation);
        }


        // second draw method with origin - used with texture to vertices
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        /// <param name="flipped"></param>
        /// <param name="rotation"></param>
        public void Draw(Vector3 position, Vector2 origin, bool flipped = false, float rotation = 0)
        {
            // Sunburn insisted on having positive x to the left, so we must reverse the FNA x position in order to render in Sunburn space.
            Vector2 sunburnPosition = new Vector2(-position.X, position.Y);
            Vector2 sunburnOrigin = new Vector2(-origin.X, origin.Y);

            mSpriteContainer.Begin();

            SunburnCameraComponent_cl camera = (SunburnCameraComponent_cl)FNA.Managers.CameraManager_cl.Instance.ActiveCamera;

            Vector2 uvSize = new Vector2(mRectangle.Width, mRectangle.Height);
            Vector2 uvPosition = new Vector2(mRectangle.U, mRectangle.V);

            // Adjust the uv coordinates appropriately if we need to flip the sprite.
            if (flipped == true)
            {
                uvPosition.X += mRectangle.Width;
                uvSize.X = -uvSize.X;
            }

            /************************************************************************
             * TODO:
             * Figure out a way to change the alpha on sprites being drawn.
             *
             * Larsson Burch - 2011/11/17 - 1:37
             ************************************************************************/

            mSpriteContainer.Add(mBaseRenderableEffect, mSize, sunburnPosition, rotation - MathHelper.Pi, sunburnOrigin, uvSize, uvPosition, position.Z - camera.Position.Z);
            mSpriteContainer.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveSpriteObject()
        {
            FNA.Game_cl.BaseInstance.SceneInterface.ObjectManager.Remove(mSpriteContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddSpriteObject()
        {
            FNA.Game_cl.BaseInstance.SceneInterface.ObjectManager.Submit(mSpriteContainer);
        }
    }
 }

