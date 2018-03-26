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

        // FPS
        private DateTime lastFrameTime = DateTime.Now;
        public double lastDeltaT { get; private set; } = 0;

        // Physics
        private BodyManager bodyManager;
        private SimEngine simEngine;

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
        public void NextFrame(double? interval = null)
        {
            // Find new frame time and delta T
            DateTime currentFrameTime = DateTime.Now;
            double deltaT = (currentFrameTime - lastFrameTime).TotalSeconds;

            // Store frame time and delta T for use next frame
            lastFrameTime = currentFrameTime;
            lastDeltaT = deltaT;

            simEngine.Update(interval ?? deltaT);
        }

        // Spawning bodies
        public void CreateBody(RigidbodyProperties properties)
        {
            Rigidbody newBody = bodyManager.CreateBody(properties);

            // Add electromagnetic force
            newBody.ExternalForces.Add(new ElectroMagnetic(newBody));
        }

        // Deleting bodies
        public void DeleteAll()
        {
            Rigidbody[] allBodies = bodyManager.bodies.ToArray();
            foreach (Rigidbody body in allBodies)
            {
                body.Delete();
            }
        }
    }
}