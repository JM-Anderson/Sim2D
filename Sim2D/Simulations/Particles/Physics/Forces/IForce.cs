using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Physics.Forces
{
    public enum Forces
    {
        ElectroMagnetic, Linear
    }

    public interface IForce
    {
        void ApplyOn(params Particle[] particle);

        Forces GetForceType();
    }
}
