using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
using System.IO;

namespace Tanks
{
    class LevelChange : RC_GameStateParent
    {

        ImageBackground back1 = null;

        Texture2D texBack1 = null;
        Texture2D texMenu = null;

        Rectangle menuRect = new Rectangle(230, -900, 300, 800);
        Vector2 fontNewGamePos = new Vector2(337, 265);
        Vector2 fontHighScorePos = new Vector2(334, 345);
        Vector2 fontHelpPos = new Vector2(360, 425);
        Vector2 fontExitPos = new Vector2(360, 500);

        int ticks = 0;
        int tickMarker = 0;


        public override void LoadContent()
        {
           
           
            
            texBack1 = Content.Load<Texture2D>("levelChangeBackground");
            back1 = new ImageBackground(texBack1, Color.White, graphicsDevice);
            texMenu = Content.Load<Texture2D>("GameOver");



            //music = Content.Load<SoundEffect>("menuMusic");
            //// music.CreateInstance();
            //// soundfile = TitleContainer.OpenStream("menuMusic");

            //// music = SoundEffect.FromStream(soundfile);
            //musicInstance = music.CreateInstance();
            //musicInstance.IsLooped = true;
            //// musicInstance.Play();



        }

        public override void Update(GameTime gameTime)
        {
            ticks++; 

            switch (Game1.playLevelSelector)
            {
                case 2:
                    texMenu = Content.Load<Texture2D>("LevelChangeL2");
                    break;
                case 3:
                    texMenu = Content.Load<Texture2D>("LevelChangeL3");
                    break;
                case 4:
                    texMenu = Content.Load<Texture2D>("GameOver");
                    break;
                case 0:
                    texMenu = Content.Load<Texture2D>("GameOver");
                    break;

            }
            if (menuRect.Y <-250)
            menuRect.Y += 5;

            if (menuRect.Y == -250)
            {
                tickMarker = ticks;
                menuRect.Y = -248;
            }

            if (menuRect.Y >= -250)
            {
                
                if ((ticks - tickMarker) > 200)
                {
                    menuRect.Y = -900;
                    if (Game1.playLevelSelector == 2)
                    {
                        Game1.levelManager.getLevel(2).LoadContent();
                        Game1.levelManager.getLevel(2).Initialize();
                        Game1.levelManager.setLevel(2);
                    }

                    else if (Game1.playLevelSelector == 3)
                    {
                        Game1.levelManager.getLevel(3).LoadContent();
                        Game1.levelManager.getLevel(3).Initialize();
                        Game1.levelManager.setLevel(3);
                    }
                    else if (Game1.playLevelSelector == 4)
                    {
                        Game1.levelManager.getLevel(0).LoadContent();
                        Game1.levelManager.getLevel(0).Initialize();
                        Game1.levelManager.setLevel(0);
                    }

                    else if (Game1.playLevelSelector == 0)
                    {
                        Game1.levelManager.getLevel(0).LoadContent();
                        Game1.levelManager.getLevel(0).Initialize();
                        Game1.levelManager.getLevel(1).LoadContent();
                        Game1.levelManager.getLevel(1).Initialize();
                        Game1.levelManager.setLevel(0);
                    }
                }
            }



        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            back1.Draw(spriteBatch);
            spriteBatch.Draw(texMenu, menuRect, Color.White);

           
            spriteBatch.End();
        }

    }
}
