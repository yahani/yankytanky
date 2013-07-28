using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Text;

namespace TankGame.Entities
{
    public class Button : DrawableGameComponent
    {
        public Button(Game1 game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            position = Vector2.Zero;
            scale = Vector2.One;
            
        }

        public SpriteBatch spriteBatch{get;set;}
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public Vector2 origin { get; set; }
        public Vector2 scale { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
