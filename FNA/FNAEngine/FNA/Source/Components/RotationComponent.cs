using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using FNA.Core;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// Rotation component keeps track of two vectors for an entity : the direction and the orientation.
    /// Also provides methods to figure our which of the 8 isometric directions the component corresponds to.
    /// </summary>
    [Serializable]
    public class RotationComponent : Component_cl, ISerializable, IExclusiveComponent
    {

        /// <summary>
        /// An enum of the eight cardinal directions that will be queried by the animation component to
        /// determine which animation from it's set of isometric animations to play based on player orientation.
        /// </summary>
        public enum CardinalDirections
        {
            /// <summary>
            /// 
            /// </summary>
            NONE,

            /// <summary>
            /// 
            /// </summary>
            N,

            /// <summary>
            /// 
            /// </summary>
            NE,

            /// <summary>
            /// 
            /// </summary>
            E,

            /// <summary>
            /// 
            /// </summary>
            SE,

            /// <summary>
            /// 
            /// </summary>
            S,

            /// <summary>
            /// 
            /// </summary>
            SW,

            /// <summary>
            /// 
            /// </summary>
            W,

            /// <summary>
            /// 
            /// </summary>
            NW
        }

        /// <summary>
        /// A dictionary that holds all the unit vectors in the cardinal directions.
        /// </summary>
        private Dictionary<CardinalDirections, Vector2> mCardinalVectors;

        /// <summary>
        /// A vector representing the direction the player is facing (right stick).
        /// </summary>
        private Vector2 mOrientation = new Vector2(1, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 Orientation
        {
            get
            {
                mDirection.Normalize();
                return mOrientation;
            }
            set
            {
                Vector2 newOrient = value;
                newOrient.Normalize();
                mOrientation = newOrient;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 OrientationRight
        {
            get
            {
                Vector2 right = new Vector2(mOrientation.Y, -mOrientation.X);
                right.Normalize();
                return right;
            }
        }

        /// <summary>
        /// A vector representing the direction the player is moving (left stick).
        /// </summary>
        private Vector2 mDirection = new Vector2(1, 0);

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 Direction
        {
            get
            {
                mDirection.Normalize();
                return mDirection;
            }
            set
            {
                mDirection = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 DirectionRight
        {
            get
            {
                Vector2 right = new Vector2(mDirection.Y, -mDirection.X);
                right.Normalize();
                return right;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Vector2 DirectionUp
        {
            get
            {
                //Vector2 up = new Vector2(0, Math.Abs(mDirection.X));
                //up.Normalize();
                //return up;

                /************************************************************************
                 * HACK:
                 * Trying to get Finale jumping working. DirectionUp will always be defined
                 * as a Vector2(0, 1)
                 *
                 * Larsson Burch - 2011/11/16 - 16:15
                 ************************************************************************/
                return new Vector2(0, 1);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent entity for this Component.</param>
        public RotationComponent(Entity_cl parent)
            : base(parent)
        {
            InitializeCardinalVectors();
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeCardinalVectors()
        {
            mCardinalVectors = new Dictionary<CardinalDirections, Vector2>();
            Vector2 vN = new Vector2(0, -1);
            Vector2 vNE = new Vector2(1, -1);
            Vector2 vE = new Vector2(1, 0);
            Vector2 vSE = new Vector2(1, 1);
            Vector2 vS = new Vector2(0, 1);
            Vector2 vSW = new Vector2(-1, 1);
            Vector2 vW = new Vector2(-1, 0);
            Vector2 vNW = new Vector2(-1, -1);
            vN.Normalize();
            vNE.Normalize();
            vE.Normalize();
            vSE.Normalize();
            vS.Normalize();
            vSW.Normalize();
            vW.Normalize();
            vNW.Normalize();
            mCardinalVectors[CardinalDirections.N] = vN;
            mCardinalVectors[CardinalDirections.NE] = vNE;
            mCardinalVectors[CardinalDirections.E] = vE;
            mCardinalVectors[CardinalDirections.SE] = vSE;
            mCardinalVectors[CardinalDirections.S] = vS;
            mCardinalVectors[CardinalDirections.SW] = vSW;
            mCardinalVectors[CardinalDirections.W] = vW;
            mCardinalVectors[CardinalDirections.NW] = vNW;
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected RotationComponent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            InitializeCardinalVectors();
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CardinalDirections GetCardinalOrientation()
        {
            Dictionary<CardinalDirections, float> dotProducts = new Dictionary<CardinalDirections, float>();
            foreach (KeyValuePair<CardinalDirections, Vector2> cardinalVector in mCardinalVectors)
            {
                Vector2 cardinalVect = cardinalVector.Value;
                float dotProduct;
                Vector2.Dot(ref mOrientation, ref cardinalVect, out dotProduct);
                dotProducts[cardinalVector.Key] = dotProduct;
            }

            KeyValuePair<CardinalDirections, float> highestDotProduct = new KeyValuePair<CardinalDirections, float>(0, 0);
            foreach (KeyValuePair<CardinalDirections, float> dotProductPair in dotProducts)
            {
                if (dotProductPair.Value > highestDotProduct.Value)
                {
                    highestDotProduct = dotProductPair;
                }
            }

            return highestDotProduct.Key;
        }
    }
}
