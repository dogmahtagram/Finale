using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FNA.Triggers;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerManager_cl : BaseManager_cl
    {
        /// <summary>
        /// 
        /// </summary>
        public enum TRIGGER_TYPES
        {
            /// 
            HIT_BOX_TRIGGER = 0,
        };

        private List<TriggerEntity_cl> mTriggers = new List<TriggerEntity_cl>();

        private List<TriggerEntity_cl> mTriggersToRemove = new List<TriggerEntity_cl>();

        private static readonly TriggerManager_cl sInstance = new TriggerManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static TriggerManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected TriggerManager_cl()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trigger"></param>
        public void AddTrigger(TriggerEntity_cl trigger)
        {
            mTriggers.Add(trigger);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trigger"></param>
        public void QueueRemove(TriggerEntity_cl trigger)
        {
            mTriggersToRemove.Add(trigger);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
            // Remove all triggers that "died" last update
            mTriggersToRemove.ForEach(i => mTriggers.Remove(i));
            mTriggersToRemove.Clear();

            // Update all remaining triggers
            mTriggers.ForEach(i => i.PreUpdate());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            mTriggers.ForEach(i => i.Update());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            mTriggers.ForEach(i => i.Draw());
        }
    }
}
