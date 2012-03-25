using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

namespace FNA.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyFrameDictionary : Dictionary<int, FloatRectangle>//, ISerializable
    {
        private string mAnimationName;

        /// <summary>
        /// 
        /// </summary>
        public string AnimationName
        {
            get
            {
                return mAnimationName;
            }
            set
            {
                mAnimationName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public KeyFrameDictionary() : base()
        {

        }

        //public KeyFrameDictionary(SerializationInfo info, StreamingContext context)
        //{
        //    var = info.GetInt32("Var");
        //}

        //[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("Var", var);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ToString(int key)
        {
            return mAnimationName + this[key].ToString();
        }
    }
}
