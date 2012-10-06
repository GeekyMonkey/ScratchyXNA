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
    /// <summary>
    /// Base for sprite and text classes
    /// </summary>
    public abstract class GraphicObject : ScratchyObject, IDrawable
    {
        protected float scale = 1.0f;

        /// <summary>
        /// Scale multiplier
        /// </summary>
        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
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
        internal float layer = 2.0f;

        public abstract void DrawObject(SpriteBatch Drawing);

        /// <summary>
        /// Position of this text in -100 to 100 range
        /// </summary>
        public Vector2 Position;
        
        /// <summary>
        /// Move to another point in a set amount of time and then stop
        /// </summary>
        /// <param name="position">Position to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        /// <param name="glideComplete">Action to do when glide is complete</param>
        public void GlideTo(Vector2 position, float seconds, Action glideComplete)
        {
            glidePosition = position;
            glideTime = this.Scene.Game.gameTime.TotalGameTime + TimeSpan.FromSeconds(seconds);
            OnGlideComplete = glideComplete;
        }

        /// <summary>
        /// Move to another point in a set amount of time and then stop
        /// </summary>
        /// <param name="position">Position to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(Vector2 position, float seconds)
        {
            GlideTo(position, seconds, null);
        }

        /// <summary>
        /// Move to another point in a set amount of time and then stop
        /// </summary>
        /// <param name="X">X Position to move to</param>
        /// <param name="X">Y Position to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(float X, float Y, float seconds)
        {
            GlideTo(new Vector2(X, Y), seconds, null);
        }

        /// <summary>
        /// Move to another point in a set amount of time and then stop
        /// </summary>
        /// <param name="X">X Position to move to</param>
        /// <param name="X">Y Position to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        /// <param name="glideComplete">Action to do when glide is complete</param>
        public void GlideTo(float X, float Y, float seconds, Action glideComplete)
        {
            GlideTo(new Vector2(X, Y), seconds, glideComplete);
        }

        /// <summary>
        /// Move to another sprite's position in a set amount of time and then stop
        /// </summary>
        /// <param name="otherSprite">Sprite to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(GraphicObject otherSprite, float seconds, Action glideComplete)
        {
            GlideTo(otherSprite.Position, seconds, glideComplete);
        }

        /// <summary>
        /// Move to another sprite's position in a set amount of time and then stop
        /// </summary>
        /// <param name="otherSprite">Sprite to move to</param>
        /// <param name="seconds">How long to take to get there</param>
        public void GlideTo(GraphicObject otherSprite, float seconds)
        {
            GlideTo(otherSprite.Position, seconds, null);
        }

        private Vector2? glidePosition = null;
        private TimeSpan glideTime;
        private Action OnGlideComplete = null;

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
        /// The game scene that owns this sprite
        /// </summary>
        public Scene Scene
        {
            get
            {
                return scene;
            }
            set
            {
                scene = value;
                Game = scene.Game;
            }
        }
        private Scene scene;

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
        private float speed = 0.0f;

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
        private float rotation = 0.0f;
        internal float rotationRadians = 0.0f;

        /// <summary>
        /// Is this text visible
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Show this if it's hidden
        /// </summary>
        public void Show()
        {
            Visible = true;
        }

        /// <summary>
        /// Hide this if it's visible
        /// </summary>
        public void Hide()
        {
            Visible = false;
        }

        private Vector2 velocity = Vector2.Zero;
        private float direction = 0.0f;
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
        /// The game
        /// </summary>
        public ScratchyXnaGame Game
        {
            //todo: is this needed as a property, or should it just point to the global
            get
            {
                if (game == null)
                {
                    game = ScratchyXnaGame.ScratchyGame;
                }
                return game;
            }
            set
            {
                game = value;
            }
        }
        private ScratchyXnaGame game;

        internal void UpdateGraphicObject(GameTime gameTime)
        {
            // Glide it
            if (glidePosition != null)
            {
                float secondsToTarget = (float)(glideTime - gameTime.TotalGameTime).TotalSeconds;
                if (secondsToTarget <= 0)
                {
                    // Finished glide
                    Position = glidePosition.Value;
                    glidePosition = null;
                    Speed = 0;
                    if (OnGlideComplete != null)
                    {
                        OnGlideComplete();
                    }
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
        /// Set the position of this text in -100 to 100 range
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(double x, double y)
        {
            this.Position = new Vector2((float)x, (float)y);
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
        /// Schedule an action to occur once in the future
        /// </summary>
        /// <param name="seconds">Seconds before the action</param>
        /// <param name="callback">Action callback function</param>
        public ScheduledEvent Wait(double seconds, Action callback)
        {
            return Scene.ScheduleEvent(this, seconds, false, callback);
        }

        /// <summary>
        /// Schedule an action to occur in the future at set intervals
        /// </summary>
        /// <param name="seconds">Seconds between action</param>
        /// <param name="callback">Action callback function</param>
        public ScheduledEvent Forever(double seconds, Action callback)
        {
            return Scene.ScheduleEvent(this, seconds, true, callback);
        }

    }
}
