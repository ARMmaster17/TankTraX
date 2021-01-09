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

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private LocalPlayerTank tank1;
        private LocalPlayerTank tank2;

        private SpriteFont debugFont;
        
        public GameView(GraphicsDeviceManager graphics)
        {
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            cameraPosition = new Vector3(0f, 0f, 60.0f);
            cameraTarget = new Vector3(0f, 0f, 0f);
            fovAngle = MathHelper.ToRadians(45);
            aspectRatio = graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            nearClippingDistance = 0.01f;
            farClippingDistance = 100f; // Max render distance.
            worldMatrix = Matrix.CreateTranslation(0f, 0f, 0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, nearClippingDistance, farClippingDistance);

            tank1 = new LocalPlayerTank();
            tank2 = new LocalPlayerTank(Keys.Up, Keys.Left, Keys.Right, Keys.Down);
        }

        public void Initialize(ContentManager Content)
        {
            debugFont = Content.Load<SpriteFont>("Fonts/Debug/DebugListing");
            
            tank1.Initialize(Content);
            tank2.Initialize(Content);
        }

        public void Update()
        {
            tank1.Update();
            tank2.Update();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDevice graphics)
        {
            tank1.Draw(viewMatrix, projectionMatrix, graphics);
            tank2.Draw(viewMatrix, projectionMatrix, graphics);

            spriteBatch.Begin();

            spriteBatch.DrawString(debugFont, (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(0, 0), Color.Yellow);

            spriteBatch.End();

            graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }
    }
}
