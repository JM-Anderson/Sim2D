using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Sim2D.Simulations.Particles.Utilities;

namespace Sim2D.Simulations.Particles.Physics.Bodies
{
    class Wall
    {
        public readonly Direction direction;
        
        public readonly Vector startPt;
        public readonly Vector endPt;
        public readonly double length;

        /// <summary>
        /// A wall able to be collided with. Will only interact in one direction, and can only face the four main directions.
        /// </summary>
        /// <param name="direction">The direction this wall will interact with</param>
        /// <param name="start">The starting end point of the wall</param>
        /// <param name="length">The length of the wall. Extends from the endpoint either up or right depending on the wall's direction</param>
        public Wall(Direction direction, Vector start, double length)
        {
            this.direction = direction;
            this.length = length;
            startPt = start;

            if (direction == Direction.Down || direction == Direction.Up)
                endPt = new Vector(start.X + length, start.Y);

            else // direction is left or right
                endPt = new Vector(start.X, start.Y + length);
        }
    }
}
