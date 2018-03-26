using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Optimizations.Organization
{
    class BoundingBoxes : IOrginization
    {
        List<Rigidbody> allBodies;

        public BoundingBoxes(List<Rigidbody> allBodies)
        {
            this.allBodies = allBodies;
        }

        public List<List<Rigidbody>> Organize()
        {
            var potentiallyColliding = new List<List<Rigidbody>>();

            foreach (Tuple<Rigidbody, Rigidbody> bodyPair in GetBodyCombinations())
            {
                double radSum = bodyPair.Item1.radius + bodyPair.Item2.radius;

                // Check if bounding boxes collide
                if (Math.Abs(bodyPair.Item1.X - bodyPair.Item2.X) < radSum && Math.Abs(bodyPair.Item1.Y - bodyPair.Item2.Y) < radSum)
                {
                    potentiallyColliding.Add(new List<Rigidbody>() { bodyPair.Item1, bodyPair.Item2 });
                }
            }

            return potentiallyColliding;
        }

        private List<Tuple<Rigidbody, Rigidbody>> GetBodyCombinations()
        {
            var combos = new List<Tuple<Rigidbody, Rigidbody>>();

            for (int i = 0; i < allBodies.Count - 1; i++)
            {
                for (int i2 = i + 1; i2 < allBodies.Count; i2++)
                {
                    combos.Add(new Tuple<Rigidbody, Rigidbody>(allBodies[i], allBodies[i2]));
                }
            }

            return combos;
        }

        // I have no idea how this works, but it returns all combinations of length n
        static List<List<T>> GetKCombs<T>(List<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new List<T> { t }).ToList();

            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new List<T> { t2 }).ToList()).ToList();
        }
    }
}
