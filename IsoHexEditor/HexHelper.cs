using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoHexEditor
{
    static class HexHelper
    {
        public const int DEFAULT_MAP_WIDTH = 10;
        public const int DEFAULT_MAP_HEIGHT = 10;

        // temp
        static Ray globalRay;


        /// <summary>
        /// Randomly sets the depths of all the hexes in the grid.
        /// </summary>
        /// <param name="grid">The grid that has the noise generated on it</param>
        /// <param name="min">Minimum depth</param>
        /// <param name="max">Maximum depth</param>
        static public void GenerateNoise(HexGrid grid, int min, int max, int smoothness)
        {
            Hexagon[,] mGrid = grid.Grid;
            
            Random rand = new Random();
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    if (smoothness != 0)
                    {
                        Hexagon[] surrounding = SurrondingHexes(grid, i, j);
                        float averageDepth = 0;
                        int counter = 0;
                        for (int k = 0; k < surrounding.Length; k++)
                        {
                            if (surrounding[k] != null)
                            {
                                averageDepth += surrounding[k].Depth;
                                counter++;
                                surrounding[k].Depth = averageDepth;
                            }
                            if (counter > 0)
                            {
                                averageDepth = averageDepth / (float)counter;
                            }
                        }

                        mGrid[i, j].Depth = (   ((float)rand.Next(min, max) + (smoothness * averageDepth)) / (float)(smoothness + 1));
                    }
                    else
                    {
                        mGrid[i, j].Depth = rand.Next(min, max);
                    }
                }
            }
        }


        /// <summary>
        /// Gets an array of Hexagons surronding the specified hex.
        /// Null hexes ARE in to the array!
        /// 
        /// They are in this order:
        /// Bottom, Bottom Right, Top Right, Top, Top Left, Bottom Left 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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

        
        // REVIST fix magic numbers
        static public Hexagon SelectHexagon(GraphicsDevice device, Effect effect, HexGrid hexGrid, int mouseX, int mouseY, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Vector3 nearsource = new Vector3((float)mouseX - 20, (float)mouseY - 150, 0f);
            Vector3 farsource = new Vector3((float)mouseX - 20, (float)mouseY - 150, 1f);

            Matrix world = Matrix.Identity;

            Vector3 nearPoint = device.Viewport.Unproject(nearsource, projectionMatrix, viewMatrix, world);

            Vector3 farPoint = device.Viewport.Unproject(farsource, projectionMatrix, viewMatrix, world);

            // Create a ray from the near clip plane to the far clip plane.
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            Ray pickRay = new Ray(nearPoint, direction);
            globalRay = pickRay;
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    hexGrid.Grid[i, j].SetBoundingXY(i, j);

                    if (hexGrid.Grid[i, j].BoundingSphere.Intersects(pickRay) != null)
                    {
                        hexGrid.Grid[i, j].SetColorScheme(Color.Blue, Color.Blue);
                        return hexGrid.Grid[i, j];
                    }
                }
            }                      


            return null;
            
        }


        static private Matrix CreateTranslationForHexGrid(int xOffset, int yOffset)
        {
            return Matrix.CreateTranslation(xOffset * 2, xOffset % 2 == 0 ? (yOffset * 3) - 1.5f : yOffset * 3, 0);
        }

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
    }
}
