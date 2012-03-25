using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Core;
using FNA.Interfaces;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Shadows;

namespace FNA.Components
{
    /// <summary>
    /// Contains shared variables and functionality of all light types.
    /// </summary>
    [Serializable]
    public class BaseLightComponent : Component_cl, IExclusiveComponent, ISerializable
    {
        ///
        public string Name { get; set; }
        ///
        public UpdateType UpdateType { get; set; }      // None, Automatic
        ///
        public bool LightEnabled { get; set; }
        ///
        public LightingType LightingType { get; set; }  // RealTime, BakedDown
        ///
        public Vector3 DiffuseColor { get; set; }
        ///
        public float Intensity { get; set; }
        ///
        public bool FillLight { get; set; }
        ///
        public float FalloffStrength { get; set; }
        
        ///
        public ShadowType ShadowType { get; set; }
        ///
        public float ShadowQuality { get; set; }
        ///
        public float ShadowPrimaryBias { get; set; }
        ///
        public float ShadowSecondaryBias { get; set; }
        ///
        public bool ShadowPerSurfaceLOD { get; set; }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        protected BaseLightComponent()
        {
        }

        /// <summary>
        /// Base constructor - registers the component and adds it to the entity.
        /// </summary>
        /// <param name="parent"></param>
        protected BaseLightComponent(Entity_cl parent)
            : base(parent)
        {
            Managers.LightManager_cl.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected BaseLightComponent(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            UpdateType = (UpdateType)(info.GetValue("UpdateType", typeof(UpdateType)));
            LightEnabled = info.GetBoolean("LightEnabled");
            LightingType = (LightingType)(info.GetValue("LightingType", typeof(LightingType)));
            DiffuseColor = (Vector3)(info.GetValue("DiffuseColor", typeof(Vector3)));
            Intensity = info.GetSingle("Intensity");
            FillLight = info.GetBoolean("FillLight");
            FalloffStrength = info.GetSingle("FalloffStrength");
            ShadowType = (ShadowType)(info.GetValue("ShadowType", typeof(ShadowType)));
            ShadowQuality = info.GetSingle("ShadowQuality");
            ShadowPrimaryBias = info.GetSingle("ShadowPrimaryBias");
            ShadowSecondaryBias = info.GetSingle("ShadowSecondaryBias");
            ShadowPerSurfaceLOD = info.GetBoolean("ShadowPerSurfaceLOD");
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("UpdateType", UpdateType);
            info.AddValue("LightEnabled", LightEnabled);
            info.AddValue("LightingType", LightingType);
            info.AddValue("DiffuseColor", DiffuseColor);
            info.AddValue("Intensity", Intensity);
            info.AddValue("FillLight", FillLight);
            info.AddValue("FalloffStrength", FalloffStrength);
            info.AddValue("ShadowType", ShadowType);
            info.AddValue("ShadowQuality", ShadowQuality);
            info.AddValue("ShadowPrimaryBias", ShadowPrimaryBias);
            info.AddValue("ShadowSecondaryBias", ShadowSecondaryBias);
            info.AddValue("ShadowPerSurfaceLOD", ShadowPerSurfaceLOD);
        }

        /// <summary>
        ///
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// The PreUpdate method will handle anything that needs to take place before the Update.
        /// </summary>
        public virtual void PreUpdate()
        {
        }

        /// <summary>
        /// Main update loop.
        /// </summary>
        public virtual void Update()
        {
        }
    }
}
