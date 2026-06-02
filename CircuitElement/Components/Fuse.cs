using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Fuse : CircuitElement
    {
        private static Image fuseImage = ResourceHelper.GetImage("fuse_8594888");

        public Fuse(int x, int y) : base("Fuse", x, y) { }

        public override void Draw(Graphics g)
        {
            if (fuseImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 10;
                g.DrawImage(fuseImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("F", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}