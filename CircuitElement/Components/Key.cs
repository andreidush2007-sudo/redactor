using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Key : CircuitElement
    {
        private static Image keyImage = ResourceHelper.GetImage("switch-open_10480830");

        public Key(int x, int y) : base("Key", x, y) { }

        public override void Draw(Graphics g)
        {
            if (keyImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(keyImage, -Width / 2, -Height / 2, Width, Height);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("K", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}