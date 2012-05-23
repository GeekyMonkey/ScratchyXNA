using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// A scheduled action
    /// </summary>
    public class Timer
    {
        internal TimeSpan Time;
        internal Action Callback;
        internal bool Repeat;
        internal TimeSpan? StartTime = null;
        internal TimeSpan TargetTime;

        /*
        /// <summary>
        /// Create a scheduled action
        /// </summary>
        /// <param name="startTime">When the timer was created (time since game start)</param>
        /// <param name="timeInterval">Time to start (time since game start)</param>
        /// <param name="callback">Action to fire</param>
        /// <param name="repeat">Repeat after firing the event</param>
        public Timer(TimeSpan startTime, TimeSpan timeInterval, Action callback)
        {
            StartTime = startTime;
            Time = timeInterval;
            Callback = callback;
            Repeat = false;
        }
        */

        /// <summary>
        /// Create a scheduled action
        /// </summary>
        /// <param name="startTime">When the timer was created (time since game start)</param>
        /// <param name="timeInterval">Time to start (time since game start)</param>
        /// <param name="callback">Action to fire</param>
        /// <param name="repeat">Repeat after firing the event</param>
        public Timer(TimeSpan startTime, double seconds, Action callback, bool repeat)
        {
            StartTime = startTime;
            Time = TimeSpan.FromSeconds(seconds);
            TargetTime = startTime + Time;
            Callback = callback;
            Repeat = repeat;
        }
    }
}
