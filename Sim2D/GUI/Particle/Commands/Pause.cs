using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sim2D.GUI;

namespace Sim2D.GUI.Particle.Commands
{
    class Pause : ICommand
    {
        ParticleSimulator simGUI;

        public Pause(ParticleSimulator simGUI)
        {
            this.simGUI = simGUI;
        }

        public CommandType GetCommandType() { return CommandType.Pause; }

        public void Run()
        {
            simGUI.Paused = true;
        }
    }
}
