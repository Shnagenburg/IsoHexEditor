using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    /// <summary>
    /// The HexTube is the cylinder base for the hexagons.
    /// </summary>
    class HexTube
    {
        const int DEFFAULT_DEPTH = Hexagon.DEFAULT_DEPTH;
        const int HEX_WIDTH = Hexagon.HEX_WIDTH;
        const int HEX_HEIGHT = Hexagon.HEX_HEIGHT;

        VertexPositionColor[] vertices = new VertexPositionColor[12];

        float fDepth;
        /// <summary>
        /// The Depth (z-value) of the tube
        /// </summary>
        public float Depth
        {
            get { return fDepth; }
            set
            {
                fDepth = value;
                SetUpVerts(fDepth); 
            }
        }


        short[] triangleStripIndices;
        short[] lineListIndices;

        public HexTube()
        {
            SetUpVerts(DEFFAULT_DEPTH);
            SetUpIndicies();
            SetDefaultColor();
        }


        //    4______3
        //    /      \
        //  5/        \2
        //   \        /
        //   0\______/1
        //
        //   10______9
        //    /      \
        // 11/        \8
        //   \        /
        //   6\______/7
        // Treats the bottom left corner of the hextube's frame as 0,0.
        private void SetUpVerts(float hexDepth)
        {
            vertices[0].Position = new Vector3(0, 0, hexDepth);
            vertices[1].Position = new Vector3(HEX_WIDTH, 0, hexDepth);
            vertices[2].Position = new Vector3(HEX_WIDTH * 2, HEX_HEIGHT * 1.5f, hexDepth);

            vertices[3].Position = new Vector3(HEX_WIDTH, HEX_HEIGHT * 3, hexDepth);
            vertices[4].Position = new Vector3(0, HEX_HEIGHT * 3, hexDepth);
            vertices[5].Position = new Vector3(-HEX_WIDTH, HEX_HEIGHT * 1.5f, hexDepth);
            

            vertices[6].Position = new Vector3(0, 0, -1);
            vertices[7].Position = new Vector3(HEX_WIDTH, 0, -1);
            vertices[8].Position = new Vector3(HEX_WIDTH * 2, HEX_HEIGHT * 1.5f, -1);
            
            vertices[9].Position = new Vector3(HEX_WIDTH, HEX_HEIGHT * 3, -1);
            vertices[10].Position = new Vector3(0, HEX_HEIGHT * 3, -1);
            vertices[11].Position = new Vector3(-HEX_WIDTH, HEX_HEIGHT * 1.5f, -1);

            
        }


        private void SetUpIndicies()
        {
            triangleStripIndices = new short[] { 
                 6, 0, 7 , 1, 8, 2, 9, 3, 10, 4, 11, 5, 6, 0
                };

            lineListIndices = new short[] {
                // Top hex
                0,1,
                1,2,
                2,3,
                3,4,
                4,5,
                5,0,

                // Bottom hex
                6,7,
                7,8,
                8,9,
                9,10,
                10,11,
                11,6,

                // Verticals
                0,6,
                1,7,
                2,8,
                3,9,
                4,10,
                5,11,

                // Diagonals
                0,7,
                1,8,
                2,9,
                3,10,
                4,11,
                5,6
            };
        }



        // These are all quite similar to hexagon.
        public void Draw(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            effect.World = HexHelper.CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 12, triangleStripIndices, 0, 12);
        }

        public void DrawWireFrame(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            SetWireFrameColor();

            effect.World = HexHelper.CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 12, lineListIndices, 0, 24);

            SetDefaultColor();
        }

        private void SetDefaultColor()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = i % 3 == 0 ? Color.Tan : Color.MintCream;
            }
        }

        private void SetWireFrameColor()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = Color.Red;
            }
        }

        public float GetVertexDepth(int index)
        {
            return vertices[index].Position.Z;
        }

        public void SetVertexDepth(int index, float depth)
        {
            vertices[index].Position.Z = depth;
        }
    }
}
