using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    public class Object3D
    {
        protected Vector3 location;
        protected float velocity;
        protected float acceleration;
        protected float maxSpeed;
        protected float angleZ;

        private Model _model;

        public Object3D()
        {
            location = new Vector3(0f, 0f, 0f);
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = 0f;
        }

        public Object3D(Vector3 modelLocation)
        {
            location = modelLocation;
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = 0f;
        }

        public Object3D(Vector3 modelLocation, float startingAngleZ)
        {
            location = modelLocation;
            velocity = 0f;
            acceleration = 0f;
            maxSpeed = 0f;
            angleZ = startingAngleZ;
        }

        public virtual void Initialize(ContentManager Content, string modelName)
        {
            _model = Content.Load<Model>(modelName);
        }

        public virtual void Initialize(Model model)
        {
            _model = model;
        }

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

        public virtual void Draw2D(SpriteBatch spriteBatch, float cameraFOV, Vector3 cameraPosition, int viewportWidth, int viewportHeight)
        {

        }

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

        protected void MoveAngledDirection(float velocity, GameTime gameTime)
        {
            float timeAdjustedVelocity = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            location.X += (float)(timeAdjustedVelocity * Math.Cos(angleZ));
            location.Y += (float)(timeAdjustedVelocity * Math.Sin(angleZ));
        }
    }
}
