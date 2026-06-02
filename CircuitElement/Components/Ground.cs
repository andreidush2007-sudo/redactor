using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Ground : CircuitElement
    {
        private static Image groundImage = ResourceHelper.GetImage("symbol_3542092");

        public Ground(int x, int y) : base("Ground", x, y) { }

        public override void Draw(Graphics g)
        {
            if (groundImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 20;
                g.DrawImage(groundImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("GND", new Font("Arial", 10), Brushes.Black, X + 25, Y + 25);
            }
        }
    }
}