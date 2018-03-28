using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Physics.Forces
{
    public class Linear : IForce
    {
        public readonly Vector Magnitude;
        public readonly bool MassDependent;

        public Linear(double X, double Y, bool massDependent)
        {
            Magnitude = new Vector(X, Y);
            MassDependent = massDependent;
        }
        public Linear(Vector forceVector, bool massDependent)
        {
            Magnitude = forceVector;
            MassDependent = massDependent;
        }

        public Forces GetForceType() { return Forces.Linear; }

        public void ApplyOn(params Particle[] particles)
        {
            foreach (Particle p in particles)
            {
                if (MassDependent)
                {
                    p.Acceleration += Magnitude / p.mass;
                }
                else
                {
                    p.Acceleration += Magnitude;
                }
            }
        }
    }
}
