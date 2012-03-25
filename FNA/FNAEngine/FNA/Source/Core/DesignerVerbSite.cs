using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;

namespace FNA.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DesignerVerbSite_cl : IMenuCommandService, ISite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verb"></param>
        public void AddVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        public MenuCommand FindCommand(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        public bool GlobalInvoke(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void RemoveCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verb"></param>
        public void RemoveVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
            throw new NotImplementedException();
        }

        // our target object
        /// <summary>
        /// 
        /// </summary>
        protected object _Component;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public DesignerVerbSite_cl(object component)
        {
            _Component = component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMenuCommandService))
                return this;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection Verbs = new DesignerVerbCollection();
                // Use reflection to enumerate all the public methods on the object

                MethodInfo[] mia = _Component.GetType().GetMethods
                        (BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo mi in mia)
                {
                    // Ignore any methods without a [Browsable(true)] attribute
                    object[] attrs = mi.GetCustomAttributes
                            (typeof(BrowsableAttribute), true);
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    if (!((BrowsableAttribute)attrs[0]).Browsable)
                        continue;
                    // Add a DesignerVerb with our VerbEventHandler
                    // The method name will appear in the command pane
                    Verbs.Add(new DesignerVerb(mi.Name,
                        new EventHandler(VerbEventHandler)));
                }
                return Verbs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerbEventHandler(object sender, EventArgs e)
        {
            // The verb is the sender
            DesignerVerb verb = sender as DesignerVerb;
            // Enumerate the methods again to find the one named by the verb
            MethodInfo[] mia = _Component.GetType().GetMethods
                    (BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo mi in mia)
            {
                object[] attrs = mi.GetCustomAttributes
                        (typeof(BrowsableAttribute), true);
                if (attrs == null || attrs.Length == 0)
                    continue;
                if (!((BrowsableAttribute)attrs[0]).Browsable)
                    continue;
                if (verb.Text == mi.Name)
                {
                    // Invoke the method on our object (no parameters)
                    mi.Invoke(_Component, null);
                    return;
                }
            }
        }

        #region ISite Members
        // ISite required to represent this object directly to the PropertyGrid

        /// <summary>
        /// 
        /// </summary>
        public IComponent Component
        {
            get { throw new NotImplementedException(); }
        }

        // ** Item of interest ** Implement the Container property
        /// <summary>
        /// 
        /// </summary>
        public IContainer Container
        {
            // Returning a null Container works fine in this context
            get { return null; }
        }

        // ** Item of interest ** Implement the DesignMode property
        /// <summary>
        /// 
        /// </summary>
        public bool DesignMode
        {
            // While this *is* called, it doesn't seem to matter whether we return true or false
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
