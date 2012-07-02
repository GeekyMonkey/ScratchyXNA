using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// When using the sprite stamp feature, these are the cropping options
    /// </summary>
    public enum StampCroppings
    {
        /// <summary>
        /// Crop the source sprite to the destination sprite's rectangle
        /// </summary>
        CropToSprite,

        /// <summary>
        /// Grow the destination sprite so that it can fit the entire source sprite
        /// </summary>
        GrowSprite
    }
}
