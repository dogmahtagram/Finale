using System;

namespace FNA.Managers
{
    /// <summary>
    /// This interface is solely so that all component managers share a common base and can thus be added to the game's list of
    /// relevant managers.
    /// </summary>
    public class BaseManager_cl
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected BaseManager_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void PreUpdate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Overridden in derived managers to draw things.
        /// May be used simply for debug printing.
        /// </summary>
        public virtual void Draw()
        {
        }
    }
}
