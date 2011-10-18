using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    class HexGrid
    {
        const int DEFAULT_MAP_WIDTH = HexHelper.DEFAULT_MAP_WIDTH;
        const int DEFAULT_MAP_HEIGHT = HexHelper.DEFAULT_MAP_WIDTH;

        private Hexagon [,] mGrid;
        public Hexagon[,] Grid
        {
            get{ return mGrid;}
        }

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

        

        public void SmoothHexesDown()
        {
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
        public void ClusterColor(int x, int y)
        {
            Hexagon [] hexes = HexHelper.SurrondingHexes(this , x, y);

            foreach (Hexagon h in hexes)
            {
                if (h != null)
                    h.SetColorScheme(Color.Red, Color.Pink);
            }
        }

        
    }
}
