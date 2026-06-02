using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Resistor : CircuitElement
    {
        private static Image resistorImage = ResourceHelper.GetImage("resistor_8594946");

        public Resistor(int x, int y) : base("Resistor", x, y) { }

        public override void Draw(Graphics g)
        {
            if (resistorImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                int imgSize = Math.Min(Width, Height) - 20;
                g.DrawImage(resistorImage, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("R", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}