using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ScratchyXna
{
    public class KeyboardInput
    {
        private KeyboardState currentState;
        private KeyboardState previousState;

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            previousState = currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Was the key pressed since the last update
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if previously up, but now down</returns>
        public bool KeyPressed(Keys key)
        {
            return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Was they key released since the last update
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if previously down, but now up</returns>
        public bool KeyReleased(Keys key)
        {
            return previousState.IsKeyDown(key) && currentState.IsKeyUp(key);
        }

        /// <summary>
        /// Is a key currently down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is down</returns>
        public bool KeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Is a key currently up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is up</returns>
        public bool KeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }
    }
}
