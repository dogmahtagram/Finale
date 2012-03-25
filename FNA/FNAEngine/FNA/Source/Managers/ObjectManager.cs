using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SynapseGaming.LightingSystem.Rendering;

namespace FNA.Managers
{
    /// <summary>
    /// ObjectManager_cl overrides Sunburn's ObjectManager which provides access to custom culling. The object
    /// manager is where FNA can receive information about which objects are within a given region.
    /// </summary>
    public class ObjectManager_cl : ObjectManager
    {
        /// <summary>
        /// Overriden find to implement custom culling.
        /// </summary>
        /// <param name="foundobjects"></param>
        /// <param name="worldbounds"></param>
        /// <param name="objectfilter"></param>
        public override void Find(List<SceneEntity> foundobjects, Microsoft.Xna.Framework.BoundingBox worldbounds, SynapseGaming.LightingSystem.Core.ObjectFilter objectfilter)
        {
            foundobjects.AddRange(DynamicObjects.Keys);
            //base.Find(foundobjects, worldbounds, objectfilter);
        }

        /// <summary>
        /// Overriden find to implement custom culling.
        /// </summary>
        /// <param name="foundobjects"></param>
        /// <param name="worldbounds"></param>
        /// <param name="objectfilter"></param>
        public override void Find(List<SceneEntity> foundobjects, Microsoft.Xna.Framework.BoundingFrustum worldbounds, SynapseGaming.LightingSystem.Core.ObjectFilter objectfilter)
        {
            foundobjects.AddRange(DynamicObjects.Keys);
            //base.Find(foundobjects, worldbounds, objectfilter);
        }

        /// <summary>
        /// Overriden find to implement custom culling.
        /// </summary>
        /// <param name="foundobjects"></param>
        /// <param name="objectfilter"></param>
        public override void Find(List<SceneEntity> foundobjects, SynapseGaming.LightingSystem.Core.ObjectFilter objectfilter)
        {
            foundobjects.AddRange(DynamicObjects.Keys);
            //base.Find(foundobjects, objectfilter);
        }

        /// <summary>
        /// Overriden find to implement custom culling.
        /// </summary>
        /// <param name="foundobjects"></param>
        public override void FindFast(List<SceneEntity> foundobjects)
        {
            foundobjects.AddRange(DynamicObjects.Keys);
            //base.FindFast(foundobjects);
        }

        /// <summary>
        /// Overriden find to implement custom culling.
        /// </summary>
        /// <param name="foundobjects"></param>
        /// <param name="worldbounds"></param>
        public override void FindFast(List<SceneEntity> foundobjects, Microsoft.Xna.Framework.BoundingBox worldbounds)
        {
            foundobjects.AddRange(DynamicObjects.Keys);
            //base.FindFast(foundobjects, worldbounds);
        }
    }
}
