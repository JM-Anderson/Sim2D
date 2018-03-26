using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;

using Sim2D.Simulations.Particles.Graphics;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    class BodyManager
    {
        public readonly GraphicsManager graphicsManager;

        public List<Rigidbody> bodies = new List<Rigidbody>();
        public List<Particle> particles = new List<Particle>();

        public BodyManager(GraphicsManager graphicsManager)
        {
            this.graphicsManager = graphicsManager;
        }

        public Rigidbody CreateBody(RigidbodyProperties properties)
        {
            // Create graphic
            Shape bodyGraphic = graphicsManager.CreateShape(properties);

            // Create rigid body using graphic
            Rigidbody newBody;
            if (properties.BodyType == BodyType.Particle)
            {
                newBody = new Particle(bodyGraphic, (ParticleProperties)properties);
                particles.Add((Particle)newBody);
            }
            else
                newBody = new Rigidbody(bodyGraphic, properties);

            // Hook into the Delete event of the body to remove the body from this manager when it is deleted
            newBody.OnDelete += BodyDeleted;

            // Track body
            bodies.Add(newBody);

            return newBody;
        }

        // Handles a body being deleted
        private void BodyDeleted(object senderBody, EventArgs args)
        {
            // Remove body from universal list
            bodies.Remove((Rigidbody)senderBody);

            // Remove from particle only list
            if (((Rigidbody)senderBody).BodyType == BodyType.Particle)
            {
                particles.Remove((Particle)senderBody);
            }
        }
    }
}
