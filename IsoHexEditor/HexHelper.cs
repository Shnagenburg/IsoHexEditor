using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoHexEditor
{
    /// <summary>
    /// This class is primarily for methods that are used to edit the grid in
    /// the editor. Most of these methods would not be used during the game
    /// (for example, GenerateNoise)
    /// </summary>
    static class HexHelper
    {
        public const int DEFAULT_MAP_WIDTH = 10;
        public const int DEFAULT_MAP_HEIGHT = 10;

        // temporary for testing purposes
        static Ray globalRay;


        /// <summary>
        /// Randomly sets the depths of all the hexes in the grid.
        /// </summary>
        /// <param name="grid">The grid that has the noise generated on it</param>
        /// <param name="min">Minimum depth</param>
        /// <param name="max">Maximum depth</param>
        static public void GenerateNoise(HexGrid grid, int min, int max)
        {
            Hexagon[,] mGrid = grid.Grid;
            
            Random rand = new Random();
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                        mGrid[i, j].Depth = rand.Next(min, max);                
                }
            }
        }


        /// <summary>
        /// Gets an array of Hexagons surronding the specified hex.
        /// Null hexes ARE in to the array!
        /// 
        /// They are in this order:
        /// 0: Bottom, 1: Bottom Right, 2: Top Right, 3: Top, 4: Top Left, 5: Bottom Left 
        /// </summary>
        /// <returns>An array of hexagons surronding the given hex, including null hexes.</returns>
        static public Hexagon[] SurrondingHexes(HexGrid grid, int x, int y)
        {
            Hexagon[] surrounding = new Hexagon[6];

            bool even = x % 2 == 0;

            Hexagon[,] mGrid = grid.Grid;

            if (y > 0)
                surrounding[0] = mGrid[x, y - 1]; // Bottom   

            if (y < DEFAULT_MAP_HEIGHT - 1)
                surrounding[3] = mGrid[x, y + 1]; // Top

            if (!even)
            {
                if (x < DEFAULT_MAP_WIDTH - 1)
                    surrounding[1] = mGrid[x + 1, y]; // Bottom Right

                if (x < DEFAULT_MAP_WIDTH - 1 && y < DEFAULT_MAP_HEIGHT - 1)
                    surrounding[2] = mGrid[x + 1, y + 1]; // Top Right                       

                if (x > 0 && y < DEFAULT_MAP_HEIGHT - 1)
                    surrounding[4] = mGrid[x - 1, y + 1]; // Top Left

                if (x > 0)
                    surrounding[5] = mGrid[x - 1, y]; // Bottom Left
            }
            else
            {
                if (x < DEFAULT_MAP_WIDTH - 1 && y > 0)
                    surrounding[1] = mGrid[x + 1, y - 1]; // Bottom Right

                if (x < DEFAULT_MAP_WIDTH - 1 && y < DEFAULT_MAP_HEIGHT)
                    surrounding[2] = mGrid[x + 1, y]; // Top Right                       

                if (x > 0 && y < DEFAULT_MAP_HEIGHT)
                    surrounding[4] = mGrid[x - 1, y]; // Top Left

                if (x > 0 && y > 0)
                    surrounding[5] = mGrid[x - 1, y - 1]; // Bottom Left
            }

            return surrounding;
        }

              
        /// <summary>
        /// This method tries to draw a ray from the camera to where the user clicks on the screen.
        /// REVISIT: It needs some serious work. Theres a lot of weird offsets that makes the ray not draw quite right.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="effect"></param>
        /// <param name="hexGrid"></param>
        /// <param name="mouseX">This X value must be adjusted based on the window location prior to this method</param>
        /// <param name="mouseY">This Y value must be adjusted based on the window location prior to this method</param>
        /// <param name="viewMatrix"></param>
        /// <param name="projectionMatrix"></param>
        /// <returns>The selected Hexagon. Returns null if it doesn't intersect anything</returns>
        static public Hexagon SelectHexagon(GraphicsDevice device, HexGrid hexGrid, int mouseX, int mouseY,
            Matrix viewMatrix, Matrix projectionMatrix, Vector3 cameraPosition)
        {

            // Uses the mouse coordinates to create a point in 2d space.
            // I think these offsets account for where the graphics window is inside the form
            // Though honestly I don't know for sure whats going wrong, because the offset of the
            // Graphics window offsets are closer to 12 x 106
            // currently im using the camera position instead of nearsource.
            Vector3 nearsource = new Vector3((float)mouseX - 20, (float)mouseY - 150, 0f);
            Vector3 farsource = new Vector3((float)mouseX - 20, (float)mouseY - 150, 1f);

            Matrix world = Matrix.Identity;

            // Translates the points in 2d space into 
            // Vector3 nearPoint = device.Viewport.Unproject(nearsource, projectionMatrix, viewMatrix, world);

            Vector3 farPoint = device.Viewport.Unproject(farsource, projectionMatrix, viewMatrix, world);

            // Create a ray from the near clip plane to the far clip plane.
            //Vector3 direction = farPoint - nearPoint;
            Vector3 direction = farPoint - cameraPosition; 
           
            direction.Normalize();

            //Ray pickRay = new Ray(nearPoint, direction);
            Ray pickRay = new Ray(cameraPosition, direction);            
            globalRay = pickRay;

            List<Hexagon> candidates = new List<Hexagon>();

            // Once we have the ray loop through it to find all the candidate hexagons.
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    // Set the bounding sphere for the grid to the correct location.
                    hexGrid.Grid[i, j].SetBoundingXY(i, j);

                    // Make a list of candidate hexes for selection.
                    if (hexGrid.Grid[i, j].BoundingSphere.Intersects(pickRay) != null)
                    {
                        candidates.Add(hexGrid.Grid[i, j]);                        
                    }
                }
            }

            if (candidates.Count == 0) // If we didn't find any hexes
            {
                return null;
            }
            else if (candidates.Count == 1) // If we found one hex
            {
                return candidates[0];
            }
            else // If we found multiple hexes, pick the one closest to the camera. REVISIT
                // doesnt quite work right
            {
                Hexagon closest = candidates[0];
                float minDist = Vector3.DistanceSquared(candidates[0].BoundingSphere.Center, cameraPosition);
                float tempDist;
                for (int i = 0; i < candidates.Count; i++)
                {
                    tempDist = Vector3.DistanceSquared(candidates[i].BoundingSphere.Center, cameraPosition);
                    if (minDist > tempDist)
                    {
                        minDist = tempDist;
                        closest = candidates[i];
                    }
                }                
                return closest;
            }            
        }

        /// <summary>
        /// Calculates the translation matrix for the hexagon given its offsets.
        /// This takes into account the moving that must be done to make the hex's align.  REVISIT to remove magic numbers
        /// </summary>
        /// <param name="xOffset">The x position the hex is at in the grid</param>
        /// <param name="yOffset">The y position the hex is at in the grid</param>
        /// <returns>The translation matrix</returns>
        static public Matrix CreateTranslationForHexGrid(int xOffset, int yOffset)
        {
            return Matrix.CreateTranslation(xOffset * 2, xOffset % 2 == 0 ? (yOffset * 3) - 1.5f : yOffset * 3, 0);
        }


        /// <summary>
        /// Draw the ray used to select hexes. Just for testing.
        /// </summary>
        static public void DrawRay(GraphicsDevice device, Effect effect)
        {
            VertexPositionColor[] rayVerts = new VertexPositionColor[2];
            rayVerts[0].Position = globalRay.Position;
            rayVerts[1].Position = globalRay.Position + (globalRay.Direction * 30);
            short[] rayIndicies = { 0, 1 };
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    rayVerts,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    2,  // number of vertices in pointList
                    rayIndicies,  // the index buffer
                    0,  // first index element to read
                    1,
                    VertexPositionColor.VertexDeclaration // number of primitives to draw
                    );
            }

        }



        /// <summary>
        /// Currently does not work. Ideally it would set the vertices of 
        /// a hex to be equal to their neighbors, giving priority to the lowest one.
        /// </summary>
        static public void SmoothHexesDown(HexGrid grid)
        {
            Hexagon[,] mGrid = grid.Grid;
            Random rand = new Random();
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        mGrid[i, j].SetVertexDepth(k, .01f * (float)rand.Next(100));
                    }
                }
            }
        }
    }
}
