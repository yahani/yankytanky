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
    class CoinPile:Entity
    {
        public int Life{get;set; }
        public int Value { get; set; }

        public CoinPile(Vector2 pos, int life, int val, int size)
            : base(pos, size)
        {
            Life = life;
            Value = val;
            this.TextureSrc = new Rectangle(64, 64, 64, 64);
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {

        }
    }
}
