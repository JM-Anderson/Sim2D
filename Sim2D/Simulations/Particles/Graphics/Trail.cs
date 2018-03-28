using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Sim2D.Simulations.Particles.Graphics
{
    public class Trail
    {
        private Shape2D shape2D;
        private Canvas canvas;

        private Polyline polyline;

        public int TargetLength
        {
            get { return _targetLength; }
            set
            {
                _targetLength = value;
                correctLength = false;

                if (polyline is null) return;
                while (polyline.Points.Count >= value)
                    polyline.Points.RemoveAt(0);
            }
        }
        private int _targetLength;
        private bool correctLength = false;

        public bool IsOn { get; private set; } = false;

        public Trail(Shape2D shape2D, int length = 120)
        {
            this.shape2D = shape2D;
            TargetLength = length;

            polyline = new Polyline()
            {
                Stroke = new SolidColorBrush(shape2D.color),
                StrokeThickness = 1,
                Points = new PointCollection()
            };

            shape2D.OnDelete += (s, e) => Delete();
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Add(polyline);
            this.canvas = canvas;
        }

        public void AddPoint(double X, double Y)
        {
            polyline.Points.Add(new Point(X, canvas.ActualHeight - Y));
        }

        private void ShapeMoved(object sender, ShapeMoveEventArgs e)
        {
            AddPoint(e.NewX, e.NewY);
            if (correctLength)
                polyline.Points.RemoveAt(0);
            else if (polyline.Points.Count == TargetLength)
                correctLength = true;
        }

        public void TurnOn()
        {
            shape2D.OnMove += ShapeMoved;
            IsOn = true;
        }

        public void TurnOff()
        {
            shape2D.OnMove -= ShapeMoved;
            Reset();
            IsOn = false;
        }

        public void Reset()
        {
            if (polyline is null) return;
            
            polyline.Points = new PointCollection();

            correctLength = false;
        }

        public void Delete()
        {
            if (canvas is null) return;
            else
                canvas.Children.Remove(polyline);
        }
    }
}
