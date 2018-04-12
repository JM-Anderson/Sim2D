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
        private bool _particleTrails = false;
        public bool ParticleTrailsOn
        {
            get { return _particleTrails; }
            set
            {
                if (value != _particleTrails)
                {
                    _particleTrails = value;
                    foreach (Rigidbody body in bodyManager.bodies)
                    {
                        body.HasTrail = _particleTrails;
                    }
                }
            }
        }

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

        public bool CollisionsOn
        {
            get { return simEngine.RigidbodyCollisions; }
            set
            {
                simEngine.RigidbodyCollisions = value;
            }
        }

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
         * Events
         */
        public event EventHandler BodySpawned;

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

        // Utility methods - used for outside interaction

        // Spawning bodies
        public void CreateBody(RigidbodyProperties properties)
        {
            Rigidbody newBody = bodyManager.CreateBody(properties);
            if (_particleTrails)
            {
                newBody.HasTrail = true;
                newBody.TrailLength = TrailLength;
            }

            // Add electromagnetic force
            newBody.ExternalForces.Add(new ElectroMagnetic(newBody)
            {
                MagneticState = ElectroMagneticState
            });

            BodySpawned(newBody, new EventArgs());
        }

        // Spawning linear forces
        public void SpawnForce(Linear force)
        {
            simEngine.worldForces.Add(force);
        }

        // Get bodies
        public Rigidbody[] GetAllBodies()
        {
            return bodyManager.bodies.ToArray();
        }
        public Rigidbody[] GetBodiesInArea(Vector bottomLeft, Vector topRight)
        {
            List<Rigidbody> bodiesInArea = new List<Rigidbody>();
            foreach (Rigidbody body in bodyManager.bodies)
            {
                if (PointInRect(body.Position, bottomLeft, topRight))
                {
                    bodiesInArea.Add(body);
                }
            }

            return bodiesInArea.ToArray();
        }
        private bool PointInRect(Vector point, Vector bottomLeft, Vector topRight)
        {
            if (point.X > bottomLeft.X
                && point.X < topRight.X
                && point.Y > bottomLeft.Y
                && point.Y < topRight.Y)
            {
                return true;
            }
            else return false;
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
    }
}