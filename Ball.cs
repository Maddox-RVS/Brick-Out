using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Diagnostics;
using System.Buffers;

namespace BrickOut
{
    internal class Ball
    {
        public enum MovementState
        {
            FOLLOW_PADDLE,
            BOUNCE
        }

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float width;
        private float height;
        private MovementState movementState;
        private SpriteFont font;
        private Color color;
        
        public Ball(Texture2D texture, float width, float height, float x, float y, MovementState movementState, SpriteFont font)
        {
            this.texture = texture;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;
            this.movementState = movementState;
            velocity = Vector2.Zero;

            this.font = font;
            color = Color.White;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) width, (int) height);
        }

        private void updateBounceMovement()
        {
            position += velocity;

            if (position.X < 0 || position.X > Game1.screenBounds.Width - width) velocity.X *= -1.0f;
            if (position.Y < 0) velocity.Y *= -1.0f;
            else if (position.Y > Game1.screenBounds.Height - height) handleMiss();

            position.X = Math.Clamp(position.X, 0, Game1.screenBounds.Width - width);
            position.Y = Math.Clamp(position.Y, 0, Game1.screenBounds.Height - height);
        }

        private void followPaddlePosition(Rectangle paddleBounds)
        {
            position.X = paddleBounds.X + (paddleBounds.Width / 2) - (width / 2);
            position.Y = paddleBounds.Y - height;
            velocity = Vector2.Zero;
        }

        private void checkMovement(Rectangle paddleBounds)
        {
            switch(movementState) {
                case MovementState.FOLLOW_PADDLE:
                {
                    followPaddlePosition(paddleBounds);
                    break;
                }
                case MovementState.BOUNCE:
                {
                    updateBounceMovement();
                    break;
                }
                default: break;
            }
        }

        public void addVelocity(float xVelo, float yVelo)
        {
            velocity += new Vector2(xVelo, yVelo * -1.0f);
        }

        public void setVelocity(float xVelo, float yVelo)
        {
            velocity = new Vector2(xVelo, yVelo * -1.0f);
        }

        public void setMovementstate(MovementState movementState)
        {
            this.movementState = movementState;
        }

        private void handleObjectCollisions(params Rectangle[] objectBounds)
        {
            foreach (Rectangle objectBound in objectBounds)
            {
                if (getBounds().Intersects(objectBound))
                {
                    float leftDist = Math.Abs(getBounds().Left - objectBound.Right);
                    float rightDist = Math.Abs(getBounds().Right - objectBound.Left);
                    float topDist = Math.Abs(getBounds().Top - objectBound.Bottom);
                    float bottomDist = Math.Abs(getBounds().Bottom - objectBound.Top);

                    if (isSmallestValue(leftDist, rightDist, topDist, bottomDist))
                    {
                        position.X += leftDist;
                        velocity.X *= -1.0f;
                    }
                    else if (isSmallestValue(rightDist, leftDist, topDist, bottomDist))
                    {
                        position.X -= rightDist;
                        velocity.X *= -1.0f;
                    }
                    else if (isSmallestValue(topDist, leftDist, rightDist, bottomDist))
                    {
                        position.Y += topDist;
                        velocity.Y *= -1.0f;
                    }
                    else if (isSmallestValue(bottomDist, leftDist, rightDist, topDist))
                    {
                        position.Y -= bottomDist;
                        velocity.Y *= -1.0f;
                    }
                }
            }
        }

        private void handleBrickCollisions(List<Brick> bricks)
        {
            foreach (Brick brick in bricks)
                if (getBounds().Intersects(brick.getBounds())) brick.isHit();
            handleObjectCollisions(bricks.Select(brick => brick.getBounds()).ToArray());
        }

        private Boolean isSmallestValue(float value, params float[] values)
        {
            foreach (float val in values)
                if (value >= val) return false;
            return true;
        }

        private void handleMiss()
        {
            Game1.hoverTexts.Add(new HoverText(font, Color.Red, "-1 Lives", position.X, position.Y, 1.0f, HoverText.Direction.UP, 8.0f));
            Game1.lives -= 1;
            movementState = MovementState.FOLLOW_PADDLE;
        }

        public MovementState getMovementState()
        {
            return movementState;
        }

        public void update(Rectangle paddleBounds, List<Brick> bricks)
        {
            checkMovement(paddleBounds);
            handleObjectCollisions(paddleBounds);
            handleBrickCollisions(bricks);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, getBounds(), color);
        }
    }
}
