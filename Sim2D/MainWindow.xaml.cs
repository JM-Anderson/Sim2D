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

namespace Sim2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KeyDown += KeyPressed;
        }

        private bool windowMaximized = false;
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (windowMaximized)
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                    windowMaximized = false;
                }
                else
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    windowMaximized = true;
                }
            }
        }

        bool pSimWasPaused = false;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParticleSimTab.IsSelected)
            {
                ParticleSim.paused = pSimWasPaused;
                SandSim.paused = true;
            }
            else if (SandSimTab.IsSelected)
            {
                pSimWasPaused = ParticleSim.paused;

                ParticleSim.paused = true;
                SandSim.paused = false;
            }
        }
    }
}
