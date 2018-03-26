using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Sim2D.Simulations.Particles.Physics.Bodies;

namespace Sim2D.Simulations.Particles.Optimizations.Organization
{
    class SpatialGrid : IOrginization
    {
        // Body collections
        private List<Rigidbody> allBodies = new List<Rigidbody>();
        public Dictionary<Vector, List<Rigidbody>> OrganizedBodies { get; private set; } = new Dictionary<Vector, List<Rigidbody>>();

        // Constants
        private readonly double gridScale;

        public SpatialGrid(List<Rigidbody> allBodies, double maxBodySize)
        {
            this.allBodies = allBodies;
            gridScale = maxBodySize;
        }

        public List<List<Rigidbody>> Organize()
        {
            foreach (Rigidbody body in allBodies)
            {
                // Calculate position in grid
                double gridX = body.X / gridScale;
                double gridY = body.Y / gridScale;

                // Calculate radius in grid units
                double bodyGridR = body.radius / gridScale;

                // Calculate node vector
                int nodeX = (int)gridX; // Truncation by casting to int is the same as flooring (and also faster) for positive numbers
                int nodeY = (int)gridY;

                AddToNode(body, nodeX, nodeY);

                bool inNodeLeft = gridX - bodyGridR < nodeX;
                bool inNodeRight = gridX + bodyGridR > nodeX + gridScale;
                bool inNodeBot = gridY - bodyGridR < nodeY;
                bool inNodeTop = gridY + bodyGridR > nodeY + gridScale;

                // Add to nodes left, right
                if (inNodeLeft) AddToNode(body, nodeX - 1, nodeY);
                if (inNodeRight) AddToNode(body, nodeX + 1, nodeY);

                // Add to nodes bottom and top while checking for diagonals
                if (inNodeBot)
                {
                    AddToNode(body, nodeX, nodeY - 1);

                    if (inNodeLeft) AddToNode(body, nodeX - 1, nodeY - 1);
                    if (inNodeRight) AddToNode(body, nodeX + 1, nodeY - 1);
                }
                if (inNodeTop)
                {
                    AddToNode(body, nodeX, nodeY + 1);

                    if (inNodeLeft) AddToNode(body, nodeX - 1, nodeY + 1);
                    if (inNodeRight) AddToNode(body, nodeX + 1, nodeY + 1);
                }
            }

            return OrganizedBodies.Values.ToList();
        }

        private void AddToNode(Rigidbody body, int nodeX, int nodeY)
        {
            Vector nodePos = new Vector(nodeX, nodeY);

            if (OrganizedBodies.ContainsKey(nodePos))
            {
                OrganizedBodies[nodePos].Add(body);
            }
            else
            {
                OrganizedBodies[nodePos] = new List<Rigidbody>() { body };
            }
        }
    }
}
