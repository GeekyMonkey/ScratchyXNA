using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace ScratchyXna
{
    abstract public class Scene : ScratchyObject
    {
        /// <summary>
        /// The game that owns this screen
        /// </summary>
        public ScratchyXnaGame Game;

        /// <summary>
        /// Has this screen been started
        /// </summary>
        public bool Started = false;

        /// <summary>
        /// The background color to draw
        /// </summary>
        public Color BackgroundColor = Color.Transparent;

        /// <summary>
        /// Sprite objects managed by this screen
        /// </summary>
        public List<Sprite> Sprites = new List<Sprite>();

        /// <summary>
        /// ScheduledEvents managed by this screen
        /// </summary>
        public List<ScheduledEvent> ScheduledEvents = new List<ScheduledEvent>();

        /// <summary>
        /// Texts managed by this screen
        /// </summary>
        public List<Text> Texts = new List<Text>();

        /// <summary>
        /// The Game's content manager
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// The current sprite batch being drawn
        /// </summary>
        public SpriteBatch Drawing;

        /// <summary>
        /// Conversion of -100 to 100 coordinates to screen pixels
        /// </summary>
        public float PixelScale;

        /// <summary>
        /// Minimum X position (left side of the screen in -100 to 100 scale)
        /// </summary>
        public float MinX;

        /// <summary>
        /// Maximum X position (right side of the screen in -100 to 100 scale)
        /// </summary>
        public float MaxX;

        internal Scene PreviousScreen = null;
        private GraphicsDevice Graphics;
        private SpriteFont font = null;
        private string fontName = null;
        private TimeSpan ScreenStartTime;
        private static SpriteFont defaultFont = null;
        private float GridSize = 10;
        public GridStyles GridStyle = GridStyles.None;
        internal bool DrawSpriteRects = false;
        public bool DebugTextVisible = false;
        public BackButtonBehaviours BackButtonBehaviour = BackButtonBehaviours.ShowFirstScreenOrExit;

        /// <summary>
        /// Init this screen at program startup
        /// </summary>
        /// <param name="game"></param>
        public void Init(ScratchyXnaGame game)
        {
            Game = game as ScratchyXnaGame;
            Graphics = game.GraphicsDevice;
            Content = game.Content;
            Drawing = game.spriteBatch;
            CalculatePixelScale();
            Load();
        }

        /// <summary>
        /// Get the name of the default font.  Setting it automatically loads the font
        /// </summary>
        public string FontName
        {
            get
            {
                return fontName;
            }
            set
            {
                fontName = value;
                font = LoadFont(fontName);
            }
        }

        /// <summary>
        /// Default font for this screen
        /// </summary>
        public SpriteFont Font
        {
            get
            {
                return font ?? defaultFont;
            }
            set
            {
                font = value;
                fontName = "";
            }
        }

        /// <summary>
        /// Load a font for texts to use (if not already loaded)
        /// </summary>
        /// <param name="fontName">Name of the font resource</param>
        /// <returns>A Font object</returns>
        public SpriteFont LoadFont(string fontName)
        {
            SpriteFont font = Game.LoadFont(fontName);
            if (defaultFont == null)
            {
                defaultFont = font;
            }
            return font;
        }

        /// <summary>
        /// Load a costume for sprites to use (if not already loaded)
        /// </summary>
        /// <param name="CostumeName">Name of the costume resource</param>
        /// <returns>A Costume object</returns>
        public Costume LoadCostume(string CostumeName)
        {
            return Game.LoadCostume(this, CostumeName);
        }


        /// <summary>
        /// Show this game screen
        /// </summary>
        public void ShowScreen()
        {
            ShowScreen(this);
        }

        /// <summary>
        /// Show a game screen
        /// </summary>
        /// <param name="ScreenToShow">The name of the screen to show</param>
        public void ShowScreen(string ScreenToShow)
        {
            Scene screenToShow = Game.GameScreens[ScreenToShow.ToLower()];
            if (screenToShow == null)
            {
                throw new Exception("Attempted to show an unknown screen named: " + ScreenToShow);
            }
            ShowScreen(screenToShow);
        }

        /// <summary>
        /// Show a game screen
        /// </summary>
        /// <param name="ScreenToShow"></param>
        public void ShowScreen(Scene ScreenToShow)
        {
            if (Game.activeGameScreen != null)
            {
                Game.activeGameScreen.StopGameScreen();
            }

            // Remember previous screen
            if (ScreenToShow != PreviousScreen)
            {
                ScreenToShow.PreviousScreen = Game.activeGameScreen;
            }
            Game.activeGameScreen = ScreenToShow;
            Game.activeGameScreen.StartGameScreen();
        }

        /// <summary>
        /// Override the Load function to setup things that happen once only when the program starts
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Override the Update function with things that need to happen whenver this screen is visible
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// This screen is starting to display
        /// </summary>
        public virtual void StartScreen()
        {
        }

        /// <summary>
        /// This screen is being hidden
        /// </summary>
        public virtual void StopScreen()
        {
        }

        /// <summary>
        /// Draw this screen. Texts and Sprites that were added will be drawn automatically.
        /// </summary>
        public virtual void Draw()
        {
        }

        /// <summary>
        /// Start the game screen
        /// </summary>
        public void StartGameScreen()
        {
            ScreenStartTime = TimeSpan.MaxValue;
            StartScreen();
            foreach (Text text in Texts)
            {
                text.Start();
            }
            Started = true;
        }

        /// <summary>
        /// Stop the game screen
        /// </summary>
        public void StopGameScreen()
        {
            Started = false;
            StopScreen();
            ScheduledEvents.Clear();
        }

        /// <summary>
        /// Update since the last game loop iteration
        /// </summary>
        /// <param name="gameTime">Time since the last update</param>
        public void UpdateScreen(GameTime gameTime)
        {
            if (ScreenStartTime == TimeSpan.MaxValue)
            {
                ScreenStartTime = gameTime.TotalGameTime;
            }
            Update(gameTime);
            foreach (Sprite sprite in Sprites)
            {
                sprite.UpdateSprite(gameTime);
            }
            foreach (Text text in Texts.ToArray())
            {
                text.Update(gameTime);
            }
            TimeSpan totalGameTime = gameTime.TotalGameTime;
            foreach (ScheduledEvent scheduledEvent in ScheduledEvents)
            {
                if (totalGameTime >= scheduledEvent.TargetTime)
                {
                    if (scheduledEvent.Repeat == false)
                    {
                        ScheduledEvents.Remove(scheduledEvent);
                    }
                    else
                    {
                        scheduledEvent.TargetTime = totalGameTime + scheduledEvent.Time;
                    }
                    scheduledEvent.Callback();
                    break;
                }
            }

            // Show/hide grid
            if (Keyboard.KeyDown(Keys.LeftControl))
            {
                if (Keyboard.KeyPressed(Keys.G))
                {
                    GridStyle = (GridStyle == GridStyles.Grid) ? GridStyles.None : GridStyles.Grid;
                }
                if (Keyboard.KeyPressed(Keys.T))
                {
                    GridStyle = (GridStyle == GridStyles.Ticks) ? GridStyles.None : GridStyles.Ticks;
                }
                if (Keyboard.KeyPressed(Keys.R))
                {
                    DrawSpriteRects = !DrawSpriteRects;
                }
                if (Keyboard.KeyPressed(Keys.D))
                {
                    DebugTextVisible = !DebugTextVisible;
                }
                if (Keyboard.KeyPressed(Keys.S))
                {
                    ShowScreen("Test");
                }
            }
        }

        /// <summary>
        /// Calculate the conversion of -100 to 100 coordinates to screen size pixel coordinates
        /// </summary>
        private void CalculatePixelScale()
        {
            PixelScale = 200f / (float)Graphics.Viewport.Height;
            MaxX = (PixelScale * (float)Graphics.Viewport.Width) / 2f;
            MinX = MaxX * -1f;
        }

        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            if (BackgroundColor != Color.Transparent)
            {
                //Graphics.Clear(ClearOptions.DepthBuffer, BackgroundColor, 100.0f, 0);
                //Graphics.Clear(ClearOptions.Target, BackgroundColor, 1.0f, 0);
                Graphics.Clear(BackgroundColor);
            }
            Drawing.Begin();

            // Draw sprites and texts using layering
            List<IDrawable> drawables = new List<IDrawable>();
            Sprites.ForEach(s => drawables.Add(s));
            Texts.ForEach(t => drawables.Add(t));
            foreach (IDrawable drawable in drawables.OrderBy(s => s.Layer))
            {
                drawable.DrawObject(Drawing);
            }

            // Custom Drawing
            Draw();

            // Draw the helper grid
            if (GridStyle != GridStyles.None && GridSize > 0.0f)
            {
                DrawGrid();
            }

            Drawing.End();
        }

        /// <summary>
        /// Draw a debugging rectangle
        /// </summary>
        internal void DrawRect(RectangleF rect, Color color)
        {
            Vector2 screenTopLeft = GetScreenPosition(rect.X, rect.Y);
            Vector2 screenBottomRight = GetScreenPosition(rect.X + rect.Width, rect.Y + rect.Height);
            DrawLinePixels(screenTopLeft, new Vector2(screenTopLeft.X, screenBottomRight.Y), 1, color);
            DrawLinePixels(screenBottomRight, new Vector2(screenTopLeft.X, screenBottomRight.Y), 1, color);
            DrawLinePixels(screenTopLeft, new Vector2(screenBottomRight.X, screenTopLeft.Y), 1, color);
            DrawLinePixels(screenBottomRight, new Vector2(screenBottomRight.X, screenTopLeft.Y), 1, color);
        }

        /// <summary>
        /// Draw a debugging grid
        /// </summary>
        private void DrawGrid()
        {
            Color gridColor = Color.White;
            if (GridStyle == GridStyles.Grid)
            {
                for (float x = -100; x <= 100; x += GridSize)
                {
                    DrawLine(new Vector2(x, -100), new Vector2(x, 100), 1, gridColor);
                }
                for (float y = -100; y <= 100; y += GridSize)
                {
                    DrawLine(new Vector2(-100, y), new Vector2(100, y), 1, gridColor);
                }
            }
            if (GridStyle == GridStyles.Ticks)
            {
                for (float x = -100; x <= 100; x += GridSize)
                {
                    DrawLine(new Vector2(x, -100), new Vector2(x, -95), 1, gridColor);
                    DrawLine(new Vector2(x, 100), new Vector2(x, 95), 1, gridColor);
                }
                for (float y = -100; y <= 100; y += GridSize)
                {
                    DrawLine(new Vector2(-100, y), new Vector2(-95, y), 1, gridColor);
                    DrawLine(new Vector2(100, y), new Vector2(95, y), 1, gridColor);
                }
                DrawLine(new Vector2(-5, 0), new Vector2(5, 0), 1, gridColor);
                DrawLine(new Vector2(0, -5), new Vector2(0, 5), 1, gridColor);
            }

            // Draw coordinate numbers on the left
            if (GridStyle != GridStyles.None)
            {
                for (float y = -100; y <= 100; y += 10f)
                {
                    new Text
                    {
                        Value = y.ToString("0.00"),
                        Position = new Vector2(MinX, y),
                        Scale = 0.3f,
                        Color = Color.SlateGray,
                        VerticalAlign = VerticalAlignments.Center,
                        Alignment = HorizontalAlignments.Left,
                        Font = this.Font
                    }.Draw(Drawing);
                }
            }
        }

        /// <summary>
        /// Draw a line on the screen using pixel coordinates
        /// </summary>
        /// <param name="point1">Point 1 in pixel coordinates</param>
        /// <param name="point2">Point 2 in pixel coordinates</param>
        /// <param name="width">Line width in pixels</param>
        /// <param name="color">Line color</param>
        internal void DrawLinePixels(Vector2 point1, Vector2 point2, float width, Color color)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            Drawing.Draw(Game.LineTexture, point1, null, color,
                                 angle, Vector2.Zero, new Vector2(length, width),
                                 SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a line on the screen using game coordinates
        /// </summary>
        /// <param name="point1">Point 1 in game coordinates</param>
        /// <param name="point2">Point 2 in game coordinates</param>
        /// <param name="width">Line width in pixels</param>
        /// <param name="color">Line color</param>
        public void DrawLine(Vector2 point1, Vector2 point2, float width, Color color)
        {
            DrawLinePixels(
                GetScreenPosition(point1),
                GetScreenPosition(point2),
                width,
                color);
        }

        /// <summary>
        /// Convert a -100 to 100 scale position into a screen pixel position
        /// </summary>
        /// <param name="x">X position in -100 to 100 scale</param>
        /// <param name="x">Y position in -100 to 100 scale</param>
        /// <returns>Pixel x/y coordinates</returns>
        public Vector2 GetScreenPosition(float x, float y)
        {
            return GetScreenPosition(new Vector2(x, y));
        }

        /// <summary>
        /// Convert a -100 to 100 scale position into a screen pixel position
        /// </summary>
        /// <param name="percentagePosition">Position in -100 to 100 scale</param>
        /// <returns>Pixel x/y coordinates</returns>
        public Vector2 GetScreenPosition(Vector2 percentagePosition)
        {
            return new Vector2(
                (float)Graphics.Viewport.Bounds.Center.X + percentagePosition.X / PixelScale,
                (float)Graphics.Viewport.Bounds.Center.Y - percentagePosition.Y / PixelScale);
        }

        /// <summary>
        /// Convert an XNA pixel position to a -100 to 100 position
        /// </summary>
        /// <param name="X">pixel X</param>
        /// <param name="Y">pixel Y</param>
        /// <returns>Position in -100 to 100 range</returns>
        public Vector2 PixelToPosition(int X, int Y)
        {
            return new Vector2(
                (X - Graphics.Viewport.Bounds.Center.X) * PixelScale,
                (Graphics.Viewport.Bounds.Center.Y - Y) * PixelScale);
        }

        /// <summary>
        /// Get the screen size in pixels
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScreenSize()
        {
            return new Vector2(Graphics.Viewport.Width, Graphics.Viewport.Height);
        }

        /// <summary>
        /// Add a text element to the screen which will automatically be drawn
        /// </summary>
        /// <param name="text">The text object to add</param>
        /// <returns>The text object added</returns>
        public Text AddText(Text text)
        {
            text.GameScreen = this;
            if (text.Font == null)
            {
                if (this.Font == null)
                {
                    throw new Exception("Can't add text unless a font name is specified");
                }
                text.Font = this.Font;
            }
            this.Texts.Add(text);
            if (Started == true)
            {
                text.Start();
            }
            return text;
        }

        /// <summary>
        /// Schedule an action to occur once in the future
        /// </summary>
        /// <param name="seconds">Seconds before the action</param>
        /// <param name="callback">Action callback function</param>
        public void Wait(double seconds, Action callback)
        {
            ScheduleEvent(seconds, false, callback);
        }

        /// <summary>
        /// Schedule an action to occur in the future at set intervals
        /// </summary>
        /// <param name="seconds">Seconds between action</param>
        /// <param name="callback">Action callback function</param>
        public void Forever(double seconds, Action callback)
        {
            ScheduleEvent(seconds, true, callback);
        }

        /// <summary>
        /// Add a scheduled event
        /// </summary>
        /// <param name="seconds">Seconds until the action</param>
        /// <param name="repeat">Should it repeat, or fire once</param>
        /// <param name="callback">Action callback</param>
        /// <returns>The created ScheduledEvent object</returns>
        internal ScheduledEvent ScheduleEvent(double seconds, bool repeat, Action callback)
        {
            ScheduledEvent scheduledEvent = new ScheduledEvent(Game.gameTime.TotalGameTime, seconds, callback, repeat);
            ScheduledEvents.Add(scheduledEvent);
            return scheduledEvent;
        }

        /// <summary>
        /// Add a sprite to the screen which will be automatically drawn and updated
        /// </summary>
        /// <typeparam name="T">Type of sprite</typeparam>
        /// <returns>The sprite which was added</returns>
        public T AddSprite<T>() where T : Sprite, new()
        {
            T sprite = new T();
            Sprites.Add(sprite);
            sprite.Init(this);
            return sprite;
        }
    }
}
