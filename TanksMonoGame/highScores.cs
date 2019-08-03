using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;


namespace Tanks
{
    public class highScores : RC_GameStateParent
    {

        ImageBackground highScore = null;
        Texture2D texHighScore = null;
        SpriteFont font1;
        Vector2 font1Pos = new Vector2(250, 350);
        Vector2 font2Pos = new Vector2(250, 400);
        Vector2 font3Pos = new Vector2(250, 450);


        static public int latestHighScore;
        static int highScore1 = 0;
        static int highScore2 = 0;
        static int highScore3 = 0;


        public override void Initialize()
        {

        }

        public override void LoadContent()
        {

            font1 = Content.Load<SpriteFont>("spritefont1");
            texHighScore = Content.Load<Texture2D>("highScores");
            highScore = new ImageBackground(texHighScore, Color.White, graphicsDevice);
            


        }

        public override void Update(GameTime gameTime)
        {
            

        }

        public override void Draw(GameTime gameTime)
        {
            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            //spriteBatch.Begin();  // depending on version you may need this
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                   
            highScore.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "1st: " + highScore1, font1Pos, Color.Red);
            spriteBatch.DrawString(font1, "2nd: " + highScore2, font2Pos, Color.Red);
            spriteBatch.DrawString(font1, "3rd: " + highScore3, font3Pos, Color.Red);


            spriteBatch.End();
        }

        static public void CalcHighestScore()
        {
            if (latestHighScore > highScore1)
            {
                highScore1 = latestHighScore;
            }
            else if (latestHighScore > highScore2)
            {
                highScore2 = latestHighScore;
            }
            else if (latestHighScore > highScore3)
            {
                highScore3 = latestHighScore;
            }
        }

    }
}

