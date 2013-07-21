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
    class Brick : Entity
    {
        int damage;

        public Brick(Vector2 pos, int size, int dmg)
            : base(pos, size)
        {
            Damage = dmg;
            this.TextureSrc = new Rectangle(128, 0, 64, 64);
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public new void Draw(SpriteBatch spriteBatch, int orgx, int orgy)
        {
            int x = orgx + (int)Position.X * Size;
            int y = orgy + (int)Position.Y * Size;
            spriteBatch.Draw(Brick.texture, new Rectangle(x, y, Size, Size), this.TextureSrc, Color.White);
        }
    }
}
