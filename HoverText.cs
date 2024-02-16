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

namespace BrickOut
{
    public class HoverText
    {
        private SpriteFont font;
        private Color color;
        private String text;
        private Vector2 position;
        private float movementSpeed;
        private Direction direction;
        private float opacityFadeSpeed;
        private float alpha;

        public enum Direction
        {
            UP,
            DOWN
        }

        public HoverText(SpriteFont font, Color color, String text, float x, float y, float movementSpeed, Direction movementDirection, float opacityFadeSpeed)
        {
            this.font = font;
            this.color = color;
            this.text = text;
            position = new Vector2(x, y);
            this.movementSpeed = movementSpeed;
            direction = movementDirection;
            this.opacityFadeSpeed = opacityFadeSpeed;
            alpha = 255.0f;
        }

        public float getAlpha()
        {
            return alpha;
        }

        public void update()
        {
            if (direction == Direction.UP) position.Y -= movementSpeed;
            else position.Y += movementSpeed;

            alpha -= opacityFadeSpeed;
            Math.Clamp(alpha, 0, 255);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, new Color(color.R, color.G, color.B, (int) alpha));
        }
    }
}
