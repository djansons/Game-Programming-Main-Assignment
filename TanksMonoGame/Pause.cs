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
    class pause : RC_GameStateParent
    {

        ImageBackground pause1 = null;
        Texture2D texPause = null;
        ColorField transparent = null;


        public override void Initialize()
        {

        }

        public override void LoadContent()
        {


            texPause = Content.Load<Texture2D>("paused");
            pause1 = new ImageBackground(texPause, Color.White, graphicsDevice);
            transparent = new ColorField(new Color(255, 255, 255, 100), new Rectangle(0, 0, 800, 600));


        }

        public override void Update(GameTime gameTime)
        {


        }

        public override void Draw(GameTime gameTime)
        {
            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            //spriteBatch.Begin();  // depending on version you may need this
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            transparent.Draw(spriteBatch);
            pause1.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}

