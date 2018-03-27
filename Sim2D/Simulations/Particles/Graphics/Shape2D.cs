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
    public class ShapeMoveEventArgs : EventArgs
    {
        public double NewX;
        public double NewY;
        public ShapeMoveEventArgs(double X, double Y)
        {
            NewX = X;
            NewY = Y;
        }
    }

    public class Shape2D
    {
        private Shape gShape;

        private Trail trail;
        public bool HasTrail
        {
            get
            {
                return trail.IsOn;
            }
            set
            {
                if (value == trail.IsOn)
                    return;
                else if (value)
                {
                    trail.TurnOn();
                }
                else
                {
                    trail.TurnOff();
                }
            }
        }

        public readonly double radius;

        public readonly ShapeType ShapeType;
        public readonly Color color;

        public double X { get; private set; }
        public double Y { get; private set; }

        public Shape2D(Shape gShape, ShapeProperties properties)
        {
            this.gShape = gShape;
            color = properties.color;
            radius = properties.size / 2;
            ShapeType = properties.shapeType;

            X = properties.X;
            Y = properties.Y;

            trail = new Trail(this);
            trail.Draw((Canvas)gShape.Parent);
        }


        // Position functions
        public event EventHandler<ShapeMoveEventArgs> OnMove;
        public void Move(double dx, double dy, bool trackMove = true)
        {
            X += dx;
            Y += dy;

            UpdatePosition();

            if (trackMove && OnMove != null)
                OnMove(this, new ShapeMoveEventArgs(X, Y));
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
