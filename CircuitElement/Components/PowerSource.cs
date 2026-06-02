using System;
using System.Drawing;

namespace CircuitEditor
{
    public class PowerSource : CircuitElement
    {
        private static Image powerImage = ResourceHelper.GetImage("forward-biased-diode_10480907");

        public PowerSource(int x, int y) : base("PowerSource", x, y) { }

        public override void Draw(Graphics g)
        {
            if (powerImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(powerImage, -Width / 2, -Height / 2, Width, Height);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("V", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}