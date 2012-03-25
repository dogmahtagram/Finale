using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FNA.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum AffectFlags
    {
        /// <summary>
        /// 
        /// </summary>
        PLAYER_1 = 0x01,

        /// <summary>
        /// 
        /// </summary>
        PLAYER_2 = 0x02,

        /// <summary>
        /// 
        /// </summary>
        PLAYER_3 = 0x04,

        /// <summary>
        /// 
        /// </summary>
        PLAYER_4 = 0x08,

        /// <summary>
        /// 
        /// </summary>
        TIGERS = 0x10,

        /// <summary>
        /// 
        /// </summary>
        ALL_PLAYERS = PLAYER_1 | PLAYER_2 | PLAYER_3 | PLAYER_4,

        /// <summary>
        /// 
        /// </summary>
        ALL_ENEMIES = TIGERS,

        /// <summary>
        /// 
        /// </summary>
        ALL = ALL_PLAYERS | ALL_ENEMIES
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// 
        /// </summary>
         void PreUpdate();

        /// <summary>
        /// 
        /// </summary>
         void Draw();
    }
}
