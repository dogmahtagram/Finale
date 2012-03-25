using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.IO;

using FNA;
using FNA.Components;
using FNA.Interfaces;
using FNA.Managers;

namespace FNA.Core
{
    /// <summary>
    /// Entities represent all the unique, different “things” in the game world.
    /// They have no data or methods in themselves except a unique identifier and a list of Components.
    /// Warning: derived classes should still encapsulate all functionality in Components.
    /// </summary>
    [Serializable]
    public class Entity_cl : ISerializable
    {
        ///         
        protected long mUniqueID;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.ReadOnly(true)]
        [System.ComponentModel.Description("The unique ID assigned to this Entity by the Game. (Read only)")]
        public long ID
        {
            get
            {
                return mUniqueID;
            }
            set
            {
                mUniqueID = value;
            }
        }

        /// 
        protected string mName;
        /// <summary>
        /// 
        /// </summary>
        //[System.ComponentModel.ReadOnly(true)]
        [System.ComponentModel.Description("The Entity's name. Change by double-clicking the name in the object panel.")]
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<Type, List<Component_cl>> mComponents;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("The Dictionary of Components on this Entity. Click '...' to edit.")]
        public Dictionary<Type, List<Component_cl>> Components
        {
            get
            {
                return mComponents;
            }
        }

#if WORLD_EDITOR
        /// <summary>
        /// The active entity when in edit mode.
        /// </summary>
        protected bool mActiveForEditing;
        /// <summary>
        /// Gets or sets whether this entity is currently active for editing.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public bool ActiveForEditing
        {
            get
            {
                return mActiveForEditing;
            }
            set
            {
                mActiveForEditing = value;
            }
        }
#endif

        ///// <summary>
        ///// Simple array container is used for fastest possible access.
        ///// ComponentManager assigns a unique ID to each Component type, so
        ///// this array acts like a super bit mask, where unused Components are set to null.
        ///// </summary>
        //protected Component_cl[] mComponents = new Component_cl[ComponentManager_cl.NumComponents];
        //public Component_cl[] Components
        //{
        //    get
        //    {
        //        return mComponents;
        //    }
        //}

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Entity_cl()
        {
            mComponents = new Dictionary<Type, List<Component_cl>>();
            mUniqueID = EntityManager_cl.Instance.RegisterNewEntity(this);
            mName = "Entity" + mUniqueID;

            // Initialize all component slots to null (empty)
            //for (int i = 0; i < mComponents.Length; i++)
            //{
            //    mComponents[i] = null;
            //}
        }

        /// <summary>
        /// Constructor.
        /// Automagically adds a PositionComponent with given position.
        /// </summary>
        /// <param name="x">Initial X position of the Entity.</param>
        /// <param name="y">Initial Y position of the Entity.</param>
        public Entity_cl(float x, float y)
        {
            mUniqueID = EntityManager_cl.Instance.RegisterNewEntity(this);
            mName = "Entity" + mUniqueID;

            // Initialize all component slots to null (empty)
            //for (int i = 0; i < mComponents.Length; i++)
            //{
            //    mComponents[i] = null;
            //}

            PositionComponent_cl pc = new PositionComponent_cl(this, x, y);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Entity_cl()
        {
            foreach (Type type in mComponents.Keys)
            {
                foreach (Component_cl component in mComponents[type])
                {
                    ComponentManager_cl.RemoveComponent(component);
                }
            }

            mComponents.Clear();
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected Entity_cl(SerializationInfo info, StreamingContext context)
        {
            mUniqueID = EntityManager_cl.Instance.RegisterNewEntity(this);
            mName = info.GetString("Name");
            /************************************************************************
             * TODO:
             * Do we need to check if an Entity already exists with the same mName?
             * Maybe not because everything uses mUniqueID, as I can recall...
             *
             * Jay Sternfield	-	2011/11/29
             ************************************************************************/

            mComponents = new Dictionary<Type, List<Component_cl>>();

            /************************************************************************
             * TODO:
             * Do we need to get the PositionComponent first?
             *
             * Jay Sternfield	-	2011/11/08
             ************************************************************************/

            int count = info.GetInt32("Component Count");
            for (int i = 0; i < count; i++)
            {
                string typeName = "Type" + i.ToString();
                string componentName = "Component" + i.ToString();
                Type type = (Type)(info.GetValue(typeName, typeof(Type)));
                List<Component_cl> componentList = (List<Component_cl>)(info.GetValue(componentName, typeof(List<Component_cl>)));

                foreach (Component_cl component in componentList)
                {
                    component.AssignOwnership(this);
                }
            }

            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (Component_cl component in componentList)
                {
                    if (typeof(FNA.Interfaces.IInitializeAble).IsAssignableFrom(component.GetType()))
                    {
                        ((FNA.Interfaces.IInitializeAble)component).Initialize();
                    }
                }
            }

            if (Game_cl.IsPlayingGame && GetComponentOfType(typeof(AnimatedComponent_cl)) != null)
            {
                //((AnimatedComponent_cl)GetComponentOfType(typeof(AnimatedComponent_cl))).QueueAnimation(((AnimatedComponent_cl)GetComponentOfType(typeof(AnimatedComponent_cl))).InitialAnimation);
            }
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);

            info.AddValue("Component Count", mComponents.Count);

            /************************************************************************
             * TODO:
             * Should we insert the PositionComponent first?
             *
             * Jay Sternfield	-	2011/11/08
             ************************************************************************/

            int count = 0;
            foreach (KeyValuePair<Type, List<Component_cl>> componentPair in mComponents)
            {
                string typeName = "Type" + count.ToString();
                string componentName = "Component" + count.ToString();
                info.AddValue(typeName, componentPair.Key);
                info.AddValue(componentName, componentPair.Value);
                count++;
            }
        }

        /// <summary>
        /// Adds a Component to this Entity.
        /// Overwrites any exclusive Component of the same type as the one being added.
        /// Adds to the list for non-exclusive Components.
        /// </summary>
        /// <param name="component">The Component to add.</param>
        public void AddComponent(Component_cl component)
        {
            Type type = component.GetType();
            if (mComponents.ContainsKey(type) == false)
            {
                mComponents.Add(type, new List<Component_cl>());
                mComponents[type].Add(component);
            }

            // Component of matching type already exists
            else if (typeof(IExclusiveComponent).IsAssignableFrom(type))
            {
                mComponents[type][0] = component;
            }
            else
            {
                mComponents[type].Add(component);
            }
        }

        /// <summary>
        /// Removes the Component of the given type from this Entity.
        /// If there are multiple, all are removed.
        /// </summary>
        /// <param name="type">The Component type.</param>
        public void RemoveComponent(Type type)
        {
            if (mComponents.ContainsKey(type))
            {
                if (typeof(IExclusiveComponent).IsAssignableFrom(type))
                {
                    ComponentManager_cl.RemoveComponent(mComponents[type][0]);
                }
                else
                {
                    foreach (Component_cl component in mComponents[type])
                    {
                        ComponentManager_cl.RemoveComponent(component);
                    }
                }

                mComponents[type].Clear();
            }
        }

        /// <summary>
        /// Removes the Component from this Entity.
        /// </summary>
        /// <param name="component">The Component.</param>
        public void RemoveComponent(Component_cl component)
        {
            Type type = component.GetType();
            if (mComponents.ContainsKey(type))
            {
                ComponentManager_cl.RemoveComponent(component);
                mComponents[type].Remove(component);
                
                // Remove an empty Component list of the type
                if (mComponents[type].Count == 0)
                {
                    mComponents.Remove(type);
                }
            }
        }

        /// <summary>
        /// Replaces the component of the corresponding type from this entity and takes the old component out of the manager.
        /// If the component type does not exist or is non-exclusive, this method adds the given component to its list.
        /// </summary>
        /// <param name="component">The Component to replace with.</param>
        public void ReplaceComponent(Component_cl component)
        {
            Type type = component.GetType();
            if (mComponents.ContainsKey(type))
            {
                ComponentManager_cl.RemoveComponent(component);
                if (typeof(IExclusiveComponent).IsAssignableFrom(type))
                {
                    mComponents[type][0] = component;
                }
                else
                {
                    mComponents[type].Add(component);
                }
            }
            else
            {
                mComponents[type].Add(component);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllComponents()
        {
            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (Component_cl component in componentList)
                {
                    ComponentManager_cl.RemoveComponent(component);
                }
            }
        }

        /// <summary>
        /// Get the Component of the given type, if it is Exclusive or there is only one.
        /// </summary>
        /// <param name="type">The Component type to get.</param>
        /// <returns>The Component of the given type.</returns>
        public Component_cl GetComponentOfType(Type type)
        {
            Component_cl component = null;

            foreach (Type key in mComponents.Keys)
            {
                if (type.IsAssignableFrom(key))
                {
                    if (typeof(IExclusiveComponent).IsAssignableFrom(type) || (mComponents[key].Count == 1))
                    {
                        component = mComponents[key][0];
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("ERROR: Try GetActiveComponentsOfType() for type " + type.ToString());
                    }
                    break;
                }
            }

            return component;
        }

        /// <summary>
        /// Get the list of active Components of the given type.
        /// </summary>
        /// <param name="type">The Component type to get.</param>
        /// <returns>The list of active Components.</returns>
        public List<Component_cl> GetActiveComponentsOfType(Type type)
        {
            List<Component_cl> componentList = new List<Component_cl>();

            foreach (Type key in mComponents.Keys)
            {
                if (type.IsAssignableFrom(key))
                {
                    foreach (Component_cl component in (mComponents[type]).Where(c => c.Enabled))
                    {
                        componentList.Add(component);
                    }
                }
            }

            if (componentList.Count > 0) return componentList;
            return null;
        }
    }
}
