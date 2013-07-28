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
using TankGame.AI;
using System.Runtime.InteropServices;

namespace TankGame
{
    class Scene
    {
        public Stone[] Stones { get; set; }
        public Water[] Waters { get; set; }
        public Brick[] Bricks { get; set; }
        public Tank[] Tanks { get; set; }
        public Tank Myplayer { get; set; }
        public List<LifePack> LifePacks { get; set; }
        public List<CoinPile> CoinPiles { get; set; }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        int size=45,originx=10,originy=10;
        Decoder dec;
        Communicator com;
        string lastmsg="";
        SpriteFont sfont1;
        PlayerAI ai;
        public Scene()
        {
            dec = new Decoder(this, size);
            com = new Communicator("127.0.0.1", 6000, 7000);
        }

        public void update()
        {
            int result = 0;
            string msg = com.listen();
            //try changing return type of dec.decode to string and handle this...
            if (msg != "")
            {
                lastmsg = msg;
                result= dec.decode(msg);
            }

            if (result == 1)        // initialize
            {
                ai = new PlayerAI();
                ai.init(10, Stones,Waters);
            }else if (result == 3)        // updaate
            {
               Vector2 dir= ai.getBestDirection(Bricks,Stones, Myplayer,Tanks, CoinPiles, LifePacks);
               com.send(dec.decodeDir(dir)+"#");
            }
            else if(result==6){
                MessageBox(new IntPtr(0), "player full", "Error", 0);

            }
            else if (result == 7)
            {
                MessageBox(new IntPtr(0), "already added", "Error", 0);

            }
            else if (result == 8)
            {
                MessageBox(new IntPtr(0), "game already started", "Error", 0);

            }
        }

        public void joinServer()
        {
            com.send("JOIN#");
        }

        public void loadScene(ContentManager Content)
        {
            sfont1 = Content.Load<SpriteFont>("sFont1");
            Entity.LoadTexture(Content);
            
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(sfont1, lastmsg, new Vector2(10, 30), Color.White);
            drawGround( spriteBatch);
            if(Bricks!=null)
                foreach (Brick brk in Bricks)
                {
                    brk.Draw(spriteBatch,originx,originy);
                }
            if(Stones!=null)
                foreach (Stone stn in Stones)
                {
                    stn.Draw(spriteBatch,originx,originy);
                }
            if (Waters != null)
                foreach (Water wtr in Waters)
                {
                    wtr.Draw(spriteBatch, originx, originy);
                }
            if (LifePacks != null)
                foreach (LifePack lpk in LifePacks)
                    lpk.Draw(spriteBatch, originx, originy);

            if (CoinPiles != null)
                foreach (CoinPile cpl in CoinPiles)
                    cpl.Draw(spriteBatch, originx, originy);

            if (Tanks != null)
                foreach (Tank tnk in Tanks)
                {
                    if (tnk == null) continue;
                    tnk.Draw(spriteBatch, originx, originy);
                }
        }

        public void drawGround(SpriteBatch spriteBatch)
        {
            Entity grnd = new Entity(size);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grnd.Position = new Vector2(i, j);
                    grnd.Draw(spriteBatch,originx,originy);
                }
            }
        }
    }
}
