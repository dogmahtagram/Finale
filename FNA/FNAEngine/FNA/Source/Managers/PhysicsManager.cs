using System;

using Microsoft.Xna.Framework;

using FNA.Core;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace FNA.Managers
{
    /// <summary>
    /// The physics manager class is a wrapper to Farseer Physics, providing access to that physics library for other components in FNA.
    /// </summary>
    public class PhysicsManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The static instance of the physics manager class.  Accessed through FNA.Managers.PhysicsManager.Instance.
        /// </summary>
        private static readonly PhysicsManager_cl sInstance = new PhysicsManager_cl();

        /// <summary>
        /// Gets the static instance of the physics manager.
        /// </summary>
        public static PhysicsManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// The World object that we get from the Farseer physics engine which handles all physics objects.
        /// </summary>
        private World mPhysicsWorld;

        /// <summary>
        /// Gets the World object that handles all physics objects for Farseer.
        /// </summary>
        public World PhysicsWorld
        {
            get
            {
                return mPhysicsWorld;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private PhysicsManager_cl()
        {
            mPhysicsWorld = new World(new Vector2(0, -20f));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            mPhysicsWorld.Step((float)(Game_cl.BaseInstance.Timer.UnpausedElapsedSeconds));
        }

        /// <summary>
        /// Clear all bodies out of the physics world. Called when loading a new scene.
        /// </summary>
        public void ClearPhysics()
        {
            mPhysicsWorld.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        public void RemovePhysicsBody(Body body)
        {
            /************************************************************************
             * TODO:
             * Make this work.
             *
             * Jay Sternfield	-	2011/12/06
             ************************************************************************/
            //mPhysicsWorld.RemoveBody(body);
        }
    }
}
