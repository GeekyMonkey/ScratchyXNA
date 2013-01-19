using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class GamepadInput
    {
        private GamePadState[] currentState = new GamePadState[4];
        private GamePadState[] previousState = new GamePadState[4];

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                previousState[i] = currentState[i] = GamePad.GetState(ToPlayerIndex(i+1));
            }
        }

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
            for (int i = 0; i < 4; i++)
            {
                previousState[i] = currentState [i];
                currentState[i] = GamePad.GetState(ToPlayerIndex(i+1));
            }
        }

        /// <summary>
        /// Set the vibration on a controller
        /// </summary>
        /// <param name="playerNumber">Player number 1 to 4</param>
        /// <param name="lowFrequency">Low frequency amount from 0 to 1</param>
        /// <param name="highFrequency">High frequency amount from 0 to 1</param>
        public void SetVibration(int playerNumber, float lowFrequency, float highFrequency)
        {
            Microsoft.Xna.Framework.Input.GamePad.SetVibration(ToPlayerIndex(playerNumber), lowFrequency, highFrequency);
        }

        /// <summary>
        /// Convert a player number to a player index
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <returns></returns>
        private PlayerIndex ToPlayerIndex(int playerNumber)
        {
            return (PlayerIndex)playerNumber-1;
        }

        public GamePadButtons GetButtons(int playerNumber)
        {
            return currentState[playerNumber - 1].Buttons;
        }

        public GamePadDPad GetDPad(int playerNumber)
        {
            return currentState[playerNumber - 1].DPad;
        }

        public Vector2 GetLeftThumbStick(int playerNumber)
        {
            return currentState[playerNumber - 1].ThumbSticks.Left;
        }

        public Vector2 GetRightThumbStick(int playerNumber)
        {
            return currentState[playerNumber - 1].ThumbSticks.Right;
        }

        public float GetLeftTrigger(int playerNumber)
        {
            return currentState[playerNumber - 1].Triggers.Left;
        }

        public float GetRightTrigger(int playerNumber)
        {
            return currentState[playerNumber - 1].Triggers.Right;
        }

        /// <summary>
        /// Was the button pressed since the last update
        /// </summary>
        /// <param name="key">Button to check</param>
        /// <returns>True if previously up, but now down</returns>
        public bool ButtonPressed(int playerNumber, Buttons button)
        {
            return previousState[playerNumber - 1].IsButtonUp(button) && ButtonDown(playerNumber, button);
        }

        /// <summary>
        /// Was they button released since the last update
        /// </summary>
        /// <param name="key">Button to check</param>
        /// <returns>True if previously down, but now up</returns>
        public bool ButtonReleased(int playerNumber, Buttons button)
        {
            return previousState[playerNumber - 1].IsButtonDown(button) && ButtonUp(playerNumber, button);
        }

        /// <summary>
        /// Is a button currently down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is down</returns>
        public bool ButtonDown(int playerNumber, Buttons button)
        {
            return currentState[playerNumber-1].IsButtonDown(button);
        }

        /// <summary>
        /// Is a button currently up
        /// </summary>
        /// <param name="key">Button to check</param>
        /// <returns>True if the Button is up</returns>
        public bool ButtonUp(int playerNumber, Buttons button)
        {
            return currentState[playerNumber - 1].IsButtonUp(button);
        }
    }
}
