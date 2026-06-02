using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Lamp : CircuitElement
    {
        private static Image lampImage = ResourceHelper.GetImage("cross_6935263");

        public Lamp(int x, int y) : base("Lamp", x, y) { }

        public override void Draw(Graphics g)
        {
            if (lampImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(lampImage, -Width / 2, -Height / 2, Width, Height);
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