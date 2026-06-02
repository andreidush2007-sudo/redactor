using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Transformer : CircuitElement
    {
        private static Image transformerImage = ResourceHelper.GetImage("power_13764345");

        public Transformer(int x, int y) : base("Transformer", x, y) { }

        public override void Draw(Graphics g)
        {
            if (transformerImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(transformerImage, -Width / 2, -Height / 2, Width, Height);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("T", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}