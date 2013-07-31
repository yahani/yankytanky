using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene scene;
        
        public Texture2D joinButton,playButton;
        public Vector2 joinBtnPos = new Vector2(540, 70);
        public MouseState mouseClick;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            scene = new Scene();
            // TODO: Add your initialization logic here
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 3.0f);
            this.IsMouseVisible = true;
            this.mouseClick = Mouse.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene.loadScene(Content);
            
            //scene.joinServer();
            
            joinButton = Content.Load<Texture2D>("joinButton");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState aMouse =  Mouse.GetState();
            if (aMouse.LeftButton == ButtonState.Pressed)
            {
                if (aMouse.X >= joinBtnPos.X && aMouse.X <= joinBtnPos.X + joinButton.Width && aMouse.Y >= joinBtnPos.Y && aMouse.Y <= joinBtnPos.Y + joinButton.Height)
                {
                    scene.joinServer();
                }
            }
            scene.update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            scene.draw(spriteBatch);
            spriteBatch.Draw(joinButton, joinBtnPos, new Rectangle(0,0,joinButton.Width,joinButton.Height), Color.White, 0, new Vector2(0, 0), new Vector2(1f,1f), SpriteEffects.None, 0);
            //spriteBatch.Draw(playButton, playBtnPos, new Rectangle(0, 0, playButton.Width, playButton.Height), Color.White, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0);
            
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
