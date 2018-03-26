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
using System.Windows.Shapes;

using Sim2D.GUI.Particle.Tools;
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

        public double? timeInterval = null;
        public bool paused = false;

        public ParticleSimulator()
        {
            InitializeComponent();

            // Initialize simulation
            particleSim = new ParticleSim(SimCanvas);

            // Setup toolbar + commandbar
            UserToolBar.Setup(SimCanvas, particleSim, ToolOptionBar);
            CommandBar.Setup(particleSim, this);

            // Capture render loop to use as main simulation loop
            CompositionTarget.Rendering += (s, e) => MainLoop();
        }

        private void MainLoop()
        {
            double? dt = timeInterval;

            if (paused) dt = 0;

            particleSim.NextFrame(dt);
            FPSDisplay.Content = (int)(1 / particleSim.lastDeltaT);
        }
    }
}
