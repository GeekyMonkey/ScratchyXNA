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
        private List<Texture2D> textures = new List<Texture2D>();
        private float frameSeconds = 1f;
        internal float elapsedFrameSeconds = 0;
        Animation animation;
        // private Sprite sprite;

        /// <summary>
        /// The current frame index
        /// </summary>
        private int currentFrameNumber = 1;

        /// <summary>
        /// The current frame index
        /// </summary>
        public int CurrentFrameNumber
        {
            get
            {
                return currentFrameNumber;
            }
            set
            {
                currentFrameNumber = value;
            }
        }

        /// <summary>
        /// Number of seconds for all frames
        /// </summary>
        public float FrameSeconds
        {
            get
            {
                return frameSeconds;
            }
            set
            {
                frameSeconds = value;
                if (this.animation != null)
                {
                    this.animation.Frames.ForEach(f => f.Seconds = value);
                }
            }
        }

        /// <summary>
        /// Animation speed multiplier (1.0 is normal, 2.0 is twice as fast)
        /// </summary>
        public float AnimationSpeed = 1.0f;

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
                return textures[CurrentFrameNumber-1];
            }
            set
            {
                while (textures.Count() < CurrentFrameNumber)
                {
                    textures.Add(null);
                }
                while (pixels.Count() < CurrentFrameNumber)
                {
                    pixels.Add(null);
                }
                textures[CurrentFrameNumber-1] = value;
                pixels[CurrentFrameNumber-1] = null;
                CalculateCenter();
            }
        }

        public void NextFrame()
        {
            if (animation != null)
            {
                CurrentFrameNumber++;
                if (CurrentFrameNumber > animation.Frames.Count())
                {
                    CurrentFrameNumber = 1;
                }
            }
            else
            {
                CurrentFrameNumber++;
                if (CurrentFrameNumber > this.FrameCount)
                {
                    CurrentFrameNumber = 1;
                }
            }
        }

        public int FrameCount
        {
            get
            {
                return textures.Count();
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
        /// Add a frame to the costume by content name
        /// </summary>
        /// <param name="name">Conent name for the frame to add</param>
        public Costume AddFrame(string name)
        {
            return AddFrame(name, this.frameSeconds);
        }

        /// <summary>
        /// Add a frame to the costume by content name
        /// </summary>
        /// <param name="name">Conent name for the frame to add</param>
        public Costume AddFrame(string name, float seconds)
        {
            Texture2D newTexture = ScratchyXnaGame.ScratchyGame.Content.Load<Texture2D>("Costumes/" + name);
            return AddFrame(newTexture, seconds);
        }

        /// <summary>
        /// Add a frame to the costume by copying a texture
        /// </summary>
        /// <param name="texture">texture to add</param>
        public Costume AddFrame(Texture2D texture)
        {
            return AddFrame(texture, this.frameSeconds);
        }

        /// <summary>
        /// Add a frame to the costume by copying a texture
        /// </summary>
        /// <param name="texture">texture to add</param>
        public Costume AddFrame(Texture2D texture, float seconds)
        {
            textures.Add(texture);
            Color[] newPixels = new Color[texture.Width * texture.Height];
            texture.GetData(newPixels);
            pixels.Add(newPixels);
            return this;
        }

        /// <summary>
        /// Get a frame
        /// </summary>
        /// <param name="frameNumber">1 based frame index</param>
        /// <returns>A texture</returns>
        public Texture2D GetFrame(int frameNumber)
        {
            return textures[frameNumber - 1];
        }

        /// <summary>
        /// Adding a costume to the game
        /// </summary>
        /// <param name="scene">Scene that owns the sprite</param>
        /// <param name="content">The content manager</param>
        /// <param name="name">Name of the content</param>
        public void Load(string name)
        {
            Load(name, 1, 1);
        }

        /// <summary>
        /// Adding a costume to the game
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void Load(string name, int frameColumns, int frameRows)
        {
            Name = name;
            CurrentFrameNumber = 1;
            Texture2D newTexture = ScratchyXnaGame.ScratchyGame.Content.Load<Texture2D>("Costumes/" + name);
            if (frameColumns != 1 || frameRows != 1)
            {
                SplitAndAddTextures(newTexture, frameColumns, frameRows);
            }
            else
            {
                Texture = newTexture;
            }

            CalculateCenter();
        }

        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        private int SplitAndAddTextures(Texture2D original, int xCount, int yCount)
        {
            int frameCount = 0;
            int partHeight = original.Height / yCount;
            int partWidth = original.Width / xCount;
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            if (original.Width % xCount != 0)
            {
                throw new Exception("Costume " + Name + " width of " + original.Width + " does not divide evenly by " + xCount);
            }
            if (original.Height % yCount != 0)
            {
                throw new Exception("Costume " + Name + " height of " + original.Height + " does not divide evenly by " + yCount);
            }

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            for (int y = 0; y < yCount * partHeight; y += partHeight)
            {
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                            {
                                partData[partIndex] = Color.Transparent;
                            }
                            else
                            {
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                            }
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    textures.Add(part);
                    pixels.Add(partData);
                    frameCount++;
                }
            }
            return frameCount;
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

        internal void UpdateCostume(GameTime gameTime)
        {
            if (animation != null)
            {
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                elapsedFrameSeconds += elapsedSeconds;
                float currentFrameTime = animation.Frames[CurrentFrameNumber - 1].Seconds / AnimationSpeed;
                while (elapsedFrameSeconds > currentFrameTime)
                {
                    elapsedFrameSeconds -= currentFrameTime;
                    NextFrame();
                    currentFrameTime = animation.Frames[CurrentFrameNumber - 1].Seconds;
                }
            }
        }

        /// <summary>
        /// Create an animation using all frames with the same time per frame
        /// </summary>
        /// <param name="frameSeconds">Seconds per frame</param>
        /// <returns>The generated animation object</returns>
        public Animation CreateAnimation(float frameSeconds)
        {
            Animation animation = new Animation();
            for (int i = 0; i < this.FrameCount; i++)
            {
                animation.AddFrame(i, frameSeconds);
            }
            return animation;
        }

        /// <summary>
        /// Start an animation of all frames in the current costume.
        /// </summary>
        public Costume StartAnimation()
        {
            return StartAnimation(FrameSeconds);
        }

        /// <summary>
        /// Start an animation of all frames in the current costume.
        /// </summary>
        /// <param name="frameSeconds">How long to show each frame</param>
        public Costume StartAnimation(float frameSeconds)
        {
            return StartAnimation(CreateAnimation(frameSeconds));
        }

        public Costume StartAnimation(Animation animation)
        {
            this.animation = animation;
            elapsedFrameSeconds = 0;
            return this;
        }

        public void StopAnimation()
        {
            this.animation = null;
        }


        /// <summary>
        /// The pixels in the current texture
        /// </summary>
        public Color[] Pixels
        {
            get
            {
                if (pixels[currentFrameNumber-1] == null && Texture != null)
                {
                    pixels[CurrentFrameNumber-1] = new Color[Width * Height];
                    Texture.GetData(pixels[CurrentFrameNumber-1]);
                }
                return pixels[CurrentFrameNumber-1];
            }
            set
            {
                pixels[CurrentFrameNumber-1] = value;
                if (value != null)
                {
                    Texture.SetData(value);
                }
            }
        }
        private List<Color[]> pixels = new List<Color[]>();

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
            for (int i = 0; i < this.textures.Count(); i++)
            {
                Texture2D source = this.textures[i];

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
                this.textures[i] = flipped;
                this.pixels[i] = flippedData;
            }
        }

    }
}
