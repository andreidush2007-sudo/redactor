using System;
using System.Drawing;

namespace CircuitEditor
{
    public class Buzzer : CircuitElement
    {
        private static Image buzzerImage = ResourceHelper.GetImage("bell_3406843");

        public Buzzer(int x, int y) : base("Buzzer", x, y) { }

        public override void Draw(Graphics g)
        {
            if (buzzerImage != null)
            {
                var state = g.Save();
                g.TranslateTransform(X + Width / 2, Y + Height / 2);
                g.RotateTransform(Rotation);
                g.DrawImage(buzzerImage, -Width / 2, -Height / 2, Width, Height);
                g.Restore(state);
            }
            else
            {
                g.DrawRectangle(Pens.Black, X, Y, Width, Height);
                g.DrawString("B", new Font("Arial", 12), Brushes.Black, X + 35, Y + 25);
            }
        }
    }
}