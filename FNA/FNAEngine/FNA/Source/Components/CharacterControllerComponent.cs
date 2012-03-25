using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FNA.Managers;

namespace FNA.Components
{
    /// <summary>
    /// The character controller component is the base component for controllers which will define character specific logic.
    /// </summary>
    public class CharacterControllerComponent : Component
    {
        /// <summary>
        /// Default constructor - made protected so it will never be called (need to use the overloaded constructor).
        /// </summary>
        protected CharacterControllerComponent()
        {

        }

        /// <summary>
        /// Base constructor - registers the component and adds it to the entity.
        /// </summary>
        /// <param name="parent"></param>
        public CharacterControllerComponent(Entity parent)
            : base(parent, "Controller")
        {
            CharacterControllerManager.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// No need for initialization as of yet.
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
        /// Main update loop method.
        /// </summary>
        public virtual void Update()
        {

        }
    }
}
