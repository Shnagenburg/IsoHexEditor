#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SysWinForms = System.Windows.Forms;
#endregion


namespace IsoHexEditor
{
    /// <summary>
    /// This is the window that holds our XNA stuff.
    /// </summary>
    class IsoHexControl : GraphicsDeviceControl
    {
        // Links to form controls
        public System.Windows.Forms.TextBox tbDepth;

        // The top right point of where the editor window is
        public Point WindowLocation;

        VectorDirectionMarker vectorDirMarker;
        HexTube hexTube;
        HexGrid hexGrid;
        public HexGrid HexGrid
        {
            get {return hexGrid;}
            set {hexGrid = value;}
        }

        private Hexagon mSelectedHex;
        public Hexagon SelectedHex
        {
            get { return mSelectedHex; }
            set { mSelectedHex = value; }
        }

        BasicEffect effect;
        Stopwatch timer;
        Camera camera;

        public bool IsDrawingWireFrame;
        public bool IsDrawingModels;
        
        MouseState mPreviousMouseState;

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
            mPreviousMouseState = Mouse.GetState();

            IsDrawingModels = true;
            IsDrawingWireFrame = true;

            // Create a Vector Direction Marker
            vectorDirMarker = new VectorDirectionMarker(GraphicsDevice.Viewport.AspectRatio);

            // Create a hextube
            hexTube = new HexTube();

            // Create a hexgrid
            hexGrid = new HexGrid();
            HexHelper.GenerateNoise(hexGrid, 0, 3);
            //hexGrid.SmoothHexesDown();
            //hexGrid.SetColorSchemeAll(Color.DarkSeaGreen, Color.DarkOrchid);
            hexGrid.ClusterColor(1, 4, Color.Red, Color.Pink);
            
            // Create our camera.
            camera = new Camera(GraphicsDevice.Viewport.AspectRatio);

            // Create our effect.
            effect = new BasicEffect(GraphicsDevice);

            // Enable to allow color
            effect.VertexColorEnabled = true;

            // Start the animation timer.
            timer = Stopwatch.StartNew();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        protected void Update()
        {
            // The mouse state gets the mouse location based on where the mouse is on the desktop.
            // We need to offset it by where the window is. How crazy is that?
            MouseState currentMouseState = Mouse.GetState();

            int fixedX = currentMouseState.X - WindowLocation.X;
            int fixedY = currentMouseState.Y - WindowLocation.Y;

            // The camera works by examining the changes in mouse location, so it doesnt 
            // need to be offset here.
            camera.Update(timer, currentMouseState, mPreviousMouseState);

            // Check to see if we clicked on a hex
            if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Hexagon newHex = HexHelper.SelectHexagon(GraphicsDevice, hexGrid, fixedX, fixedY, camera.ViewMatrix, effect.Projection, camera.Position);
                if (newHex != null)
                    HexSelected(newHex);
            }
            
            // Update the orthomarker
            vectorDirMarker.Rotation = camera.Rotation;

            // Reset the timer
            timer.Restart();

            mPreviousMouseState = currentMouseState;
            
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            Update();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            // Set renderstates.
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                              Vertices, 0, 1);

            HexHelper.DrawRay(GraphicsDevice, effect);
             
            if (IsDrawingModels)
                hexGrid.Draw(GraphicsDevice, effect);

            if (IsDrawingWireFrame)
                hexGrid.DrawWireFrame(GraphicsDevice, effect);

            vectorDirMarker.Draw(GraphicsDevice, effect);

            //hexTube.Draw(GraphicsDevice, effect, 0, 0);
            //hexTube.DrawWireFrame(GraphicsDevice, effect, 0, 0);

            
        }

        private void HexSelected(Hexagon newHex)
        {
            if (mSelectedHex != null)
                mSelectedHex.SetDefaultColorScheme();
            newHex.SetSelectedColorScheme();
            mSelectedHex = newHex;

            tbDepth.Text = mSelectedHex.Depth.ToString();
        }
        
    }
}
