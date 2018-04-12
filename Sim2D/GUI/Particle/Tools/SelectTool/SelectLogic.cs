using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;

using Sim2D.Simulations.Particles;
using Bodies = Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.GUI.Particle.Tools.SelectTool
{
    class SelectLogic : ToolLogic
    {
        private Canvas simCanvas;

        // UI elements
        private Border selectPreview = new Border()
        {
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Color.FromRgb(135, 206, 235)),
            Background = new SolidColorBrush(Color.FromArgb(128, 135, 206, 235)),
            Visibility = Visibility.Collapsed
        };
        private SelectionWindow selectionWindow;
        private Vector startPos;

        // Engine
        private ParticleSim particleSim;
        private Bodies.Rigidbody[] selectedBodies;

        public SelectLogic(Canvas simCanvas, ParticleSim particleSim)
        {
            this.simCanvas = simCanvas;
            this.particleSim = particleSim;

            OptionBar = new SelectOptions();
            ToolType = ToolType.Select;

            // Set up marquee select
            simCanvas.Children.Add(selectPreview);
            Panel.SetZIndex(selectPreview, 901);

            // Setup selection window
            selectionWindow = new SelectionWindow();
            selectionWindow.Activated += SelectionWindowActivated;
            Application.Current.MainWindow.Closing += (s, e) =>
            {
                selectionWindow.Show();
                selectionWindow.Close();
            };
            (OptionBar as SelectOptions).ViewSelectionButton.Click += (s, e) =>
            {
                if (selectionWindow.IsVisible)
                    selectionWindow.Hide();
                else
                    selectionWindow.Show();
            };

            particleSim.BodySpawned += (s, e) => UpdateSelectionViewer();

            selectionWindow.SelectedItemView.SelectionChanged += (s, e) =>
            {
                while (selectionWindow.PropertiesPanel.Children.Count != 0)
                    selectionWindow.PropertiesPanel.Children.RemoveAt(0);

                int selectedIndex = (s as ListView).SelectedIndex;

                if (selectedIndex == -1) return;

                Bodies.Rigidbody body = selectedBodies[selectedIndex];
                AddDataEntry(new Dictionary<string, object>()
                {
                    { "Radius", body.radius },
                    { "Charge", body.Charge }
                });
            };
        }

        private void AddDataEntry(Dictionary<string, object> dataTable)
        {
            foreach (KeyValuePair<string, object> dataPair in dataTable)
            {
                Label line = new Label()
                {
                    Content = dataPair.Key + ": " + dataPair.Value.ToString()
                };

                selectionWindow.PropertiesPanel.Children.Add(line);
            }
        }

        private void UpdateSelectionViewer()
        {
            selectedBodies = particleSim.GetAllBodies();

            while (selectionWindow.SelectedItemView.Items.Count != 0)
                selectionWindow.SelectedItemView.Items.RemoveAt(0);

            PopulateSelectionViewer();
        }

        private void PopulateSelectionViewer()
        {
            foreach (Bodies.Rigidbody body in selectedBodies)
            {
                selectionWindow.SelectedItemView.Items.Add(CreateDataEntry(body));
            }
        }

        private UIElement CreateDataEntry(Bodies.Rigidbody body)
        {
            StackPanel entry = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            entry.Children.Add(new Ellipse()
            {
                Height = 10,
                Width = 10,
                Fill = new SolidColorBrush(body.color)
            });
            entry.Children.Add(new Label()
            {
                Content = "Particle " + body.UID
            });

            return entry;
        }

        private void SelectionWindowActivated(object sender, EventArgs e)
        {
            selectionWindow.Owner = Application.Current.MainWindow;
            selectionWindow.Activated -= SelectionWindowActivated;
        }

        public override void LeftMouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(simCanvas);
            startPos = new Vector(mousePos.X, mousePos.Y);

            Canvas.SetLeft(selectPreview, mousePos.X);
            Canvas.SetTop(selectPreview, mousePos.Y);

            selectPreview.Visibility = Visibility.Visible;
        }
        public override void LeftMouseDragging(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(simCanvas);

            double dx = mousePos.X - startPos.X;
            double dy = mousePos.Y - startPos.Y;

            if (dy < 0)
                Canvas.SetTop(selectPreview, mousePos.Y);
            if (dx < 0)
                Canvas.SetLeft(selectPreview, mousePos.X);

            selectPreview.Height = Math.Abs(dy);
            selectPreview.Width = Math.Abs(dx);
        }
        public override void LeftMouseUp(object sender, MouseEventArgs e)
        {
            selectPreview.Visibility = Visibility.Collapsed;
        }
    }
}
