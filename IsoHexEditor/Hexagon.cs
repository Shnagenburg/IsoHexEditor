using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    /// <summary>
    /// A hexagon represents one tile of the map
    /// </summary>
    class Hexagon
    {        
        public const int DEFAULT_DEPTH = 0;
        public const int HEX_WIDTH = 1;
        public const int HEX_HEIGHT = 1;

        BoundingSphere mBoundingSphere;
        /// <summary>
        /// The bounding sphere is set at the center of the hex to check for mouse selection.
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return mBoundingSphere; }
            set { mBoundingSphere = value; }
        }
        
        float fDepth;
        /// <summary>
        /// The depth (z-value) of the hexagon. Positive numbers means a higher depth.
        /// </summary>
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
        
        // How the strip and line list are drawn
        short[] triangleStripIndices;
        short[] lineListIndices;

        // The two Colors of the hexagon
        // These will eventually become textures, I picked two for now just for testing.
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

            // Set the bounding sphere. It is shifted when
            // the user clicks (since the hex doens't know where 
            // it is) and when the depth is changed.
            mBoundingSphere = new BoundingSphere(new Vector3(0, 0, DEFAULT_DEPTH), HEX_WIDTH + 0.5f);
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

        /// <summary>
        /// Draws a hexagon
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="effect">The effect being used</param>
        /// <param name="xOffset">The x position the hex is at in the grid</param>
        /// <param name="yOffset">The y position the hex is at in the grid</param>
        public void Draw(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            effect.World = HexHelper.CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 6, triangleStripIndices, 0, 4);
            mHexTube.Draw(device, effect, xOffset, yOffset);
        }

        /// <summary>
        /// Draws a hexagon's wireframe. The wireframe is NOT all the actual triangles being drawn.
        /// It is an outline of the hexagon.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="effect">The effect being used</param>
        /// <param name="xOffset">The x position the hex is at in the grid</param>
        /// <param name="yOffset">The y position the hex is at in the grid</param>
        public void DrawWireFrame(GraphicsDevice device, BasicEffect effect, int xOffset, int yOffset)
        {
            SetWireFrameColor();

            effect.World = HexHelper.CreateTranslationForHexGrid(xOffset, yOffset);

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 6, lineListIndices, 0, 6);
            mHexTube.DrawWireFrame(device, effect, xOffset, yOffset);

            SetColorScheme(mColor1, mColor2);
        }

        /// <summary>
        /// Change the colors of the hexagon.
        /// </summary>
        public void SetColorScheme(Color color1, Color color2)
        {
            mColor1 = color1;
            mColor2 = color2;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = i % 3 == 0 ? color1 : color2;
            }
        }

        /// <summary>
        /// Change the color of the hexagon to whatever we decide a hex thats not selected looks like.
        /// Currently green
        /// </summary>
        public void SetDefaultColorScheme()
        {
            SetColorScheme(Color.Green, Color.DarkGreen);
        }

        /// <summary>
        /// Change the color of the hexagon to whatever we decide a hex thats selected looks like.
        /// Currently blue
        /// </summary>
        public void SetSelectedColorScheme()
        {
            SetColorScheme(Color.LightBlue, Color.Blue);
        }

        /// <summary>
        /// Change the color's of the hexagon's wireframe.
        /// </summary>
        private void SetWireFrameColor()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = Color.Red;
            }
        }


        /// <summary>
        /// Returns the depth of one of the hexagon's vertices.
        ///    4______3
        ///    /      \
        ///  5/        \2
        ///   \        /
        ///   0\______/1
        /// </summary>
        public float GetVertexDepth(int index)
        {
            return vertices[index].Position.Z;
        }

        /// <summary>
        /// Sets the depth for one of the hexagon's vertices.
        ///    4______3
        ///    /      \
        ///  5/        \2
        ///   \        /
        ///   0\______/1
        /// </summary>
        public void SetVertexDepth(int index, float depth)
        {
            vertices[index].Position.Z = depth;
            mHexTube.SetVertexDepth(index, depth);
        }

        /// <summary>
        /// Calculates the X and Y position of the bounding sphere based on
        /// the hexagon's position in the grid. REVISIT to remove magic numbers
        /// </summary>
        /// <param name="xOffset">The x position the hex is at in the grid</param>
        /// <param name="yOffset">The y position the hex is at in the grid</param>
        public void SetBoundingXY(int xOffset, int yOffset)
        {
            mBoundingSphere.Center.X = xOffset * 2;
            mBoundingSphere.Center.Y = xOffset % 2 == 0 ? (yOffset * 3) - 1.5f : yOffset * 3;
            mBoundingSphere.Center.Y += 1.5f;
        }
    }
}
