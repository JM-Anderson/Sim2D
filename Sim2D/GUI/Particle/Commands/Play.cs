using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim2D.GUI.Particle.Commands
{
    class Play : ICommand
    {
        ParticleSimulator simGUI;

        public Play(ParticleSimulator simGUI)
        {
            this.simGUI = simGUI;
        }

        public CommandType GetCommandType() { return CommandType.Play; }

        public void Run()
        {
            simGUI.paused = false;
        }
    }
}
