using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    public class ParticleProperties : RigidbodyProperties
    {
        public readonly double mass;
        public Vector velocity;

        public ParticleProperties(
            // Shape properties
            double X, double Y, Color color, double radius,

            // Physics properties
            double mass, Vector velocity, int charge)

            : base(X, Y, Graphics.ShapeType.Ellipse, color, radius, charge)
        {
            this.mass = mass;
            this.velocity = velocity;
            BodyType = BodyType.Particle;
        }
    }
}
