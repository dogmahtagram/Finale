using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FNA.WorldEditor
{
    /// <summary>
    /// 
    /// </summary>
    public class Button
    {
        /// The delegates for this Button
        public delegate void ButtonPressedHandler(object sender);
        
        /// All of the possible events that can be subscribed to
        public event ButtonPressedHandler ButtonPressed;
        
        private Texture2D mTexture;
        private Rectangle mRectangle;

        private Vector2 mPosition;
        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
                UpdateSizeInformation();
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
                UpdateSizeInformation();
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
                UpdateSizeInformation();
            }
        }

        private Color mUpColor;
        /// <summary>
        /// 
        /// </summary>
        public Color UpColor
        {
            set
            {
                mUpColor = value;
            }
        }
        
        private Color mDownColor;
        /// <summary>
        /// 
        /// </summary>
        public Color DownColor
        {
            set
            {
                mDownColor = value;
            }
        }
        
        private Color mRolloverColor;
        /// <summary>
        /// 
        /// </summary>
        public Color RolloverColor
        {
            set
            {
                mRolloverColor = value;
            }
        }

        private Color mLabelColor;
        /// <summary>
        /// 
        /// </summary>
        public Color LabelColor
        {
            set
            {
                mLabelColor = value;
            }
        }

        private string mLabel;
        /// <summary>
        /// 
        /// </summary>
        public string Label
        {
            set
            {
                mLabel = value;
                UpdateSizeInformation();                
            }
        }

        private LabelAlignment mAlignment;
        /// <summary>
        /// 
        /// </summary>
        public LabelAlignment Alignment
        {
            set
            {
                mAlignment = value;
            }
        }

        private bool mIsMouseOver;
        private bool mIsPressed;

        private Vector2 mLabelOffset;

        /// <summary>
        /// 
        /// </summary>
        public enum LabelAlignment
        {
            /// -
            LEFT_ALIGNED,
            /// -
            CENTER_ALIGNED
        }        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Button(Vector2 position, float width, float height)
        {
            mPosition = position;
            mWidth = width;
            mHeight = height;

            mLabel = String.Empty;
            mLabelOffset = Vector2.Zero;

            Initialize();
            UpdateSizeInformation();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            mTexture = new Texture2D(Game.BaseInstance.GraphicsDevice, 1, 1);
            mTexture.SetData(new Color[] { Color.White });

            mIsMouseOver = false;
            mIsPressed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentMouseState"></param>
        /// <param name="previousMouseState"></param>
        /// <returns></returns>
        public bool Update(MouseState currentMouseState, MouseState previousMouseState)
        {
            bool result = false;

            mIsMouseOver = false;
            mIsPressed = false;

            if (mRectangle.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    mIsPressed = true;
                    result = true;
                    if (previousMouseState.LeftButton == ButtonState.Released)
                    {
                        this.NotifyButtonPressed();
                    }
                }
                else
                {
                    mIsMouseOver = true;
                }
            }

            return result;
        }

        /// <summary>
        /// This is called when the Button should draw itself.
        /// </summary>
        public void Draw()
        {
            Color modulationColor = mUpColor;

            if (mIsMouseOver)
            {
                modulationColor = mRolloverColor;
            }
            else if (mIsPressed)
            {
                modulationColor = mDownColor;
            }

            Game.BaseInstance.SpriteBatch.Draw(mTexture, mRectangle, modulationColor);

            // Find the center of the string
            //Vector2 labelOrigin = WorldEditor.Instance.Font.MeasureString(mLabel) / 2;
            // Draw the string
            if (mLabel != String.Empty)
            {
                Vector2 position = mPosition;

                position.Y += mLabelOffset.Y;

                if (mAlignment == LabelAlignment.CENTER_ALIGNED)
                {
                    position.X += mLabelOffset.X;
                }

                Game.BaseInstance.SpriteBatch.DrawString(Game.BaseInstance.DebugFont, 
                                                            mLabel, 
                                                            position, 
                                                            mLabelColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
            
        }

        private void NotifyButtonPressed()
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(this);
            }
        }

        private void UpdateSizeInformation()
        {
            mRectangle = new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)mWidth, (int)mHeight);
            
            // Find the center of the string
            Vector2 labelOrigin = Game.BaseInstance.DebugFont.MeasureString(mLabel);

            mLabelOffset = new Vector2(mWidth, mHeight) - labelOrigin;
            mLabelOffset /= 2.0f;
        }
    }
}
