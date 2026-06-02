using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Diode : CircuitElement
    {
        private static Image diodeImage = ResourceHelper.GetImage("diode_14020824");

        public Diode(int x, int y) : base("Diode", x, y) { }

        public override void Draw(Graphics g)
        {
            if (diodeImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 10;
                g.DrawImage(diodeImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("D", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}