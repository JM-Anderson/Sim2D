using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Diagnostics;

using Sim2D.GUI.Particle.Tools;
using Sim2D.GUI.Particle;
using Sim2D.Simulations.Particles;
using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.GUI
{
    /// <summary>
    /// Interaction logic for ParticleSimulator.xaml
    /// </summary>
    public partial class ParticleSimulator : UserControl
    {
        private ParticleSim particleSim;
        private SettingsWindow settingsWindow;

        private bool paused = false;
        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                PausedLabel.Visibility = (value) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public ParticleSimulator()
        {
            InitializeComponent();

            Application.Current.MainWindow.Closing += OnClosing;

            // Initialize simulation
            particleSim = new ParticleSim(SimCanvas);

            // Setup toolbar + commandbar
            UserToolBar.Setup(SimCanvas, particleSim, ToolOptionBar);
            CommandBar.Setup(particleSim, this);
            TopBarDockPanel.PreviewMouseWheel += (s, e) =>
            {
                if (e.Delta < 0)
                    OptionBarScrollViewer.LineRight();
                else
                    OptionBarScrollViewer.LineLeft();
                e.Handled = true;
            };

            // Setup settings window
            settingsWindow = new SettingsWindow(this, particleSim);
            settingsWindow.Activated += SettingsWindowActivated;
            SettingsButton.Click += (s, e) =>
            {
                if (settingsWindow.IsVisible)
                    settingsWindow.Hide();
                else
                    settingsWindow.Show();
            };

            // Capture render loop to use as main simulation loop
            CompositionTarget.Rendering += (s, e) => MainLoop();
        }

        // UI Events
        private void OnClosing(object sender, CancelEventArgs e)
        {
            settingsWindow.Show();
            settingsWindow.Close();
        }
        private void SettingsWindowActivated(object sender, EventArgs e)
        {
            settingsWindow.Owner = Application.Current.MainWindow;
            settingsWindow.Activated -= SettingsWindowActivated;
        }

        private void MainLoop()
        {
            SimTime simTime = settingsWindow.GetTime();

            particleSim.NextFrame((paused) ? 0 : simTime.TimeInterval);
            FPSDisplay.Content = (int)(simTime.FPS);
        }
    }
}
