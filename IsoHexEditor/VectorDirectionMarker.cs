using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoHexEditor
{
    class VectorDirectionMarker
    {
        VertexPositionColor[] orthoVertices = new VertexPositionColor[6];
        VertexPositionColor[] letterXVertices = new VertexPositionColor[4];
        VertexPositionColor[] letterYVertices = new VertexPositionColor[4];
        VertexPositionColor[] letterZVertices = new VertexPositionColor[4];
        
        short[] orthoIndices;
        short[] letterXIndices;
        short[] letterYIndices;
        short[] letterZIndices;

        Matrix viewMatrix;
        Matrix projectionMatrix;

        // Tracks the yaw and pitch of the camera
        Quaternion quatRotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return quatRotation; }
            set { quatRotation = value; }
        }
        public VectorDirectionMarker(float aspectRatio)
        {
            SetUpVertices();
            SetUpIndices();
            SetUpCamera(aspectRatio);
        }

        private void SetUpVertices()
        {
            orthoVertices[0].Position = new Vector3(0, 0, 0);
            orthoVertices[1].Position = new Vector3(1, 0, 0);

            orthoVertices[2].Position = new Vector3(0, 0, 0);
            orthoVertices[3].Position = new Vector3(0, 1, 0);
            
            orthoVertices[4].Position = new Vector3(0, 0, 0);
            orthoVertices[5].Position = new Vector3(0, 0, 1);

            for (int i = 0; i < orthoVertices.Length; i++)
            {
                if (i <= 1)
                    orthoVertices[i].Color = Color.Red;
                else if (i <= 3)
                    orthoVertices[i].Color = Color.Green;
                else
                    orthoVertices[i].Color = Color.Blue;
            
            }

            letterXVertices[0].Position = new Vector3(1, -0.125f, -0.125f);
            letterXVertices[1].Position = new Vector3(1, 0.125f, -0.125f);
            letterXVertices[2].Position = new Vector3(1, -0.125f, 0.125f);
            letterXVertices[3].Position = new Vector3(1, 0.125f, 0.125f);

            for(int i = 0; i < letterXVertices.Length; i ++)
            {
                letterXVertices[i].Color = Color.Red;
            }

            letterYVertices[0].Position = new Vector3(0, 1, -0.125f);
            letterYVertices[1].Position = new Vector3(0, 1, 0);
            letterYVertices[2].Position = new Vector3(-0.125f, 1, 0.125f);
            letterYVertices[3].Position = new Vector3(0.125f, 1, 0.125f);

            for (int i = 0; i < letterXVertices.Length; i++)
            {
                letterYVertices[i].Color = Color.Green;
            }

            letterZVertices[0].Position = new Vector3(-0.125f, -0.125f, 1);
            letterZVertices[1].Position = new Vector3(0.125f, -0.125f, 1);
            letterZVertices[2].Position = new Vector3(-0.125f, 0.125f, 1);
            letterZVertices[3].Position = new Vector3(0.125f, 0.125f, 1);

            for (int i = 0; i < letterXVertices.Length; i++)
            {
                letterZVertices[i].Color = Color.Blue;
            }
        }

        private void SetUpIndices()
        {
            orthoIndices = new short[6] { 0, 1, 2, 3, 4, 5 };

            letterXIndices = new short[4] { 0, 3, 1, 2 };
            letterYIndices = new short[6] { 0, 1, 1, 2, 1, 3 };
            letterZIndices = new short[6] { 2, 3, 3, 0, 0, 1 };
        }

        private void SetUpCamera(float aspectRatio)
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(0,0,5), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspectRatio, 1.0f, 300.0f);
        }

        // REVISIT This has magic numbers based on the size of the rendering area, need to fix.
        public void Draw(GraphicsDevice device, BasicEffect effect)
        {            
            effect.View = viewMatrix;
            effect.World = Matrix.CreateFromQuaternion(Quaternion.Inverse( quatRotation )) * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(new Vector3(-2.75f,-1.5f,0) );

            effect.CurrentTechnique.Passes[0].Apply();

            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, orthoVertices, 0, 6, orthoIndices, 0, 3);

            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, letterXVertices, 0, 4, letterXIndices, 0, 2);

            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, letterYVertices, 0, 4, letterYIndices, 0, 3);

            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, letterZVertices, 0, 4, letterZIndices, 0, 3);
                        
        }
    }
}
