using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    class Hexagon
    {
        const int DEFAULT_DEPTH = 0;
        const int HEX_WIDTH = 1;
        const int HEX_HEIGHT = 1;

        BoundingSphere mBoundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return mBoundingSphere; }
            set { mBoundingSphere = value; }
        }


        // The depth (z-value) of the hexagon.
        float fDepth;
        public float Depth
        {
            get { return fDepth; }
            set
            {
                fDepth = value;
                SetUpVerts(fDepth);
                mHexTube.Depth = value;

                mBoundingSphere.Center.Z = value;
            }
        }
        
        // The hexagons's base tube
        HexTube mHexTube;

        // 6 Verticies in a hexagon
        VertexPositionColor[] vertices = new VertexPositionColor[6];
        
        short[] triangleStripIndices;
        short[] lineListIndices;

        // The two Colors of the hexagon
        private Color mColor1;
        private Color mColor2;

        public Hexagon()
        {
            mColor1 = Color.Green;
            mColor2 = Color.DarkGreen;
            mHexTube = new HexTube();
            SetUpVerts(DEFAULT_DEPTH);
            SetUpIndicies();
            SetColorScheme(mColor1, mColor2);

            // Average the two sides of the hex to get the center of the sphere
            mBoundingSphere = new BoundingSphere(
                (vertices[2].Position + vertices[5].Position) / 2.0f,
                HEX_WIDTH);
        }


        //    4______3
        //    /      \
        //  5/        \2
        //   \        /
        //   0\______/1
        // Treats the bottom left corner of the hexagon's frame as 0,0.
        private void SetUpVerts(float depth)
        {
            vertices[0].Position = new Vector3(0, 0, depth);
            vertices[1].Position = new Vector3(HEX_WIDTH, 0, depth);
            vertices[2].Position = new Vector3(HEX_WIDTH * 2, HEX_HEIGHT * 1.5f, depth);

            vertices[3].Position = new Vector3(HEX_WIDTH, HEX_HEIGHT * 3, depth);
            vertices[4].Position = new Vector3(0, HEX_HEIGHT * 3, depth);
            vertices[5].Position = new Vector3(-HEX_WIDTH, HEX_HEIGHT * 1.5f, depth);
                        
        }
        
                  
        private void SetUpIndicies()
        {
            triangleStripIndices = new short[] { 
                 5, 0, 4, 1, 3, 2
                };

            lineListIndices = new short[] {
                0,1,
                1,2,
                2,3,
                3,4,
                4,5,
                5,0
            };
        }

        public void Draw(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            effect.World = CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 6, triangleStripIndices, 0, 4);
            mHexTube.Draw(device, effect, xOffset, yOffset);
        }

        public void DrawWireFrame(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            SetWireFrameColor();

            effect.World = CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 6, lineListIndices, 0, 6);
            mHexTube.DrawWireFrame(device, effect, xOffset, yOffset);

            SetColorScheme(mColor1, mColor2);
        }

        public void SetColorScheme(Color color1, Color color2)
        {
            mColor1 = color1;
            mColor2 = color2;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = i % 3 == 0 ? color1 : color2;
            }
        }

        private void SetWireFrameColor()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = Color.Red;
            }
        }

        private Matrix CreateTranslationForHexGrid(int xOffset, int yOffset)
        {            
            return Matrix.CreateTranslation(  xOffset * 2 , xOffset % 2 == 0 ? (yOffset * 3) - 1.5f : yOffset * 3, 0);
        }

        public float GetVertexDepth(int index)
        {
            return vertices[index].Position.Z;
        }

        public void SetVertexDepth(int index, float depth)
        {
            vertices[index].Position.Z = depth;
            mHexTube.SetVertexDepth(index, depth);
        }

        public void SetBoundingXY(int xOffset, int yOffset)
        {
            mBoundingSphere.Center.X = xOffset * 2;
            mBoundingSphere.Center.Y = xOffset % 2 == 0 ? (yOffset * 3) - 1.5f : yOffset * 3;
            mBoundingSphere.Center.Y += 1.5f;
        }
    }
}
