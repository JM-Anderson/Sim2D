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

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.GUI.Particle.Tools
{
    /// <summary>
    /// Interaction logic for ParticleToolOptions.xaml
    /// </summary>
    public partial class ParticleToolOptions : UserControl
    {
        private static Random randomGen = new Random();

        public ParticleToolOptions()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler SpawnButtonClick;
        private void SpawnButton_Click(object sender, RoutedEventArgs e)
        {
            SpawnButtonClick(sender, e);
        }

        public ParticleProperties ParticleOptions
        {
            get
            {
                ParticleProperties properties = new ParticleProperties
                (
                    X: XUpDown.Value ?? 100,
                    Y: YUpDown.Value ?? 100,

                    velocity: new Vector(VelXUpDown.Value ?? 0, VelYUpDown.Value ?? 0),

                    mass: MassUpDown.Value ?? 10,
                    charge: ChargeUpDown.Value ?? 0,

                    radius: RadUpDown.Value ?? 5,
                    color: ParticleColorCanvas.SelectedColor ?? Colors.Red
                );

                if (RandomColorCheckBox.IsChecked ?? false)
                {
                    ParticleColorCanvas.SelectedColor = new Color()
                    {
                        R = (byte)randomGen.Next(0, 256),
                        G = (byte)randomGen.Next(0, 256),
                        B = (byte)randomGen.Next(0, 256),
                        A = 255
                    };
                }

                return properties;
            }
        }

        private void ColorCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            UpdateSelectedColor();
        }
        private void UpdateSelectedColor()
        {
            ParticleColorDisplay.Background = new SolidColorBrush(ParticleColorCanvas.SelectedColor ?? Colors.Red);
        }
    }
}
