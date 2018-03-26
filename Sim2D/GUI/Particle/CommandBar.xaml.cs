using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Sim2D.GUI.Particle.Commands;
using Sim2D.Simulations.Particles;

namespace Sim2D.GUI.Particle
{
    /// <summary>
    /// Interaction logic for CommandBar.xaml
    /// </summary>
    public partial class CommandBar : UserControl
    {
        ParticleSim particleSim;
        private Dictionary<CommandType, ICommand> registeredCommands = new Dictionary<CommandType, ICommand>();

        public CommandBar()
        {
            InitializeComponent();
        }

        public void Setup(ParticleSim particleSim, ParticleSimulator simGUI)
        {
            this.particleSim = particleSim;

            RegisterCommands(
                new Tuple<Button, ICommand>(DeleteAllButton, new DeleteAll(particleSim)),
                new Tuple<Button, ICommand>(PauseButton, new Pause(simGUI)),
                new Tuple<Button, ICommand>(PlayButton, new Play(simGUI))
            );
        }

        private void RegisterCommands(params Tuple<Button, ICommand>[] commands)
        {
            foreach (Tuple<Button, ICommand> cmdPair in commands)
            {
                registeredCommands.Add(cmdPair.Item2.GetCommandType(), cmdPair.Item2);
                cmdPair.Item1.Click += (s, e) => cmdPair.Item2.Run();
            }
        }
    }
}
