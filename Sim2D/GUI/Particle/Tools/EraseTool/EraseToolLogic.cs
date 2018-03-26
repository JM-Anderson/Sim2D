using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim2D.GUI.Particle.Tools
{
    class EraseToolLogic : ToolLogic
    {
        public EraseToolLogic()
        {
            ToolType = ToolType.Erase;
            OptionBar = new EraseTool.EraseToolOptions();
        }
    }
}
