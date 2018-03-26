using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim2D.GUI.Particle.Commands
{
    public interface ICommand
    {
        CommandType GetCommandType();
        void Run();
    }
}
