using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    /// <summary>
    /// Represents an object that can be represented in 3D space with XY acceleration, velocity, and translation.
    /// </summary>
    public class Object3D
    {
        protected Vector3 location;
        protected float velocity;
        protected float acceleration;
        protected float maxSpeed;
        protected float angleZ;

        private Model _model;

        /// <summary>
        /// Initializes an Object3D with all zero-ed out parameters.
        /// </summary>
        public Object3D()
        {
            location = new Vector3(0f, 0f, 0f);
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = 0f;
        }

        /// <summary>
        /// Initializes an Object3D.
        /// </summary>
        /// <param name="modelLocation">Starting location of 3D model.</param>
        public Object3D(Vector3 modelLocation)
        {
            location = modelLocation;
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = 0f;
        }

        /// <summary>
        /// Initializes an Object3D.
        /// </summary>
        /// <param name="modelLocation">Starting location of 3D model.</param>
        /// <param name="startingAngleZ">Starting angle of 3D model (look/acceleration).</param>
        public Object3D(Vector3 modelLocation, float startingAngleZ)
        {
            location = modelLocation;
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = startingAngleZ;
        }

        /// <summary>
        /// Loads the specified 3D model from the content store.
        /// </summary>
        /// <param name="Content">MonoGame content manager.</param>
        /// <param name="modelName">Path to 3D model file in content package.</param>
        public virtual void Initialize(ContentManager Content, string modelName)
        {
            _model = Content.Load<Model>(modelName);
        }

        /// <summary>
        /// Initializes the Object3D to use a pre-loaded 3D model.
        /// </summary>
        /// <param name="model">Pre-loaded 3D model.</param>
        public virtual void Initialize(Model model)
        {
            _model = model;
        }

        /// <summary>
        /// Updates the current object translation through current velocity and acceleration parameters.
        /// </summary>
        /// <param name="gameTime">Current GameTime measurement for time/step normalization.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (velocity + acceleration < maxSpeed && velocity + acceleration > (maxSpeed * -1))
            {
                velocity += acceleration;
            }
            else if (velocity < 0)
            {
                velocity = maxSpeed * -1;
            }
            else
            {
                velocity = maxSpeed;
            }

            MoveAngledDirection(velocity, gameTime);
        }

        /// <summary>
        /// Handles the drawing of 3D assets controlled by the Object3D.
        /// </summary>
        /// <param name="view">Globally-controlled view translation matrix.</param>
        /// <param name="projection">Globally-controlled projection translation matrix.</param>
        /// <param name="graphics">Active GraphicsDevice object to be drawn to.</param>
        public virtual void Draw3D(Matrix view, Matrix projection, GraphicsDevice graphics)
        {
            Matrix translationMatrix = Matrix.CreateRotationZ(angleZ) * Matrix.CreateTranslation(location);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = translationMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// Handles drawing of any 2D assets (fonts/sprites) controlled by the Object3D. Assumes spriteBatch.begin() has already been called and the depth buffer has been set.
        /// </summary>
        /// <param name="spriteBatch">Instance of helper class for drawing 2D assets.</param>
        /// <param name="cameraFOV">Current FOV angle of camera in degrees.</param>
        /// <param name="cameraPosition">Current position of camera in 3D world.</param>
        /// <param name="viewportWidth">Width of 3D viewport in pixels.</param>
        /// <param name="viewportHeight">Height of 3D viewport in pixels.</param>
        public virtual void Draw2D(SpriteBatch spriteBatch, float cameraFOV, Vector3 cameraPosition, int viewportWidth, int viewportHeight)
        {

        }

        /// <summary>
        /// Gets the obects XY location on the monitor (if the object is in view).
        /// </summary>
        /// <param name="cameraFOV">Camera field-of-view in degrees.</param>
        /// <param name="cameraPosition">Camera position in world.</param>
        /// <param name="viewportWidth">Width of 3D viewport.</param>
        /// <param name="viewportHeight">Height of 3D viewport.</param>
        /// <param name="yOffset">Offset to add to returned Y value.</param>
        /// <returns>2D location on screen of 3D object (or (-1,-1) if not in the field of view).</returns>
        protected Vector2 Get2DScreenDrawingLocation(float cameraFOV, Vector3 cameraPosition, int viewportWidth, int viewportHeight, int yOffset)
        {
            float worldViewExtent = cameraPosition.Z * (float)Math.Tan(cameraFOV / 2);

            bool outOfFOV = false;
            if (location.X < cameraPosition.X - worldViewExtent) outOfFOV = true;
            if (location.X > cameraPosition.X + worldViewExtent) outOfFOV = true;
            if (location.Y < cameraPosition.Y - worldViewExtent) outOfFOV = true;
            if (location.Y > cameraPosition.Y + worldViewExtent) outOfFOV = true;
            if (outOfFOV) return new Vector2(-1, -1);

            float x = ((location.X + worldViewExtent) * viewportWidth) / (worldViewExtent * 2);
            float y = ((location.Y + worldViewExtent) * viewportHeight) / (worldViewExtent * 2);

            return new Vector2(x, viewportHeight - y - yOffset);
        }

        /// <summary>
        /// Moves the Object3D in the currently-facing direction by the specified amount.
        /// </summary>
        /// <param name="velocity">Amount to move along the angleZ vector.</param>
        /// <param name="gameTime">Current GameTime object for time/step normalizing.</param>
        protected void MoveAngledDirection(float velocity, GameTime gameTime)
        {
            float timeAdjustedVelocity = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            location.X += (float)(timeAdjustedVelocity * Math.Cos(angleZ));
            location.Y += (float)(timeAdjustedVelocity * Math.Sin(angleZ));
        }
    }
}
