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

using Sim2D.Simulations.Particles.Physics.Forces;

namespace Sim2D.GUI.Particle.Tools.LinearForceTool
{
    /// <summary>
    /// Interaction logic for LinearForceOptions.xaml
    /// </summary>
    public partial class LinearForceOptions : UserControl
    {
        public bool MassDependent
        {
            get { return MassDependentCheckBox.IsChecked ?? false; }
            set { MassDependentCheckBox.IsChecked = value; }
        }

        public Vector ForceVector
        {
            get { return new Vector(XUpDown.Value ?? 0, YUpDown.Value ?? 0); }
            set
            {
                XUpDown.Value = value.X;
                YUpDown.Value = value.Y;
            }
        }

        public LinearForceOptions()
        {
            InitializeComponent();

            MassDependentCheckBox.Checked += IsMassDependentChanged;
            MassDependentCheckBox.Unchecked += IsMassDependentChanged;
        }

        private void IsMassDependentChanged(object sender, RoutedEventArgs e)
        {
            LinearTypeTextBlock.Text = (MassDependent) ? "force" : "acceleration";
        }
    }
}
