using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sim2D.Simulations.Particles;

namespace Sim2D.GUI.Particle.Commands
{
    class DeleteAll : ICommand
    {
        ParticleSim particleSim;

        public DeleteAll(ParticleSim particleSim)
        {
            this.particleSim = particleSim;
        }

        public CommandType GetCommandType() { return CommandType.DeleteAll; }

        public void Run()
        {
            particleSim.DeleteAll();
        }
    }
}
