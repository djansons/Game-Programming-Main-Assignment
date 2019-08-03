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
    class help : RC_GameStateParent
    {

        ImageBackground help1 = null;
        Texture2D texHelp = null;
       


        public override void Initialize()
        {

        }

        public override void LoadContent()
        {


            texHelp = Content.Load<Texture2D>("helpScreen");
            help1 = new ImageBackground(texHelp, Color.White, graphicsDevice);
   


        }

        public override void Update(GameTime gameTime)
        {


        }

        public override void Draw(GameTime gameTime)
        {
            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            //spriteBatch.Begin();  // depending on version you may need this
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

     
            help1.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}

