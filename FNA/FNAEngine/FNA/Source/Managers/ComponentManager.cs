using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FNA.Components;
using FNA.Components.Cameras;
using FNA.Graphics;
using FNA.Interfaces;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The static instance of this class.
        /// </summary>
        private static readonly ComponentManager_cl mInstance = new ComponentManager_cl();

        /// <summary>
        /// Accessor for the static instance.
        /// </summary>
        public static ComponentManager_cl Instance
        {
            get
            {
                return mInstance;
            }
        }

        /// <summary>
        /// ComponentManager is the base manager for all components in FNA.  Instead of having
        /// sub managers for every component type - physics, controllers, etc - this base manager
        /// will be the sole place for querying collections of components.
        /// </summary>
        private static Dictionary<Type, List<Component_cl>> mComponents = new Dictionary<Type, List<Component_cl>>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<Type, List<Component_cl>> Components
        {
            get
            {
                return mComponents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static List<CharacterControllerComponent_cl> mControllerComponents = new List<CharacterControllerComponent_cl>();

        /// <summary>
        /// 
        /// </summary>
        public static List<CharacterControllerComponent_cl> ControllerComponents
        {
            get
            {
                return mControllerComponents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static List<BaseLightComponent> mLightComponents = new List<BaseLightComponent>();

        /// <summary>
        /// 
        /// </summary>
        public static List<BaseLightComponent> LightComponents
        {
            get
            {
                return mLightComponents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static List<CameraComponent_cl> mCameraComponents = new List<CameraComponent_cl>();

        /// <summary>
        /// 
        /// </summary>
        public static List<CameraComponent_cl> CameraComponents
        {
            get
            {
                return mCameraComponents;
            }
        }

        private static List<Component_cl> mPreAddedComponents = new List<Component_cl>();

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private ComponentManager_cl()
        {
        }

        /// <summary>
        /// Add a component to the pre added component dictionary.
        /// </summary>
        /// <param name="newComponent">the new component added to the world</param>
        public static void PreAddComponent(Component_cl newComponent)
        {
            if (mPreAddedComponents.Contains(newComponent) == false)
            {
                mPreAddedComponents.Add(newComponent);
            }
        }

        /// <summary>
        /// Add a component to the master dictionary of all components matched with their string class name.
        /// </summary>
        /// <param name="newComponent">the new component added to the world</param>
        private static void AddComponent(Component_cl newComponent)
        {
            if (mComponents.ContainsKey(newComponent.GetType()))
            {
                mComponents[newComponent.GetType()].Add(newComponent);
            }
            else
            {
                List<Component_cl> componentList = new List<Component_cl>();
                componentList.Add(newComponent);
                mComponents.Add(newComponent.GetType(), componentList);
            }
        }

        /// <summary>
        /// Remove a component from the manager if it exists in the master dictionary.
        /// </summary>
        /// <param name="component">the component to remove from the master dictionary.</param>
        public static void RemoveComponent(Component_cl component)
        {
            /************************************************************************
             * TODO:
             * Verify that this does not need to store the component to be deleted
             * in another list to then remove on next PreUpdate.
             *
             * Jay Sternfield	-	2011/12/06
             ************************************************************************/
            Type componentType = component.GetType();
            //if (mComponents.ContainsKey(componentType))
            //{
            //    if (mComponents[componentType].Contains(deadComponent))
            //    {
                    //if (componentType == typeof(RenderableComponent_cl))
                    //{
                        //RenderableManager_cl.Instance.UnregisterComponent((RenderableComponent_cl)component);
                        /************************************************************************
                         * TODO:
                         * Remove the sprite.
                         *
                         * Jay Sternfield	-	2011/12/06
                         ************************************************************************/
                    //}
                    //else if (componentType == typeof(PhysicsComponent_cl))
                    //{
                    //    PhysicsManager_cl.Instance.RemovePhysicsBody(((PhysicsComponent_cl)(mComponents[typeof(PhysicsComponent_cl)][0])).PhysicsBody);
                    //}
                    if (mComponents.Keys.Contains(componentType))
                    {
                        mComponents[componentType].Remove(component);
                    }
                //}
            //}
        }

        /// <summary>
        /// Gets a list of components associated with a specified component class type.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <returns>A list of components of the specified type.</returns>
        public static List<Component_cl> GetEnabledComponents(Type type)
        {
            if (mComponents.ContainsKey(type))
            {
                List<Component_cl> enabledComponentList = new List<Component_cl>();

                foreach (Component_cl component in (mComponents[type]).Where(c => c.Enabled))
                {
                    enabledComponentList.Add(component);
                }

                return enabledComponentList;
            }
            else
            {
                return new List<Component_cl>();
            }
        }

        /// <summary>
        /// Run the PreUpdate function on all components that implement one.
        /// </summary>
        public override void PreUpdate()
        {
            foreach (Component_cl component in mPreAddedComponents)
            {
                AddComponent(component);
            }
            mPreAddedComponents.Clear();

            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (IPreUpdateAble preUpdatable in componentList.Where(component => component.Enabled && typeof(IPreUpdateAble).IsAssignableFrom(component.GetType())))
                {
                    preUpdatable.PreUpdate();
                }
            }
        }

        /// <summary>
        /// Run the update function on all components that implement one.
        /// </summary>
        public override void Update()
        {
            if (mComponents.ContainsKey((Type)typeof(AnimatedComponent_cl)))
            {
                foreach (AnimatedComponent_cl component in mComponents[(Type)typeof(AnimatedComponent_cl)])
                {
                    component.Update();
                }
            }

            if (mComponents.ContainsKey((Type)typeof(PhysicsComponent_cl)))
            {
                foreach (PhysicsComponent_cl component in mComponents[(Type)typeof(PhysicsComponent_cl)])
                {
                    component.Update();
                }
            }

            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (IUpdateAble updatable in componentList.Where(component => component.Enabled && typeof(IUpdateAble).IsAssignableFrom(component.GetType())))
                {
                    updatable.Update();
                }
            }
        }

        /// <summary>
        /// Run the Draw function on all components that implement one.
        /// </summary>
        public override void Draw()
        {
            foreach (List<Component_cl> componentList in mComponents.Values)
            {
                foreach (IDrawAble drawable in componentList.Where(component => component.Enabled && typeof(IDrawAble).IsAssignableFrom(component.GetType())))
                {
                    drawable.Draw();
                }
            }            
        }

        /// <summary>
        /// Release references to all existing Components.
        /// </summary>
        public static void Clear()
        {
            mPreAddedComponents.Clear();
            mComponents.Clear();
        }
    }
}
