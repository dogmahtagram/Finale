using System;

namespace FNA.Interfaces
{
    /// <summary>
    /// IPreUpdatable specifies a PreUpdate function for all components that will implement one.
    /// </summary>
    public interface IPreUpdateAble
    {
        /// <summary>
        /// 
        /// </summary>
        void PreUpdate();
    }
}
