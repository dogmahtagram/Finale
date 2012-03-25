using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FNA.Core;

namespace FNA.Scripts
{
    /// <summary>
    /// INode is the base interface for a node. It is used by triggers and script nodes.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="next"></param>
        void ProcessNode(Entity_cl entity, string next);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        void SetInputValue(object input);
    }
}
