using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    public class Ball : Object3D
    {
        private float speed = 10f;
        private int TTLMS = 2000;

        private float liveTime;
        private bool isDead;
        
        public Ball(Vector3 startingLocation, float launchAngle) : base(startingLocation, launchAngle)
        {
            liveTime = 0f;
            isDead = false;
        }

        public override void Update(GameTime gameTime)
        {
            liveTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (liveTime > TTLMS) isDead = true;

            MoveAngledDirection(speed, gameTime);

            base.Update(gameTime);
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}
