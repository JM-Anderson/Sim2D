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

namespace Sim2D.GUI.Particle.Tools.RulerTool
{
    /// <summary>
    /// Interaction logic for RulerOptions.xaml
    /// </summary>
    public partial class RulerOptions : UserControl
    {
        public double XLength
        {
            set { XLengthLabel.Text = value.ToString(); }
        }

        public double YLength
        {
            set { YLengthLabel.Text = value.ToString(); }
        }

        public RulerOptions()
        {
            InitializeComponent();
        }
    }
}
