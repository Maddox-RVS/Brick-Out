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
    internal class ParallaxBackground
    {
        private Texture2D[] layers;
        private Vector2[] positions;
        private float[] movementSpeeds;

        public ParallaxBackground(Texture2D layer1, Texture2D layer2, Texture2D layer3, Texture2D layer4, Texture2D layer5, Texture2D layer6)
        {
            layers = new Texture2D[]
            {
                layer1,
                layer2,
                layer3,
                layer4,
                layer5,
                layer6
            };

            positions = new Vector2[layers.Length];
            for (int i = 0; i < positions.Length; i++) positions[i] = Vector2.Zero;

            movementSpeeds = new float[]
            {
                1.2f,
                0.8f,
                0.6f,
                0.2f,
                0.1f,
                0.0f
            };
        }

        public void update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left) && !Game1.paddleHittingWall)
                for (int i = 0; i < positions.Length; i++) positions[i].X -= movementSpeeds[i];
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right) && !Game1.paddleHittingWall)
                for (int i = 0; i < positions.Length; i++) positions[i].X += movementSpeeds[i];

            for (int i = 0; i < positions.Length; i++) positions[i].X = Math.Clamp(positions[i].X, -50, 50);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
                spriteBatch.Draw(layers[i], new Rectangle((int)positions[i].X - 50, (int)positions[i].Y, Game1.screenBounds.Width + 100, Game1.screenBounds.Height), Color.White);
        } 
    }
}
