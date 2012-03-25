using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using Microsoft.Xna.Framework;

namespace FNA.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyEffect_cl
    {
        /// <summary>
        /// 
        /// </summary>
        public ParticleEffect particle;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 particlelocation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        public PartyEffect_cl(Vector2 location)
        {
            particle = new ParticleEffect();
            particlelocation = location;
        }
     
    }
}
