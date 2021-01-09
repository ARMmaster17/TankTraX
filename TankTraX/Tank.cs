using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    public class Tank : Object3D
    {
        protected string tankName;
        private SpriteFont tankNameFont;

        protected List<Ball> balls;
        protected int fireCooldown;

        private int health;

        protected float speed;

        Model ballModel;

        public Tank() : base()
        {
            Random r = new Random();
            tankName = r.Next(100, 999).ToString();
            balls = new List<Ball>();
            fireCooldown = 0;
            health = 5;
            speed = 0.1f;
        }

        public virtual void Initialize(ContentManager Content)
        {
            tankNameFont = Content.Load<SpriteFont>("Fonts/Debug/DebugListing");

            ballModel = Content.Load<Model>("Models/ball");

            maxSpeed = 10f;

            base.Initialize(Content, "Models/tank");
        }

        public override void Update(GameTime gametime)
        {
            fireCooldown = (int)Math.Max(fireCooldown - gametime.ElapsedGameTime.TotalMilliseconds, 0);
            
            foreach (Ball ball in balls)
            {
                ball.Update(gametime);
            }

            for(int i = 0; i < balls.Count; i++)
            {
                if (balls[i].IsDead())
                {
                    balls.RemoveAt(i);
                    i--;
                }
            }
            
            base.Update(gametime);
        }

        public override void Draw2D(SpriteBatch spriteBatch, float cameraFOV, Vector3 cameraPosition, int viewportWidth, int viewportHeight)
        {
            spriteBatch.DrawString(tankNameFont, getHUDTag(), Get2DScreenDrawingLocation(cameraFOV, cameraPosition, viewportWidth, viewportHeight, 100), Color.Yellow);
        }

        public override void Draw3D(Matrix view, Matrix projection, GraphicsDevice graphics)
        {
            foreach (Ball ball in balls)
            {
                ball.Draw3D(view, projection, graphics);
            }
            
            base.Draw3D(view, projection, graphics);
        }

        public Vector3 GetLocation()
        {
            return location;
        }

        public string GetName()
        {
            return tankName;
        }

        protected void fireBall()
        {
            if (fireCooldown > 0) return;
            
            Ball newBall = new Ball(location, angleZ);
            newBall.Initialize(ballModel);
            balls.Add(newBall);

            fireCooldown += 500;
        }

        private string getHUDTag()
        {
            return tankName + " " + getHealthString() + " " + getSpeedString();
        }

        private string getHealthString()
        {
            string result = "[";
            for (int i = 1; i <= health; i++)
            {
                if (i <= health) result += "#";
                else result += "_";
            }
            result += "]";
            return result;
        }

        private string getSpeedString()
        {
            return velocity + " m/s";
        }
    }
}
