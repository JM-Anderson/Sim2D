using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    public class Particle : Rigidbody
    {
        // Physics Properties
        public readonly double mass;
        public double timeLeft = 0;
        public Vector Velocity = new Vector(0, 0);
        public Vector Acceleration = new Vector(0, 0);

        public Particle(Shape gShape, ParticleProperties properties) : base(gShape, properties)
        {
            mass = properties.mass;
            Velocity = properties.velocity;
        }

        /// <summary>
        /// Moves the particle forward in time by the specified amount using the particle's velocity
        /// </summary>
        /// <param name="dt">Time to simulate</param>
        public void SimMove(double dt)
        {
            Move(Velocity.X * dt, Velocity.Y * dt);
        }

        public void ApplyAcceleration(double dt)
        {
            Velocity += dt * Acceleration;
        }
    }
}
