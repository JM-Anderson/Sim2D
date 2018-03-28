using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Physics.Forces
{
    public enum ElectroMagneticState
    {
        Normal, Repel, Attract
    }

    class ElectroMagnetic : IForce
    {
        public readonly Rigidbody source;

        public ElectroMagneticState MagneticState = ElectroMagneticState.Normal;

        public static readonly double CoulumbConst = 8.988 * Math.Pow(10, 9);
        public static readonly double ElementaryCharge = 1.602 * Math.Pow(10, -19);
        public static readonly double ElementaryChargeSqr = Math.Pow(ElementaryCharge, 2);

        public ElectroMagnetic(Rigidbody rigidbody)
        {
            source = rigidbody;
        }

        public Forces GetForceType()
        {
            return Forces.ElectroMagnetic;
        }

        public void ApplyOn(params Particle[] particles)
        {
            foreach (Particle particle in particles)
            {
                if (particle == source) continue;

                double distX = source.X - particle.X;
                double distY = source.Y - particle.Y;

                // The particles are directly on top of one another - skip calculations
                if (distX + distY == 0) continue;

                double centerDistSqr = Math.Pow(distX, 2.0) + Math.Pow(distY, 2.0);
                double centerDist = Math.Sqrt(centerDistSqr);

                // Calculate magnetic force using Coulumb's Law
                double force = Math.Pow(10, 32) * -1 * CoulumbConst * source.Charge * particle.Charge * ElementaryChargeSqr / centerDistSqr;

                // Change direction of force if not using charge's to determine direction
                if (MagneticState != ElectroMagneticState.Normal)
                {
                    if (MagneticState == ElectroMagneticState.Repel)
                        force = -Math.Abs(force);
                    else
                        force = Math.Abs(force);
                }

                // Converts magnitude of force from scalar to properly directioned vector
                double magConst = (force / particle.mass) / centerDist;
                double accX = distX * magConst;
                double accY = distY * magConst;

                // Apply acceleration
                particle.Acceleration.X += accX;
                particle.Acceleration.Y += accY;
            }
        }
    }
}
