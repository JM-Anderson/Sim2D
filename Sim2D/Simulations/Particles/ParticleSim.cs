using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

using Sim2D.Simulations.Particles.Graphics;
using Sim2D.Simulations.Particles.Physics.Bodies;
using Sim2D.Simulations.Particles.Physics;
using Sim2D.Simulations.Particles.Physics.Forces;

namespace Sim2D.Simulations.Particles
{
    public class ParticleSim
    {
        /*
         * Members
         */

        // Graphics
        private Canvas simCanvas;
        private GraphicsManager graphicsManager;

        // Particle trail settings
        private bool ParticleTrails = false;
        private int _trailLength = 120;
        public int TrailLength
        {
            get { return _trailLength; }
            set
            {
                foreach (Rigidbody body in bodyManager.bodies)
                {
                    body.TrailLength = value;
                }
                _trailLength = value;
            }
        }

        // Physics
        private BodyManager bodyManager;
        private SimEngine simEngine;

        // Electro Magnetic force settings
        private ElectroMagneticState elecMagState = ElectroMagneticState.Normal;
        public ElectroMagneticState ElectroMagneticState
        {
            get
            {
                return elecMagState;
            }
            set
            {
                foreach (Rigidbody body in bodyManager.bodies)
                {
                    foreach (IForce force in body.ExternalForces)
                    {
                        if (force.GetForceType() == Forces.ElectroMagnetic)
                        {
                            (force as ElectroMagnetic).MagneticState = value;
                        }
                    }
                }
                elecMagState = value;
            }
        }

        /*
         * Methods
         */

        // Intializer
        public ParticleSim(Canvas simCanvas)
        {
            this.simCanvas = simCanvas;
            graphicsManager = new GraphicsManager(simCanvas);
            bodyManager = new BodyManager(graphicsManager);
            simEngine = new SimEngine(bodyManager);
        }

        // Main Loop
        public void NextFrame(double interval)
        {
            simEngine.Update(interval);
        }

        // Spawning bodies
        public void CreateBody(RigidbodyProperties properties)
        {
            Rigidbody newBody = bodyManager.CreateBody(properties);
            if (ParticleTrails) newBody.HasTrail = true;

            // Add electromagnetic force
            newBody.ExternalForces.Add(new ElectroMagnetic(newBody)
            {
                MagneticState = ElectroMagneticState
            });
        }

        // Spawning linear forces
        public void SpawnForce(Linear force)
        {
            simEngine.worldForces.Add(force);
        }

        // Deleting bodies
        public void DeleteAll()
        {
            Rigidbody[] allBodies = bodyManager.bodies.ToArray();
            foreach (Rigidbody body in allBodies)
            {
                body.Delete();
            }

            simEngine.worldForces = new List<IForce>();
        }

        // Toggle particle trails
        public void ToggleTrails()
        {
            ParticleTrails = !ParticleTrails;

            foreach(Rigidbody body in bodyManager.bodies)
            {
                body.HasTrail = ParticleTrails;
            }
        }

        // Toggle particle / rigidbody collisions
        public void ToggleCollisions()
        {
            simEngine.RigidbodyCollisions = !simEngine.RigidbodyCollisions;
        }
    }
}