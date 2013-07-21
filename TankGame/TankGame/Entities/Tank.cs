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
    class Tank: Entity
    {
        public string Name { get;  set; }
        public int Health { get; set; }
        public int Coins { get; set; }
        public int Points { get; set; }
        public bool Shot { get; set; }
        public Vector2 Direction { get; set; }

        public Tank(string name, Vector2 pos, Vector2 dir,int size): base(pos,size)
        {
           Name=name;
           Direction = dir;
           this.TextureSrc = new Rectangle(256, 0, 64, 64);
        }
        public Tank( int size): base(size)
        {
            this.TextureSrc = new Rectangle(256, 0, 64, 64);
        }

        public void update(string name, Vector2 pos, Vector2 dir, int health, int coins, int points, bool shot)
        {
            Name = name;
            Position = pos;
            Direction = dir;
            Health = health;
            Coins = coins;
            Points = points;
            Shot = shot;
        }
        public new void Draw(SpriteBatch spriteBatch, int orgx, int orgy)
        {
            int x = orgx + Size / 2 + (int)Position.X * Size;
            int y = orgy + Size / 2 + (int)Position.Y * Size;
            float rot = (float)Math.Atan2(Direction.Y, Direction.X) + (float)Math.PI / 2;
            spriteBatch.Draw(Brick.texture, new Rectangle(x, y, Size, Size), this.TextureSrc, Color.White,rot,new Vector2(32,32),SpriteEffects.None,1);
        }
    }
}
