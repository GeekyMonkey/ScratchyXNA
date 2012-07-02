using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class Costume
    {
        /// <summary>
        /// Positioning center in pixel coordinates
        /// </summary>
        public Vector2 Center = Vector2.Zero;

        private HorizontalAlignments xCenter = HorizontalAlignments.Center;
        private VerticalAlignments yCenter = VerticalAlignments.Center;
        private Scene gameScreen;

        /// <summary>
        /// Name of the costume
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Texture for the costume
        /// </summary>
        internal Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Adding a costume to the game
        /// </summary>
        /// <param name="gameScreen"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void Load(Scene gameScreen, ContentManager content, string name)
        {
            this.gameScreen = gameScreen;
            Name = name;
            Texture = content.Load<Texture2D>("Costumes/" + name);
            CalculateCenter();
        }

        /// <summary>
        /// Positioning X Center method
        /// </summary>
        public HorizontalAlignments XCenter
        {
            get
            {
                return xCenter;
            }
            set
            {
                xCenter = value;
                CalculateCenter();
            }
        }

        /// <summary>
        /// Positioning Y Center method
        /// </summary>
        public VerticalAlignments YCenter
        {
            get
            {
                return yCenter;
            }
            set
            {
                yCenter = value;
                CalculateCenter();
            }
        }

        /// <summary>
        /// Calculate the positioning center in pixel coordinates
        /// </summary>
        private void CalculateCenter()
        {
            switch (XCenter)
            {
                case HorizontalAlignments.Center:
                    Center.X = Texture.Bounds.Center.X;
                    break;
                case HorizontalAlignments.Left:
                    Center.X = Texture.Bounds.Left;
                    break;
                case HorizontalAlignments.Right:
                    Center.X = Texture.Bounds.Right;
                    break;
            }
            switch (YCenter)
            {
                case VerticalAlignments.Center:
                    Center.Y = Texture.Bounds.Center.Y;
                    break;
                case VerticalAlignments.Top:
                    Center.Y = Texture.Bounds.Top;
                    break;
                case VerticalAlignments.Bottom:
                    Center.Y = Texture.Bounds.Bottom;
                    break;
            }
        }

        public Color[] Pixels
        {
            get
            {
                if (pixels == null && Texture != null)
                {
                    pixels = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(pixels);
                }
                return pixels;
            }
            set
            {
                pixels = value;
                Texture.SetData(pixels);
            }
        }
        private Color[] pixels;
    }
}
