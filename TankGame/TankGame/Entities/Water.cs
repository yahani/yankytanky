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
    class Water:Entity
    {
        public Water(Vector2 pos, int size)
            : base(pos, size)
        {
            this.TextureSrc = new Rectangle(0, 0, 64, 64);
        }

    }
}
