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
    public class AccelerometerInput
    {
        private Vector3 accelReading = new Vector3();
        bool accelActive = false;

#if WINDOWS_PHONE
        private Accelerometer accelSensor;
#endif

        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
#if WINDOWS_PHONE
            accelSensor = new Accelerometer();
            // Add the accelerometer event handler to the accelerometer sensor.
            accelSensor.ReadingChanged +=
                new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);
            
            // Start the accelerometer
            try
            {
                accelSensor.Start();
                accelActive = true;
            }
            catch (AccelerometerFailedException e)
            {
                // the accelerometer couldn't be started.  No fun!
                accelActive = false;
            }
            catch (UnauthorizedAccessException e)
            {
                // This exception is thrown in the emulator-which doesn't support an accelerometer.
                accelActive = false;
            }
#endif
        }

        public float X
        {
            get
            {
                return accelReading.X;
            }
        }

        public float Y
        {
            get
            {
                return accelReading.Y;
            }
        }

        public float Z
        {
            get
            {
                return accelReading.Z;
            }
        }

#if WINDOWS_PHONE
        public void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X;
            accelReading.Y = (float)e.Y;
            accelReading.Z = (float)e.Z;
        }
#endif

        /// <summary>
        /// Update each game loop
        /// </summary>
        internal void Update()
        {
#if WINDOWS_PHONE
#endif
        }

    }
}
