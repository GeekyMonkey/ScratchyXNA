using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    public abstract class ScratchyObject
    {
        /// <summary>
        /// The Game's keyboard input
        /// </summary>
        public KeyboardInput Keyboard
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.KeyboardInput;
            }
        }

        /// <summary>
        /// The Game's mouse input
        /// </summary>
        public MouseInput Mouse
        {
            get {
                return ScratchyXnaGame.ScratchyGame.MouseInput; 
            }
        }

        /// <summary>
        /// The Game's touch input
        /// </summary>
        public TouchInput Touch
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.TouchInput;
            }
        }

        /// <summary>
        /// Random number generator
        /// </summary>
        public Random Random
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.Random;
            }
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        public void PlaySound(string soundName, bool loop)
        {
            ScratchyXnaGame.ScratchyGame.PlaySound(soundName, loop);
        }
        /// <summary>
        /// Play a sound once
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, false);
        }

        /// <summary>
        /// Stop a looping sound
        /// </summary>
        /// <param name="soundName">Name of the sound to stop</param>
        public void StopSound(string soundName)
        {
            ScratchyXnaGame.ScratchyGame.StopSound(soundName);
        }

        /// <summary>
        /// Add a sound to the sprite
        /// </summary>
        /// <param name="soundName">Add a sound to the sprite</param>
        public void AddSound(string soundName)
        {
            ScratchyXnaGame.ScratchyGame.LoadSound(soundName);
        }

        /// <summary>
        /// Player data object
        /// </summary>
        public PlayerData PlayerData
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.PlayerData;
            }
        }
    }
}
