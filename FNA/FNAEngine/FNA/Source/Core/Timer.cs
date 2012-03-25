using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FNA.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Timer_cl
    {
        private bool mPaused;

        /// <summary>
        /// 
        /// </summary>
        public bool Paused
        {
            get
            {
                return mPaused;
            }
            set
            {
                mPaused = value;
            }
        }

        private GameTime mActualTime = new GameTime();

        /// <summary>
        /// Returns the GameTime that our FNA.Game is using;
        /// </summary>
        public GameTime ActualTime
        {
            get
            {
                return mActualTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ElapsedMilliseconds
        {
            get
            {
                return mActualTime.ElapsedGameTime.Milliseconds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float ElapsedSeconds
        {
            get
            {
                return (mActualTime.ElapsedGameTime.Milliseconds * 0.001f);
            }
        }

        private int mFramesPerSecond = 30;

        /// <summary>
        /// 
        /// </summary>
        public int FramesPerSecond
        {
            get
            {
                return mFramesPerSecond;
            }
            set
            {
                mFramesPerSecond = value;
            }
        }

        private int mCurrentFrame = 1;

        /// <summary>
        /// 
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                return mCurrentFrame;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool IsNthFrame(int frame)
        {
            return (mCurrentFrame % frame == 0);
        }

        private int mUnpausedElapsedMilliseconds;

        /// <summary>
        /// 
        /// </summary>
        public int UnpausedElapsedMilliseconds
        {
            get
            {
                return mUnpausedElapsedMilliseconds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float UnpausedElapsedSeconds
        {
            get
            {
                return mUnpausedElapsedMilliseconds * 0.001f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Timer_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            mActualTime = time;

            if (mPaused)
            {
                mUnpausedElapsedMilliseconds = 0;
            }
            else
            {
                mUnpausedElapsedMilliseconds = time.ElapsedGameTime.Milliseconds;
            }

            mCurrentFrame++;
            if (mCurrentFrame > 60)
            {
                mCurrentFrame = 1;
            }
        }
    }
}
