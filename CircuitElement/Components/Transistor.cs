using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Transistor : CircuitElement
    {
        private static Image transistorImage = ResourceHelper.GetImage("transistor_2547002");

        public Transistor(int x, int y) : base("Transistor", x, y) { }

        public override void Draw(Graphics g)
        {
            if (transistorImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 20;
                g.DrawImage(transistorImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("Q", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}