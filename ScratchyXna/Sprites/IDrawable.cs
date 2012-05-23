using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ScratchyXna
{
    public interface IDrawable
    {
        float Layer { get; set; }
        void DrawObject(SpriteBatch Drawing);
    }
}
