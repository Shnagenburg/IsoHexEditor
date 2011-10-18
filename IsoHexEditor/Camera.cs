
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Made by Dan Fuller
namespace IsoHexEditor
{
    class Camera
    {
        // Move speed and turn speed of the camera
        const int MOVE_SPEED = 1;
        const float TURN_SPEED = 0.5f;

        // Tracks the yaw and pitch of the camera
        Quaternion quatRotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return quatRotation; }
            set { quatRotation = value; }
        }

        // Where the camera is
        Vector3 cameraPosition;

        // The inital direction of the camera
        Vector3 cameraOriginalTarget;

        // What is initally up
        Vector3 cameraOriginalUpVector;

        // a point in front of the camera
        Vector3 cameraLookAtPt;

        MouseState originalMouseState;
        MouseState lastMouseState;

        // Position of the Camera, where it's looking, which way is up.
        private Matrix mViewMatrix;        
        public Matrix ViewMatrix
        {
            get {return mViewMatrix;}
            set {value = mViewMatrix;}
        }       

        // Angle of the lens, aspect ratio, near clip and far clip.
        private Matrix mProjectionMatrix;
        public Matrix ProjectionMatrix
        {
            get { return mProjectionMatrix; }
            set { value = mProjectionMatrix; }
        }    

        /// <summary>
        /// Creates a new Camera to view the map
        /// </summary>
        /// <param name="aspectRatio">The camera needs to know the aspect ratio from the graphics device.</param>
        public Camera(float aspectRatio)
        {
            //Mouse.SetPosition(viewWidth, viewHeight);
            originalMouseState = Mouse.GetState();

            cameraPosition = new Vector3(0.0f, 0.0f, 5.0f);

            cameraOriginalTarget = Vector3.Forward;
            cameraOriginalUpVector = Vector3.Up;
            mViewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            mProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspectRatio, 1.0f, 300.0f);

        }

        // Update the camera
        public void Update(Stopwatch theTime, MouseState currentMouseState)
        {
            float time = (float)theTime.ElapsedTicks / 100000.0f;
            ProcessInput(time, currentMouseState);            
        }
        
        private void ProcessInput(float amount, MouseState currentMouseState)
        {
            // Reset the vector and rotation
            Vector3 moveVector = Vector3.Zero;
            float leftRightRot = 0;
            float upDownRot = 0;


            // This is when the user first clicks middle mouse, indicating they want to move the camera.
            if (currentMouseState.MiddleButton == ButtonState.Pressed
                && lastMouseState.MiddleButton == ButtonState.Released)
            {
                originalMouseState = currentMouseState;
                
            }

            // Handles if the user is moving the mouse
            if (currentMouseState != originalMouseState && currentMouseState.MiddleButton == ButtonState.Pressed)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftRightRot -= TURN_SPEED * xDifference * .01f; //* amount;
                upDownRot -= TURN_SPEED * yDifference * .01f; //* amount;
                Mouse.SetPosition(originalMouseState.X, originalMouseState.Y);
            }

            // Handles if the user is using the keyboard to move
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z) || keyState.IsKeyDown(Keys.LeftShift))
                moveVector += new Vector3(0, -1, 0);
           
            // Take our move and rotation and apply it to the camera.
            AddToCameraPosition(moveVector * amount, leftRightRot, upDownRot);

            lastMouseState = currentMouseState;
        }

        // Calculate the camera's new position
        private void AddToCameraPosition(Vector3 vectorToAdd, float leftRightRot, float upDownRot)
        {
            Quaternion additionalRot = Quaternion.CreateFromYawPitchRoll(leftRightRot, upDownRot, 0.0f);
            //* Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
            quatRotation *= additionalRot;


            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, Matrix.CreateFromQuaternion(quatRotation));
            cameraPosition += MOVE_SPEED * rotatedVector;

            UpdateViewMatrix();
        }

        // Update the view matrix based on the camera's new position
        private void UpdateViewMatrix()
        {
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, Matrix.CreateFromQuaternion(quatRotation));
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, Matrix.CreateFromQuaternion(quatRotation));
            cameraLookAtPt = cameraFinalTarget;
            mViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        

    }
}
