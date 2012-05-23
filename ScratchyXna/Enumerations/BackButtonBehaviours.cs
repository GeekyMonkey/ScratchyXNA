using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// What should happen automatically on a screen when the back button is pressed
    /// </summary>
    public enum BackButtonBehaviours
    {
        /// <summary>
        /// Show the first screen in the screen list. Normally the title screen.  If already on the title screen, then the game will exit.  (Default)
        /// </summary>
        ShowFirstScreenOrExit,

        /// <summary>
        /// The game will exit when the back button is pressed.
        /// </summary>
        ExitGame,

        /// <summary>
        /// The previous screen will automatically be shown when the back button is pressed
        /// </summary>
        ShowPreviousScreen,

        /// <summary>
        /// Automatic handling of the back button will not happen.  You will do your own cutom handling.
        /// </summary>
        Ignore
    }
}
