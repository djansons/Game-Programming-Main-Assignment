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
    class splashScreen : RC_GameStateParent
    {

        ImageBackground back2 = null;

        Texture2D texSplash = null;

        int timerTicks = 200;


        public override void LoadContent()
        {
            texSplash = Content.Load<Texture2D>("splashScreen");
            back2 = new ImageBackground(texSplash, Color.White, graphicsDevice);

        }

        public override void Update(GameTime gameTime)
        {
            timerTicks--;
            if (timerTicks <= 0)
            {
                Game1.levelManager.setLevel(0);
                
                
            }

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            back2.Draw(spriteBatch);
            spriteBatch.End();

        }

    }
}
