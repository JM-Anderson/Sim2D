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

using System.Diagnostics;

using Sim2D.Simulations.Sand;

namespace Sim2D.GUI
{
    public partial class SandSimulator : UserControl
    {
        public bool paused = false;

        // Intialize sim
        private SandSim sandSim;
        public SandSimulator()
        {
            InitializeComponent();
        }
        private void simCanvas_Loaded(object sender, RoutedEventArgs eArgs)
        {
            sandSim = new SandSim(simCanvas, 3);
            CompositionTarget.Rendering += (s, e) => MainLoop();
        }

        // Main loop
        private void MainLoop()
        {
            if (!paused)
            {
                if (MouseDragging)
                    SpawnGrain();
                sandSim.Update();
            }
        }

        // Handle grain spawning
        private bool MouseDragging = false;
        private void simCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseDragging = true;
        }
        private void simCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseDragging = false;
        }
        private void SpawnGrain()
        {
            sandSim.CanvasClick(Mouse.GetPosition(simCanvas));
        }

        // Handle window resize
        private void simCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sandSim.WindowResize();
        }
    }
}
