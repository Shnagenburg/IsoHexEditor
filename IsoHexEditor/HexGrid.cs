using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    /// <summary>
    /// A collection of hexagons.
    /// </summary>
    class HexGrid
    {
        const int DEFAULT_MAP_WIDTH = HexHelper.DEFAULT_MAP_WIDTH;
        const int DEFAULT_MAP_HEIGHT = HexHelper.DEFAULT_MAP_WIDTH;

        private Hexagon [,] mGrid;
        /// <summary>
        /// The matrix that represents the grid of hexagons.
        /// </summary>
        public Hexagon[,] Grid
        {
            get{ return mGrid;}
        }

        /// <summary>
        /// Creates a new hexgrid with the default map width and height and fills it with hexagons.
        /// </summary>
        public HexGrid()
        {
            mGrid = new Hexagon[DEFAULT_MAP_WIDTH, DEFAULT_MAP_HEIGHT];

            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    mGrid[i, j] = new Hexagon();
                }
            }

        }

        /// <summary>
        /// Draw all of the hexagons in the grid.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="effect">The current effect being used</param>
        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    mGrid[i, j].Draw(device, effect, i, j);
                }
            }
        }

        /// <summary>
        /// Draw the wireframes of all the hexagons in the grid.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="effect">The current effect being used</param>
        public void DrawWireFrame(GraphicsDevice device, BasicEffect effect)
        {
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    mGrid[i, j].DrawWireFrame(device, effect, i, j);
                }
            }
        }

     
        /// <summary>
        /// Sets the colors for all the hexagons in the grid.
        /// </summary>
        public void SetColorSchemeAll(Color color1, Color color2)
        {
            for (int i = 0; i < DEFAULT_MAP_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_MAP_HEIGHT; j++)
                {
                    mGrid[i, j].SetColorScheme(color1, color2);                    
                }
            }
        }

        /// <summary>
        /// Colors all the hexagons around a given hex.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ClusterColor(int x, int y, Color color1, Color color2)
        {
            Hexagon [] hexes = HexHelper.SurrondingHexes(this , x, y);

            foreach (Hexagon h in hexes)
            {
                if (h != null)
                    h.SetColorScheme(color1, color2);
            }
        }

        
    }
}
