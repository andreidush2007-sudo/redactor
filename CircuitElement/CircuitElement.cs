using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CircuitEditor
{
    [Serializable]
    public abstract class CircuitElement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; } = 80;
        public int Height { get; set; } = 60;
        public int Rotation { get; set; } = 0;

        public const int GridSize = 20;

        protected CircuitElement(string type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public abstract void Draw(Graphics g);

        public virtual void DrawSelection(Graphics g)
        {
            using (Pen pen = new Pen(Color.Aqua, 2))
            {
                pen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(pen, X - 2, Y - 2, Width + 4, Height + 4);
            }
        }

        public virtual bool IsHit(int mouseX, int mouseY)
        {
            return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
        }

        public virtual Point GetConnectionPoint()
        {
            return new Point(X + Width / 2, Y + Height / 2);
        }

        public void snapToGrid()
        {
            X = (int)Math.Round((double)X / GridSize) * GridSize;
            Y = (int)Math.Round((double)Y / GridSize) * GridSize;
        }

        public void Rotate()
        {
            Rotation = (Rotation + 90) % 360;
            if (Rotation == 90 || Rotation == 270)
            {
                int temp = Width;
                Width = Height;
                Height = temp;
            }
        }
    }

    public class Connection
    {
        public string StartElementId { get; set; }
        public string EndElementId { get; set; }

        [JsonIgnore]
        public CircuitElement StartElement;

        [JsonIgnore]
        public CircuitElement EndElement;

        public void Draw(Graphics g)
        {
            if (StartElement == null || EndElement == null) return;

            Point start = StartElement.GetConnectionPoint();
            Point end = EndElement.GetConnectionPoint();

            List<Point> pathPoints = GetOrthogonalPath(start, end);

            using (Pen pen = new Pen(Color.Black, 2.0f))
            {
                for (int i = 0; i < pathPoints.Count - 1; i++)
                {
                    g.DrawLine(pen, pathPoints[i], pathPoints[i + 1]);
                }
            }

            DrawJunctionPoint(g, start);
            DrawJunctionPoint(g, end);
        }

        private List<Point> GetOrthogonalPath(Point start, Point end)
        {
            List<Point> points = new List<Point>();
            points.Add(start);
            points.Add(new Point(end.X, start.Y));
            points.Add(end);
            return points;
        }

        private void DrawJunctionPoint(Graphics g, Point point)
        {
            int dotSize = 4;
            g.FillEllipse(Brushes.Black, point.X - dotSize / 2, point.Y - dotSize / 2, dotSize, dotSize);
        }

        public void DrawSelection(Graphics g)
        {
            if (StartElement == null || EndElement == null) return;

            Point start = StartElement.GetConnectionPoint();
            Point end = EndElement.GetConnectionPoint();

            List<Point> pathPoints = GetOrthogonalPath(start, end);

            using (Pen pen = new Pen(Color.Blue, 2))
            {
                pen.DashStyle = DashStyle.Dash;
                for (int i = 0; i < pathPoints.Count - 1; i++)
                {
                    g.DrawLine(pen, pathPoints[i], pathPoints[i + 1]);
                }
            }
        }

        public bool IsHit(int mouseX, int mouseY)
        {
            if (StartElement == null || EndElement == null) return false;

            Point start = StartElement.GetConnectionPoint();
            Point end = EndElement.GetConnectionPoint();

            List<Point> pathPoints = GetOrthogonalPath(start, end);

            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                float distance = DistanceToLine(pathPoints[i], pathPoints[i + 1], new Point(mouseX, mouseY));
                if (distance < 5) return true;
            }

            return false;
        }

        private float DistanceToLine(Point a, Point b, Point p)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);

            if (len == 0) return Distance(a, p);

            float t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (len * len);
            t = Math.Max(0, Math.Min(1, t));

            float projX = a.X + t * dx;
            float projY = a.Y + t * dy;

            return Distance(new Point((int)projX, (int)projY), p);
        }

        private float Distance(Point a, Point b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}