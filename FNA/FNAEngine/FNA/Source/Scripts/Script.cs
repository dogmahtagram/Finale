using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FNA.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Script : List<ScriptRoutine>
    {
        private string mName;

        /// <summary>
        /// 
        /// </summary>
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
        public Script() : base()
        {

        }
    }
}
