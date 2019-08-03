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


namespace Tanks
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public RC_GameStateManager levelManager;

        Rectangle playArea;

        static public bool showBB = false;

       

        static public bool paused = false;
        static public bool help = false;
        int count = 0;
        static public int playLevelSelector = 2;
       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 800;
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            playArea = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            levelManager = new RC_GameStateManager();

            levelManager.AddLevel(0, new MainMenu()); // note menu is level 0
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();
            levelManager.getLevel(0).Initialize();
            levelManager.setLevel(0);



            levelManager.AddLevel(1, new playLevel()); // note level 1 is level 1
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();
            levelManager.getLevel(1).Initialize();
  
            levelManager.AddLevel(2, new playLevel2()); // note level 2 is level 2
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();
            levelManager.getLevel(2).Initialize();     

            levelManager.AddLevel(3, new playLevel3()); // note level 3 is level 3
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();
            levelManager.getLevel(3).Initialize();

            levelManager.AddLevel(4, new LevelChange()); // note level4 is levelChanger
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(4).LoadContent();
            levelManager.getLevel(4).Initialize();

            levelManager.AddLevel(5, new pause()); // note level5 is pause
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();

            levelManager.AddLevel(6, new help()); // note level6 is help
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(6).LoadContent();

            levelManager.AddLevel(7, new highScores()); // note level7 is highscores
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            levelManager.AddLevel(8, new splashScreen()); // note level8 is splashscreen
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();

            levelManager.setLevel(0);


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           
   
            // TODO: use this.Content to load your game content here
        }

        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            count++;
            
            RC_GameStateParent.getKeyboardAndMouse();
            //RC_GameStateParent.prevKeyState = RC_GameStateParent.keyState;
            //RC_GameStateParent.keyState = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            levelManager.getCurrentLevel().Update(gameTime);

            if (MainMenu.exitGame == true)
            {                     
                    Exit();
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.B) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.B))
            {
                if (!showBB)
                {
                    showBB = true;
                }

                else
                {
                    showBB = false;
                }

            }


            if (RC_GameStateParent.keyState.IsKeyDown(Keys.P) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.P))
            {
                Console.WriteLine();
                if (paused == false)
                {
                    levelManager.pushLevel(5);
                    paused = true;
                }     
                else
                {
                    levelManager.popLevel();
                    paused = false;
                }

            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.H) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.H))
            {
                
                if (help == false)
                {
                    levelManager.pushLevel(6);
                    help = true;
                }
                else
                {
                    levelManager.popLevel();
                    help = false;
                }

            }

            if (help == true && (RC_GameStateParent.keyState.IsKeyDown(Keys.Escape) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Escape)))
            {
                levelManager.popLevel();
                help = false;
            }

            if (levelManager.getCurrentLevelNum() == 7 && (RC_GameStateParent.keyState.IsKeyDown(Keys.Escape) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Escape)))
            {
                levelManager.popLevel();
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            levelManager.getCurrentLevel().Draw(gameTime);
            base.Draw(gameTime);
        }

        



        
        
    }

}





