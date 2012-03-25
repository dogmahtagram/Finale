using System;

namespace FNA.Managers
{
    /// <summary>
    /// Manages the game state.
    /// </summary>
    public sealed class GameStateManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The static instance of this class.
        /// </summary>
        private static readonly GameStateManager_cl mInstance = new GameStateManager_cl();

        /// <summary>
        /// Accessor for the static instance.
        /// </summary>
        public static GameStateManager_cl Instance
        {
            get
            {
                return mInstance;
            }
        }

        /// <summary>
        /// State of the game.
        /// </summary>
        public enum GameState
        {
            /// Loading state
            GAME_STATE_LOADING = 0,
            /// Menu state
            GAME_STATE_MENU,
            /// In game state
            GAME_STATE_INGAME,
            /// Editor state
            GAME_STATE_EDITOR,
            /// Final state
            GAME_STATE_FINAL
        };

        private static GameState mState;
        /// <summary>
        /// Accessor and mutator for the game state
        /// </summary>
        public static GameState State
        {
            get
            {
                return mState;
            }
            set
            {
                mState = value;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private GameStateManager_cl()
        {
        }
    }
}
