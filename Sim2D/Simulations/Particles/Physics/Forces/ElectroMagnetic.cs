using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Physics.Forces
{
    class ElectroMagnetic : IForce
    {
        public readonly Rigidbody source;

        public static readonly double CoulumbConst = 8.988 * Math.Pow(10, 9);
        public static readonly double ElementaryCharge = 1.602 * Math.Pow(10, -19);
        public static readonly double ElementaryChargeSqr = Math.Pow(ElementaryCharge, 2);

        public ElectroMagnetic(Rigidbody rigidbody)
        {
            source = rigidbody;
        }

        public void ApplyOn(params Particle[] particles)
        {
            foreach (Particle particle in particles)
            {
                if (particle == source) continue;

                // Engine length units are in decimeters - to convert to meters -> divide by 10

                double distX = source.X - particle.X;
                double distY = source.Y - particle.Y;

                double distMagSqr = Math.Pow(distX, 2.0) + Math.Pow(distY, 2.0);

                // The particles are directly on top of one another - skip calculations
                if (distMagSqr == 0) continue;

                double distMag = Math.Sqrt(distMagSqr);

                double force = Math.Pow(10, 32) * -1 * CoulumbConst * source.Charge * particle.Charge * ElementaryChargeSqr / distMagSqr;

                // Engine mass units are in amu - to convert to kg -> multiply by 1.661e-27
                // Multiply by 10 to convert back to decimeters from meters
                double magConst = force / particle.mass;

                double accX = distX * magConst / distMag;
                double accY = distY * magConst / distMag;

                particle.Acceleration.X += accX;
                particle.Acceleration.Y += accY;
            }
        }
    }
}
