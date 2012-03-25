using System;
using System.Collections.Generic;

using FNA.Components.Cameras;

namespace FNA.Managers
{
    /// <summary>
    /// A singleton manager class which handles the CameraComponents in FNA.
    /// Any camera-centric functionality should be contained within this class.
    /// </summary>
    public class CameraManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        private static readonly CameraManager_cl sInstance = new CameraManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static CameraManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// The currently active CameraComponent within the game world.
        /// </summary>
        /// <note>Only one CameraComponent can be 'active' at a time.</note>
        private CameraComponent_cl mActiveCamera;

        /// <summary>
        /// 
        /// </summary>
        public CameraComponent_cl ActiveCamera
        {
            get
            {
                return mActiveCamera;
            }
            set
            {
                mActiveCamera = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private CameraManager_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
        }

        /// <summary>
        /// Updates all Camera Components registered with this manager.
        /// </summary>
        public override void Update()
        {
            mActiveCamera.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        }
    }
}
