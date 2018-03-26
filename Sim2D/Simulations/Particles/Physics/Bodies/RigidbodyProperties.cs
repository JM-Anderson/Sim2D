using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Sim2D.Simulations.Particles.Graphics;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    public class RigidbodyProperties : ShapeProperties
    {
        public BodyType BodyType { get; protected set; }

        public readonly double radius;
        public readonly int charge;

        public RigidbodyProperties(
            // Shape properties
            double X, double Y, ShapeType shapeType, Color color, double radius,

            // Physics properties
            int charge
            )
            : base(X, Y, shapeType, color, radius * 2)
        {
            BodyType = BodyType.Rigidbody;
            this.charge = charge;
            this.radius = radius;
        }
    }
}
