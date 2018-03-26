using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sim2D.Simulations.Particles.Graphics
{
    public enum ShapeType
    {
        Ellipse, Rectangle
    }

    public class ShapeProperties
    {
        public ShapeProperties(double X, double Y, ShapeType shapeType, Color color, double size)
        {
            this.shapeType = shapeType;
            this.color = color;
            this.size = size;
            this.X = X;
            this.Y = Y;
        }

        public readonly ShapeType shapeType;

        public readonly Color color;

        public readonly double size;

        public double X;

        public double Y;
    }
}
