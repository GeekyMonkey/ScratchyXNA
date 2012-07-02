using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public static class MatrixExtensions
    {
        public static void Decompose2D(this Matrix matrix, out Vector2 position, out float rotation, out float scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y).Length(); // Not sure if this should just be one of the values, or maybe an average
        }
    }
}
