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
     class MainMenu : RC_GameStateParent
    {

        ImageBackground back1 = null;

        Texture2D texBack1 = null;
        Texture2D texMenu = null;

        Rectangle menuRect = new Rectangle(230, 200, 300, 400);
        Vector2 fontNewGamePos = new Vector2(337, 265);
        Vector2 fontHighScorePos = new Vector2(334, 345);
        Vector2 fontHelpPos = new Vector2(360, 425);   
        Vector2 fontExitPos = new Vector2(360, 500);

        Color color1 = Color.Gray;
        Color color2 = Color.Gray;
        Color color3 = Color.Gray;
        Color color4 = Color.Gray;
        Color selectedColor = Color.Red;
        Color unSelectedColor = Color.Black;

        public static int menuSelector = 1;
        float musicVolume = 1;

        public static bool exitGame = false;
        bool fadeMusic = false;

        SoundEffect click;
        SoundEffect select;


        SoundEffect music;
        SoundEffectInstance musicInstance;
        Stream soundfile;



        SpriteFont font1;

        



        public override void LoadContent()
        {
            fadeMusic = false;
            musicVolume = 1;
            texMenu = Content.Load<Texture2D>("splashMenu");
            texBack1 = Content.Load<Texture2D>("titleScreen");
            font1 = Content.Load<SpriteFont>("spritefont1");
            back1 = new ImageBackground(texBack1, Color.White, graphicsDevice);
            click = Content.Load<SoundEffect>("click");
            select = Content.Load<SoundEffect>("select");


            music = Content.Load<SoundEffect>("menuMusic");
            music.CreateInstance();           
            musicInstance = music.CreateInstance();
            musicInstance.IsLooped = true;
            musicInstance.Play();

            
            
        }

        public override void Update(GameTime gameTime)
        {
            

            

            if ((keyState.IsKeyDown(Keys.Down) && prevKeyState.IsKeyUp(Keys.Down)) || (keyState.IsKeyDown(Keys.S) && prevKeyState.IsKeyUp(Keys.S)))
            {
                click.Play();
                menuSelector += 1;
                if (menuSelector > 4)
                {
                    menuSelector = 1;
                }
            }
            
            else if ((keyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up)) || (keyState.IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W)))
            {
                click.Play();
                menuSelector -= 1;
                if (menuSelector < 1)
                {
                    menuSelector = 4;
                }
            }

            else if ((keyState.IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter)))
            {
               select.Play();
                switch (menuSelector)
                {
                    case 1://New Game

                        fadeMusic = true;                           
                        break;
                    case 2: //High Score
                        Game1.levelManager.pushLevel(7);
                        break;
                    case 3: //Help
                        Game1.help = true;
                        Game1.levelManager.pushLevel(6);  
                        break;
                    case 4: //Exit
                        exitGame = true;                       
                        break;

                }

            }

            color1 = unSelectedColor;
            color2 = unSelectedColor;
            color3 = unSelectedColor;
            color4 = unSelectedColor;

            switch (menuSelector)
            {
                case 1:
                    color1 = selectedColor;
                    break;
                case 2:
                    color2 = selectedColor;
                    break;
                case 3:
                    color3 = selectedColor;
                    break;
                case 4:
                    color4 = selectedColor;
                    break;

            }

            if (fadeMusic)
            {
               musicInstance.Volume = musicVolume;
               musicVolume -= 0.01f;
               if (musicVolume <= 0) musicVolume = 0;
                if (musicVolume == 0)
                {
                    Game1.levelManager.getLevel(1).LoadContent();
                    Game1.levelManager.getLevel(1).Initialize();
                    Game1.levelManager.setLevel(1);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            back1.Draw(spriteBatch);
            spriteBatch.Draw(texMenu, menuRect, Color.White);
          
            spriteBatch.DrawString(font1, "New Game", fontNewGamePos, color1);
            spriteBatch.DrawString(font1, "High Score", fontHighScorePos, color2);
            spriteBatch.DrawString(font1, "Help", fontHelpPos, color3);
            spriteBatch.DrawString(font1, "Exit", fontExitPos, color4);
            spriteBatch.End();
        }

    }
}
