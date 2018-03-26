using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace Sim2D.Simulations.Sand.Grains
{
    class BaseGrain
    {
        private Rectangle graphic;

        // Sim info
        private double grainScale;
        public bool IsStatic = false;

        // Physics info
        public double maxSlope { get; private set; } // Essentially a measure of friction
        public double density { get; private set; }

        // Actual position on canvas
        private double _xPos;
        private double _yPos;

        // Position on the simulation's grid
        private int _simXPos;
        private int _simYPos;

        /// <summary>
        /// Sand grain to be used by simulator - Contains utility functions to handle grain
        /// </summary>
        /// <param name="x">Grain X position in simulator grid coordinates</param>
        /// <param name="y">Grain Y position in simulator grid coordinates</param>
        /// <param name="size">Size of the grain's graphic</param>
        /// <param name="grainScale">Scale of the simulation - should be equal for all grains</param>
        public BaseGrain(int x, int y, double grainScale, double maxSlope = 1)
        {
            this.grainScale = grainScale;
            this.maxSlope = maxSlope;

            _simXPos = x;
            _simYPos = y;

            _xPos = x * grainScale;
            _yPos = y * grainScale;

            graphic = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = grainScale,
                Height = grainScale
            };

            Canvas.SetLeft(graphic, _xPos);
            Canvas.SetBottom(graphic, _yPos);
        }

        // Getters
        public Vector SimPos() { return new Vector(_simXPos, _simYPos); }
        public int simX { get { return _simXPos; } }
        public int simY { get { return _simYPos; } }

        public double X { get { return _xPos; } }
        public double Y { get { return _yPos; } }

        // Adds graphic to canvas
        public void Draw(Canvas simCanvas)
        {
            simCanvas.Children.Add(graphic);
        }

        // Position manipulation
        public void SimMove(int dx, int dy)
        {
            Move(dx * grainScale, dy * grainScale);

            _simXPos += dx;
            _simYPos += dy;
        }
        private void Move(double dx, double dy)
        {
            SetPosition(dx + _xPos, dy + _yPos);
        }
        private void SetPosition(double x, double y)
        {
            Canvas.SetLeft(graphic, x);
            Canvas.SetBottom(graphic, y);

            _xPos = x;
            _yPos = y;
        }
    }
}
