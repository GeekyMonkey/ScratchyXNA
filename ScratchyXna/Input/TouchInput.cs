using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace ScratchyXna
{
    public class TouchInput
    {
        private TouchCollection TouchState;
        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        private List<Vector2> taps;

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
#if !XBOX
            TouchPanel.EnabledGestures = GestureType.Tap;
#endif
        }

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
#if !XBOX
            TouchState = TouchPanel.GetState();

            // Clear the Gesture buffer from last update, so we don't act on it again.
            Gestures.Clear();
 
            // Add all the stored gestures from the TouchPanel in the Gestures List.
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
#endif
        }

        /// <summary>
        /// All of the tap positions
        /// </summary>
        public IEnumerable<Vector2> Taps
        {
            get
            {
                if (taps == null)
                {
                    taps = new List<Vector2>();
                    foreach (var tap in Gestures.Where(g => g.GestureType == GestureType.Tap))
                    {
                        taps.Add(ScratchyXnaGame.ScratchyGame.activeGameScene.PixelToPosition((int)tap.Position.X, (int)tap.Position.Y));
                    }
                }
                return taps;
            }
        }
    }
}
