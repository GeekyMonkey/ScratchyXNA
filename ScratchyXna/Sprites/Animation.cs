using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    public class Animation
    {
        public Action OnComplete;

        public List<AnimationFrame> Frames = new List<AnimationFrame>();

        public Animation AddFrame(AnimationFrame frame)
        {
            this.Frames.Add(frame);
            return this;
        }

        public Animation AddFrame(int number, float seconds)
        {
            this.Frames.Add(new AnimationFrame(number, seconds));
            return this;
        }
    }
}
