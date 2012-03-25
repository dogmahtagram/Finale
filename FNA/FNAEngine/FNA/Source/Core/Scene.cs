using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Components;
using FNA.Managers;

namespace FNA.Core
{
    /// <summary>
    /// A class that handles the contents of a scene.
    /// </summary>
    [Serializable]
    public class Scene_cl : ISerializable
    {
        /// The ambient light in the scene.
        private Entity_cl mAmbientLight;
        /// <summary>
        /// Accessor for the ambient light.
        /// </summary>
        public Entity_cl AmbientLight
        {
            get
            {
                return mAmbientLight;
            }
        }

        /// The list of lights in the scene.
        private List<Entity_cl> mLights;
        /// <summary>
        /// Mutator for the lights.
        /// </summary>
        public List<Entity_cl> Lights
        {
            get
            {
                return mLights;
            }
            set
            {
                mLights = value;
            }
        }

        /// The list of entities in the scene.
        private List<Entity_cl> mEntities;
        /// <summary>
        /// Mutator for the entities.
        /// </summary>
        public List<Entity_cl> Entities
        {
            get
            {
                return mEntities;
            }
            set
            {
                mEntities = value;
            }
        }

        /// The list of layers in the scene.
        private List<Entity_cl> mLayers;
        /// <summary>
        /// Mutator for the layers.
        /// </summary>
        public List<Entity_cl> Layers
        {
            get
            {
                return mLayers;
            }
            set
            {
                mLayers = value;
            }
        }

        /************************************************************************
         * TODO:
         * What else goes into making a scene?
         *
         * Jay Sternfield	-	2011/11/05
         ************************************************************************/

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Scene_cl()
        {
            mAmbientLight = new Entity_cl();
            AmbientLightComponent ambientComponent = new AmbientLightComponent(mAmbientLight, 0.0f, Color.White.ToVector3(), 1.0f);
            LightManager_cl.Instance.ActiveAmbientLight = ambientComponent;
            mLights = new List<Entity_cl>();
            mEntities = new List<Entity_cl>();
            mLayers = new List<Entity_cl>();
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">The serialization info to deserialize with.</param>
        /// <param name="context">The context in which to deserialize...?</param>
        protected Scene_cl(SerializationInfo info, StreamingContext context)
        {
            mAmbientLight = (Entity_cl)info.GetValue("AmbientLight", typeof(Entity_cl));
            LightManager_cl.Instance.ActiveAmbientLight = (AmbientLightComponent)mAmbientLight.GetComponentOfType(typeof(AmbientLightComponent));

            mLights = (List<Entity_cl>)info.GetValue("Lights", typeof(List<Entity_cl>));
            mEntities = (List<Entity_cl>)info.GetValue("Prefabs", typeof(List<Entity_cl>));
            mLayers = (List<Entity_cl>)info.GetValue("Layers", typeof(List<Entity_cl>));
        }

        /// <summary>
        /// Fills a serialization info object from this class.
        /// </summary>
        /// <param name="info">The serialization info to serialize with.</param>
        /// <param name="context">The context in which to serialize...?</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AmbientLight", mAmbientLight);
            info.AddValue("Lights", mLights);
            info.AddValue("Prefabs", mEntities);
            info.AddValue("Layers", mLayers);
        }

        /// <summary>
        /// Sets the ambient light, given another light.
        /// </summary>
        /// <param name="light">The new ambient light.</param>
        public void SetAmbientLight(Entity_cl light)
        {
            if (light.Components.ContainsKey(typeof(AmbientLightComponent)))
            {
                mAmbientLight = light;
            }
#if DEBUG
            else
            {
                System.Windows.Forms.MessageBox.Show("ERROR: Entity must contain AmbientLightComponent");
            }
#endif
        }

        /// <summary>
        /// Changes the settings for the ambient light in the Scene.
        /// </summary>
        /// <param name="color">The new color.</param>
        /// <param name="intensity">The new intensity.</param>
        public void SetAmbientLight(Vector3 color, float intensity)
        {
            AmbientLightComponent ambientLight = (AmbientLightComponent)mAmbientLight.GetComponentOfType(typeof(AmbientLightComponent));
            ambientLight.DiffuseColor = color;
            ambientLight.Intensity = intensity;
        }

        /// <summary>
        /// Changes the color of the ambient light.
        /// </summary>
        /// <param name="color">The new colorr.</param>
        public void SetAmbientLightIntensity(Vector3 color)
        {
            AmbientLightComponent ambientLight = (AmbientLightComponent)mAmbientLight.GetComponentOfType(typeof(AmbientLightComponent));
            ambientLight.DiffuseColor = color;
        }

        /// <summary>
        /// Changes the intensity of the ambient light.
        /// </summary>
        /// <param name="intensity">The new intensity.</param>
        public void SetAmbientLightIntensity(float intensity)
        {
            AmbientLightComponent ambientLight = (AmbientLightComponent)mAmbientLight.GetComponentOfType(typeof(AmbientLightComponent));
            ambientLight.Intensity = intensity;
        }

        /// <summary>
        /// Adds a non-light Entity to the Scene's list of Entities.
        /// </summary>
        /// <param name="entity">The Entity to add to the Scene.</param>
        public void AddEntity(Entity_cl entity)
        {
            /************************************************************************
             * TODO:
             * What do we need to safeguard against here?
             *
             * Jay Sternfield	-	2011/11/05
             ************************************************************************/

            if (entity.Components.ContainsKey(typeof(AmbientLightComponent)))
            {
                System.Windows.Forms.MessageBox.Show("ERROR: THere can only be one ambient light.\nUse SetAmbientLight to replace it.");
            }
            else if (entity.Components.ContainsKey(typeof(PointLightComponent))
                    || entity.Components.ContainsKey(typeof(DirectionalLightComponent)))
            {
                /************************************************************************
                 * TODO:
                 * Do we need to put restrictions on the number of lights we can have?
                 * 
                 * Jay Sternfield	-	2011/11/05
                 ************************************************************************/
                mLights.Add(entity);
            }
            else
            {
                mEntities.Add(entity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity_cl entity)
        {
            entity.RemoveAllComponents();

            mEntities.Remove(entity);
        }

        /// <summary>
        /// Adds a layer Entity to the Scene's list of Layers.
        /// </summary>
        /// <param name="layer">The layer Entity to add to the Scene.</param>
        public void AddLayer(Entity_cl layer)
        {
            /************************************************************************
             * TODO:
             * What is a layer?
             *
             * Jay Sternfield	-	2011/11/05
             ************************************************************************/
            mLayers.Add(layer);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            mAmbientLight = null;
            mLights.Clear();
            mEntities.Clear();
            mLayers.Clear();
        }
    }
}
