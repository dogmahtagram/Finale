using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TigerWoods
{
    public class Camera
    {
        protected float mZoom;
        public float Zoom
        {
            get
            {
                return mZoom;
            }
            set
            {
                mZoom = value;
                RecalculateTransformation();
            }
        }

        private Matrix mTransform;
        public Matrix Transform
        {
            get
            {
                return mTransform;
            }
        }

        private Vector2 mPosition;
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
                RecalculateTransformation();
            }
        }

        private float mRotation;
        public float Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                mRotation = value;
                RecalculateTransformation();
            }
        }

        public Camera()
        {
            mZoom = 1.0f;
            mRotation = 0.0f;
            mPosition = Vector2.Zero;

            RecalculateTransformation();
        }

        public void Move(Vector2 amount)
        {
            mPosition += amount;
            RecalculateTransformation();
        }

        public void RecalculateTransformation()
        {
            mTransform = Matrix.CreateTranslation(new Vector3(-mPosition.X, -mPosition.Y, 0.0f)) *
                         Matrix.CreateRotationZ(mRotation) *
                         Matrix.CreateScale(new Vector3(mZoom, mZoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(640, 360, 0));
        }
    }
}
