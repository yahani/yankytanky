using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankGame.Entities
{
    class LifePack:Entity 
    {
        int life;

        public int Life
        {
            get { return life; }
            set { life = value; }
        }
        public LifePack(Vector2 pos,int life, int size)
            : base(pos, size)
        {
            Life = life;
            this.TextureSrc = new Rectangle(0, 64, 64, 64);
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {

        }
    }
}
