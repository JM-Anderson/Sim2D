using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

using Sim2D.Simulations.Particles;
using Sim2D.Simulations.Particles.Physics.Forces;

namespace Sim2D.GUI.Particle.Tools.LinearForceTool
{
    class LinearForceLogic : ToolLogic
    {
        private Canvas simCanvas;
        private ParticleSim particleSim;

        private Line forcePreviewLine = new Line()
        {
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 3,
            Visibility = Visibility.Hidden,
            StrokeEndLineCap = PenLineCap.Triangle,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeDashCap = PenLineCap.Triangle,
            StrokeDashArray = new DoubleCollection() { 3 }
        };

        public LinearForceLogic(Canvas simCanvas, ParticleSim particleSim)
        {
            this.simCanvas = simCanvas;
            this.particleSim = particleSim;

            ToolType = ToolType.LinearForce;
            OptionBar = new LinearForceOptions();

            (OptionBar as LinearForceOptions).SpawnButton.Click += SpawnButton_Click;

            Panel.SetZIndex(forcePreviewLine, 900);

            simCanvas.Children.Add(forcePreviewLine);
        }

        private void SpawnButton_Click(object sender, RoutedEventArgs e)
        {
            particleSim.SpawnForce(new Linear(
                forceVector: (OptionBar as LinearForceOptions).ForceVector,
                massDependent: (OptionBar as LinearForceOptions).MassDependent
                ));
        }

        public override void LeftMouseDown(object sender, MouseEventArgs e)
        {
            forcePreviewLine.Visibility = Visibility.Visible;

            Point mousePos = e.GetPosition(simCanvas);
            forcePreviewLine.X1 = mousePos.X;
            forcePreviewLine.Y1 = mousePos.Y;

            forcePreviewLine.X2 = mousePos.X;
            forcePreviewLine.Y2 = mousePos.Y;
        }
        public override void LeftMouseDragging(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(simCanvas);
            forcePreviewLine.X2 = mousePos.X;
            forcePreviewLine.Y2 = mousePos.Y;
        }
        public override void LeftMouseUp(object sender, MouseEventArgs e)
        {
            Vector vector = new Vector(
                x: forcePreviewLine.X2 - forcePreviewLine.X1,
                y: forcePreviewLine.Y1 - forcePreviewLine.Y2
                );

            particleSim.SpawnForce(new Linear(
                forceVector: vector,
                massDependent: (OptionBar as LinearForceOptions).MassDependent
                ));

            (OptionBar as LinearForceOptions).ForceVector = vector;

            forcePreviewLine.Visibility = Visibility.Hidden;
        }
    }
}