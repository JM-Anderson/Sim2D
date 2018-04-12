using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sim2D.GUI.Particle.Tools
{
    abstract class ToolLogic
    {
        public ToolType ToolType { get; protected set; } = ToolType.None;

        public UserControl OptionBar;

        public virtual void Selected()
        {

        }

        public virtual void Unselected()
        {

        }

        public virtual void LeftMouseDown(object sender, MouseEventArgs e)
        {

        }

        public virtual void LeftMouseDragging(object sender, MouseEventArgs e)
        {

        }

        public virtual void LeftMouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
