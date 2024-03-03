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
using System.Diagnostics.Tracing;

namespace BrickOut
{
    internal class Brick
    {
        private Texture2D texture;
        private SpriteFont font;
        private Vector2 position;
        private float width, height;
        private int hitPoints;
        private int maxHitPoints;
        private BrickType brickType;
        private int damage;
        private AnimationSheet poweredGlintAnimation;
        private RandomHueShifter rndHueShifter;

        public enum BrickType
        {
            NORMAL,
            POWERED
        }

        public Brick(Texture2D texture, SpriteFont font, float width, float height, float x, float y, int hitPoints, BrickType brickType, AnimationSheet poweredGlintAnimation)
        {
            this.texture = texture;
            this.font = font;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;
            maxHitPoints = 100;
            this.hitPoints = Math.Clamp(hitPoints, 1, maxHitPoints);
            this.brickType = brickType;
            damage = 5;
            this.poweredGlintAnimation = poweredGlintAnimation;
            rndHueShifter = new RandomHueShifter(100, 255, 1.0f);
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) width, (int) height);
        }

        public int getHitPoints()
        {
            return hitPoints;
        }

        private Color generateColor()
        {
            float red = 255 * ((float) hitPoints / (float) maxHitPoints);
            return new Color((int) red, 0, 255 - (int) red);
        }

        private Vector2 hitPointDisplayPos()
        {
            if (hitPoints >= 100)
            {
                return new Vector2(position.X + 5, position.Y + 13);
            }
            else if (hitPoints >= 10)
            {
                return new Vector2(position.X + 10, position.Y + 13);
            }
            else
            {
                return new Vector2(position.X + 18, position.Y + 13);
            }
        }

        public void isHit()
        {
            int damageDone = 0;
            if (hitPoints - damage >= 0) damageDone = damage;
            else damageDone = hitPoints;
            hitPoints -= damageDone;
            Color brickColor = generateColor();
            Game1.hoverTexts.Add(new HoverText(font, new Color(100 + brickColor.R, 100, 100 + brickColor.B), "+" + damageDone, position.X, position.Y, 1.0f, HoverText.Direction.DOWN, 5.0f));

            //if (brickType == BrickType.POWERED) { }
        }

        public void update(GameTime gameTime)
        {
            poweredGlintAnimation.update(gameTime);
            rndHueShifter.update();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Color brickColor = brickType == BrickType.NORMAL ? generateColor() : rndHueShifter.getColor();
            spriteBatch.Draw(texture, getBounds(), brickColor);
            if (brickType == BrickType.NORMAL) spriteBatch.DrawString(font, hitPoints.ToString(), hitPointDisplayPos(), new Color(100 + brickColor.R, 100, 100 + brickColor.B));
            if (brickType == BrickType.POWERED) spriteBatch.Draw(poweredGlintAnimation.getFrame(), getBounds(), Color.White);
        }
    }
}
