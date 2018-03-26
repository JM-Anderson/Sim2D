using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Sim2D.Simulations.Particles.Graphics
{
    class GraphicsManager
    {
        private Canvas simCanvas;

        public Vector CanvasSize { get { return new Vector(simCanvas.ActualWidth, simCanvas.ActualHeight); } }

        private static Dictionary<ShapeType, Type> shapeDict = new Dictionary<ShapeType, Type>()
        {
            {ShapeType.Ellipse, typeof(Ellipse)},
            {ShapeType.Rectangle, typeof(Rectangle)}
        };

        public GraphicsManager(Canvas simCanvas)
        {
            this.simCanvas = simCanvas;
        }

        public Shape CreateShape(ShapeProperties shapeProperties)
        {
            // Instantiate XAML shape based on type
            Shape newShape = (Shape)Activator.CreateInstance(shapeDict[shapeProperties.shapeType]);

            // Set shape to proper size
            newShape.Height = shapeProperties.size;
            newShape.Width = shapeProperties.size;

            // Set shape color
            newShape.Fill = new SolidColorBrush(shapeProperties.color);

            // Position shape
            Canvas.SetLeft(newShape, shapeProperties.X + shapeProperties.size);
            Canvas.SetBottom(newShape, shapeProperties.Y + shapeProperties.size);

            // Add shape to the window
            simCanvas.Children.Add(newShape);

            return newShape;
        }
    }
}
