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
    abstract public class Sprite : ScratchyObject, IDrawable
    {
        /// <summary>
        /// Is this sprite visible
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// The game screen that owns this sprite
        /// </summary>
        public Scene GameScreen
        {
            get
            {
                return gameScreen;
            }
            set
            {
                gameScreen = value;
                Game = gameScreen.Game;
            }
        }
        private Scene gameScreen;

        /// <summary>
        /// The game content manager
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// The game
        /// </summary>
        public ScratchyXnaGame Game;

        /// <summary>
        /// Position of this sprite in -100 to 100 range
        /// </summary>
        public Vector2 Position = Vector2.Zero;

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

        /// <summary>
        /// Specifies the order that the sprites are drawn. Default is 1. Layer 1 is in the back, and larger numbers are drawn on top of this.
        /// </summary>
        public float Layer
        {
            get
            {
                return layer;
            }
            set
            {
                layer = value;
            }
        }

        private float rotation = 0.0f;
        private float rotationRadians = 0.0f;
        private Vector2 velocity = Vector2.Zero;
        private Costume costume;
        private string costumeName;
        private float direction = 0.0f;
        private float speed = 0.0f;
        private List<Costume> SpriteCostumes = new List<Costume>();
        private float Alpha = 1.0f;
        private float layer = 1.0f;

        /// <summary>
        /// Cutom costume when the costume needs to be modified for this sprite instance
        /// </summary>
        private RenderTarget2D customTexture;

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
        /// Get the costume name
        /// </summary>
        public string CostumeName
        {
            get
            {
                return costumeName;
            }
        }

        /// <summary>
        /// Set the costume for this sprite
        /// </summary>
        /// <param name="name">Name of the costume</param>
        public Costume SetCostume(string name)
        {
            Costume = Game.LoadCostume(GameScreen, name);
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
                costumeName = costume.Name;
                customTexture = null;
                this.customTexturePixels = null;
            }
        }

        /// <summary>
        /// Get a costume ready to use
        /// </summary>
        /// <param name="costumeName"></param>
        public Costume AddCostume(string costumeName)
        {
            // Use the shared content loader
            Costume costume = Game.LoadCostume(GameScreen, costumeName);

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
                return (float)Costume.Texture.Width * GameScreen.PixelScale * Scale;
            }
        }

        /// <summary>
        /// Height in viewport scale
        /// </summary>
        public float Height
        {
            get
            {
                return (float)Costume.Texture.Height * GameScreen.PixelScale * Scale;
            }
        }

        /// <summary>
        /// Rotation in degrees
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                rotationRadians = MathHelper.ToRadians(rotation);
            }
        }


        /// <summary>
        /// 2D velocity of this sprite
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                speed = velocity.Length();
                if (speed != 0f)
                {
                    float angle = (float)Math.Atan2(velocity.Y, velocity.X);
                    direction = (angle < 0) ? MathHelper.ToDegrees(angle + MathHelper.TwoPi) : MathHelper.ToDegrees(angle);
                }
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
        /// Direction that the sprite is moving in (right = 0, up = 90, left = 180, down = 270)
        /// </summary>
        public float Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                velocity = Vector2.UnitX * speed;
                velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(MathHelper.ToRadians(direction)));
            }
        }

        /// <summary>
        /// Speed that the sprite is moving
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                velocity = Vector2.UnitX * speed;
                velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(MathHelper.ToRadians(direction)));
            }
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
        /// <param name="gameScreen"></param>
        public void Init(Scene gameScreen)
        {
            GameScreen = gameScreen;
            Game = gameScreen.Game;
            Load();
        }

        /// <summary>
        /// Update this sprite. The base implementation changes the position based on the velocity and direction
        /// </summary>
        /// <param name="gameTime">Time since the last update</param>
        public void UpdateSprite(GameTime gameTime)
        {
            Update(gameTime);

            // Glide it
            if (glidePosition != null) {
                float secondsToTarget = (float)(glideTime - gameTime.TotalGameTime).TotalSeconds;
                if (secondsToTarget <= 0)
                {
                    // Finished glide
                    Position = glidePosition.Value;
                    glidePosition = null;
                    Speed = 0;
                }
                else
                {
                    float distanceToTarget = DistanceTo(glidePosition.Value);
                    DirectionTowards(glidePosition.Value);
                    Speed = distanceToTarget / secondsToTarget / 100f;
                }
            }

            // Move it
            Position += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Game.SpeedMultiplier);
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
        public void DrawObject(SpriteBatch Drawing)
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
            Vector2 screenPos = GameScreen.GetScreenPosition(Position);
            Drawing.Draw(Texture, screenPos, null, SpriteColor * Alpha, rotationRadians, Costume.Center, Scale / GameScreen.PixelScale, SpriteEffects.None, Depth);

            // Draw the collision rect
            if (GameScreen.DrawSpriteRects)
            {
                GameScreen.DrawRect(Rect, Color.Gray);
                GameScreen.DrawRect(new RectangleF(Position.X -0.5f, Position.Y -0.5f, 1.0f, 1.0f), Color.Magenta);
            }
        }

        /// <summary>
        /// Get the current texture for this sprite, either from a custom costume, or the current costume
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return customTexture ?? costume.Texture;
            }
        }

        public Color[] Pixels
        {
            get
            {
                if (customTexture == null)
                {
                    return Costume.Pixels;
                }
                if (customTexturePixels == null && customTexture != null)
                {
                    customTexturePixels = new Color[customTexture.Width * customTexture.Height];
                    customTexture.GetData(customTexturePixels);
                }
                return customTexturePixels;
            }
            set
            {
                if (customTexture == null)
                {
                    Costume.Pixels = value;
                }
                else
                {
                    customTexture = new RenderTarget2D(Game.GraphicsDevice, customTexture.Width, customTexture.Height);
                    customTexturePixels = value;
                    customTexture.SetData(customTexturePixels);
                }
            }
        }
        private Color[] customTexturePixels;

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
        /// Get the matrix tranform for this sprite's texture offset, rotation, scale, and position
        /// </summary>
        public Matrix Transform
        {
            get
            {
                // Matrix scaleMatrix = Matrix.CreateScale(Scale);
                Matrix transform =
                    Matrix.CreateTranslation(new Vector3(-Costume.Center.X, Costume.Center.Y - costume.Texture.Height, 0f)) *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationZ(rotationRadians) *
                    Matrix.CreateTranslation(new Vector3(Position, 0));
                return transform;
            }
        }

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
                    0, 0, costume.Texture.Width, costume.Texture.Height);
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
            return spriteRect.Right < GameScreen.MinX
                || spriteRect.Left > GameScreen.MaxX
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
                this.Transform, this.Costume.Texture.Width, this.Costume.Texture.Height, this.Costume.Pixels,
                otherSprite.Transform, otherSprite.Costume.Texture.Width, otherSprite.Costume.Texture.Height, otherSprite.Costume.Pixels);
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
        /// Get the distance between this sprites center point and another sprite center point
        /// </summary>
        /// <param name="OtherSprite">The other sprite to measure to</param>
        /// <returns>The distance in -100 to 100 scale</returns>
        public float DistanceTo(Sprite OtherSprite)
        {
            return Vector2.Distance(this.Position, OtherSprite.Position);
        }

        /// <summary>
        /// Get the distance between this sprites center point and another point
        /// </summary>
        /// <param name="OtherSprite">The other sprite to measure to</param>
        /// <returns>The distance in -100 to 100 scale</returns>
        public float DistanceTo(Vector2 Point)
        {
            return Vector2.Distance(this.Position, Point);
        }

        /// <summary>
        /// Schedule an action to occur once in the future
        /// </summary>
        /// <param name="seconds">Seconds before the action</param>
        /// <param name="callback">Action callback function</param>
        public void Wait(double seconds, Action callback)
        {
            GameScreen.AddTimer(seconds, false, callback);
        }

        /// <summary>
        /// Schedule an action to occur in the future at set intervals
        /// </summary>
        /// <param name="seconds">Seconds between action</param>
        /// <param name="callback">Action callback function</param>
        public void Forever(double seconds, Action callback)
        {
            GameScreen.AddTimer(seconds, true, callback);
        }


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
        /// Get the angle between this object and another point
        /// </summary>
        /// <param name="otherPoint">Point to look at</param>
        public float AngleTowards(Vector2 otherPoint)
        {
            Vector2 differenceVector = otherPoint - this.Position;
            return MathHelper.ToDegrees((float)Math.Atan2(differenceVector.Y, differenceVector.X));
        }

        /// <summary>
        /// Set the direction of this sprite towards another point
        /// </summary>
        /// <param name="otherPoint">Point to go to</param>
        public void DirectionTowards(Vector2 otherPoint)
        {
            Direction = AngleTowards(otherPoint);
        }

        /// <summary>
        /// Set the direction of this sprite towards another point
        /// </summary>
        /// <param name="otherSprite">Sprite to go to</param>
        public void DirectionTowards(Sprite otherSprite)
        {
            Direction = AngleTowards(otherSprite.Position);
        }

        /// <summary>
        /// Set the sprite's direction and speed based on which key is down
        /// </summary>
        /// <param name="keyboard">Keyboard input device</param>
        /// <param name="speed">Speed if any direction keys are pressed</param>
        public void DirectionFrom(KeyboardInput keyboard, float speed)
        {
            Speed = speed;
            if (keyboard.KeyDown(Keys.Right))
            {
                Direction = 0;
                if (keyboard.KeyDown(Keys.Up))
                {
                    Direction = 45;
                }
                else if (keyboard.KeyDown(Keys.Down))
                {
                    Direction = -45;
                }
            }
            else if (keyboard.KeyDown(Keys.Left))
            {
                Direction = 180;
                if (keyboard.KeyDown(Keys.Up))
                {
                    Direction = 135;
                }
                else if (keyboard.KeyDown(Keys.Down))
                {
                    Direction = -135;
                }
            }
            else if (keyboard.KeyDown(Keys.Up))
            {
                Direction = 90;
            }
            else if (keyboard.KeyDown(Keys.Down))
            {
                Direction = -90;
            }
            else
            {
                Speed = 0;
            }
        }

        /// <summary>
        /// Rotate this sprite towards another point
        /// </summary>
        /// <param name="otherPoint">Point to go to</param>
        /// <param name="adjustment">Depending on the costume, you may need to adjust by 90 degrees or some other amount</param>
        public void RotateTowards(Vector2 otherPoint, float adjustment)
        {
            Rotation = AngleTowards(otherPoint) * -1 + adjustment;
        }
        /// <summary>
        /// Rotate this sprite towards another point
        /// </summary>
        /// <param name="otherPoint">Point to go to</param>
        /// <param name="adjustment">Depending on the costume, you may need to adjust by 90 degrees or some other amount</param>
        public void RotateTowards(Vector2 otherPoint)
        {
            RotateTowards(otherPoint, 0f);
        }

        /// <summary>
        /// Rotate this sprite towards another point
        /// </summary>
        /// <param name="otherSprite">Sprite to look at</param>
        /// <param name="adjustment">Depending on the costume, you may need to adjust by 90 degrees or some other amount</param>
        public void RotateTowards(Sprite otherSprite, float adjustment)
        {
            Rotation = AngleTowards(otherSprite.Position) * -1 + adjustment;
        }

        /// <summary>
        /// Move to another point in a set amount of time and then stop
        /// </summary>
        /// <param name="position">Position to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(Vector2 position, float seconds)
        {
            glidePosition = position;
            glideTime = this.GameScreen.Game.gameTime.TotalGameTime + TimeSpan.FromSeconds(seconds);
        }
        private Vector2? glidePosition = null;
        private TimeSpan glideTime;

        /// <summary>
        /// Is this sprite currently gliding towards a point
        /// </summary>
        public bool Gliding
        {
            get
            {
                return (glidePosition != null);
            }
        }

        /// <summary>
        /// Stop the current glide
        /// </summary>
        public void GlideStop()
        {
            glidePosition = null;
        }

        /// <summary>
        /// Move to another sprite's position in a set amount of time and then stop
        /// </summary>
        /// <param name="otherSprite">Sprite to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(Sprite otherSprite, float seconds)
        {
            GlideTo(otherSprite.Position, seconds);
        }

        /// <summary>
        /// Move this sprite on top of all other sprites
        /// </summary>
        public void GoToFront()
        {
            Layer = GameScreen.Sprites.OrderByDescending(s => s.Layer).First().Layer + 1;
        }

        /// <summary>
        /// Move this sprite underneath all other sprites
        /// </summary>
        public void GoToBack()
        {
            Layer = GameScreen.Sprites.OrderBy(s => s.Layer).First().Layer - 1;
        }

        /// <summary>
        /// Begin creating a custom costume by copying the current costume
        /// </summary>
        public void CustomizeCostume()
        {
            if (this.customTexture == null)
            {
                this.customTexture = new RenderTarget2D(this.Game.GraphicsDevice, Costume.Texture.Width, Costume.Texture.Height);
                this.customTexturePixels = null;

                // Set render target 
                this.Game.GraphicsDevice.SetRenderTarget(customTexture);

                // Copy the current costume
                this.Game.spriteBatch.Begin();
                this.Game.GraphicsDevice.Clear(Color.Transparent);
                this.Game.spriteBatch.Draw(this.Costume.Texture, Vector2.Zero, Color.White);
                this.Game.spriteBatch.End();

                // Unset render target 
                this.Game.GraphicsDevice.SetRenderTarget(null);

                // Force reload of pixels if needed
                customTexturePixels = null;
            }
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their screen positions. The other sprite will be drawn as normal and cropped to the rectangle of the current sprite.
        /// </summary>
        /// <param name="otherSprite">The sprite to stamp onto this one</param>
        public void Stamp(Sprite otherSprite)
        {
            Stamp(otherSprite, StampMethods.Normal, StampCroppings.CropToSprite);
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their screen positions. The other sprite will be cropped to the rectangle of the current sprite.
        /// </summary>
        /// <param name="otherSprite">The sprite to stamp onto this one</param>
        /// <param name="stampMethod">Stamp drawing method (Normal or Cutout)</param>
        public void Stamp(Sprite otherSprite, StampMethods stampMethod)
        {
            Stamp(otherSprite, stampMethod, StampCroppings.CropToSprite);
        }

        /// <summary>
        /// Stamp another sprite onto this sprite based on their screen positions
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

            int width = Texture.Width;
            int height = Texture.Height;
            RenderTarget2D newTexture = new RenderTarget2D(
                this.Game.GraphicsDevice,
                width, height,
                /*mipMap:*/ false, 
                /*preferredFormat:*/ SurfaceFormat.Color,
                /*preferredDepthFormat:*/ DepthFormat.Depth24Stencil8,
                /*preferredMultiSampleCount:*/ 1,
                /*usage:*/ RenderTargetUsage.DiscardContents);
            switch (stampMethod)
            {
                case StampMethods.Normal:
                    {
                        // Calculate a matrix which transforms from A's local space into
                        // world space and then into B's local space
                        Matrix transformAToB = otherSprite.Transform * Matrix.Invert(this.Transform);
                        Vector2 position;
                        float rotation;
                        float scale;
                        transformAToB.Decompose2D(out position, out rotation, out scale);

                        // Set render target 
                        this.Game.GraphicsDevice.SetRenderTarget(newTexture);

                        this.Game.GraphicsDevice.Clear(Color.Transparent);
                        this.Game.spriteBatch.Begin();
                        // Copy the current costume
                        this.Game.spriteBatch.Draw(this.Texture, Vector2.Zero, Color.White);
                        // Draw the other sprite
                        this.Game.spriteBatch.Draw(otherSprite.Texture, position, null, Color.White, rotation, Vector2.Zero, otherSprite.Scale / Scale, SpriteEffects.None, 0f);
                        this.Game.spriteBatch.End();

                        // Unset render target 
                        this.Game.GraphicsDevice.SetRenderTarget(null);
                    }
                    break;
                case StampMethods.Cutout:
                    {
                        CustomizeCostume();
                        Color[] newPixels = StampAlpha(
                            this.Transform, this.Costume.Texture.Width, this.Costume.Texture.Height, this.Costume.Pixels,
                            otherSprite.Transform, otherSprite.Costume.Texture.Width, otherSprite.Costume.Texture.Height, otherSprite.Costume.Pixels, false);
                        Pixels = newPixels;
                        newTexture = customTexture;
                    }
                    break;
                case StampMethods.CutoutInverted:
                    {
                        CustomizeCostume();
                        Color[] newPixels = StampAlpha(
                            this.Transform, this.Costume.Texture.Width, this.Costume.Texture.Height, this.Costume.Pixels,
                            otherSprite.Transform, otherSprite.Costume.Texture.Width, otherSprite.Costume.Texture.Height, otherSprite.Costume.Pixels, true);
                        Pixels = newPixels;
                        newTexture = customTexture;
                    }
                    break;
                default:
                    throw new Exception("Sprite.Stamp() does not yet support stampMethod " + stampMethod);
            }
            this.customTexture = newTexture;
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

            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            transformAToB.Decompose(out scale, out rotation, out translation);
            transformAToB = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation.Z * -1) * Matrix.CreateTranslation(translation);

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
