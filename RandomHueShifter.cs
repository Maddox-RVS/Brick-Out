using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickOut
{
    internal class RandomHueShifter
    {
        private Random rnd;
        private Color targetColor;
        private Color currentColor;
        private int minColorRange;
        private int maxColorRange;
        private float hueShiftSpeed;

        public RandomHueShifter(int minColorRange, int maxColorRange, float hueShiftSpeed)
        {
            rnd = new Random();
            currentColor = generateRandomColor();
            targetColor = generateRandomColor();
            this.minColorRange = minColorRange;
            this.maxColorRange = maxColorRange;
            this.hueShiftSpeed = hueShiftSpeed;
        }

        private Color generateRandomColor()
        {
            return new Color(rnd.Next(minColorRange, maxColorRange + 1), rnd.Next(minColorRange, maxColorRange + 1), rnd.Next(minColorRange, maxColorRange + 1));
        }

        public void update()
        {
            float r = currentColor.R;
            float g = currentColor.G;
            float b = currentColor.B;
            
            if (Math.Abs(r - targetColor.R) < hueShiftSpeed) r = targetColor.R;
            else if (r < targetColor.R) r += hueShiftSpeed;
            else r -= hueShiftSpeed;

            if (Math.Abs(g - targetColor.G) < hueShiftSpeed) g = targetColor.G;
            else if (g < targetColor.G) g += hueShiftSpeed;
            else g -= hueShiftSpeed;

            if (Math.Abs(b - targetColor.B) < hueShiftSpeed) b = targetColor.B;
            else if (b < targetColor.B) b += hueShiftSpeed;
            else b -= hueShiftSpeed;

            currentColor = new Color((int) r, (int) g, (int) b);
            if (targetColor == currentColor) targetColor = generateRandomColor();
        }

        public Color getColor()
        {
            return currentColor;
        }
    }
}
