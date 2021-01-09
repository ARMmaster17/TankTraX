using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TankTraX
{
    public class GameView
    {
        private Vector3 cameraPosition;
        private Vector3 cameraTarget;

        private float fovAngle;
        private float aspectRatio;
        private float nearClippingDistance;
        private float farClippingDistance;

        private float cameraHeight;
        private float worldViewExtent;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private LocalPlayerTank tank1;
        private LocalPlayerTank tank2;

        List<Tank> tanks;

        private SpriteFont debugFont;
        
        public GameView(GraphicsDeviceManager graphics)
        {
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            cameraHeight = 60.0f;
            cameraPosition = new Vector3(0f, 0f, cameraHeight);
            cameraTarget = new Vector3(0f, 0f, 0f);
            float fovAngleDegrees = 45f;
            fovAngle = MathHelper.ToRadians(fovAngleDegrees);
            worldViewExtent = cameraHeight * (float)Math.Tan(fovAngleDegrees / 2f);
            aspectRatio = graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            nearClippingDistance = 0.01f;
            farClippingDistance = 100f; // Max render distance.
            worldMatrix = Matrix.CreateTranslation(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, nearClippingDistance, farClippingDistance);

            tanks = new List<Tank>();
            tanks.Add(new LocalPlayerTank());
            tanks.Add(new LocalPlayerTank(Keys.Up, Keys.Left, Keys.Right, Keys.Down));
        }

        public void Initialize(ContentManager Content)
        {
            debugFont = Content.Load<SpriteFont>("Fonts/Debug/DebugListing");
            
            foreach(Tank tank in tanks)
            {
                tank.Initialize(Content);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Tank tank in tanks)
            {
                tank.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDevice graphics)
        {
            graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            foreach (Tank tank in tanks)
            {
                tank.Draw3D(viewMatrix, projectionMatrix, graphics);
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(debugFont, (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(0, 0), Color.Yellow);
            
            for(int i = 0; i < tanks.Count; i++)
            {
                spriteBatch.DrawString(debugFont, "Tank " + tanks[i].GetName() + ": " + tanks[i].GetLocation().ToString(), new Vector2(0, 16 * (i + 1)), Color.Yellow);
            }

            foreach(Tank tank in tanks)
            {
                tank.Draw2D(spriteBatch, fovAngle, cameraPosition, graphics.DisplayMode.Width, graphics.DisplayMode.Height);
            }

            spriteBatch.End();

            graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }
    }
}
