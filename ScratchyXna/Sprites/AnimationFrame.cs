using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    public class AnimationFrame
    {
        /// <summary>
        /// Create an animation frame
        /// </summary>
        /// <param name="number"></param>
        /// <param name="seconds"></param>
        public AnimationFrame(int number, float seconds)
        {
            this.Number = number;
            this.Seconds = seconds;
        }

        /// <summary>
        /// The Frame Number (1 based)
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// Seconds to show this frame
        /// </summary>
        public float Seconds
        {
            get;
            set;
        }
    }
}
