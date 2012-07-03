using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// A Timer
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// When the timer started (or was reset)
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// Get the amount of time the timer has been running
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return DateTime.Now - StartTime;
            }
        }

        /// <summary>
        /// Create a new timer object
        /// </summary>
        public Timer()
        {
            Reset();
        }

        /// <summary>
        /// Reset the timer's start time to now
        /// </summary>
        public void Reset()
        {
            StartTime = DateTime.Now;
        }
    }
}
