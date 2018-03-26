using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

using Sim2D.Simulations.Particles.Physics;
using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Physics
{
    enum CollisionType
    {
        Generic, ParticleParticle, ParticleWall
    }

    class Collision
    {
        // Primary object - will always be defined
        private Rigidbody b1;

        // Secondary object - may be null
        private Rigidbody b2 = null;
        private Wall wall = null;

        public CollisionType CollType { get; private set; }

        public Collision(Rigidbody b1, Rigidbody b2, double timeLeft)
        {
            CollisionType collType;
            if (b1.BodyType == BodyType.Particle && b2.BodyType == BodyType.Particle)
                collType = CollisionType.ParticleParticle;
            else
                collType = CollisionType.Generic;

            Init(b1, collType);
            this.b2 = b2;
        }
        public Collision(Particle p1, Wall wall)
        {
            Init(p1, CollisionType.ParticleWall);
            this.wall = wall;
        }

        private void Init(Rigidbody b1, CollisionType collType)
        {
            this.b1 = b1;
            CollType = collType;
        }

        public bool Resolve()
        {
            switch (CollType)
            {
                case CollisionType.ParticleParticle:
                    return HandleCollision((Particle)b1, (Particle)b2);

                case CollisionType.ParticleWall:
                    return HandleCollision((Particle)b1, wall);
            }

            return false;
        }

        // Determine if two circular rigid bodies are colliding
        public static bool AreColliding(Rigidbody b1, Rigidbody b2)
        {
            double xDist = b1.X - b2.X;
            double yDist = b1.Y - b2.Y;

            double distSqr = xDist * xDist + yDist * yDist; // This is more efficient than Math.Pow
            double radiiSum = b1.radius + b2.radius;

            return distSqr < radiiSum * radiiSum;
        }

        // Particle to particle handling
        public static bool HandleCollision(Particle p1, Particle p2)
        {
            double collTime = CalcCollTime(p1, p2);
            if (Double.IsNaN(collTime) || Math.Abs(collTime) > p1.timeLeft || Math.Abs(collTime) > p2.timeLeft) return false;

            p1.SimMove(collTime);
            p2.SimMove(collTime);

            Tuple<Vector, Vector> exitVels = CalcExitVelocities(p1, p2);

            p1.Velocity = exitVels.Item1;
            p2.Velocity = exitVels.Item2;

            p1.timeLeft -= collTime;
            p2.timeLeft -= collTime;

            return true;
        }
        public static Tuple<Vector, Vector> CalcExitVelocities(Particle p1, Particle p2)
        {
            // Calculate differences in position
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            // distance between both particles squared
            double distSqr = dx * dx + dy * dy;

            // Calculate differences in velocity
            double dVelX = p1.Velocity.X - p2.Velocity.X;
            double dVelY = p1.Velocity.Y - p2.Velocity.Y;

            // Do some random math that will be involved in the final answer
            double dSum = dx * dVelX + dy * dVelY;
            double denominator = distSqr * (p1.mass + p2.mass);
            double numConst = (2.0 * dSum) / denominator;
            double numConst1 = numConst * p2.mass;
            double numConst2 = numConst * p1.mass;

            // Final answers
            Vector p1FinalVel = new Vector(
                p1.Velocity.X - numConst1 * dx,
                p1.Velocity.Y - numConst1 * dy
            );

            Vector p2FinalVel = new Vector(
                p2.Velocity.X + numConst2 * dx,
                p2.Velocity.Y + numConst2 * dy
            );

            return new Tuple<Vector, Vector>(p1FinalVel, p2FinalVel);
        }
        public static double CalcCollTime(Particle p1, Particle p2)
        {
            Vector position12 = p1.Position - p2.Position;
            Vector velocity12 = p1.Velocity - p2.Velocity;

            double a = velocity12 * velocity12;
            double b = 2 * (position12 * velocity12);
            double c = position12 * position12 - Math.Pow(p1.radius + p2.radius, 2.0);

            double discrim = b * b - 4 * a * c;

            if (discrim <= 0) 
                return Double.NaN;

            double collTime = (-b - Math.Sqrt(discrim)) / (2 * a);
            return collTime;
        }

        // Particle to wall handling
        public static bool HandleCollision(Particle p, Wall wall)
        {
            double collTime = 0;

            double collY;
            double collX;

            switch (wall.direction)
            {
                case Utilities.Direction.Up:
                    // Collision not possible
                    if (p.Velocity.Y >= 0) return false;

                    collTime = (p.radius - p.Y + wall.startPt.Y) / p.Velocity.Y;
                    
                    // Collision won't happen this frame
                    if (collTime > p.timeLeft) return false;

                    collX = p.X + p.Velocity.X * collTime;

                    // Wall is too short to collide with particle
                    if (collX < wall.startPt.X || collX > wall.endPt.X) return false;

                    p.SimMove(collTime);
                    p.Velocity.Y = Math.Abs(p.Velocity.Y);
                    break;

                case Utilities.Direction.Right:
                    // Collision not possible
                    if (p.Velocity.X >= 0) return false;

                    collTime = (p.radius - p.X + wall.startPt.X) / p.Velocity.X;

                    // Collision won't happen this frame
                    if (collTime > p.timeLeft) return false;
                    
                    collY = p.Y + p.Velocity.Y * collTime;

                    // Wall is too short to collide with particle
                    if (collY < wall.startPt.Y || collY > wall.endPt.Y) return false;

                    p.SimMove(collTime);
                    p.Velocity.X = Math.Abs(p.Velocity.X);

                    break;

                case Utilities.Direction.Left:
                    // Collision not possible
                    if (p.Velocity.X <= 0) return false;

                    collTime = (wall.startPt.X - p.X - p.radius) / p.Velocity.X;

                    // Collision won't happen this frame
                    if (collTime > p.timeLeft) return false;

                    collY = p.Y + p.Velocity.Y * collTime;

                    // Wall is too short to collide with particle
                    if (collY < wall.startPt.Y || collY > wall.endPt.Y) return false;

                    p.SimMove(collTime);
                    p.Velocity.X = -1 * Math.Abs(p.Velocity.X);

                    break;

                case Utilities.Direction.Down:
                    // Collision not possible
                    if (p.Velocity.Y <= 0) return false;

                    collTime = (wall.startPt.Y - p.Y - p.radius) / p.Velocity.Y;

                    // Collision won't happen this frame
                    if (collTime > p.timeLeft) return false;

                    collX = p.X + p.Velocity.X * collTime;

                    // Wall is too short to collide with particle
                    if (collX < wall.startPt.X || collX > wall.endPt.X) return false;

                    p.SimMove(collTime);
                    p.Velocity.Y = -1 * Math.Abs(p.Velocity.Y);

                    break;
            }

            p.timeLeft -= collTime;
            return true;
        }
    }
}
