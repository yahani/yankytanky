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
    class Entity
    {
        public Vector2 Position { get; set; }
        public int Size { get; set; }
        public Rectangle TextureSrc { get; set; }
        public static Texture2D texture;

        public Entity(Vector2 pos, int size)
        {
           Position= pos;
           Size = size;
        }
        public Entity( int size)
        {
            Size = size;
            this.TextureSrc = new Rectangle(64, 0, 64, 64);
        }

        public void Draw(SpriteBatch spriteBatch, int orgx, int orgy)
        {
            int x = orgx + (int)Position.X * Size;
            int y = orgy + (int)Position.Y * Size;
            spriteBatch.Draw(Brick.texture, new Rectangle(x, y, Size, Size), this.TextureSrc, Color.White);
        }

        public static void LoadTexture(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites");
           
        }
    }
}
