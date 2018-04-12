using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Sim2D.GUI.Particle.Tools.RulerTool
{
    class RulerLogic : ToolLogic
    {
        private Line rulerLine = new Line()
        {
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2,
            Visibility = Visibility.Hidden,
            StrokeEndLineCap = PenLineCap.Triangle
        };
        private Label startPosLabel = new Label()
        {
            Visibility = Visibility.Hidden,
            Content = "start"
        };
        private Label endPosLabel = new Label()
        {
            Visibility = Visibility.Hidden,
            Content = "end"
        };

        private Canvas simCanvas;

        public RulerLogic(Canvas simCanvas)
        {
            this.simCanvas = simCanvas;

            ToolType = ToolType.Ruler;
            OptionBar = new RulerOptions();

            Panel.SetZIndex(rulerLine, 900);
            Panel.SetZIndex(endPosLabel, 900);
            Panel.SetZIndex(startPosLabel, 900);

            simCanvas.Children.Add(rulerLine);
            simCanvas.Children.Add(endPosLabel);
            simCanvas.Children.Add(startPosLabel);
        }

        public override void LeftMouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(simCanvas);
            rulerLine.X1 = mousePos.X;
            rulerLine.Y1 = mousePos.Y;

            rulerLine.X2 = mousePos.X;
            rulerLine.Y2 = mousePos.Y;

            SetCanvasPos(startPosLabel, mousePos.X, mousePos.Y);
            startPosLabel.Content = CoordFormat(mousePos.X, mousePos.Y);

            SetCanvasPos(endPosLabel, mousePos.X, mousePos.Y);
            endPosLabel.Content = CoordFormat(mousePos.X, mousePos.Y);

            rulerLine.Visibility = Visibility.Visible;
            startPosLabel.Visibility = Visibility.Visible;
            endPosLabel.Visibility = Visibility.Visible;
        }

        public override void LeftMouseDragging(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(simCanvas);
            rulerLine.X2 = mousePos.X;
            rulerLine.Y2 = mousePos.Y;
            
            SetCanvasPos(endPosLabel, mousePos.X, mousePos.Y);
            endPosLabel.Content = CoordFormat(mousePos.X, mousePos.Y);

            (OptionBar as RulerOptions).XLength = rulerLine.X2 - rulerLine.X1;
            (OptionBar as RulerOptions).YLength = rulerLine.Y1 - rulerLine.Y2;
        }

        public override void LeftMouseUp(object sender, MouseEventArgs e)
        {
            if (rulerLine.X2 - rulerLine.X1 == 0 && rulerLine.Y1 - rulerLine.Y2 == 0)
            {
                rulerLine.Visibility = Visibility.Hidden;
                startPosLabel.Visibility = Visibility.Hidden;
                endPosLabel.Visibility = Visibility.Hidden;
            }
        }

        private void SetCanvasPos(UIElement element, double x, double y)
        {
            Canvas.SetTop(element, y - 10);
            Canvas.SetLeft(element, x);
        }

        private string CoordFormat(double x, double y)
        {
            return String.Format("({0:n0}, {1:n0})", x, simCanvas.ActualHeight - y);
        }
    }
}
