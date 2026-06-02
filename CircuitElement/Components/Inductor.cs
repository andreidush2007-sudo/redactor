using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Inductor : CircuitElement
    {
        private static Image inductorImage = ResourceHelper.GetImage("inductor_5999087");

        public Inductor(int x, int y) : base("Inductor", x, y) { }

        public override void Draw(Graphics g)
        {
            if (inductorImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(inductorImage, -Width / 2, -Height / 2, Width, Height);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("L", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}