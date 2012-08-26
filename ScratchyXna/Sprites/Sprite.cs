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
    abstract public class Sprite : GraphicObject 
    {
        /// <summary>
        /// The game content manager
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// Scale (1.0 is normal)
        /// </summary>
        public float Scale = 1.0f;

        /// <summary>
        /// The color for this sprite.  White = no change.
        /// </summary>
        public Color SpriteColor = Color.White;

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

        private Costume costume;
        private List<Costume> SpriteCostumes = new List<Costume>();
        private float Alpha = 1.0f;

        /// <summary>
        /// Get or set the size of the sprite where 100% is the default size
        /// </summary>
        public float Size
        {
            get
            {
                return Scale * 100f;
            }
            set
            {
                Scale = value / 100f;
            }
        }

        /// <summary>
        /// Set the size of the sprite where 100% is the default size
        /// </summary>
        /// <param name="newSize"></param>
        public void SetSize(float newSize)
        {
            Size = newSize;
        }

        /// <summary>
        /// Change the size by some percentage
        /// </summary>
        /// <param name="changeAmount">Percentage amouunt for change. Positive numbers make it bigger, negative make it smaller.</param>
        public void ChangeSize(float changeAmount)
        {
            Size += changeAmount;
        }

        /// <summary>
        /// Get the current costume name
        /// </summary>
        public string CostumeName
        {
            get
            {
                if (Costume == null)
                {
                    return "";
                }
                return Costume.Name;
            }
        }

        /// <summary>
        /// Set the costume for this sprite
        /// </summary>
        /// <param name="name">Name of the costume</param>
        public Costume SetCostume(string name)
        {
            Costume = SpriteCostumes.FirstOrDefault(sc => sc.Name == name);
            if (Costume == null)
            {
                Costume = Game.LoadCostume(Scene, name);
            }
            return Costume;
        }

        /// <summary>
        /// Switch to a specific costume number (in the order they were added)
        /// </summary>
        /// <param name="number"></param>
        public void SetCostume(int number)
        {
            Costume = SpriteCostumes[number - 1];
        }

        /// <summary>
        /// Switch to the next costume
        /// </summary>
        public void NextCostume()
        {
            int nextCostumeNumber = GetCostumeNumber() + 1;
            if (nextCostumeNumber > SpriteCostumes.Count)
            {
                nextCostumeNumber = 1;
            }
            SetCostume(nextCostumeNumber);
        }

        /// <summary>
        /// Switch to the previous costume
        /// </summary>
        public void PreviousCostume()
        {
            int nextCostumeNumber = GetCostumeNumber() - 1;
            if (nextCostumeNumber < 1)
            {
                nextCostumeNumber = SpriteCostumes.Count;
            }
            SetCostume(nextCostumeNumber);
        }

        /// <summary>
        /// Get the current costume number
        /// </summary>
        /// <returns></returns>
        public int GetCostumeNumber()
        {
            int costumeIndex = 0;
            try
            {
                costumeIndex = SpriteCostumes.IndexOf(Costume);
            }
            catch { }
            costumeIndex++;
            return costumeIndex;
        }

        /// <summary>
        /// The current costume for this sprite
        /// </summary>
        public Costume Costume
        {
            get
            {
                return costume;
            }
            set
            {
                costume = value;
            }
        }

        /// <summary>
        /// Get a costume ready to use
        /// </summary>
        /// <param name="costumeName"></param>
        public Costume AddCostume(string costumeName)
        {
            // Use the shared content loader
            Costume costume = Game.LoadCostume(Scene, costumeName);

            // Keep a local list of sprite specific costumes
            SpriteCostumes.Add(costume);

            if (Costume == null)
            {
                Costume = costume;
            }
            return costume;
        }

        /// <summary>
        /// Width in viewport scale
        /// </summary>
        public float Width
        {
            get
            {
                return (float)Costume.Texture.Width * Scene.PixelScale * Scale;
            }
        }

        /// <summary>
        /// Height in viewport scale
        /// </summary>
        public float Height
        {
            get
            {
                return (float)Costume.Texture.Height * Scene.PixelScale * Scale;
            }
        }

        /// <summary>
        /// X position in range -100 to 100
        /// </summary>
        public double X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = (float)value;
            }
        }

        /// <summary>
        /// Y position in range -100 to 100
        /// </summary>
        public double Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = (float)value;
            }
        }

        /// <summary>
        /// Go to a new X,Y Position
        /// </summary>
        /// <param name="x">New X Position</param>
        /// <param name="y">New Y Position</param>
        public void GoTo(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        /// <summary>
        /// Go to a new X,Y Position
        /// </summary>
        /// <param name="position">New Position</param>
        public void GoTo(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Go to the same position as another sprite
        /// </summary>
        /// <param name="otherSprite"></param>
        public void GoTo(Sprite otherSprite)
        {
            Position = otherSprite.Position;
        }

        /// <summary>
        /// Show this sprite if it's hidden
        /// </summary>
        public void Show()
        {
            Visible = true;
        }

        /// <summary>
        /// Hide this sprite if it's visible
        /// </summary>
        public void Hide()
        {
            Visible = false;
        }

        /// <summary>
        /// Override to Load this sprite. You should set a costume.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Init this sprite
        /// </summary>
        /// <param name="scene">The scene that owns this sprite</param>
        public void Init(Scene scene)
        {
            Scene = scene;
            Game = scene.Game;
            Load();
        }

        /// <summary>
        /// Update this sprite. The base implementation changes the position based on the velocity and direction
        /// </summary>
        /// <param name="gameTime">Time since the last update</param>
        public void UpdateSprite(GameTime gameTime)
        {
            Update(gameTime);
            UpdateGraphicObject(gameTime);
        }

        /// <summary>
        /// Move in the current direction by some distance
        /// </summary>
        /// <param name="distance">Distance to move</param>
        public void Move(float distance)
        {
            Position += Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(MathHelper.ToRadians(Direction))) * distance;
        }

        /// <summary>
        /// Draw this sprite.  The base implementation does standard drawing
        /// </summary>
        /// <param name="Drawing">SpriteBatch drawing context</param>
        public override void DrawObject(SpriteBatch Drawing)
        {
            if (Visible == true && Costume != null)
            {
                Draw(Drawing);
            }
        }

        /// <summary>
        /// Update this sprite
        /// </summary>
        /// <param name="gameTime">Time since the last update</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Normal drawing for this sprite
        /// </summary>
        /// <param name="Drawing">Sprite drawing context</param>
        public virtual void Draw(SpriteBatch Drawing)
        {
            Vector2 screenPos = Scene.GetScreenPosition(Position);
            Drawing.Draw(Costume.Texture,
                screenPos,
                null,
                SpriteColor * Alpha,
                rotationRadians,
                Costume.Center,
                Scale / Scene.PixelScale,
                SpriteEffects.None,
                Depth);

            // Draw the collision rect
            if (Scene.DrawSpriteRects)
            {
                Scene.DrawRect(Rect, Color.Gray);
                Scene.DrawRect(new RectangleF(Position.X -0.5f, Position.Y -0.5f, 1.0f, 1.0f), Color.Magenta);
            }
        }

        /*
        /// <summary>
        /// Rectangle in screen pixels
        /// </summary>
        public RectangleF ScreenRect
        {
            get
            {
                RectangleF rect = Rect;
                Vector2 screenTopLeft = GameScreen.GetScreenPosition(rect.Left, rect.Top);
                Vector2 screenBottomRight = GameScreen.GetScreenPosition(rect.Right, rect.Bottom);
                return new RectangleF(screenTopLeft.X, screenTopLeft.Y,
                    screenBottomRight.X - screenTopLeft.X, screenBottomRight.Y - screenTopLeft.Y);
            }
        }
        */

        /// <summary>
        /// Rectangle in -100 to 100 scale
        /// </summary>
        public RectangleF Rect
        {
            get
            {
               // Matrix scaleMatrix = Matrix.CreateScale(Scale);
                Matrix transform = Transform;

                var rectangle = new Microsoft.Xna.Framework.Rectangle(
                    0, 0, Costume.Width, Costume.Height);
                Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
                Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
                Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
                Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

                // Transform all four corners into work space
                Vector2.Transform(ref leftTop, ref transform, out leftTop);
                Vector2.Transform(ref rightTop, ref transform, out rightTop);
                Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
                Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

                // Find the minimum and maximum extents of the rectangle in world space
                Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                          Vector2.Min(leftBottom, rightBottom));
                Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                          Vector2.Max(leftBottom, rightBottom));

                // Return that as a rectangle
                return new RectangleF(min.X, min.Y,
                                     (max.X - min.X), (max.Y - min.Y));
            }
        }

        /// <summary>
        /// Is this sprite touching the right edge of the screen
        /// </summary>
        /// <returns>True if touching</returns>
        public bool IsTouchingRight()
        {
            return Rect.Right >= 100f;
        }

        /// <summary>
        /// Is this sprite touching the left edge of the screen
        /// </summary>
        /// <returns>True if touching</returns>
        public bool IsTouchingLeft()
        {
            return Rect.Left <= -100f;
        }

        /// <summary>
        /// Is this sprite touching the top edge of the screen
        /// </summary>
        /// <returns>True if touching</returns>
        public bool IsTouchingTop()
        {
            return Rect.Top >= 100f;
        }

        /// <summary>
        /// Is this sprite touching the bottom edge of the screen
        /// </summary>
        /// <returns>True if touching</returns>
        public bool IsTouchingBottom()
        {
            return Rect.Bottom <= -100f;
        }

        /// <summary>
        /// Is this sprite touching any of the screen edges
        /// </summary>
        /// <returns>True if touching an edge</returns>
        public bool IsTouchingEdge()
        {
            return IsTouchingLeft() || IsTouchingRight() || IsTouchingTop() || IsTouchingBottom();
        }

        /// <summary>
        /// Is the sprite completely off of the screen
        /// </summary>
        /// <returns></returns>
        public bool IsOffScreen()
        {
            RectangleF spriteRect = Rect; //todo: don't use temp variable once Rect property is cached
            return spriteRect.Right < Scene.MinX
                || spriteRect.Left > Scene.MaxX
                || spriteRect.Top < -100
                || spriteRect.Bottom > 100;
        }

        /// <summary>
        /// Is this sprite touching another sprite
        /// </summary>
        /// <param name="otherSprite">Sprite to check for collision</param>
        /// <returns>True if touching</returns>
        public bool IsTouching(Sprite otherSprite)
        {
            // Don't look for collisions with hidden sprites
            if (otherSprite == null || otherSprite.Visible == false)
            {
                return false;
            }

            // Do the bounding rectangles intersect
            if (!Rect.Intersects(otherSprite.Rect))
            {
                return false;
            }

            // use pixel collision checking
            return IntersectPixels(
                this.Transform,
                this.Costume.Texture.Width,
                this.Costume.Texture.Height,
                this.Costume.Pixels,
                otherSprite.Transform,
                otherSprite.Costume.Texture.Width,
                otherSprite.Costume.Texture.Height,
                otherSprite.Costume.Pixels);
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + ((heightA - yA - 1) * widthA)];
                        Color colorB = dataB[xB + ((heightB - yB - 1) * widthB)];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Is the sprite touching a specific point
        /// </summary>
        /// <param name="point">position to check</param>
        /// <returns>True if touching</returns>
        public bool IsTouching(IEnumerable<Vector2> points)
        {
            return points.Any(p => IsTouching(p));
        }

        /// <summary>
        /// Is the sprite touching a specific point
        /// </summary>
        /// <param name="point">position to check</param>
        /// <returns>True if touching</returns>
        public bool IsTouching(Vector2 point)
        {
            if (Rect.Contains(point.X, point.Y))
            {
                // pixel collision checking

                // Calculate the position of the point within the sprite
                Vector2 posInSprite = Vector2.Transform(point, Matrix.Invert(Transform));
                int width = this.Costume.Texture.Width;
                int height = this.Costume.Texture.Height;
                Color[] pixels = this.Costume.Pixels;

                // Round to the nearest pixel
                int xB = (int)Math.Round(posInSprite.X);
                int yB = (int)Math.Round(posInSprite.Y);

                // If the pixel lies within the bounds of B
                if (0 <= xB && xB < width &&
                    0 <= yB && yB < height)
                {
                    // Get the colors of the overlapping pixels
                    Color colorA = pixels[xB + ((height - yB - 1) * width)];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Schedule an action to occur once in the future
        /// </summary>
        /// <param name="seconds">Seconds before the action</param>
        /// <param name="callback">Action callback function</param>
        public void Wait(double seconds, Action callback)
        {
            Scene.ScheduleEvent(seconds, false, callback);
        }

        /// <summary>
        /// Schedule an action to occur in the future at set intervals
        /// </summary>
        /// <param name="seconds">Seconds between action</param>
        /// <param name="callback">Action callback function</param>
        public void Forever(double seconds, Action callback)
        {
            Scene.ScheduleEvent(seconds, true, callback);

        }
       /// <summary>
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
        /// Move this sprite on top of all other sprites
        /// </summary>
        public void GoToFront()
        {
            Layer = Scene.Sprites.OrderByDescending(s => s.Layer).First().Layer + 1;
        }

        /// <summary>
        /// Move this sprite underneath all other sprites
        /// </summary>
        public void GoToBack()
        {
            Layer = Scene.Sprites.OrderBy(s => s.Layer).First().Layer - 1;
        }

        /// <summary>
        /// Get the matrix tranform for this sprite's texture offset, rotation, scale, and position
        /// </summary>
        public Matrix Transform
        {
            get
            {
                Vector2 pos = Position;
                Matrix transform =
                    Matrix.CreateTranslation(new Vector3(Costume.Center.X - costume.Width, Costume.Center.Y - costume.Height, 0f)) *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationZ(rotationRadians * -1) *
                    Matrix.CreateTranslation(new Vector3(pos, 0));
                return transform;
            }
        }

        /// <summary>
        /// Get the matrix tranform for this sprite's texture offset, rotation, scale, and position
        /// </summary>
        public Matrix TransformAlt
        {
            get
            {
                Vector2 pos = Position;
                pos.Y = pos.Y * -1;
                Matrix transform =
                    Matrix.CreateTranslation(new Vector3(-Costume.Center.X, -Costume.Center.Y, 0f)) *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationZ(rotationRadians) *
                    Matrix.CreateTranslation(new Vector3(pos, 0));
                return transform;
            }
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their scene positions. The other sprite will be drawn as normal and cropped to the rectangle of the current sprite.
        /// </summary>
        /// <param name="otherSprite">The sprite to stamp onto this one</param>
        public void Stamp(Sprite otherSprite)
        {
            Stamp(otherSprite, StampMethods.Normal, StampCroppings.CropToSprite);
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their scene positions. The other sprite will be cropped to the rectangle of the current sprite.
        /// </summary>
        /// <param name="otherSprite">The sprite to stamp onto this one</param>
        /// <param name="stampMethod">Stamp drawing method (Normal or Cutout)</param>
        public void Stamp(Sprite otherSprite, StampMethods stampMethod)
        {
            Stamp(otherSprite, stampMethod, StampCroppings.CropToSprite);
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their scene positions
        /// </summary>
        /// <param name="otherSprite">The sprite to stamp onto this one</param>
        /// <param name="stampMethod">Stamp drawing method (Normal or Cutout)</param>
        /// <param name="stampMethod">Stamp cropping method (Crop or Grow)</param>
        public void Stamp(Sprite otherSprite, StampMethods stampMethod, StampCroppings stampCropping)
        {
            switch (stampCropping)
            {
                case StampCroppings.CropToSprite:
                    break;
                case StampCroppings.GrowSprite:
                default:
                    throw new Exception("Sprite.Stamp() does not yet support cropping mode " + stampCropping);
            }

            int width = Costume.Width;
            int height = Costume.Height;
            switch (stampMethod)
            {
                case StampMethods.Normal:
                    {
                        RenderTarget2D newTexture = new RenderTarget2D(
                            this.Game.GraphicsDevice,
                            width,
                            height,
                            /*mipMap:*/ false,
                            /*preferredFormat:*/ SurfaceFormat.Color,
                            /*preferredDepthFormat:*/ DepthFormat.Depth24Stencil8,
                            /*preferredMultiSampleCount:*/ 1,
                            /*usage:*/ RenderTargetUsage.DiscardContents);

                        // Calculate a matrix which transforms from A's local space into
                        // world space and then into B's local space
                        Matrix TransformA = this.TransformAlt;
                        Matrix TransformB = otherSprite.TransformAlt;

                        Matrix transformAToB = TransformB * Matrix.Invert(TransformA);
                        Vector2 position;
                        float rotation;
                        float scale;
                        transformAToB.Decompose2D(out position, out rotation, out scale);

                        // Set render target 
                        this.Game.GraphicsDevice.SetRenderTarget(newTexture);

                        this.Game.GraphicsDevice.Clear(Color.Transparent);
                        this.Game.spriteBatch.Begin();
                        // Copy the current costume
                        this.Game.spriteBatch.Draw(Costume.Texture, Vector2.Zero, Color.White);

                        // Draw the other sprite
                        this.Game.spriteBatch.Draw(otherSprite.Costume.Texture,
                            position,
                            null,
                            otherSprite.SpriteColor * otherSprite.Alpha,
                            rotation,
                            Vector2.Zero, // otherSprite.Costume.Center,
                            otherSprite.Scale / Scale,
                            SpriteEffects.None,
                            0f);
                        this.Game.spriteBatch.End();

                        // Unset render target 
                        this.Game.GraphicsDevice.SetRenderTarget(null);

                        Costume.Texture = newTexture;
                    }
                    break;
                case StampMethods.Cutout:
                    {
                        Color[] newPixels = StampAlpha(
                            this.Transform, this.Costume.Texture.Width, this.Costume.Texture.Height, this.Costume.Pixels,
                            otherSprite.Transform, otherSprite.Costume.Texture.Width, otherSprite.Costume.Texture.Height, otherSprite.Costume.Pixels, false);
                        Costume.Pixels = newPixels;
                    }
                    break;
                case StampMethods.CutoutInverted:
                    {
                        Color[] newPixels = StampAlpha(
                            this.Transform, this.Costume.Texture.Width, this.Costume.Texture.Height, this.Costume.Pixels,
                            otherSprite.Transform, otherSprite.Costume.Texture.Width, otherSprite.Costume.Texture.Height, otherSprite.Costume.Pixels, true);
                        Costume.Pixels = newPixels;
                    }
                    break;
                default:
                    throw new Exception("Sprite.Stamp() does not yet support stampMethod " + stampMethod);
            }
        }

        /// <summary>
        /// Stamp the alpha values from one sprite onto another sprite
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        private static Color[] StampAlpha(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB, bool Invert)
        {
            Color[] newPixels = new Color[dataA.Count()];

            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            /*
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            transformAToB.Decompose(out scale, out rotation, out translation);
            */
            //transformAToB = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(translation);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);
                    int indexA = xA + ((heightA - yA - 1) * widthA);
                    Color colorA = dataA[indexA];

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorB = dataB[xB + ((heightB - yB - 1) * widthB)];

                        byte alpha = colorA.A;
                        byte alphaB = colorB.A;
                        if (Invert)
                        {
                            alphaB = (byte)(0xff - alphaB);
                        }
                        alpha = (byte)(Math.Max(0, alpha - alphaB));
                        // newPixels[indexA] = colorB;
                        newPixels[indexA] = colorA *(alpha / 255f);
                    }
                    else
                    {
                        if (Invert)
                        {
                            newPixels[indexA] = Color.Transparent;
                        }
                        else
                        {
                            newPixels[indexA] = colorA;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            return newPixels;
        }

    }
}
