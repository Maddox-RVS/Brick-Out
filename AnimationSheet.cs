using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickOut
{
    internal class AnimationSheet
    {
        private Texture2D animationSheet;
        private int cellWidth, cellHeight;
        private float frameTimeMilliSecs;
        private float waitTimeMillisLastFrame;
        private float timeElapsed;
        private Texture2D[] frames;
        private int frameIndex;

        public AnimationSheet(Texture2D animationSheet, int cellWidth, int cellHeight, int fps, float waitTimeLastFrame, GraphicsDevice graphicsDevice)
        {
            this.animationSheet = animationSheet;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            frameTimeMilliSecs = 1000.0f / fps;
            this.waitTimeMillisLastFrame = waitTimeLastFrame;
            frameIndex = 0;

            frames = generateFrames(graphicsDevice);
        }
        public AnimationSheet(Texture2D animationSheet, int cellWidth, int cellHeight, int fps, GraphicsDevice graphicsDevice)
        {
            this.animationSheet = animationSheet;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            frameTimeMilliSecs = 1000.0f / fps; 
            waitTimeMillisLastFrame = 0.0f;
            frameIndex = 0;

            frames = generateFrames(graphicsDevice);
        }

        private Texture2D[] generateFrames(GraphicsDevice graphicsDevice)
        {
            int frameCount = animationSheet.Width / cellWidth;
            Texture2D[] frames = new Texture2D[frameCount];
            for (int i = 0; i < frames.Length; i++)
            {
                Texture2D frame = new Texture2D(graphicsDevice, cellWidth, cellHeight);
                Color[] colorData = new Color[cellWidth * cellHeight];
                Rectangle rect = new Rectangle(i * cellWidth, 0, cellWidth, cellHeight);
                animationSheet.GetData(0, rect, colorData, 0, colorData.Length);
                frame.SetData(colorData);
                frames[i] = frame;
            }
            return frames;
        }

        public Texture2D getFrame()
        {
            return frames[frameIndex];
        }

        public Texture2D[] getFrames()
        {
            return frames;
        }

        public void update(GameTime gameTime)
        {
            timeElapsed += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeElapsed >= frameTimeMilliSecs)
            {
                if (frameIndex != frames.Length - 1 || timeElapsed >= frameTimeMilliSecs + waitTimeMillisLastFrame)
                {
                    timeElapsed = 0.0f;
                    frameIndex += 1;
                    if (frameIndex >= frames.Length) frameIndex = 0;
                }
            }
        }
    }
}
