using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    public class Animation : ScratchyObject
    {
        public Action OnComplete;
        private int frameCount = 0;

        private List<AnimationFrame> frames = new List<AnimationFrame>();

        public List<AnimationFrame> Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = value;
                frameCount = frames.Count();
            }
        }

        public int FrameCount
        {
            get
            {
                return frameCount;
            }
        }

        public Animation AddFrame(AnimationFrame frame)
        {
            this.Frames.Add(frame);
            frameCount++;
            return this;
        }

        public Animation AddFrame(int number, float seconds)
        {
            this.Frames.Add(new AnimationFrame(number, seconds));
            frameCount++;
            return this;
        }
    }
}
