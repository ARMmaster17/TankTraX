using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankTraX
{
    public class Tank
    {
        private Model model;
        protected Vector3 location;
        protected float angleZ;

        public Tank()
        {

        }

        public virtual void Initialize(ContentManager Content)
        {
            model = Content.Load<Model>("Models/tank");
            location = new Vector3(0f, 0f, 0f);
            angleZ = 0f;
        }

        public virtual void Update()
        {
            
        }

        public virtual void Draw(Matrix view, Matrix projection, GraphicsDevice graphics)
        {
            Matrix translationMatrix = Matrix.CreateRotationZ(angleZ) * Matrix.CreateTranslation(location);

            graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            foreach (ModelMesh mesh in model.Meshes)
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
    }
}
