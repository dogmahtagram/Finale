using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

using FNA.Interfaces;
using FNA.Graphics;
using FNA.Components;

using Microsoft.Xna.Framework.Graphics;



namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class MusicManager : BaseManager, ILoadAble
    {

        private static MusicManager sInstance = new MusicManager();

        /// <summary>
        /// 
        /// </summary>
        public static MusicManager Instance
        {
            get
            {
                return sInstance;
            }
        }

        private Song song;
        
        /// <summary>
        /// 
        /// </summary>
        public VisualizationData visData = new VisualizationData();

        /// <summary>
        /// constructor
        /// </summary>
        private MusicManager()
        {
            song = FNA.Game.BaseInstance.Content.Load<Song>("Audio/TestSong");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsVisualizationEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.GetVisualizationData(visData);
            }

            if (calculateFreq(0) >= 50)
            {
                foreach (ParticleComponent p in PartyManager.Instance.PartyList)
                {
                    PartyManager.Instance.ChangeParticleColor(p, Color.White);
                }
            }

            else
            {
                foreach (ParticleComponent p in PartyManager.Instance.PartyList)
                {
                    PartyManager.Instance.ChangeParticleColor(p, Color.Red);
                }
            }

            //if (calculateSample(index) <= -0.2f)
            //{
            //    foreach (ParticleComponent p in PartyManager.Instance.PartyList)
            //    {
            //        PartyManager.Instance.ChangeParticleColor(p, Color.White);
            //    }
            //}

            //else
            //{
            //    foreach (ParticleComponent p in PartyManager.Instance.PartyList)
            //    {
            //        PartyManager.Instance.ChangeParticleColor(p, Color.Red);
            //    }
            //}

            base.Update();
        }

        void ILoadAble.Load()
        {
            //MediaPlayer.Play(song);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            FNA.Game.BaseInstance.SpriteBatch.Begin();

            //string frequencyCount = "VisData Frequencies Count: " + visData.Frequencies.Count.ToString();
            //FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, frequencyCount, new Vector2(220, 10), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            //for (int s = 0; s < visData.Samples.Count; s++)
            //{
            //    string samples = s.ToString() + ": VisData Samples : " + visData.Samples[s].ToString();
            //    FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, samples, new Vector2(800, 20 + 15 * s), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0.5f);
            //}
            
            //for (int f = 0; f < 51; f++)
            //{
            //    float test = visData.Frequencies[f] * 100;
            //    string frequency = f.ToString() + ": VisData Frequencies : " + test.ToString();
            //    FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, frequency, new Vector2(50, 20 + 15 * f), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0.5f);
            //}

            //for (int g = 51; g < 102; g++)
            //{
            //    float test = visData.Frequencies[g] * 100;
            //    string frequency2 = g.ToString() + ": VisData Frequencies : " + test.ToString();
            //    FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, frequency2, new Vector2(275, 20 + 15 * (g - 51)), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0.5f);
            //}

            //for (int h = 102; h < 153; h++)
            //{
            //    float test = visData.Frequencies[h] * 100;
            //    string frequency3 = h.ToString() + ": VisData Frequencies : " + test.ToString();
            //    FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, frequency3, new Vector2(500, 20 + 15 * (h - 102)), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0.5f);
            //}

            ////base.Draw();

            string playPosition = "PlayPosition: " + MediaPlayer.PlayPosition.ToString();
            FNA.Game.BaseInstance.SpriteBatch.DrawString(FNA.Game.BaseInstance.DebugFont, playPosition, new Vector2(220, 10), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            FNA.Game.BaseInstance.SpriteBatch.End();
       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int calculateFreq(int index)
        {
            int freq = (int)(visData.Frequencies[index] * 100);
            
            return freq;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float calculateSample(int index)
        {
            float sample = visData.Samples[index];

            return sample;
        }
    }
}
