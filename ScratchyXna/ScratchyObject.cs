using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace ScratchyXna
{
    public abstract class ScratchyObject
    {
        /// <summary>
        /// Has this object been removed from the scene
        /// </summary>
        internal bool Removed = false;

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
        /// The Game's gamepad input
        /// </summary>
        public GamepadInput Gamepad
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.GamepadInput;
            }
        }

        /// <summary>
        /// The Game's Gyroscope input
        /// </summary>
        public GyroscopeInput Gyroscope
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.GyroscopeInput;
            }
        }

        /// <summary>
        /// The Game's accelerometer input
        /// </summary>
        public AccelerometerInput Accelerometer
        {
            get
            {
                return ScratchyXnaGame.ScratchyGame.AccelerometerInput;
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
        /// Add a song to the sprite
        /// </summary>
        /// <param name="soundName">Add a sound to the sprite</param>
        public void AddSong(string songName)
        {
            ScratchyXnaGame.ScratchyGame.LoadSong(songName);
        }

        /// <summary>
        /// Play a song
        /// </summary>
        /// <param name="songName">Song to play</param>
        /// <param name="repeat">Repeat when done</param>
        public void PlaySong(string songName, bool repeat)
        {
            ScratchyXnaGame.ScratchyGame.PlaySong(songName, repeat);
        }

        /// <summary>
        /// Play a song
        /// </summary>
        /// <param name="song">Song to play</param>
        /// <param name="repeat">Repeat when done</param>
        public void PlaySong(Song song, bool repeat)
        {
            ScratchyXnaGame.ScratchyGame.PlaySong(song, repeat);
        }

        /// <summary>
        /// Stop playing the song
        /// </summary>
        public void StopSong()
        {
            ScratchyXnaGame.ScratchyGame.StopSong();
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
