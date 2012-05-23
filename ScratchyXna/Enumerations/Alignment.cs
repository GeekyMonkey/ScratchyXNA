using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// Horizontal (X) Alignments
    /// </summary>
    public enum HorizontalAlignments
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Vertical (Y) Alignments
    /// </summary>
    public enum VerticalAlignments
    {
        Top,
        Center,
        Bottom
    }

    /// <summary>
    /// 90 degree increment directions or rotations
    /// </summary>
    public enum Directions
    {
        Right = 0,
        Up = 90,
        Left = 180,
        Down = 270
    }
}
