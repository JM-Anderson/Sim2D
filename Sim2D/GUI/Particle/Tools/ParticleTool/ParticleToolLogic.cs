using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

using Sim2D.Simulations.Particles.Physics.Bodies;
using Sim2D.Simulations.Particles;
using System.Windows.Input;

namespace Sim2D.GUI.Particle.Tools
{
    class ParticleToolLogic : ToolLogic
    {
        // Particle preview and spawning
        private Line velPreview;
        private Ellipse particlePreview;
        private ParticleProperties particlePreviewProps;

        // Mouse state
        private bool mouseDragged = false;

        private ParticleSim particleSim;
        private Canvas simCanvas;

        public ParticleToolLogic(Canvas simCanvas, ParticleSim particleSim)
        {
            this.simCanvas = simCanvas;
            this.particleSim = particleSim;
            OptionBar = new ParticleToolOptions();
            (OptionBar as ParticleToolOptions).SpawnButtonClick += SpawnParticleButton_Clicked;

            ToolType = ToolType.Particle;

            velPreview = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 2,
                Visibility = Visibility.Hidden,

                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = 0
            };
            particlePreview = new Ellipse();
            simCanvas.Children.Add(velPreview);
            simCanvas.Children.Add(particlePreview);
        }

        public override void LeftMouseDown(object sender, MouseEventArgs e)
        {
            StartParticlePreview();
        }
        public override void LeftMouseDragging(object sender, MouseEventArgs e)
        {
            UpdateParticlePreview();
            mouseDragged = true;
        }
        public override void LeftMouseUp(object sender, MouseEventArgs e)
        {
            EndParticlePreview();
            mouseDragged = false;
        }

        private void StartParticlePreview()
        {
            Point mousePos = Mouse.GetPosition(simCanvas);

            // Get and update option bar properties
            particlePreviewProps = (OptionBar as ParticleToolOptions).ParticleOptions;
            particlePreviewProps.X = mousePos.X;
            particlePreviewProps.Y = simCanvas.ActualHeight - mousePos.Y;

            // Update particle preview
            particlePreview.Fill = new SolidColorBrush(particlePreviewProps.color);
            particlePreview.Height = particlePreviewProps.size;
            particlePreview.Width = particlePreviewProps.size;
            Canvas.SetLeft(particlePreview, mousePos.X - particlePreviewProps.radius);
            Canvas.SetBottom(particlePreview, simCanvas.ActualHeight - mousePos.Y - particlePreviewProps.radius);

            // Update velocity line preview
            velPreview.Stroke = new SolidColorBrush(particlePreviewProps.color);
            velPreview.X1 = mousePos.X;
            velPreview.Y1 = mousePos.Y;
        }
        private void UpdateParticlePreview()
        {
            Point mousePos = Mouse.GetPosition(simCanvas);
            velPreview.X2 = mousePos.X;
            velPreview.Y2 = mousePos.Y;

            velPreview.Visibility = Visibility.Visible;
            particlePreview.Visibility = Visibility.Visible;
        }
        private void EndParticlePreview()
        {
            if (mouseDragged)
            {
                particlePreviewProps.velocity = new Vector(velPreview.X2 - velPreview.X1, velPreview.Y1 - velPreview.Y2);
            }

            SpawnParticle(properties: particlePreviewProps);

            velPreview.Visibility = Visibility.Hidden;
            particlePreview.Visibility = Visibility.Hidden;
        }

        private void SpawnParticleButton_Clicked(object sender, RoutedEventArgs e)
        {
            SpawnParticle();
        }
        private void SpawnParticle(double? X = null, double? Y = null, ParticleProperties properties = null)
        {
            if (properties is null) properties = (OptionBar as ParticleToolOptions).ParticleOptions;
            if (X != null) properties.X = (double)X;
            if (Y != null) properties.Y = (double)Y;

            particleSim.CreateBody(properties);
        }
    }
}
