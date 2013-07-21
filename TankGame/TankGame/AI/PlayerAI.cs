using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankGame.Entities;
using Microsoft.Xna.Framework;

namespace TankGame.AI
{
    class PlayerAI
    {
        AStarSearch pathFinder;
        int gridsize;

        public void init(int griddim, Stone[] stones,Water[] waters )
        {
            Vector2[] stns=new Vector2[stones.Length + waters.Length];
            for (int i = 0; i < stones.Length; i++)
			    stns[i]=stones[i].Position;
            for (int i = 0; i < waters.Length; i++)
                stns[stones.Length + i] = waters[i].Position;

            this.gridsize = griddim;
            pathFinder=new AStarSearch(gridsize,stns);
        }

        public Vector3 getGreedyDirection(Brick[] bricks, Tank myplayer, List<CoinPile> coins, List<LifePack> lifes)
        {
            Vector3[] brks = new Vector3[bricks.Length];
            for (int i = 0; i < bricks.Length; i++){
                brks[i].X = bricks[i].Position.X;
                brks[i].Y = bricks[i].Position.Y;
                brks[i].Z = 4-bricks[i].Damage;
            }
            Vector3[] goals = new Vector3[coins.Count + lifes.Count];

            for (int j = 0; j < coins.Count; j++)
            {
                goals[j].X = coins[j].Position.X;
                goals[j].Y = coins[j].Position.Y;
                goals[j].Z = coins[j].Value;
            }
            for (int j = 0; j < lifes.Count; j++)
            {
                goals[coins.Count + j].X = lifes[j].Position.X;
                goals[coins.Count + j].Y = lifes[j].Position.Y;
                goals[coins.Count + j].Z = 1000;
            }

            return pathFinder.getNextDirection(brks,myplayer.Position,myplayer.Direction,goals);
        }

        public Vector2 getBestDirection(Brick[] bricks, Stone[] stones, Tank myplayer, Tank[] tanks, List<CoinPile> coins, List<LifePack> lifes)
        {
            Vector3 greedyDir=getGreedyDirection(bricks, myplayer, coins, lifes);
            Vector3 fightDir = getFightDirection(bricks, stones, myplayer, tanks);
            if ( fightDir.Z>0)
                return new Vector2(0, 0);
            if(greedyDir.Z>=fightDir.Z)
                return new Vector2(greedyDir.X, greedyDir.Y);
            else
                return new Vector2(0, 0);
        }

        public Vector3 getFightDirection( Brick[] bricks, Stone[] stones,Tank myplayer, Tank[] tanks)
        {
            int[,] map = new int[gridsize, gridsize];
            for (int i = 0; i < stones.Length; i++)
                map[(int)stones[i].Position.X, (int)stones[i].Position.Y] = 1;
            for (int i = 0; i < bricks.Length; i++)
                map[(int)bricks[i].Position.X, (int)bricks[i].Position.Y] = 1;
            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i] != null)
                    map[(int)tanks[i].Position.X, (int)tanks[i].Position.Y] = 5+i;
            }

            bool shoot = false;
            Vector2 cell = myplayer.Position;
            for (int i = 1; i < 4; i++)
            {
                cell = myplayer.Position + myplayer.Direction * i;
                if (cell.X < 0 || cell.Y < 0 || cell.X >= gridsize || cell.Y >= gridsize)
                    break;
                if (map[(int)cell.X, (int)cell.Y] == 1)
                    break;
                else if (map[(int)cell.X, (int)cell.Y] >= 5)
                {
                    shoot = true;
                    break;
                }
            }
            int profit=0;
            if (shoot)
            {
                Tank t = tanks[map[(int)cell.X, (int)cell.Y]-5];
                if (t.Health>0)
                profit = (int)(t.Coins/t.Health *2.5);
            }
            return new Vector3(0, 0, profit);
        }
    }
}
