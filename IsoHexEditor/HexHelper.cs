using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsoHexEditor
{
    static class HexHelper
    {
        public const int DEFAULT_MAP_WIDTH = 10;
        public const int DEFAULT_MAP_HEIGHT = 10;

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
    }
}
