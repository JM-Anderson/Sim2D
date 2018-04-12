using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

using Sim2D.Simulations.Particles.Graphics;
using Sim2D.Simulations.Particles.Physics.Forces;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    public class Rigidbody : Shape2D
    {
        // Physics properties
        public readonly BodyType BodyType;
        public readonly int Charge;
        public int UID;

        // Forces originating from this body
        public List<IForce> ExternalForces = new List<IForce>();

        public Vector Position
        {
            get
            {
                return new Vector(X, Y);
            }
        }

        public Rigidbody(Shape gShape, RigidbodyProperties properties) : base(gShape, properties)
        {
            BodyType = properties.BodyType;
            Charge = properties.charge;
        }
    }
}
