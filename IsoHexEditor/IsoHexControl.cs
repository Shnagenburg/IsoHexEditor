#region Using Statements
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion


namespace IsoHexEditor
{
    class IsoHexControl : GraphicsDeviceControl
    {
        VectorDirectionMarker vectorDirMarker;
        HexTube hexTube;
        HexGrid hexGrid;
        public HexGrid HexGrid
        {
            get {return hexGrid;}
            set {hexGrid = value;}
        }
        Hexagon hex;
        BasicEffect effect;
        Stopwatch timer;
        Camera camera;

        public bool IsDrawingWireFrame;
        public bool IsDrawingModels;

        

        // Vertex positions and colors used to display a spinning triangle.
        public readonly VertexPositionColor[] Vertices =
        {
            new VertexPositionColor(new Vector3(-1, -1, -1), Color.Black),
            new VertexPositionColor(new Vector3( 1, -1, -1), Color.Black),
            new VertexPositionColor(new Vector3( 0,  1, -1), Color.Black),
        };


        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            IsDrawingModels = true;
            IsDrawingWireFrame = true;

            // Create a Vector Direction Marker
            vectorDirMarker = new VectorDirectionMarker(GraphicsDevice.Viewport.AspectRatio);

            // Create a hextube
            hexTube = new HexTube();

            // Create a hexgrid
            hexGrid = new HexGrid();
            HexHelper.GenerateNoise(hexGrid, 0, 20, 5);
            //hexGrid.SmoothHexesDown();
            //hexGrid.SetColorSchemeAll(Color.DarkSeaGreen, Color.DarkOrchid);
            hexGrid.ClusterColor(1, 4);

            // Create a hex
            hex = new Hexagon();

            // Create our camera.
            camera = new Camera(GraphicsDevice.Viewport.AspectRatio);

            // Create our effect.
            effect = new BasicEffect(GraphicsDevice);

            effect.VertexColorEnabled = true;

            // Start the animation timer.
            timer = Stopwatch.StartNew();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        protected void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            camera.Update(timer, currentMouseState);
            vectorDirMarker.Rotation = camera.Rotation;
            timer.Restart();
            
            
        }

        private void HandleInput()
        {

        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            Update();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Spin the triangle according to how much time has passed.
            float time = (float)timer.Elapsed.TotalSeconds;

            //float yaw = time * 0.7f;
            //float pitch = time * 0.8f;
            //float roll = time * 0.9f;

            // Set transform matrices.
            float aspect = GraphicsDevice.Viewport.AspectRatio;

            effect.World = Matrix.Identity; //Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

            //effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5),
            //                                Vector3.Zero, Vector3.Up);
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            // Set renderstates.
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                              Vertices, 0, 1);
             
            if (IsDrawingModels)
                hexGrid.Draw(GraphicsDevice, effect);

            if (IsDrawingWireFrame)
                hexGrid.DrawWireFrame(GraphicsDevice, effect);

            vectorDirMarker.Draw(GraphicsDevice, effect);

            //hex.Draw(GraphicsDevice, effect, 0, 0);
            //hex.DrawWireFrame(GraphicsDevice, effect, 0, 0);

            //hexTube.Draw(GraphicsDevice, effect, 0, 0);
            //hexTube.DrawWireFrame(GraphicsDevice, effect, 0, 0);
        }
    }
}
