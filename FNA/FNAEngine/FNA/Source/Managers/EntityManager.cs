using System;
using System.Collections.Generic;

using FNA.Core;

namespace FNA.Managers
{
    /************************************************************************
     * TODO:
     * 
     *
     * Larsson Burch - 2011/11/02 - 22:45
     ************************************************************************/
    /// <summary>
    /// 
    /// </summary>
    public class EntityManager_cl : BaseManager_cl
    {
        private static readonly EntityManager_cl sInstance = new EntityManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static EntityManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        private long mUniqueID = 0;

        /// 
        private List<Entity_cl> mEntities = new List<Entity_cl>();

        /// <summary>
        /// 
        /// </summary>
        public List<Entity_cl> Entities
        {
            get
            {
                return mEntities;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private EntityManager_cl()
        {
        }

        /// <summary>
        /// Adds an Entity to this manager's list of all Entities in the game.
        /// </summary>
        /// <param name="e">The Entity to add.</param>
        /// <returns>The unique ID assigned to the new Entity.</returns>
        public long RegisterNewEntity(Entity_cl e)
        {
            mEntities.Add(e);

            return mUniqueID++;
        }
    }
}
