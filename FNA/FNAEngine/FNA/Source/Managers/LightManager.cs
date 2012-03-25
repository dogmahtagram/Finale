using System;
using System.Collections.Generic;

using FNA.Components;

namespace FNA.Managers
{
    /// <summary>
    /// A singleton manager class which handles all of the LightingComponents in FNA.
    /// Any lighting-centric functionality should be contained within this class.
    /// </summary>
    public class LightManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        private static readonly LightManager_cl sInstance = new LightManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static LightManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// The currently active Ambient Light within the game world.
        /// </summary>
        /// <note>Only one Ambient Light can be 'active' at a time.</note>
        private AmbientLightComponent mAmbientLight;

        /// <summary>
        /// 
        /// </summary>
        public AmbientLightComponent ActiveAmbientLight
        {
            get
            {
                return mAmbientLight;
            }
            set
            {
                mAmbientLight = value;
            }
        }

        /// <summary>
        /// The currently active Directional Light(s) within the game world.
        /// </summary>
        private List<DirectionalLightComponent> mDirectionalLights;

        /// <summary>
        /// 
        /// </summary>
        public List<DirectionalLightComponent> DirectionalLights
        {
            get
            {
                return mDirectionalLights;
            }
        }

        /// <summary>
        /// All of the Point Lights within the game world.
        /// </summary>
        private List<PointLightComponent> mPointLights;

        /// <summary>
        /// 
        /// </summary>
        public List<PointLightComponent> PointLights
        {
            get
            {
                return mPointLights;
            }
        }
        /************************************************************************
         * TODO:
         * Once a world partitioning system is in place, change all of the Lists over to use that syste, or at least reference it.
         * Obviously, once this change is made, a lot of additional stuff (both in this file and outside of it) needs to be changed.
         *
         * Jerad Dunn - 2011/11/05 - 20:26
         ************************************************************************/

        /// <summary>
        /// Constructor.
        /// </summary>
        private LightManager_cl()
        {
            // Initializing the lighting information.
            mAmbientLight = null;
            mDirectionalLights = new List<DirectionalLightComponent>();
            mPointLights = new List<PointLightComponent>();
        }


        /// <summary>
        /// Adds a Component to this manager's list of all PointLightComponents.
        /// Overrides IManager's implementation.
        /// </summary>
        /// <param name="c">The Component to add.</param>
        public void RegisterComponent(BaseLightComponent c)
        {
            if (c.GetType() == typeof(PointLightComponent))
            {
                mPointLights.Add((PointLightComponent)c);
            }
            else if (c.GetType() == typeof(DirectionalLightComponent))
            {
                mDirectionalLights.Add((DirectionalLightComponent)c);
            }
        }

        /// <summary>
        /// Removes a Component from this manager's list of all PointLightComponents.
        /// Overrides IManager's implementation.
        /// </summary>
        /// <param name="c"></param>
        public void UnregisterComponent(BaseLightComponent c)
        {
            if (c.GetType() == typeof(PointLightComponent))
            {
                mPointLights.Remove((PointLightComponent)c);
            }
            else if (c.GetType() == typeof(DirectionalLightComponent))
            {
                mDirectionalLights.Remove((DirectionalLightComponent)c);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
        }

        /// <summary>
        /// Updates all PointLightComponents registered with this manager.
        /// Overrides IManager's implementation.
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        }
    }
}
