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
            color = new Color(0, 0, 255);
            hueShiftSpeed = 15.0f;
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

            if (pos.X != position.X) color = new Color((int) (color.R + hueShiftSpeed), color.G, (int) (color.B - hueShiftSpeed));
            else color = new Color((int) (color.R - hueShiftSpeed), color.G, (int) (color.B + hueShiftSpeed));
        }

        public void checkWalls()
        {
            position.X = Math.Clamp(position.X, 0, Game1.screenBounds.Width - width);
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
