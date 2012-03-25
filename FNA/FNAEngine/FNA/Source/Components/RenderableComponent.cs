/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// TODO: Probably change they way textures and whatnot are handled in this component after discussing with Larsson [JSD 09/29/11]
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using FNA.Core;
using FNA.Managers;
using FNA.Graphics;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// The RenderableComponent ties the rendering of sprites into the component system.
    /// </summary>
    [Serializable]
    public class RenderableComponent_cl : Component_cl, ISerializable, IExclusiveComponent
    {
        /// <summary>
        /// mSprite is the sprite associated with this renderable component.
        /// </summary>
        private SunburnSprite_cl mSprite;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public SunburnSprite_cl Sprite
        {
            get
            {
                return mSprite;
            }
            set
            {
                mSprite = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="colorTexture"></param>
        /// <param name="normalMap"></param>
        /// <param name="offsetMap"></param>
        public RenderableComponent_cl(Entity_cl parent, string colorTexture, string normalMap, string offsetMap) : base(parent)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // TODO: Add in a debug assert that the parent Entity has a PositionComponent.  Make the RenderableComponent require that an Entity has
            //       a PositionComponent before it can be added.
            // [JSD 09/28/11]
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
            
            RenderableManager_cl.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);

            mSprite = new SunburnSprite_cl();
            mSprite.LoadContent(colorTexture);
            mSprite.InitSunburnStuff();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="colorTexture"></param>
        public RenderableComponent_cl(Entity_cl parent, string colorTexture)
            : base(parent)
        {
            RenderableManager_cl.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);

            mSprite = new SunburnSprite_cl();
            mSprite.LoadContent(colorTexture);
            mSprite.InitSunburnStuff();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public RenderableComponent_cl(Entity_cl parent)
            : base(parent)
        {
            RenderableManager_cl.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);

            mSprite = new SunburnSprite_cl();
            mSprite.InitSunburnStuff();
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorTexture"></param>
        public void LoadSpriteContent(string colorTexture)
        {
            mSprite.LoadContent(colorTexture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorTexture"></param>
        /// <param name="normalMap"></param>
        /// <param name="offsetMap"></param>
        public void LoadSpriteContent(string colorTexture, string normalMap, string offsetMap)
        {
            mSprite.LoadContent(colorTexture);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected RenderableComponent_cl(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            mSprite = (SunburnSprite_cl)info.GetValue("Sprite", typeof(SunburnSprite_cl));

            //mSprite = new SunburnSprite_cl();
            //mSprite.Rectangle = (FloatRectangle)info.GetValue("Rectangle", typeof(FloatRectangle));
            //mSprite.Size = (Vector2)info.GetValue("SpriteSize", typeof(Vector2));

            if (FNA.Game_cl.IsPlayingGame == true)
            {
                mSprite.LoadContent(mSprite.MaterialName);
                mSprite.InitSunburnStuff();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public override void AssignOwnership(Entity_cl parent)
        {
            base.AssignOwnership(parent);

            RenderableManager_cl.Instance.RegisterComponent(this);
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Sprite", mSprite);
            //info.AddValue("TextureName", mSprite.MaterialName);
            //info.AddValue("Rectangle", mSprite.Rectangle, typeof(FloatRectangle));
            //info.AddValue("SpriteSize", mSprite.Size, typeof(Vector2));
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw()
        {
            bool flipSprite = false;
            RotationComponent rotation = ((RotationComponent)mParentEntity.GetComponentOfType(typeof(RotationComponent)));
            if (rotation != null && rotation.Direction.X < 0)
            {
                flipSprite = true;
            }

            PositionComponent_cl position = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            PhysicsComponent_cl physics = ((PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl)));

            if (physics != null && physics.IsTerrain)
            {
                // texture to vertices
                Vector2 origin = Vector2.Zero;// physics.mTextureToVerticesOrigin;
                mSprite.Draw(position.Position3D, origin, flipSprite, position.Rotation);
            }
            else
            {
                mSprite.Draw(position.Position3D, flipSprite, position.Rotation);
            }
        }
    }
}
