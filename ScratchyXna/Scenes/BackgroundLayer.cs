using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class BackgroundLayer : GraphicObject
    {
        private Texture2D texture;
        private Background background;

        /// <summary>
        /// Positioning center in pixel coordinates
        /// </summary>
        public Vector2 Center = Vector2.Zero;

        private HorizontalAlignments xCenter = HorizontalAlignments.Center;
        private VerticalAlignments yCenter = VerticalAlignments.Center;

        /// <summary>
        /// Construct a background layer
        /// </summary>
        public BackgroundLayer(Background background)
        {
            this.background = background;
            Layer = 1;
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
        /// <param name="scene"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void Load(Scene scene, ContentManager content, string name)
        {
            this.Scene = scene;
            Name = name;
            Texture = content.Load<Texture2D>("Backgrounds/" + name);
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
        /// Name of the costume
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        private float Alpha = 1.0f;

        /// <summary>
        /// Get or set the ghost effect. 0 = fully visible, 100 = fully invisible
        /// </summary>
        public float GhostEffect
        {
            get
            {
                return 100f - (Alpha * 100f);
            }
            set
            {
                Alpha = Math.Max(0.0f, Math.Min(1.0f, ((100f - value) / 100f)));
            }
        }

        /// <summary>
        /// The color for this layer.  White = no change.
        /// </summary>
        public Color LayerColor = Color.White;

        /// <summary>
        /// Draw depth
        /// </summary>
        internal float Depth
        {
            get
            {
                return 0.5f - (Layer) / 1000;
            }
        }

        /// <summary>
        /// Draw this background layer
        /// </summary>
        /// <param name="Drawing"></param>
        public override void DrawObject(SpriteBatch Drawing)
        {
            if (Visible && Texture != null)
            {
                Vector2 screenPos = Scene.GetScreenPosition(Position);
                Drawing.Draw(this.Texture,
                    screenPos,
                    null,
                    LayerColor * Alpha,
                    rotationRadians,
                    this.Center,
                    this.background.Scale / Scene.PixelScale,
                    SpriteEffects.None,
                    Depth);
            }
        }

        /// <summary>
        /// Update the background layer
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public void UpdateBackgroundLayer(GameTime gameTime)
        {
        }

    }
}
