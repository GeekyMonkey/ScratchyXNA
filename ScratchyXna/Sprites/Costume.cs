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
        private Scene scene;
        private Texture2D texture;

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
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                pixels = null;
                CalculateCenter();
            }
        }

        /// <summary>
        /// Get the width of the costume
        /// </summary>
        public int Width
        {
            get
            {
                return Texture.Width;
            }
        }

        /// <summary>
        /// Get the height of the costume
        /// </summary>
        public int Height
        {
            get
            {
                return Texture.Height;
            }
        }

        /// <summary>
        /// Adding a costume to the game
        /// </summary>
        /// <param name="scene">Scene that owns the sprite</param>
        /// <param name="content">The content manager</param>
        /// <param name="name">Name of the content</param>
        public void Load(Scene scene, ContentManager content, string name)
        {
            this.scene = scene;
            Name = name;
            Texture = content.Load<Texture2D>("Costumes/" + name);
            CalculateCenter();
        }

        /// <summary>
        /// Adding a costume to the game
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void Load(Scene scene, ContentManager content, string name, int frameColumns, int frameRows)
        {
            this.scene = scene;
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

        /// <summary>
        /// The pixels in the current texture
        /// </summary>
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

        /// <summary>
        /// Begin creating a custom costume by copying the current costume
        /// </summary>
        public Costume Copy()
        {
            return Copy(this.Name + "_Copy");
        }

        /// <summary>
        /// Begin creating a custom costume by copying the current costume
        /// </summary>
        public Costume Copy(string name)
        {
            Costume newCostume = new Costume
            {
                Name = name,
                xCenter = this.xCenter,
                yCenter = this.yCenter,
                scene = this.scene
            };

            newCostume.Texture = new Texture2D(ScratchyXnaGame.ScratchyGame.GraphicsDevice, this.Width, this.Height);
            newCostume.Pixels = this.Pixels;

            newCostume.CalculateCenter();
            return newCostume;
        }

        /// <summary>
        /// Flip this costume horizontally
        /// </summary>
        public Costume FlipX()
        {
            Flip(true, false);
            return this;
        }

        /// <summary>
        /// Flip this cosume vertically
        /// </summary>
        public Costume FlipY()
        {
            Flip(false, true);
            return this;
        }

        /// <summary>
        /// Common flip functioniality
        /// </summary>
        /// <param name="horizontal">Flipping horizontally</param>
        /// <param name="vertical">Flipping vertically</param>
        private void Flip(bool horizontal, bool vertical)
        {
            Texture2D source = this.Texture;
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData(flippedData);
            this.Texture = flipped;
            this.Pixels = flippedData;
        }

    }
}
