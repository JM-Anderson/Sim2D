using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Optimizations.Organization
{
    interface IOrginization
    {
        List<List<Rigidbody>> Organize();
    }
}
