using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchyXna
{
    /// <summary>
    /// When using the sprite stamp feature, these are the methods
    /// </summary>
    public enum StampMethods
    {
        /// <summary>
        /// Just draw the source onto the destination
        /// </summary>
        Normal,

        /// <summary>
        /// Apply the alpha from the source to the destination 
        /// </summary>
        Cutout,

        /// <summary>
        /// The negative of the alpha from the source will be applied to the destination
        /// </summary>
        CutoutInverted
    }
}
