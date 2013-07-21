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
    class AStarSearch
    {

        int size;
        AStarCell[,] asgrid;
        AStarCell scell;
        AStarCell[] goals;
        Vector2[] stones;
        int[] dir = { 0, -1, -1, 0, 1, 0, 0, 1 };

        public AStarSearch(int gridsize, Vector2[] stones)
        {
            size = gridsize;
            asgrid = new AStarCell[size, size];
            this.stones = stones;
        }

        public Vector3 getNextDirection(Vector3[] bricks, Vector2 startpos, Vector2 startdir, Vector3[] coins)
        {
            // Initialize the AStar grid
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    asgrid[i, j] = new AStarCell(new Vector2(i, j));
                }
            }

            // Set stones and bricks locations
            for (int i = 0; i < stones.Length; i++) asgrid[(int)stones[i].X, (int)stones[i].Y].setType(2, 100);
            for (int i = 0; i < bricks.Length; i++) asgrid[(int)bricks[i].X, (int)bricks[i].Y].setType(1, (int)bricks[i].Z);

            // Set coin locations
            goals = new AStarCell[coins.Length];
            for (int i = 0; i < coins.Length; i++)
            {
                goals[i] = asgrid[(int)coins[i].X, (int)coins[i].Y];
                goals[i].profit = (int)coins[i].Z;
            }

            // Start cell
            scell = asgrid[(int)startpos.X, (int)startpos.Y];
            scell.dir = startdir;

            SimulateAStar();
            return AStarMove();
        }


        private void SimulateAStar()
        {
            List<AStarCell> list = new List<AStarCell>();
            list.Add(scell);
            bool done = false;

            while (list.Count > 0 && !done)
            {
                AStarCell ccell = list.Min();
                list.Remove(ccell);
                ccell.finished = true;
                for (int i = 0; i < 4; i++)     // foe each cell arround
                {
                    int x = (int)ccell.pos.X + dir[i * 2], y = (int)ccell.pos.Y + dir[i * 2 + 1];
                    if (x >= 0 && x < size && y >= 0 && y < size && !asgrid[x, y].finished && asgrid[x, y].cost < 10)
                    {
                        if (asgrid[x, y].findF(ccell, goals))
                        {
                            list.Add(asgrid[x, y]);
                        }
                        if (goals.Contains(asgrid[x, y])) { done = true; break; }   // goal reached
                    }
                }
            }
        }

        private Vector3 AStarMove()
        {
            int profit = 0;
            AStarCell prnt = scell, child = scell;
            foreach (AStarCell d in goals)
            {
                if (d.parent != null)
                {
                    profit = d.profit;
                    prnt = d;
                    while (!prnt.Equals(scell))
                    {
                        child = prnt;
                        prnt = prnt.parent;
                    }
                }
            }

            Vector3 dir=new Vector3( child.pos.X - prnt.pos.X,child.pos.Y - prnt.pos.Y,profit);
            return dir;
        }
    }
}
