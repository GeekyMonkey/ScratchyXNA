using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
#if !XBOX
using Microsoft.Xna.Framework.Input.Touch;
#endif
#if WINDOWS_PHONE
using Microsoft.Devices.Sensors;
#endif


namespace ScratchyXna
{
    public class GyroscopeInput
    {
        private Vector3 gyroReading = new Vector3();
        bool gyroActive = false;

#if WINDOWS_PHONE
        private Accelerometer accelSensor;
#endif

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
#if WINDOWS_PHONE
            gyroActive = true;
#endif
        }

        public float X
        {
            get
            {
                return gyroReading.X;
            }
        }

        public float Y
        {
            get
            {
                return gyroReading.Y;
            }
        }

        public float Z
        {
            get
            {
                return gyroReading.Z;
            }
        }

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
#if WINDOWS_PHONE
            if (gyroActive && Gyroscope.IsSupported)
            {
                //get current rotation rate, display happens in Draw()
                GyroscopeReading gr = new GyroscopeReading();
                gyroReading = gr.RotationRate;
            }
#endif
        }

    }
}
