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
using TankGame.Entities;

namespace TankGame
{
    class Decoder
    {

        Scene mainscn;
        int size;

        public Decoder(Scene scn, int size)
        {
            mainscn = scn;
            this.size = size;
        }

        public int decode(string str)
        {
            str = str.TrimEnd(new char[] { '#' });
            //// --------------player start hack
            if (str.Substring(0, 1).Equals("S")) str=str.Replace(';', ':');
            //// --------------

            string[] tokens = str.Split(new char[] { ':' });

            if (tokens[0].Equals("I")) { decodeInit(tokens); return 1; }
            else if (tokens[0].Equals("S")){ decodePlayer(tokens); return 2; }
            else if (tokens[0].Equals("G")) {decodeUpdate(tokens); return 3; }
            else if (tokens[0].Equals("C")) {decodeCoins(tokens); return 4; }
            else if (tokens[0].Equals("L")) { decodeLifePack(tokens); return 5; }
            else { return 0; }
        }

        private void decodeInit(string[] tokens)
        {
            Vector3[] brks = decodeXYZ(tokens[2], 0);
            Vector2[] stn = decodeXY(tokens[3]);
            Vector2[] wtr = decodeXY(tokens[4]);

            mainscn.Bricks = new Brick[brks.Length];
            mainscn.Stones = new Stone[stn.Length];
            mainscn.Waters = new Water[wtr.Length];

            for (int i = 0; i < brks.Length; i++)
                mainscn.Bricks[i] = new Brick(new Vector2(brks[i].X, brks[i].Y), size, 0);
            for (int i = 0; i < stn.Length; i++)
                mainscn.Stones[i] = new Stone(stn[i], size);
            for (int i = 0; i < wtr.Length; i++)
                mainscn.Waters[i] = new Water(wtr[i], size);

            mainscn.LifePacks = new List<LifePack>();
            mainscn.CoinPiles = new List<CoinPile>();
        }

        private void decodePlayer(string[] tokens)
        {
            mainscn.Tanks = new Tank[5];
            string pname = tokens[1];
            int i = int.Parse(pname.Substring(1));
            int dir = int.Parse(tokens[3].TrimEnd(new char[] { '#' }));

            mainscn.Tanks[i] = new Tank(pname, decodeXY(tokens[2])[0], new Vector2((2 - dir) % 2, (dir - 1) % 2), size);
            mainscn.Myplayer = mainscn.Tanks[i];
        }

        private void decodeUpdate(string[] tokens)
        {
            int pcount=tokens.Length-2;
            if (mainscn.Tanks == null) { mainscn.Tanks = new Tank[5]; }

            for (int i = 1; i <= pcount; i++)
            {
                string[] player = tokens[i].Split(new char[] { ';' });
                Tank p;
                if (mainscn.Tanks[i - 1] == null)
                    p = mainscn.Tanks[i - 1] = new Tank(size);
                else p = mainscn.Tanks[i - 1];

                int dir = int.Parse(player[2]);
                p.update(player[0], decodeXY(player[1])[0], new Vector2((2 - dir) % 2, (dir - 1) % 2),
                    int.Parse(player[4]), int.Parse(player[5]), int.Parse(player[6]), player[3].Equals("1"));
            }

            Vector3[] brks = decodeXYZ(tokens[pcount+1], 4);
            for (int i = 0; i < brks.Length; i++)
                mainscn.Bricks[i].Damage = (int)brks[i].Z;

            updateCoinsLpacks();
        }

        private void decodeCoins(string[] tokens)
        {
            CoinPile coin = new CoinPile(decodeXY(tokens[1])[0],int.Parse(tokens[2]),int.Parse(tokens[3].TrimEnd(new char[] { '#' })),size);
            mainscn.CoinPiles.Add(coin);
        }

        private void decodeLifePack(string[] tokens)
        {
            LifePack lifePack = new LifePack(decodeXY(tokens[1])[0],int.Parse(tokens[2].TrimEnd(new char[] { '#' })),size);
            mainscn.LifePacks.Add(lifePack);
        }

        private Vector2[] decodeXY(string str)
        {
            str = str.TrimEnd(new char[] { '#' });
            string[] pos = str.Split(new char[] { ';' });
            Vector2[] list = new Vector2[pos.Length];

            for (int i = 0; i < pos.Length; i++)
            {
                string[] xys = pos[i].Split(new char[] { ',' });
                list[i] = new Vector2(int.Parse(xys[0]), int.Parse(xys[1]));
            }
            return list;
        }

        private Vector3[] decodeXYZ(string str, int defaultval)
        {
            str = str.TrimEnd(new char[] { '#' });
            string[] pos = str.Split(new char[] { ';' });
            Vector3[] list = new Vector3[pos.Length];

            for (int i = 0; i < pos.Length; i++)
            {
                string[] xys = pos[i].Split(new char[] { ',' });
                int z = defaultval;
                if (xys.Length == 3) z = int.Parse(xys[2]);
                list[i] = new Vector3(int.Parse(xys[0]), int.Parse(xys[1]), z);
            }
            return list;
        }

        private void updateCoinsLpacks()
        {
            foreach (Tank tnk in mainscn.Tanks)
            {
                if (tnk == null) continue;
                bool collide = false;
                foreach (CoinPile cpl in mainscn.CoinPiles)
                    if (tnk.Position.X == cpl.Position.X && tnk.Position.Y == cpl.Position.Y)
                    {
                        mainscn.CoinPiles.Remove(cpl);
                        collide = true;  break;
                    }
                if (collide) break;
                foreach (LifePack lpk in mainscn.LifePacks)
                    if (tnk.Position.X == lpk.Position.X && tnk.Position.Y == lpk.Position.Y)
                    {
                        mainscn.LifePacks.Remove(lpk);
                        break;
                    }
            }

            // reduce coins life
            if (mainscn.CoinPiles != null)
                foreach (CoinPile cpl in mainscn.CoinPiles)
                {
                    if (cpl.Life == 0)
                        mainscn.CoinPiles.Remove(cpl);
                    else
                        cpl.Life--;
                }
            // reduce Lifepack life
            if (mainscn.LifePacks != null)
                foreach (LifePack lpk in mainscn.LifePacks)
                {
                    if (lpk.Life == 0)
                        mainscn.LifePacks.Remove(lpk);
                    else
                        lpk.Life--;
                }
        }

        public string decodeDir(Vector2 dir)
        {
            if (dir.X == 1) return "RIGHT";
            else if (dir.X == -1) return "LEFT";
            else if (dir.Y == 1) return "DOWN";
            else if (dir.Y == -1) return "UP";
            else return "SHOOT";
        }
    }
}
