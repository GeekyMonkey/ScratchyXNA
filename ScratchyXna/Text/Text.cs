using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class Text : GraphicObject
    {
        /// <summary>
        /// Add this to your text value to move down to the next line
        /// </summary>
        public static string NewLine = "\r\n";

        /// <summary>
        /// Color used to draw this text
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Font used to draw this text
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// Horizontal (X) alignment
        /// </summary>
        public HorizontalAlignments Alignment = HorizontalAlignments.Left;

        /// <summary>
        /// Vertical (Y) alignment
        /// </summary>
        public VerticalAlignments VerticalAlign = VerticalAlignments.Top;

        /// <summary>
        /// The one animation type for this text
        /// </summary>
        public TextAnimations AnimationType = TextAnimations.None;

        /// <summary>
        /// The speed of the animation. Lower is faster.  Default is 0.1
        /// </summary>
        public double AnimationSeconds = 0.1;

        /// <summary>
        /// How strong will the animation be.  1=normal
        /// </summary>
        public double AnimationIntensity = 1.0;

        /// <summary>
        /// Delay before the animation starts
        /// </summary>
        public double AnimationDelaySeconds = 0.0;

        /// <summary>
        /// Did the animation complete
        /// </summary>
        private bool AnimationComplete = true;

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
        /// Action to take when the animation is complete
        /// </summary>
        public Action<Text> OnAnimationComplete = null;

        /// <summary>
        /// Has the animation started
        /// </summary>
        public bool Started = false;

        private bool ready = false;
        private Vector2 screenCenter;
        private string value = "";
        private string ValueToDraw = "";
        private TimeSpan AnimationElapsed = TimeSpan.MinValue;
        private double AnimationStartTotalSeconds = 0.0;
        private float FontScale;
        private bool Dirty = true;
        private Vector2 ScreenPosition;
        private Vector2 Size;
        private float ScaleToDraw = 1.0f;        

        /// <summary>
        /// Construct a text object
        /// </summary>
        public Text()
        {
            // Default text on top of other elements
            layer = 100.0f;
        }

        /// <summary>
        /// The text to display
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                ValueToDraw = value;
                Dirty = true;
            }
        }


        /// <summary>
        /// Scale of the text.  Scale 1.0 = 20 lines of text vertically
        /// </summary>
        public override float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                this.scale = value;
                ScaleToDraw = value;
                Dirty = true;
            }
        }

        /// <summary>
        /// Start the animation
        /// </summary>
        public void Start()
        {
            Started = true;
            AnimationStartTotalSeconds = 0.0;
            if (AnimationType != TextAnimations.None)
            {
                AnimationComplete = false;
                switch (AnimationType)
                {
                    case TextAnimations.Typewriter:
                        ValueToDraw = "";
                        break;
                    case TextAnimations.None:
                    case TextAnimations.Throb:
                    default:
                        break;
                }
            }
            if (AnimationDelaySeconds > 0.0)
            {
                Visible = false;
            }
        }

        /// <summary>
        /// Show this text if hidden
        /// </summary>
        public void Show()
        {
            Visible = true;
        }

        /// <summary>
        /// Hide this text if visible
        /// </summary>
        public void Hide()
        {
            Visible = false;
        }

        /// <summary>
        /// Get or set the X position
        /// </summary>
        public float X
        {
            get
            {
                return this.Position.X;
            }
            set
            {
                this.Position.X = value;
                Dirty = true;
            }
        }

        /// <summary>
        /// Get or set the Y position
        /// </summary>
        public float Y
        {
            get
            {
                return this.Position.Y;
            }
            set
            {
                this.Position.Y = value;
                Dirty = true;
            }
        }

        /// <summary>
        /// Set the horizontal (X) Alignment for this text
        /// </summary>
        /// <param name="alignment">Alignment type</param>
        public void SetAlignment(HorizontalAlignments alignment)
        {
            this.Alignment = alignment;
            ProcessText();
        }

        /// <summary>
        /// Set the font for this text
        /// </summary>
        /// <param name="font"></param>
        private void SetFont(SpriteFont font)
        {
            this.Font = font;
            Dirty = true;
        }

        /// <summary>
        /// Process the text (calcualte it's position and size)
        /// </summary>
        private void ProcessText()
        {
            if (ready == false)
            {
                return;
            }
            FontScale = (Font.LineSpacing / 400f);
            Vector2 unscaledSize = Font.MeasureString(Value);
            Size = Vector2.Transform(unscaledSize, Matrix.CreateScale(ScaleToDraw * FontScale));
            ScreenPosition = Scene.GetScreenPosition(Position);
            screenCenter = Vector2.Zero;
            if (Alignment == HorizontalAlignments.Right)
            {
                ScreenPosition.X = ScreenPosition.X - Size.X;
                // screenCenter.X = Size.X;
            }
            else if (Alignment == HorizontalAlignments.Center)
            {
                ScreenPosition.X = ScreenPosition.X - (Size.X / 2.0f);
                //screenCenter.X = Size.X / -2f;
            }
            if (VerticalAlign == VerticalAlignments.Bottom)
            {
                ScreenPosition.Y = ScreenPosition.Y - Size.Y;
                //screenCenter.Y = - Size.Y;
            }
            else if (VerticalAlign == VerticalAlignments.Center)
            {
                ScreenPosition.Y = ScreenPosition.Y - (Size.Y / 2.0f);
                //screenCenter.Y = Size.Y / 2f;
            }
            Dirty = false;
        }

        /// <summary>
        /// Update this text
        /// </summary>
        /// <param name="gameTime">Time since the last update</param>
        public void Update(GameTime gameTime)
        {
            Vector2 prevPosition = Position;
            UpdateGraphicObject(gameTime);
            if (prevPosition != Position)
            {
                ProcessText();
            }

            if (AnimationStartTotalSeconds == 0.0)
            {
                AnimationStartTotalSeconds = gameTime.TotalGameTime.TotalSeconds;
                AnimationElapsed = gameTime.ElapsedGameTime;
            }
            if (Dirty)
            {
                ProcessText();
            }
            if (AnimationComplete == false)
            {
                AnimationElapsed += gameTime.ElapsedGameTime;
                if (AnimationElapsed.TotalSeconds > AnimationSeconds)
                {
                    if (gameTime.TotalGameTime.TotalSeconds > (AnimationStartTotalSeconds + AnimationDelaySeconds))
                    {
                        Visible = true;
                    }
                    if (Visible)
                    {
                        switch (AnimationType)
                        {
                            case TextAnimations.Throb:
                                ScaleToDraw = Scale + (float)(Math.Cos((gameTime.TotalGameTime.TotalSeconds - (AnimationStartTotalSeconds + AnimationDelaySeconds))) * (AnimationIntensity * Scale));
                                Dirty = true;
                                break;
                            case TextAnimations.SeeSaw:
                                Rotation = (float)(Math.Cos((gameTime.TotalGameTime.TotalSeconds - (AnimationStartTotalSeconds + AnimationDelaySeconds))) * (AnimationIntensity * Scale));
                                Dirty = true;
                                break;
                            case TextAnimations.Typewriter:
                                int len = (int)((gameTime.TotalGameTime.TotalSeconds - (AnimationStartTotalSeconds + AnimationDelaySeconds)) / AnimationSeconds);
                                if (len < 0)
                                {
                                    len = 0;
                                }
                                if (len >= Value.Length)
                                {
                                    AnimationComplete = true;
                                    len = Value.Length;
                                }
                                ValueToDraw = Value.Substring(0, len);
                                break;
                        }
                    }
                    if (Dirty)
                    {
                        ProcessText();
                    }
                }
                if (AnimationComplete)
                {
                    if (OnAnimationComplete != null)
                    {
                        OnAnimationComplete(this);
                    }
                }
            }
        }

        /// <summary>
        /// Draw this text
        /// </summary>
        /// <param name="Drawing">SpriteBatch drawing context</param>
        public void Draw(SpriteBatch Drawing)
        {
            Drawing.DrawString(
                Font,
                ValueToDraw /* + " X=" + ScreenPosition.X + " Y=" + ScreenPosition.Y */,
                ScreenPosition,
                Color,
                Rotation,
                screenCenter,
                ScaleToDraw * FontScale,
                SpriteEffects.None,
                Depth
                );
        }

        public override void DrawObject(SpriteBatch drawing)
        {
            if (Visible && ValueToDraw != null)
            {
                Draw(drawing);
            }
            ready = true;
        }

        /// <summary>
        /// Create a sprite copy of the current state of this text object
        /// </summary>
        /// <returns>A new sprite</returns>
        public TextSprite CreateSprite()
        {
            if (Dirty)
            {
                ProcessText();
            }
            Vector2 screenPos = this.ScreenPosition;
            Vector2 pos = this.Game.PixelToPosition((int)screenPos.X, (int)screenPos.Y);
            float PixelScale = 200f / (float)this.Game.GraphicsDevice.Viewport.Height;
            TextSprite textSprite = new TextSprite
            {
                Costume = new Costume
                {
                    Name = "TextSprite",
                },
                Layer = this.Layer,
                Position = this.Position, // pos,
                Scale = PixelScale // 1f / (this.ScaleToDraw) //todo
            };
            var customTexture = new RenderTarget2D(this.Game.GraphicsDevice, (int)this.Size.X, (int)this.Size.Y);
            textSprite.Costume.Texture = customTexture;
            textSprite.Costume.XCenter = this.Alignment;
            switch (this.VerticalAlign)
            {
                case VerticalAlignments.Top:
                    textSprite.Costume.YCenter = VerticalAlignments.Top;
                    break;
                case VerticalAlignments.Center:
                    textSprite.Costume.YCenter = VerticalAlignments.Center;
                    break;
                case VerticalAlignments.Bottom:
                    textSprite.Costume.YCenter = VerticalAlignments.Bottom;
                    break;
            }

            // Set render target 
            this.Game.GraphicsDevice.SetRenderTarget(customTexture);

            // Copy the current costume
            this.Game.spriteBatch.Begin();
            this.Game.GraphicsDevice.Clear(Color.Transparent);
            this.Game.spriteBatch.DrawString(
                Font,
                ValueToDraw,
                Vector2.Zero,
                Color,
                Rotation,
                Vector2.Zero,
                ScaleToDraw * FontScale,
                SpriteEffects.None,
                Depth
                );
            this.Game.spriteBatch.End();

            // Unset render target 
            this.Game.GraphicsDevice.SetRenderTarget(null);

            return textSprite;
        }
    }
}
