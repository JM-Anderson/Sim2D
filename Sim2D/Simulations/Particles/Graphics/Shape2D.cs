using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Sim2D.Simulations.Particles.Graphics
{
    public class Shape2D
    {
        private Shape gShape;

        public readonly double radius;

        public readonly ShapeType ShapeType;

        public double X { get; private set; }
        public double Y { get; private set; }

        public Shape2D(Shape gShape, ShapeProperties properties)
        {
            this.gShape = gShape;
            radius = properties.size / 2;
            ShapeType = properties.shapeType;

            X = properties.X;
            Y = properties.Y;
        }


        // Position functions
        public void Move(double dx, double dy)
        {
            X += dx;
            Y += dy;

            UpdatePosition();
        }
        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Canvas.SetLeft(gShape, X - radius);
            Canvas.SetBottom(gShape, Y - radius);
        }

        // Delete shape event
        public event EventHandler OnDelete;
        public void Delete()
        {
            Canvas simCanvas = (Canvas)VisualTreeHelper.GetParent(gShape);
            simCanvas.Children.Remove(gShape);

            OnDelete(this, new EventArgs());
        }
    }
}
