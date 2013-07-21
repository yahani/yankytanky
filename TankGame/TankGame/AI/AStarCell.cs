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

namespace TankGame.AI
{
    class AStarCell : IComparable<AStarCell>
    {
        public Vector2 pos, dir;
        public int g, f = 9999;
        public int profit, cost = 1;
        public int type;
        public AStarCell parent;
        public Boolean finished;

        public AStarCell(Vector2 pos)
        {
            this.pos = pos;
        }
        public void setValues(Vector2 dir, int g, int f)
        {
            this.dir = dir;
            this.g = g;
            this.f = f;
        }
        public void setType(int type, int life)
        {
            this.cost = life + 1;
            if (type == 2) this.cost = 100;
            this.type = type;
        }
        public Boolean findF(AStarCell src, AStarCell[] goals)
        {
            Vector2 indir = pos - src.pos;
            int gg, h = 0;
            Boolean updated = false;

            foreach (AStarCell dest in goals)
            {
                Vector2 dist = dest.pos - pos;
                if (src.dir.X == indir.X && src.dir.Y == indir.Y)
                    gg = src.g + cost;
                else gg = src.g + cost + 1;

                if (Math.Abs(dist.X * dist.Y) > 0)
                    h = (int)Math.Abs(dist.X) + (int)Math.Abs(dist.Y) + 1;
                else h = (int)Math.Abs(dist.X) + (int)Math.Abs(dist.Y);

                if (indir.X * dist.X <= 0 && indir.Y * dist.Y <= 0)
                    h = h + 1;

                if (f > gg + h)
                {
                    f = gg + h;
                    g = gg;
                    dir = indir;
                    profit = dest.profit;
                    parent = src;
                    updated = true;
                }
            }
            return updated;
        }

        #region IComparable<AStarCell> Members

        int IComparable<AStarCell>.CompareTo(AStarCell other)
        {
            return this.f.CompareTo(other.f);
        }

        #endregion
    }

}
