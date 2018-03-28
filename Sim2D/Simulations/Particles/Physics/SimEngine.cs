using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

using Sim2D.Simulations.Particles.Physics.Bodies;
using Sim2D.Simulations.Particles.Physics.Forces;
using Sim2D.Simulations.Particles.Optimizations.Organization;

namespace Sim2D.Simulations.Particles.Physics
{
    class SimEngine
    {
        private BodyManager bodyManager;
        private Vector ContainerSize { get { return bodyManager.graphicsManager.CanvasSize; } }

        public bool RigidbodyCollisions = true;

        // Forces that apply to everything
        public List<IForce> worldForces = new List<IForce>();

        public SimEngine(BodyManager bodyManager)
        {
            this.bodyManager = bodyManager;
        }

        // Walls
        private Wall BottomWall { get { return new Wall(Utilities.Direction.Up, new Vector(0, 0), ContainerSize.X); } }
        private Wall LeftWall { get { return new Wall(Utilities.Direction.Right, new Vector(0, 0), ContainerSize.Y); } }
        private Wall RightWall { get { return new Wall(Utilities.Direction.Left, new Vector(ContainerSize.X, 0), ContainerSize.Y); } }
        private Wall TopWall { get { return new Wall(Utilities.Direction.Down, new Vector(0, ContainerSize.Y), ContainerSize.X); } }

        // Update - Runs every frame
        public void Update(double dt)
        {
            Particle[] allParticles = bodyManager.particles.ToArray();

            // Setup particle values for this Update
            foreach (Particle p in bodyManager.particles)
            {
                p.timeLeft = dt;

                // Update acceleration from particle forces
                foreach (IForce force in p.ExternalForces)
                {
                    force.ApplyOn(allParticles);
                }
            }

            // Update acceleration from world forces
            foreach (IForce force in worldForces)
            {
                force.ApplyOn(allParticles);
            }

            // Apply acceleration - Update velocity based on current particle positions
            foreach (Particle p in allParticles)
            {
                // Apply acceleration
                p.ApplyAcceleration(dt);

                p.Acceleration = new Vector(0, 0);
            }

            if (RigidbodyCollisions)
            {
                // Organize bodies to avoid unnecessary calculations
                IOrginization organization = new BoundingBoxes(bodyManager.bodies);
                //IOrginization organization = new SpatialGrid(bodyManager.bodies, 10);
                var organizedBodies = organization.Organize();

                // Handle particle particle collisions
                foreach (List<Rigidbody> nearBodies in organizedBodies)
                {
                    if (nearBodies.Count > 1)
                    {
                        new Collision(nearBodies[0], nearBodies[1], dt).Resolve();
                    }
                }
            }

            // Handle particle wall collisions
            // Move each particle for its remaining timeLeft
            // Make sure each particle is in bounds
            foreach (Particle p in bodyManager.particles)
            {
                foreach (Wall wall in new Wall[] { BottomWall, LeftWall, RightWall, TopWall })
                {
                    if (new Collision(p, wall).Resolve())
                        break;
                }

                p.SimMove(p.timeLeft);

                // Check if particle is out of bounds - if so -> place in bounds
                if (p.Y > ContainerSize.Y - p.radius)
                {
                    p.Move(0, ContainerSize.Y - p.radius - p.Y);
                }
                if (p.X > ContainerSize.X - p.radius)
                {
                    p.Move(ContainerSize.X - p.radius - p.X, 0);
                }
            }
        }
    }
}
