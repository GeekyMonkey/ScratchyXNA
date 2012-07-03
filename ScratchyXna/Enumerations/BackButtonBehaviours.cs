using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// What should happen automatically on a scene when the back button is pressed
    /// </summary>
    public enum BackButtonBehaviours
    {
        /// <summary>
        /// Show the first scene in the scene list. Normally the title scene.  If already on the title scene, then the game will exit.  (Default)
        /// </summary>
        ShowFirstSceneOrExit,

        /// <summary>
        /// The game will exit when the back button is pressed.
        /// </summary>
        ExitGame,

        /// <summary>
        /// The previous scene will automatically be shown when the back button is pressed
        /// </summary>
        ShowPreviousScene,

        /// <summary>
        /// Automatic handling of the back button will not happen.  You will do your own cutom handling.
        /// </summary>
        Ignore
    }
}
