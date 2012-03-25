using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

namespace FNA.Graphics
{
    /// <summary>
    /// A rectangle with float width and height.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct FloatRectangle : ISerializable
    {
        private float mU;
        /// <summary>
        /// 
        /// </summary>
        public float U
        {
            get
            {
                return mU;
            }
            set
            {
                mU = value;
            }
        }

        private float mV;
        /// <summary>
        /// 
        /// </summary>
        public float V
        {
            get
            {
                return mV;
            }
            set
            {
                mV = value;
            }
        }

        private float mWidth;
        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get
            {
                return mWidth;
            }
            set
            {
                mWidth = value;
            }
        }

        private float mHeight;
        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public FloatRectangle(float u, float v, float width, float height)
        {
            mU = u;
            mV = v;
            mWidth = width;
            mHeight = height;
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        public FloatRectangle(SerializationInfo info, StreamingContext context)
        {
            mU = info.GetSingle("U");
            mV = info.GetSingle("V");
            mWidth = info.GetSingle("Width");
            mHeight = info.GetSingle("Height");
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("U", mU);
            info.AddValue("V", mV);
            info.AddValue("Width", mWidth);
            info.AddValue("Height", mHeight);
        }
    }
}
