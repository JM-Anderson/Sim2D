using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

using System.Diagnostics;

namespace Sim2D.Simulations.Sand.Grains
{
    class GrainHandler
    {
        Canvas simCanvas;

        double grainScale;
        double rectScale;

        public int maxGrainWidth { get; private set; }

        // Stores grains by position
        public Dictionary<Vector, BaseGrain> activeGrains = new Dictionary<Vector, BaseGrain>();

        // Initializer
        public GrainHandler(Canvas simCanvas, double grainScale)
        {
            this.simCanvas = simCanvas;
            this.grainScale = grainScale;

            WindowResized();

            // Calculate grain graphic size
            rectScale = grainScale - 1;
            while (rectScale <= 0) rectScale += 0.1;
        }

        // Create grains
        public BaseGrain TryCreateGrain(double x, double y)
        {
            // Convert from regular coordinates to sim coordinates
            int simXPos = (int)(x / grainScale);
            int simYPos = (int)(y / grainScale);

            // Check if grain is out of bounds
            if (   simXPos > maxGrainWidth
                || simXPos < 0
                || y + rectScale > simCanvas.ActualHeight)
            {
                Debug.WriteLine("Grain out of bounds");
                return null;
            }
            
            // Create grain
            return SimCreateGrain(simXPos, simYPos);
        }
        private BaseGrain SimCreateGrain(int x, int y)
        {
            Vector grainPosition = new Vector(x, y);

            if (activeGrains.ContainsKey(grainPosition))
            {
                Debug.WriteLine("Grain already exists at that position");
                return null;
            }

            // Create grain
            BaseGrain grain = new BaseGrain(x, y, grainScale);

            // Add to canvas
            grain.Draw(simCanvas);

            // Add to dictionary of active grains
            activeGrains.Add(new Vector(x, y), grain);

            return grain;
        }

        // Move grains
        public void MoveGrain(BaseGrain grain, int dx, int dy)
        {
            
            if (activeGrains.ContainsKey(new Vector(dx, dy) + grain.SimPos()))
            {
                Debug.WriteLine("Position blocked");
                return;
            }

            activeGrains.Remove(grain.SimPos());

            grain.SimMove(dx, dy);

            activeGrains.Add(grain.SimPos(), grain);
        }

        // Utility
        public bool GrainAt(int x, int y)
        {
            return activeGrains.ContainsKey(new Vector(x, y));
        }
        public void WindowResized()
        {
            maxGrainWidth = (int)(simCanvas.ActualWidth / grainScale);
        }
    }
}
