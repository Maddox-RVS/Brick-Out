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
    internal class Paddle
    {
        private Texture2D texture;
        private Vector2 position;
        private float width, height;
        private float speed;
        private Color color;
        private float hueShiftSpeed;

        public Paddle(Texture2D texture, float width, float height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.position = new Vector2((Game1.screenBounds.Width / 2) - (width / 2), Game1.screenBounds.Height - height - 20);
            speed = 10;
            color = new Color(100, 0, 0);
            hueShiftSpeed = 5.0f;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) width, (int) height);
        }

        public void checkMovement()
        {
            Vector2 pos = position;

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += speed;
            }

            if (pos.X != position.X) color = new Color((int) (color.R + hueShiftSpeed), color.G, color.B);
            else color = new Color((int) (color.R - hueShiftSpeed), color.G, color.B);
            if (color.R < 200) color.R = 200;
        }

        public void checkWalls()
        {
            float beforePos = position.X;
            position.X = Math.Clamp(position.X, 0, Game1.screenBounds.Width - width);
            if (beforePos != position.X) Game1.paddleHittingWall = true;
            else Game1.paddleHittingWall = false;
        }

        public void update()
        {
            checkMovement();
            checkWalls();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, getBounds(), color);
        }
    }
}
