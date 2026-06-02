using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Capacitor : CircuitElement
    {
        private static Image capacitorImage = ResourceHelper.GetImage("capacitor_17902399");

        public Capacitor(int x, int y) : base("Capacitor", x, y) { }

        public override void Draw(Graphics g)
        {
            if (capacitorImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 20;
                g.DrawImage(capacitorImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("C", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}