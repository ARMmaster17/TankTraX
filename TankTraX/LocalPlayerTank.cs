using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    public class LocalPlayerTank : Tank
    {
        Keys forwardKey;
        Keys leftKey;
        Keys rightKey;
        Keys backwardsKey;
        
        public LocalPlayerTank() : base()
        {
            forwardKey = Keys.W;
            leftKey = Keys.A;
            rightKey = Keys.D;
            backwardsKey = Keys.S;
        }

        public LocalPlayerTank(Keys forward, Keys left, Keys right, Keys backwards) : base()
        {
            forwardKey = forward;
            leftKey = left;
            rightKey = right;
            backwardsKey = backwards;
        }

        public override void Initialize(ContentManager Content)
        {
            base.Initialize(Content);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(leftKey)) angleZ += 0.03f;
            if (state.IsKeyDown(rightKey)) angleZ -= 0.03f;

            angleZ %= 360f;

            if (state.IsKeyDown(forwardKey)) acceleration = speed;
            else if (state.IsKeyDown(backwardsKey)) acceleration = speed * -1;
            else acceleration = 0;

            if (state.IsKeyDown(Keys.Space))
            {
                fireBall();
            }

            base.Update(gameTime);
        }
    }
}
