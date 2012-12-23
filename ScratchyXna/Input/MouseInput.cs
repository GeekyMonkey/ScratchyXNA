using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class ScratchyMouseState
    {
        public int X;
        public int Y;
        public bool Button1Down;
        public bool Button2Down;
        public bool MiddleButtonDown;
        public int ScrollValue;

#if XBOX
        /// <summary>
        /// Construct an empty Mouse State since XBox doesn't have a mouse
        /// </summary>
        public ScratchyMouseState()
        {
            X = 0;
            Y = 0;
            Button1Down = false;
            Button2Down = false;
            MiddleButtonDown = false;
        }
#else
        /// <summary>
        /// Construct a ScratchyMouseState from an XNA Mouse State
        /// </summary>
        /// <param name="mouseState"></param>
        public ScratchyMouseState(MouseState mouseState)
        {
            X = mouseState.X;
            Y = mouseState.Y;
            Button1Down = (mouseState.LeftButton == ButtonState.Pressed);
            Button2Down = (mouseState.RightButton == ButtonState.Pressed);
            MiddleButtonDown = (mouseState.MiddleButton == ButtonState.Pressed);
        }
#endif
    }

    public class MouseInput
    {
        private ScratchyMouseState currentState;
        private ScratchyMouseState previousState;

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
#if XBOX
            previousState = new ScratchyMouseState();
#else
            previousState = currentState = new ScratchyMouseState(Mouse.GetState());
#endif
        }

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
#if !XBOX
            previousState = currentState;
            currentState = new ScratchyMouseState(Mouse.GetState());
            position = Vector2.Zero; // Clear the cached position
#endif
        }

        /// <summary>
        /// Current mouse position in -100 to 100 scale
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // Use cached value?
                if (position == Vector2.Zero)
                {
                    position = ScratchyXnaGame.ScratchyGame.activeGameScene.PixelToPosition(currentState.X, currentState.Y);
                }
                return position;
            }
        }
        /// <summary>
        /// Cached position
        /// </summary>
        private Vector2 position;
        

        /// <summary>
        /// Current mouse X position in -100 to 100 scale
        /// </summary>
        public float X
        {
            get
            {
                return Position.X;
            }
        }

        /// <summary>
        /// Current mouse Y position in -100 to 100 scale
        /// </summary>
        public float Y
        {
            get
            {
                return Position.Y;
            }
        }

        /// <summary>
        /// Has the mouse scrolled up since the last update
        /// </summary>
        /// <returns></returns>
        public bool ScrolledUp()
        {
            return (previousState.ScrollValue < currentState.ScrollValue);
        }

        /// <summary>
        /// Has the mouse scrolled down since the last update
        /// </summary>
        /// <returns></returns>
        public bool ScrolledDown()
        {
            return (previousState.ScrollValue > currentState.ScrollValue);
        }

        /// <summary>
        /// Distance moved in pixels since the last update
        /// </summary>
        /// <returns>X and Y distance traveled since the last update</returns>
        public Vector2 MoveDistance()
        {
            return new Vector2(currentState.X - previousState.X, currentState.Y - previousState.Y);
        }

        /// <summary>
        /// Was the left button pressed since the last update
        /// </summary>
        /// <returns>True if previously up, but now down</returns>
        public bool Button1Pressed()
        {
            return !previousState.Button1Down && currentState.Button1Down;
        }
        /// <summary>
        /// Was the right button pressed since the last update
        /// </summary>
        /// <returns>True if previously up, but now down</returns>
        public bool Button2Pressed()
        {
            return !previousState.Button2Down && currentState.Button2Down;
        }
        /// <summary>
        /// Was the middle button pressed since the last update
        /// </summary>
        /// <returns>True if previously up, but now down</returns>
        public bool MiddleButtonPressed()
        {
            return !previousState.MiddleButtonDown && currentState.MiddleButtonDown;
        }

        /// <summary>
        /// Was they left button released since the last update
        /// </summary>
        /// <returns>True if previously down, but now up</returns>
        public bool Button1Released()
        {
            return previousState.Button1Down && !currentState.Button1Down;
        }
        /// <summary>
        /// Was the right button released since the last update
        /// </summary>
        /// <returns>True if previously down, but now up</returns>
        public bool Button2Released()
        {
            return previousState.Button2Down && !currentState.Button2Down;
        }
        /// <summary>
        /// Was the middle button released since the last update
        /// </summary>
        /// <returns>True if previously down, but now up</returns>
        public bool MiddleButtonReleased()
        {
            return previousState.MiddleButtonDown && !currentState.MiddleButtonDown;
        }

        /// <summary>
        /// Is the mouse left button currently down
        /// </summary>
        /// <returns>True if the button is down</returns>
        public bool Button1Down()
        {
            return currentState.Button1Down;
        }
        /// <summary>
        /// Is the mouse right button currently down
        /// </summary>
        /// <returns>True if the button is down</returns>
        public bool Button2Down()
        {
            return currentState.Button1Down;
        }
        /// <summary>
        /// Is the mouse left button currently down
        /// </summary>
        /// <returns>True if the button is down</returns>
        public bool MiddleButtonDown()
        {
            return currentState.Button1Down;
        }

        /// <summary>
        /// Is the mouse left button currently up
        /// </summary>
        /// <returns>True if the button is up</returns>
        public bool Button1Up()
        {
            return currentState.Button1Down;
        }
        /// <summary>
        /// Is the mouse right button currently up
        /// </summary>
        /// <returns>True if the button is up</returns>
        public bool Button2Up()
        {
            return currentState.Button1Down;
        }
        /// <summary>
        /// Is the mouse middle button currently up
        /// </summary>
        /// <returns>True if the button is up</returns>
        public bool MiddleButtonUp()
        {
            return currentState.Button1Down;
        }
    }
}
