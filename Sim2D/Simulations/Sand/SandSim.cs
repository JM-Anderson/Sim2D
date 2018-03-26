using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

using Sim2D.Simulations.Sand.Grains;

using System.Diagnostics;

namespace Sim2D.Simulations.Sand
{
    class SandSim
    {
        /* Sim class members
         * 
        */
        private Canvas simCanvas;
        private double grainScale;

        private List<BaseGrain> allGrains;

        private Random randomGen = new Random();
        private int rand = 0;

        // Handlers
        private GrainHandler grainHandler;

        // Initializer
        public SandSim(Canvas simCanvas, double grainScale = 1)
        {
            // Store members
            this.simCanvas = simCanvas;
            this.grainScale = grainScale;

            // Initializers
            allGrains = new List<BaseGrain>();

            grainHandler = new GrainHandler(simCanvas, grainScale);
        }

        // Main loop
        public void Update()
        {
            rand++;
            if (rand > 100) rand = 0;

            foreach (BaseGrain grain in allGrains)
            {
                UpdateGrain(grain);
            }
        }
        private void UpdateGrain(BaseGrain grain)
        {
            if (grain.IsStatic)
            {
                return;
            }

            // Stop falling if on floor
            if (grain.simY == 0)
            {
                //grain.IsStatic = CheckStatic(grain);
                
            }
            // If there is a grain below, try to fall to the side
            else if (grainHandler.GrainAt(grain.simX, grain.simY - 1))
            {
                //grain.IsStatic = CheckStatic(grain);
                //if (grain.IsStatic) return;

                TryFlowDown(grain);
            }
            // If not on the floor and there is no grain below, fall directly down
            else
            {
                grainHandler.MoveGrain(grain, 0, -1);
            }
        }

        public void WindowResize()
        {
            grainHandler.WindowResized();
        }

        // Create grain on canvas click
        public void CanvasClick(Point mousePos)
        {
            BaseGrain newGrain = grainHandler.TryCreateGrain(
                mousePos.X,
                simCanvas.ActualHeight - mousePos.Y);

            if (newGrain != null)
            {
                allGrains.Add(newGrain);
            }
        }

        // Utility
        private Vector GetTopMostGrain(int x)
        {
            Vector topMost = new Vector(x, -1);

            int y = 0;
            while (true)
            {
                Vector currPos = new Vector(x, y);

                if (grainHandler.activeGrains.ContainsKey(currPos))
                {
                    topMost = currPos;
                    y++;
                }
                else
                {
                    break;
                }
            }

            return topMost;
        }
        private void TryFlowDown(BaseGrain grain)
        {
            bool canFallLeft = false;
            bool canFallRight = false;

            // If the grain is not on the left wall and there is no grain to the left, try to fall left
            if (grain.simX > 0 && !grainHandler.GrainAt(grain.simX - 1, grain.simY))
            {
                Vector leftGrain = GetTopMostGrain(grain.simX - 1);
                double slopeL = (leftGrain.Y - grain.simY) / (leftGrain.X - grain.simX);

                if (slopeL > grain.maxSlope)
                    canFallLeft = true;
            }

            // If the grain is not on the right wall and there is no grain to the right, try to fall right
            if (grain.simX < grainHandler.maxGrainWidth && !grainHandler.GrainAt(grain.simX + 1, grain.simY))
            {
                Vector rightGrain = GetTopMostGrain(grain.simX + 1);
                double slopeR = (grain.simY - rightGrain.Y) / (grain.simX - rightGrain.X);

                if (slopeR < -1 * grain.maxSlope)
                    canFallRight = true;
            }


            int xMovement;

            // If the grain can move either left OR right, pick at random depending if on the frame number is even or odd
            if (canFallRight && canFallLeft)
            {
                xMovement = (randomGen.Next() % 2 == 0) ? -1 : 1;
                if (xMovement < 0) Debug.WriteLine("chose left");
                else Debug.WriteLine("chose right");
            }
                

            // If the grain can't move at all, don't
            else if (!canFallRight && !canFallLeft)
                return;

            // Move left if canFallLeft, else move right
            else
                xMovement = (canFallLeft) ? -1 : 1;

            int yMovement = 0;
            if (grain.maxSlope >= 1)
                yMovement = -1;

            grainHandler.MoveGrain(grain, xMovement, yMovement);
        }
        private bool CheckStatic(BaseGrain grain)
        {
            bool leftCovered = false;
            bool rightCovered = false;
            bool bottomCovered = false;
            bool topCovered = false;

            if (grain.simX == 0 || grainHandler.GrainAt(grain.simX - 1, grain.simY))
                leftCovered = true;

            if (grain.simX == grainHandler.maxGrainWidth || grainHandler.GrainAt(grain.simX + 1, grain.simY))
                rightCovered = true;

            if (grain.simY == 0 || grainHandler.GrainAt(grain.simX, grain.simY - 1))
                bottomCovered = true;

            if (grainHandler.GrainAt(grain.simX, grain.simY + 1))
                topCovered = true;

            return leftCovered && rightCovered && bottomCovered && topCovered;
        }
    }
}
