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
    class playLevel3 : RC_GameStateParent
    {
        //Fonts

        SpriteFont font1;

        //Vectors
        Vector2 fontHealthPos = new Vector2(600, 10);
        Vector2 mousePos;
        Vector2 tempPos;

        //Textures

        Texture2D texTankBody = null, texTankGun = null;
        Texture2D texCrossHairs = null;
        Texture2D texSmallMissle = null;
        Texture2D texBigMissle = null;
        Texture2D texBricks = null;
        Texture2D texExplosion = null;
        Texture2D texSmallExplosion = null;
        Texture2D texMap3 = null;
        Texture2D texGoldTruck = null;
        Texture2D texBus = null;
        Texture2D texArmouredTruck = null;

        //Sprite Effects
        SpriteEffects spriteEffects;

        //Sprites

        Sprite3 tankBody = null, tankGun = null;

        Sprite3 crossHairs = null;
        Sprite3 smallMissle = null;
        Sprite3 bigMissle = null;

        // SpriteLists

        SpriteList smallMissleList = null;
        SpriteList playerMissleList = null;
        SpriteList expolsionList = null;

        //Sounds

        SoundEffect bigBoom = null;
        SoundEffect bounceLow = null;
        SoundEffect bounceMid = null;
        SoundEffect bounceHigh = null;
        SoundEffect playerFire = null;
        SoundEffect enemyFire = null;
        SoundEffect shellExplosion = null;
        SoundEffect track = null;
        SoundEffectInstance trackInstance = null;
        LimitSound limBuzz;
        SoundEffect buzz;

        //Booleans

        bool missleFired = false;
        bool verticalReflection = false;
        bool lineReturn = false;
        bool reset = false;
        bool forceStart = false;
        bool blockRefelection = false;
        bool playerHit = false;
        bool playerExplosionRan = false;
        bool texCollision = false;
        bool enemysDead = false;
        bool playerDead = false; 


        //Enum
        enum collision
        {
            top,
            bottom,
            left,
            right,
            nothing
        }


        //Integers

        int tankRotateSpeed = 5;
        int lineIntersect = 0;
        int ticks = 0;
        int direction = 0;
        int playAreaHorizontalOffset = 80;
        int playAreaVerticalOffset = 23;
        int rectCollision = (int)collision.nothing;
        int maxPlayerMissileCount = 3;
        int maxEnemyMissleCount = 8;
        int bounceRandomSound = 0;
        int enemyMissleSpeed = 4;
        int playerMissleSpeed = 2;
        int health = 100;
        int tickmarker = 0;

        //Floats
        float tankStartPosX = 600;
        float tankStartPosY = 600;
        float ET1StartPosX = 150;
        float ET1StartPosY = 250;
        float ET2StartPosX = 600;
        float ET2StartPosY = 250;
        float mouseX, mouseY;
        float tankSpeed = 1; // needs to be 1 for collisions to work with train - Do not touch
        float lineRotateSpeed = 0.5f;
        float degrees = 0;

        //Tanks
        EnemyTank ET1;
        EnemyTank ET2;

        //Variables
        Rectangle Map3;
        Rectangle playArea;
        Rectangle goldTruck;
        Rectangle armouredTruck;
        Rectangle bus;
        Rectangle tankBB;

        //Colours

        Color lineColor = Color.Black;

        //Random Numbers

        Random rand = new Random();


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            health = 100;
            tickmarker = 0;
            enemysDead = false;
            playerDead = false;
            //Map Initilization

            ET1 = new EnemyTank("EnemyTankBody", "EnemyTankGun", ET1StartPosX, ET1StartPosY);
            ET2 = new EnemyTank("EnemyTankBody", "EnemyTankGun", ET2StartPosX, ET2StartPosY);

            font1 = Content.Load<SpriteFont>("spritefont1");

            Map3 = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            playArea = new Rectangle(playAreaHorizontalOffset, 150, graphicsDevice.Viewport.Width - (playAreaHorizontalOffset * 2), graphicsDevice.Viewport.Height - 300);
            tempPos = new Vector2(0, playArea.Height);
            goldTruck = new Rectangle(300, 150, 75, 150);
            armouredTruck = new Rectangle(100, 400, 75, 150);
            bus = new Rectangle(320, 450, 390, 100);

            mousePos = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            //Player Tank Initialization

            tankBody = new Sprite3(true, texTankBody, tankStartPosX, tankStartPosY);
            tankBody.setWidthHeight(tankBody.getWidth() / 1.5f, tankBody.getHeight() / 1.5f);
            tankBody.setBBandHSFractionOfTexCentered(0.9f);


            tankGun = new Sprite3(true, texTankGun, tankStartPosX, tankStartPosY);
            tankGun.setWidthHeight(tankGun.getWidth() / 1.3f, tankGun.getHeight() / 1.3f);
            tankGun.setBBandHSFractionOfTexCentered(1.0f);
            tankGun.setDisplayAngleOffsetDegrees(-90);

            tankBB = tankBody.getBoundingBoxAA();

            trackInstance = track.CreateInstance();
            trackInstance.Volume = 0.5f;

            //Crosshair Initilization

            crossHairs = new Sprite3(true, texCrossHairs, 200, 200);
            //crossHairs.setWidthHeight(crossHairs.getWidth() / 5, crossHairs.getHeight() / 5);
            crossHairs.setBBandHSFractionOfTexCentered(1.0f);

            smallMissleList = new SpriteList();
            playerMissleList = new SpriteList();
            expolsionList = new SpriteList();
            limBuzz = new LimitSound(buzz, 1);
            playerHit = false;

            // CreateMissle(tankGun.getPosX(), tankGun.getPosY(), true);


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphicsDevice);
            spriteEffects = new SpriteEffects();
            LineBatch.init(graphicsDevice);
            texTankBody = Content.Load<Texture2D>("tankBody");
            texTankGun = Content.Load<Texture2D>("tankGun");

            texCrossHairs = Content.Load<Texture2D>("crossHairs");
            texSmallMissle = Content.Load<Texture2D>("resizedmissle");
            texBigMissle = Content.Load<Texture2D>("bigMissle");

            texBricks = Content.Load<Texture2D>("bricks");
            texExplosion = Content.Load<Texture2D>("explosion");
            texSmallExplosion = Content.Load<Texture2D>("smallExplosion");

            texMap3 = Content.Load<Texture2D>("map3");
            texGoldTruck = Content.Load<Texture2D>("goldTruck");
            texArmouredTruck = Content.Load<Texture2D>("armouredVan");
            texBus = Content.Load<Texture2D>("bus");

            bigBoom = Content.Load<SoundEffect>("bigBoom");
            bounceLow = Content.Load<SoundEffect>("bounceLow");
            bounceMid = Content.Load<SoundEffect>("bounceMid");
            bounceHigh = Content.Load<SoundEffect>("bounceHigh");
            playerFire = Content.Load<SoundEffect>("playerFire");
            enemyFire = Content.Load<SoundEffect>("enemyFire");
            shellExplosion = Content.Load<SoundEffect>("shellExplosion");
            track = Content.Load<SoundEffect>("track");
            buzz = Content.Load<SoundEffect>("buzz");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Allows the game to exit
            degrees = degrees + 1f;

            tankBB = tankBody.getBoundingBoxAA();

            ticks++;

            expolsionList.animationTick();

            bounceRandomSound = rand.Next(1, 3);

            bool reset = false;

          

            mouseX = currentMouseState.X;
            mouseY = currentMouseState.Y;
            mousePos.X = mouseX;
            mousePos.Y = mouseY;

            smallMissleList.setListMoveSpeed(enemyMissleSpeed);
            smallMissleList.moveAngleSpeed();

            playerMissleList.setListMoveSpeed(playerMissleSpeed);
            playerMissleList.moveAngleSpeed();

            crossHairs.setPos(mousePos);
            if (crossHairs.getPosX() <= 0)
            {
                crossHairs.setPosX(0);
            }
            if (crossHairs.getPosY() <= 0)
            {
                crossHairs.setPosY(0);
            }
            if (crossHairs.getPosX() >= graphicsDevice.Viewport.Width)
            {
                crossHairs.setPosX(graphicsDevice.Viewport.Width);
            }
            if (crossHairs.getPosY() >= graphicsDevice.Viewport.Height)
            {
                crossHairs.setPosY(graphicsDevice.Viewport.Height);
            }

            tankGun.setDisplayAngleRadians(tankGun.angleTo(crossHairs));

            texCollision = false;

            if (keyState.IsKeyDown(Keys.W))
            {

                rectCollision = didRectangleCollide(armouredTruck, tankBody);
                if (rectCollision == 4)
                rectCollision = didRectangleCollide(bus, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(goldTruck, tankBody);

                if (rectCollision != (int)collision.bottom && tankBody.getBoundingBoxAA().Y >= playArea.Y) // keeps out of rectangle and in the play area
                {

                    rotateTankToVertical(tankBody);
                    if (tankBody.getDisplayAngleDegrees() == 180 || tankBody.getDisplayAngleDegrees() == 0)
                    {
                        if (ticks % 5 == 0) trackInstance.Play();
                        tankBody.setPosY(tankBody.getPosY() - tankSpeed);
                        tankGun.setPosY(tankGun.getPosY() - tankSpeed);
                    }
                }

            }

            else if (keyState.IsKeyDown(Keys.S))
            {

                rectCollision = didRectangleCollide(armouredTruck, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(bus, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(goldTruck, tankBody);
                if (rectCollision != (int)collision.top && tankBody.getBoundingBoxAA().Bottom <= playArea.Bottom)
                {

                    rotateTankToVertical(tankBody);
                    if (tankBody.getDisplayAngleDegrees() == 180 || tankBody.getDisplayAngleDegrees() == 0)
                    {
                        if (ticks % 5 == 0) trackInstance.Play();
                        tankBody.setPosY(tankBody.getPosY() + tankSpeed);
                        tankGun.setPosY(tankGun.getPosY() + tankSpeed);
                    }
                }

            }

            else if (keyState.IsKeyDown(Keys.A))
            {
                rectCollision = didRectangleCollide(armouredTruck, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(bus, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(goldTruck, tankBody);
                if (rectCollision != (int)collision.right && tankBody.getBoundingBoxAA().X >= playArea.X)
                {

                    rotateTankToHorizontal(tankBody);
                    if (tankBody.getDisplayAngleDegrees() == 90 || tankBody.getDisplayAngleDegrees() == -90)
                    {
                        if (ticks % 5 == 0) trackInstance.Play();
                        tankBody.setPosX(tankBody.getPosX() - tankSpeed);
                        tankGun.setPosX(tankGun.getPosX() - tankSpeed);
                    }
                }


            }

            else if (keyState.IsKeyDown(Keys.D))
            {
                rectCollision = didRectangleCollide(armouredTruck, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(bus, tankBody);
                if (rectCollision == 4)
                    rectCollision = didRectangleCollide(goldTruck, tankBody);
                if (rectCollision != (int)collision.left && tankBody.getBoundingBoxAA().Right <= playArea.Right)
                {

                    rotateTankToHorizontal(tankBody);
                    if (tankBody.getDisplayAngleDegrees() == 90 || tankBody.getDisplayAngleDegrees() == -90)
                    {
                        if (ticks % 5 == 0) trackInstance.Play();
                        tankBody.setPosX(tankBody.getPosX() + tankSpeed);
                        tankGun.setPosX(tankGun.getPosX() + tankSpeed);
                    }
                }

            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                //if(playerMissleCount < maxPlayerMissileCount)
                if (playerMissleList.countActive() < maxPlayerMissileCount)
                {

                    CreatePlayerMissle(tankGun.getPosX(), tankGun.getPosY());
                    missleFired = true;

                }

            }

            if (ticks == tickmarker + 100 && playerHit == true)
            {
                Game1.levelManager.setLevel(4);
                Game1.playLevelSelector = 0;
            }
            if (playerHit == false) // So we only get little explosion if enemy did not hit player tank. 
            {

                for (int i = 0; i < smallMissleList.count(); i++)
                {
                    Sprite3 temp = smallMissleList.getSprite(i);
                    temp.animationTick();


                    if ((temp.getPosX() < playArea.Left + temp.getHeight() || temp.getPosX() > playArea.Right - temp.getHeight()))
                    {
                        //need to put something about collison being tru.
                        //smallMissle.setMoveAngleRadians(calcRefletionAngleDegrees(smallMissle));
                        //smallMissle.setDisplayAngleRadians(calcRefletionAngleDegrees(smallMissle));

                        calcRefletionAngleDegrees(temp, true);
                        temp.hitPoints += 1;
                        missleFired = false;

                        if (temp.hitPoints < temp.maxHitPoints) bounceSound();


                        //smallMissle.setMoveAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(smallMissle.getDisplayAngleRadians()) - ((MathHelper.ToDegrees(smallMissle.getDisplayAngleRadians()) - 90) * 2)));
                        //smallMissle.setDisplayAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(smallMissle.getDisplayAngleRadians()) - ((MathHelper.ToDegrees(smallMissle.getDisplayAngleRadians()) - 90) * 2)));
                    }

                    else if ((temp.getPosY() < playArea.Top + temp.getHeight() || temp.getPosY() > playArea.Bottom))
                    {
                        calcRefletionAngleDegrees(temp, false);
                        temp.hitPoints += 1;
                        missleFired = false;
                        if (temp.hitPoints < temp.maxHitPoints) bounceSound();

                    }

                    calcSurfaceCollission(temp, goldTruck);
                    calcSurfaceCollission(temp, armouredTruck);
                    calcSurfaceCollission(temp, bus);


                    if (tankBody.collision(temp) == true)
                    {
                        temp.hitPoints = temp.maxHitPoints;
                        playerHit = true;
                        tankBody.active = false;
                        tankGun.active = false;

                        
                        CreateBigExplosion(temp.getPosX(), temp.getPosY());
                        Game1.levelManager.setLevel(4);
                        Game1.playLevelSelector = 0;


                    }



                    if (temp.hitPoints == temp.maxHitPoints && (tankBody.collision(temp) == false))
                    {
                        temp.active = false;
                        CreateSmallExplosion(temp.getPosX(), temp.getPosY());
                    }

                    for (int count = 0; count < playerMissleList.count(); count++)
                    {
                        Sprite3 playerTemp = playerMissleList.getSprite(count);

                        if (playerTemp.collision(temp))
                        {
                            temp.active = false;
                            playerTemp.active = false;
                            //need to add player temp = false.....
                            CreateSmallExplosion(playerTemp.getPosX(), playerTemp.getPosY());
                        }
                    }


                }


                if (health <= 0)
                {
                    tickmarker = ticks;
                    health = 0;
                    playerHit = true;
                    tankBody.active = false;
                    tankGun.active = false;
                    CreateBigExplosion(tankBody.getPosX(), tankBody.getPosY());
                    
                }



                for (int count = 0; count < playerMissleList.count(); count++)
                {
                    Sprite3 playerTemp = playerMissleList.getSprite(count);
                    playerTemp.animationTick();

                    if (playerTemp.collision(ET1.EnemyTankBody))
                    {
                        playerTemp.active = false;
                        ET1.EnemyTankBody.active = false;
                        ET1.EnemyTankGun.active = false;
                        CreateBigExplosion(playerTemp.getPosX(), playerTemp.getPosY());

                    }

                    if (playerTemp.collision(ET2.EnemyTankBody))
                    {
                        playerTemp.active = false;
                        ET2.EnemyTankBody.active = false;
                        ET2.EnemyTankGun.active = false;
                        CreateBigExplosion(playerTemp.getPosX(), playerTemp.getPosY());

                    }

                    if ((playerTemp.getPosX() < playArea.Left + playerTemp.getHeight() || playerTemp.getPosX() > playArea.Right - playerTemp.getHeight()))
                    {

                        calcRefletionAngleDegrees(playerTemp, true);
                        playerTemp.hitPoints += 1;
                        missleFired = false;
                        if (playerTemp.hitPoints < playerTemp.maxHitPoints) bounceSound();


                    }

                    else if ((playerTemp.getPosY() < playArea.Top || playerTemp.getPosY() > playArea.Bottom))
                    {
                        calcRefletionAngleDegrees(playerTemp, false);
                        playerTemp.hitPoints += 1;
                        missleFired = false;
                        if (playerTemp.hitPoints < playerTemp.maxHitPoints) bounceSound();

                    }

                    calcSurfaceCollission(playerTemp, armouredTruck);
                    calcSurfaceCollission(playerTemp, goldTruck);
                    calcSurfaceCollission(playerTemp, bus);

                    if (playerTemp.hitPoints == playerTemp.maxHitPoints)
                    {
                        playerTemp.active = false;
                        CreateSmallExplosion(playerTemp.getPosX(), playerTemp.getPosY());
                    }

                    if (tankBody.collision(playerTemp) == true)
                    {
                        tankBody.active = false;
                        tankGun.active = false;
                        playerTemp.active = false;
                        CreateBigExplosion(tankBody.getPosX(), tankBody.getPosY());
                        playerTemp.hitPoints = playerTemp.maxHitPoints;
                        playerHit = true;
                        playerDead = true;
                    }
                }


            }

            lineColor = Color.Black;
            ET1.EnemyTankGun.setDisplayAngleRadians(ET1.EnemyTankGun.angleTo(ET1.EnemyLineEnd));
            ET2.EnemyTankGun.setDisplayAngleRadians(ET2.EnemyTankGun.angleTo(ET2.EnemyLineEnd));


            ET1.EnemyLineStart = ET1.EnemyTankBody.getPos();
            ET2.EnemyLineStart = ET2.EnemyTankBody.getPos();

            ET1.Line1Angle = MathHelper.ToDegrees((float)Math.Atan2(ET1.EnemyLineEnd.Y - ET1.EnemyLineStart.Y, ET1.EnemyLineEnd.X - ET1.EnemyLineStart.X));
            ET2.Line1Angle = MathHelper.ToDegrees((float)Math.Atan2(ET2.EnemyLineEnd.Y - ET2.EnemyLineStart.Y, ET2.EnemyLineEnd.X - ET2.EnemyLineStart.X));

            int enemyLineIntersect = 10;

            double x = 0;
            double y = 0;
            blockRefelection = false;



            moveEnemyLine(ET1);
            moveEnemyLine(ET2);
            //The next 12 lines deal with collisions with map objects. 

            calcLine1Obstruction(goldTruck, ET1);
            calcLine1Obstruction(armouredTruck, ET1);
            calcLine1Obstruction(bus, ET1);

            calcLine1Obstruction(armouredTruck, ET2);
            calcLine1Obstruction(goldTruck, ET2);
            calcLine1Obstruction(bus, ET2);

            ET1.EnemyLineStart2 = ET1.EnemyLineEnd;
            ET2.EnemyLineStart2 = ET2.EnemyLineEnd;
            ET1.EnemyLineEnd2 = keepLineInPlayArea(ET1.EnemyLineStart2, ET1.EnemyLineEnd2);
            ET2.EnemyLineEnd2 = keepLineInPlayArea(ET2.EnemyLineStart2, ET2.EnemyLineEnd2);

            ET1.EnemyLineStart3 = ET1.EnemyLineEnd2;
            ET2.EnemyLineStart3 = ET2.EnemyLineEnd2;
            calcLine2Obstruction(goldTruck, ET1);
            calcLine2Obstruction(armouredTruck, ET1);
            calcLine2Obstruction(bus, ET1);

            calcLine2Obstruction(goldTruck, ET2);
            calcLine2Obstruction(armouredTruck, ET2);
            calcLine2Obstruction(bus, ET2);

            calcLine3Obstruction(goldTruck, ET1);
            calcLine3Obstruction(armouredTruck, ET1);
            calcLine3Obstruction(bus, ET1);

            calcLine3Obstruction(goldTruck, ET2);
            calcLine3Obstruction(armouredTruck, ET2);
            calcLine3Obstruction(bus, ET2);


            if (ticks % 5 == 0)
            {
                if (ET1.EnemyTankBody.active == true)
                {
                    targetInLaserRange(ET1, ET1.EnemyLineStart, ET1.EnemyLineEnd, tankBB);
                    targetInLaserRange(ET1, ET1.EnemyLineStart2, ET1.EnemyLineEnd2, tankBB);
                    targetInLaserRange(ET1, ET1.EnemyLineStart3, ET1.EnemyLineEnd3, tankBB);
                }
                if (ET2.EnemyTankBody.active == true)
                {
                    targetInLaserRange(ET2, ET2.EnemyLineStart, ET2.EnemyLineEnd, tankBB);
                    targetInLaserRange(ET2, ET2.EnemyLineStart2, ET2.EnemyLineEnd2, tankBB);
                    targetInLaserRange(ET2, ET2.EnemyLineStart3, ET2.EnemyLineEnd3, tankBB);
                }                   
            }


            if (ET1.EnemyTankBody.active == false && ET2.EnemyTankBody.active == false)
            {
                if (tickmarker == 0)enemysDead = true;
                if (ticks == tickmarker + 200)
                {
                    highScores.latestHighScore = health;
                    highScores.CalcHighestScore();
                    Game1.levelManager.setLevel(4);
                    Game1.playLevelSelector = 4;
                }

            }

            if (enemysDead)
            {
                tickmarker = ticks;
                enemysDead = false;     
            }

            if (playerDead)
            {
                if (tickmarker == 0) tickmarker = ticks; 
            }


            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            graphicsDevice.Clear(Color.CornflowerBlue);



            spriteBatch.Draw(texMap3, Map3, Color.White);
            spriteBatch.Draw(texGoldTruck, goldTruck, Color.White);
            spriteBatch.Draw(texBus, bus, Color.White);
            spriteBatch.Draw(texArmouredTruck, armouredTruck, Color.White);
            if (Game1.showBB)
            {
                ET1.EnemyTankBody.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);
                ET2.EnemyTankBody.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);
                LineBatch.drawLineRectangle(spriteBatch, bus, Color.Pink);
                LineBatch.drawLineRectangle(spriteBatch, goldTruck, Color.Pink);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Pink);
                tankBody.drawInfo(spriteBatch, Color.HotPink, Color.Honeydew);
                crossHairs.drawInfo(spriteBatch, Color.HotPink, Color.Honeydew);
                tankGun.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);  
                smallMissleList.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);
                playerMissleList.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);
                expolsionList.drawInfo(spriteBatch, Color.HotPink, Color.AliceBlue);
            }

            if (ET1.EnemyTankBody.active == true)
            {
                LineBatch.drawLine(spriteBatch, Color.Red, ET1.EnemyLineStart, ET1.EnemyLineEnd);
                LineBatch.drawLine(spriteBatch, Color.Red, ET1.EnemyLineStart2, ET1.EnemyLineEnd2);
                LineBatch.drawLine(spriteBatch, Color.Red, ET1.EnemyLineStart3, ET1.EnemyLineEnd3);
            }

            if (ET2.EnemyTankBody.active == true)
            {
                LineBatch.drawLine(spriteBatch, Color.Red, ET2.EnemyLineStart, ET2.EnemyLineEnd);
                LineBatch.drawLine(spriteBatch, Color.Red, ET2.EnemyLineStart2, ET2.EnemyLineEnd2);
                LineBatch.drawLine(spriteBatch, Color.Red, ET2.EnemyLineStart3, ET2.EnemyLineEnd3);
            }
            tankBody.Draw(spriteBatch);            
            crossHairs.draw(spriteBatch);
            tankGun.Draw(spriteBatch);          
            ET1.EnemyTankBody.Draw(spriteBatch);
            ET1.EnemyTankGun.Draw(spriteBatch);

            ET2.EnemyTankBody.Draw(spriteBatch);
            ET2.EnemyTankGun.Draw(spriteBatch);

            

            smallMissleList.Draw(spriteBatch);
            
            playerMissleList.Draw(spriteBatch);
            
            

            


            // LineBatch.drawFillRectangle(spriteBatch, wall1, Color.Red);


            expolsionList.Draw(spriteBatch);
           
            // TODO: Add your drawing code here
            spriteBatch.DrawString(font1, "Health: " + health, fontHealthPos, Color.Red);
            spriteBatch.End();

        }

        private void targetInLaserRange(EnemyTank ET, Vector2 lineStart, Vector2 lineEnd, Rectangle BBRectangle)
        {
            double x = 0;
            double y = 0;
            int tankCollision = 10;
            tankCollision = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, BBRectangle.X, BBRectangle.Y, BBRectangle.X + BBRectangle.Width, BBRectangle.Y);
            if (tankCollision == 0)
            { 
            health-=2;
            limBuzz.playSoundIfOk();
            }
            
            tankCollision = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, BBRectangle.X, BBRectangle.Y, BBRectangle.X, BBRectangle.Y + BBRectangle.Height);
            if (tankCollision == 0)
            {
                if (smallMissleList.countActive() < maxEnemyMissleCount)
                    health -= 2;
                limBuzz.playSoundIfOk();
            }
            tankCollision = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, BBRectangle.X + BBRectangle.Width, BBRectangle.Y, BBRectangle.X + BBRectangle.Width, BBRectangle.Y + BBRectangle.Height);
            if (tankCollision == 0)
            {
                if (smallMissleList.countActive() < maxEnemyMissleCount)
                    health -= 2;
                limBuzz.playSoundIfOk();
            }
            tankCollision = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, BBRectangle.X, BBRectangle.Y + BBRectangle.Height, BBRectangle.X + BBRectangle.Width, BBRectangle.Y + BBRectangle.Height);
            if (tankCollision == 0)
            {
                if (smallMissleList.countActive() < maxEnemyMissleCount)
                    health -= 2;
                limBuzz.playSoundIfOk();
            }
        }

        private void CreateMissle(EnemyTank ET, float misslePosX, float misslePosY, bool enemyMissle)
        {
            if (enemyMissle)
            {
                enemyFire.Play();
                bigMissle = new Sprite3(true, texBigMissle, misslePosX, misslePosY);
                smallMissleList.addSpriteReuse(bigMissle);
                bigMissle.setHSoffset(new Vector2(210, 0));
                bigMissle.setWidthHeight(17, 35);
                bigMissle.setBBToTexture();
                bigMissle.maxHitPoints = 3;
                bigMissle.setDisplayAngleOffsetDegrees(90);
                bigMissle.setPos(ET.EnemyTankGun.getPos());
                bigMissle.setMoveAngleRadians(ET.EnemyTankGun.getDisplayAngleRadians());
                bigMissle.setDisplayAngleRadians(ET.EnemyTankGun.getDisplayAngleRadians());
                bigMissle.setMoveSpeed(50);
                bigMissle.moveByAngleSpeed();
            }
            else
            {
                smallMissle = new Sprite3(true, texSmallMissle, misslePosX, misslePosY);
                smallMissleList.addSpriteReuse(smallMissle);
                smallMissle.setHSoffset(new Vector2(20, 0));
                smallMissle.setXframes(13);
                smallMissle.setYframes(1);
                smallMissle.setWidthHeight(17, 35);
                smallMissle.setBB(5, 0, 17, 20);
                Vector2[] seqsmallMissle = new Vector2[13];
                seqsmallMissle[0].X = 0; seqsmallMissle[0].Y = 0;
                seqsmallMissle[1].X = 1; seqsmallMissle[1].Y = 0;
                seqsmallMissle[2].X = 2; seqsmallMissle[2].Y = 0;
                seqsmallMissle[3].X = 3; seqsmallMissle[3].Y = 0;
                seqsmallMissle[4].X = 4; seqsmallMissle[4].Y = 0;
                seqsmallMissle[5].X = 5; seqsmallMissle[5].Y = 0;
                seqsmallMissle[6].X = 6; seqsmallMissle[6].Y = 0;
                seqsmallMissle[7].X = 7; seqsmallMissle[7].Y = 0;
                seqsmallMissle[8].X = 8; seqsmallMissle[8].Y = 0;
                seqsmallMissle[9].X = 9; seqsmallMissle[9].Y = 0;
                seqsmallMissle[10].X = 10; seqsmallMissle[10].Y = 0;
                seqsmallMissle[11].X = 11; seqsmallMissle[11].Y = 0;
                seqsmallMissle[12].X = 12; seqsmallMissle[12].Y = 0;
                smallMissle.setAnimationSequence(seqsmallMissle, 0, 12, 3);
                smallMissle.animationStart();
                smallMissle.maxHitPoints = 2;
                smallMissle.setDisplayAngleOffsetDegrees(90);
                smallMissle.setPos(tankGun.getPos());
                smallMissle.setMoveAngleRadians(tankGun.getDisplayAngleRadians());
                smallMissle.setDisplayAngleRadians(tankGun.getDisplayAngleRadians());
                smallMissle.setMoveSpeed(50);
                smallMissle.moveByAngleSpeed();
            }
        }


        private void CreatePlayerMissle(float misslePosX, float misslePosY)
        {

            playerFire.Play();
            smallMissle = new Sprite3(true, texSmallMissle, misslePosX, misslePosY);
            playerMissleList.addSpriteReuse(smallMissle);
            smallMissle.setHSoffset(new Vector2(20, 0));
            smallMissle.setXframes(13);
            smallMissle.setYframes(1);
            smallMissle.setWidthHeight(17, 35);
            smallMissle.setBB(5, 0, 17, 20);
            Vector2[] seqsmallMissle = new Vector2[13];
            seqsmallMissle[0].X = 0; seqsmallMissle[0].Y = 0;
            seqsmallMissle[1].X = 1; seqsmallMissle[1].Y = 0;
            seqsmallMissle[2].X = 2; seqsmallMissle[2].Y = 0;
            seqsmallMissle[3].X = 3; seqsmallMissle[3].Y = 0;
            seqsmallMissle[4].X = 4; seqsmallMissle[4].Y = 0;
            seqsmallMissle[5].X = 5; seqsmallMissle[5].Y = 0;
            seqsmallMissle[6].X = 6; seqsmallMissle[6].Y = 0;
            seqsmallMissle[7].X = 7; seqsmallMissle[7].Y = 0;
            seqsmallMissle[8].X = 8; seqsmallMissle[8].Y = 0;
            seqsmallMissle[9].X = 9; seqsmallMissle[9].Y = 0;
            seqsmallMissle[10].X = 10; seqsmallMissle[10].Y = 0;
            seqsmallMissle[11].X = 11; seqsmallMissle[11].Y = 0;
            seqsmallMissle[12].X = 12; seqsmallMissle[12].Y = 0;
            smallMissle.setAnimationSequence(seqsmallMissle, 0, 12, 3);
            smallMissle.animationStart();
            smallMissle.maxHitPoints = 2;
            smallMissle.setDisplayAngleOffsetDegrees(90);
            smallMissle.setPos(tankGun.getPos());
            smallMissle.setMoveAngleRadians(tankGun.getDisplayAngleRadians());
            smallMissle.setDisplayAngleRadians(tankGun.getDisplayAngleRadians());
            smallMissle.setMoveSpeed(60);
            smallMissle.moveByAngleSpeed();

        }


        public Vector2 moveVectorByAngleSpeed(Vector2 pos, float moveAngleInRadians, float moveSpeed)
        {
            Vector2 posNew;
            Vector2 temp = Util.moveByAngleDist(pos, moveAngleInRadians, moveSpeed);
            posNew.X = temp.X;
            posNew.Y = temp.Y;
            return posNew;
        }

        private void keepOutOfRectangle(Sprite3 sprite, Rectangle rectangle)
        {
            if (sprite.getBoundingBoxAA().Top >= rectangle.Bottom) ;
        }

        private Vector2 keepLineInPlayArea(Vector2 lineStart, Vector2 lineEnd)
        {
            double x = 0;
            double y = 0;
            Vector2 newLineEnd = new Vector2((float)x, (float)y);

            int enemyLine2Intersect = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y + 1, lineEnd.X, lineEnd.Y, playArea.X, playArea.Y, playArea.X + playArea.Width, playArea.Y);//1 TOP
            if (enemyLine2Intersect == 0)
                newLineEnd = new Vector2((float)x, (float)y);

            enemyLine2Intersect = Util.intersect2D(ref x, ref y, lineStart.X, lineStart.Y - 1, lineEnd.X, lineEnd.Y, playArea.X, playArea.Y + playArea.Height, playArea.X + playArea.Width, playArea.Y + playArea.Height);//4 BOTTOM
            if (enemyLine2Intersect == 0)
                newLineEnd = new Vector2((float)x, (float)y);

            enemyLine2Intersect = Util.intersect2D(ref x, ref y, lineStart.X + 1, lineStart.Y, lineEnd.X, lineEnd.Y, playArea.X, playArea.Y, playArea.X, playArea.Y + playArea.Height);//2 LEFT
            if (enemyLine2Intersect == 0)
                newLineEnd = new Vector2((float)x, (float)y);

            enemyLine2Intersect = Util.intersect2D(ref x, ref y, lineStart.X - 1, lineStart.Y, lineEnd.X, lineEnd.Y, playArea.X + playArea.Width, playArea.Y, playArea.X + playArea.Width, playArea.Y + playArea.Height);//3 RIGHT
            if (enemyLine2Intersect == 0)
                newLineEnd = new Vector2((float)x, (float)y);

            return newLineEnd;

        }




        private int didRectangleCollide(Rectangle wall, Sprite3 sprite)
        {

            int enemyLineIntersect = 10;

            double x = 0;
            double y = 0;

            int stuckSide = 4;

            enemyLineIntersect = Util.intersect2D(ref x, ref y, sprite.getBoundingBoxAA().X, sprite.getBoundingBoxAA().Y, sprite.getBoundingBoxAA().X, sprite.getBoundingBoxAA().Bottom, wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3 RH side of rectangle collision
            if (enemyLineIntersect == 1)
            {
                stuckSide = 3;
            }
            enemyLineIntersect = Util.intersect2D(ref x, ref y, sprite.getBoundingBoxAA().X, sprite.getBoundingBoxAA().Y, sprite.getBoundingBoxAA().Right, sprite.getBoundingBoxAA().Y, wall.X, wall.Y + wall.Height, wall.X + wall.Width, wall.Y + wall.Height);//4 Bottom of rectangle collision
            if (enemyLineIntersect == 2)
            {
                stuckSide = 1;
            }
            enemyLineIntersect = Util.intersect2D(ref x, ref y, sprite.getBoundingBoxAA().X, sprite.getBoundingBoxAA().Bottom, sprite.getBoundingBoxAA().Right, sprite.getBoundingBoxAA().Bottom, wall.X, wall.Y, wall.X + wall.Width, wall.Y);//1 Top side rectangle colision
            if (enemyLineIntersect == 2)
            {
                stuckSide = 0;
            }
            enemyLineIntersect = Util.intersect2D(ref x, ref y, sprite.getBoundingBoxAA().Right, sprite.getBoundingBoxAA().Y, sprite.getBoundingBoxAA().Right, sprite.getBoundingBoxAA().Bottom, wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
            if (enemyLineIntersect == 1)
            {
                stuckSide = 2;
            }

            return stuckSide;

        }


        private void calcLine1Obstruction(Rectangle wall, EnemyTank ET)
        {

            int enemyLineIntersect = 10;

            double x = 0;
            double y = 0;


            if (Math.Abs(ET.EnemyLineStart.Y - wall.Bottom) < Math.Abs(ET.EnemyLineStart.Y - wall.Top)) // Calcs if top of rectangle or bottom is closer to line start. 
            {

                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart.X, ET.EnemyLineStart.Y, tempPos.X, tempPos.Y, wall.X, wall.Y + wall.Height, wall.X + wall.Width, wall.Y + wall.Height);//4 Bottom of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd = new Vector2((float)x, (float)y);
                    //enemyLineEnd = enemyLineCollision;
                    //enemyLineEnd.Y -= 2;
                    ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, false));
                    ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 850);
                    ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    if (ET.EnemyLineEnd2.X >= playArea.Width || ET.EnemyLineEnd2.X <= playArea.X)
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true)); // end of line 2 is on vertical line
                    }

                    else
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false)); //end of line 2 is on horizontal line
                    }
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);


                }
            }


            else
            {
                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart.X, ET.EnemyLineStart.Y, tempPos.X, tempPos.Y, wall.X, wall.Y, wall.X + wall.Width, wall.Y);//1 Top side rectangle colision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd = new Vector2((float)x, (float)y);
                    //desiredLineEnd = lineCollision;
                    //enemyLineEnd.Y -= 2;
                    ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, false));
                    ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 850);
                    ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    if (ET.EnemyLineEnd2.X >= playArea.Width || ET.EnemyLineEnd2.X <= playArea.X)
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));// end of line 2 is on vertical line
                    }
                    else
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));//end of line 2 is on horizontal line
                    }
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    // ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    //blockRefelection = true;
                }
            }
            Console.WriteLine(Math.Abs(-wall.Right));

            if (Math.Abs(ET.EnemyLineStart.X - wall.Right) > Math.Abs(ET.EnemyLineStart.X - wall.Left))
            {


                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart.X, ET.EnemyLineStart.Y, tempPos.X, tempPos.Y, wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd = new Vector2((float)x - 1, (float)y);
                    //enemyLineEnd = enemyLineCollision;
                    ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, true));
                    ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 850);
                    ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    if (ET.EnemyLineEnd2.Y >= playArea.Height || ET.EnemyLineEnd2.Y <= playArea.Y)
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                    }
                    else
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    }
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    // ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    //enemyLineEnd.Y -= 2;
                }
            }

            else
            {

                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart.X, ET.EnemyLineStart.Y, tempPos.X, tempPos.Y, wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3 RH side of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd = new Vector2((float)x + 1, (float)y);
                    //enemyLineEnd = enemyLineCollision;
                    //enemyLineEnd.Y -= 2;
                    ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, true));
                    ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 850);
                    ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    if (ET.EnemyLineEnd2.Y >= playArea.Height || ET.EnemyLineEnd2.Y <= playArea.Y)
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                    }
                    else
                    {
                        ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    }
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    //ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    //ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                }
            }

        }


        private void calcLine2Obstruction(Rectangle wall, EnemyTank ET)
        {

            int enemyLineIntersect = 10;

            double x = 0;
            double y = 0;


            //if ((uint)(ET.EnemyLineStart2.Y - wall.Bottom) > (uint)(ET.EnemyLineStart2.Y - wall.Top)) // Calcs if top of rectangle or bottom is closer to line start. 
            {

                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart2.X, ET.EnemyLineStart2.Y + 1, ET.EnemyLineEnd2.X, ET.EnemyLineEnd2.Y, wall.X, wall.Y + wall.Height, wall.X + wall.Width, wall.Y + wall.Height);//4 Bottom of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd2 = new Vector2((float)x, (float)y);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;

                    //enemyLineEnd = enemyLineCollision;
                    //enemyLineEnd.Y -= 2;
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);


                }
            }


            // else
            {
                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart2.X, ET.EnemyLineStart2.Y - 1, ET.EnemyLineEnd2.X, ET.EnemyLineEnd2.Y, wall.X, wall.Y, wall.X + wall.Width, wall.Y);//1 Top side rectangle colision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd2 = new Vector2((float)x, (float)y);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    //desiredLineEnd = lineCollision;
                    //enemyLineEnd.Y -= 2;
                    //ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    // ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    //blockRefelection = true;
                }
            }

            //     if ((uint)(ET.EnemyLineStart.X - wall.Right) > (uint)(ET.EnemyLineStart.X - wall.Left))
            {


                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart2.X, ET.EnemyLineStart2.Y, ET.EnemyLineEnd2.X, ET.EnemyLineEnd2.Y, wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd2 = new Vector2((float)x, (float)y);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    //enemyLineEnd = enemyLineCollision;
                    //ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    //ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    // ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    //enemyLineEnd.Y -= 2;
                }
            }

            //       else
            {

                enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart2.X, ET.EnemyLineStart2.Y, ET.EnemyLineEnd2.X, ET.EnemyLineEnd2.Y, wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3 RH side of rectangle collision
                if (enemyLineIntersect == 0)
                {
                    ET.EnemyLineEnd2 = new Vector2((float)x, (float)y);
                    ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);
                    //enemyLineEnd = enemyLineCollision;
                    //enemyLineEnd.Y -= 2;
                    // ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                    // ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                    //ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line2Angle, false));
                    //ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 850);
                }
            }

        }


        private void calcLine3Obstruction(Rectangle wall, EnemyTank ET)
        {

            int enemyLineIntersect = 10;

            double x = 0;
            double y = 0;

            enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart3.X, ET.EnemyLineStart3.Y + 1, ET.EnemyLineEnd3.X, ET.EnemyLineEnd3.Y, wall.X, wall.Y + wall.Height, wall.X + wall.Width, wall.Y + wall.Height);//4 Bottom of rectangle collision
            if (enemyLineIntersect == 0)
            {
                ET.EnemyLineEnd3 = new Vector2((float)x, (float)y);
            }

            enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart3.X, ET.EnemyLineStart3.Y - 1, ET.EnemyLineEnd3.X, ET.EnemyLineEnd3.Y, wall.X, wall.Y, wall.X + wall.Width, wall.Y);//1 Top side rectangle colision
            if (enemyLineIntersect == 0)
            {
                ET.EnemyLineEnd3 = new Vector2((float)x, (float)y);
            }

            enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart3.X - 1, ET.EnemyLineStart3.Y, ET.EnemyLineEnd3.X, ET.EnemyLineEnd3.Y, wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
            if (enemyLineIntersect == 0)
            {
                ET.EnemyLineEnd3 = new Vector2((float)x, (float)y);
            }

            enemyLineIntersect = Util.intersect2D(ref x, ref y, ET.EnemyLineStart3.X + 1, ET.EnemyLineStart3.Y, ET.EnemyLineEnd3.X, ET.EnemyLineEnd3.Y, wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3 RH side of rectangle collision
            if (enemyLineIntersect == 0)
            {
                ET.EnemyLineEnd3 = new Vector2((float)x, (float)y);
            }

        }



        public void calcSurfaceCollission(Sprite3 rocket, Rectangle wall)
        {

            int enemyLineIntersect = 10;
            bool ran = false;

            double x = 0;
            double y = 0;
            enemyLineIntersect = Util.intersect2D(ref x, ref y, rocket.getPosX(), rocket.getPosY(), rocket.getPosX() + 3, rocket.getPosY(), wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3 RH side of rectangle collision
            if (enemyLineIntersect == 0)
            {
                calcRefletionAngleDegrees(rocket, true);
                rocket.hitPoints++;
                if (rocket.hitPoints < rocket.maxHitPoints) bounceSound();
            }
            enemyLineIntersect = Util.intersect2D(ref x, ref y, rocket.getPosX(), rocket.getPosY(), rocket.getPosX(), rocket.getPosY() + 3, wall.X, wall.Y + wall.Height, wall.X + wall.Width, wall.Y + wall.Height);//4 Bottom of rectangle collision
            if (enemyLineIntersect == 0)
            {
                calcRefletionAngleDegrees(rocket, false);
                rocket.hitPoints++;
                if (rocket.hitPoints < rocket.maxHitPoints) bounceSound();
            }
            enemyLineIntersect = Util.intersect2D(ref x, ref y, rocket.getPosX(), rocket.getPosY(), rocket.getPosX() + 1, rocket.getPosY() + 3, wall.X, wall.Y, wall.X + wall.Width, wall.Y);//1 Top side rectangle colision
            if (enemyLineIntersect == 0)
            {
                calcRefletionAngleDegrees(rocket, false);
                rocket.hitPoints++;
                if (rocket.hitPoints < rocket.maxHitPoints) bounceSound();
            }

            enemyLineIntersect = Util.intersect2D(ref x, ref y, rocket.getPosX(), rocket.getPosY(), rocket.getPosX() + 3, rocket.getPosY(), wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
            //enemyLineIntersect = Util.intersect2D(ref x, ref y, rocket.getPosX(), rocket.getPosY(), rocket.getPosX() + rocket.getWidth(), rocket.getPosY() + rocket.getHeight(), wall.X, wall.Y, wall.X, wall.Y + wall.Height);//2 LH side of rectangle collision
            if (enemyLineIntersect == 0)
            {
                calcRefletionAngleDegrees(rocket, true);
                rocket.hitPoints++;
                if (rocket.hitPoints < rocket.maxHitPoints) bounceSound();
            }



        }

        private void calcRefletionAngleDegrees(Sprite3 sprite, bool verticalSurfaceReflection)
        {
            //if (MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) == 0)
            //{
            //    sprite.setMoveAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(180)));
            //    sprite.setDisplayAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(180)));
            //}

            if (verticalSurfaceReflection == false)
            {
                sprite.setMoveAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) * -1));
                sprite.setDisplayAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) * -1));
            }

            else if (verticalSurfaceReflection == true)
            {
                if (MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) <= 0 && MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) >= -180)
                {
                    sprite.setMoveAngleRadians(MathHelper.ToRadians(-180 + MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) * -1));
                    sprite.setDisplayAngleRadians(MathHelper.ToRadians(-180 + MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) * -1));
                }
                else if (MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) > 0 && MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) <= 180)
                {
                    sprite.setMoveAngleRadians(MathHelper.ToRadians(180 - MathHelper.ToDegrees(sprite.getDisplayAngleRadians())));
                    sprite.setDisplayAngleRadians(MathHelper.ToRadians(180 - MathHelper.ToDegrees(sprite.getDisplayAngleRadians())));
                }

            }



        }

        private float calcRefletionAngleDegrees(float startAngle, bool verticalSurfaceReflection)
        {
            //if (MathHelper.ToDegrees(sprite.getDisplayAngleRadians()) == 0)
            //{
            //    sprite.setMoveAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(180)));
            //    sprite.setDisplayAngleRadians(MathHelper.ToRadians(MathHelper.ToDegrees(180)));
            //}
            float endAngle = 0;

            if (verticalSurfaceReflection == false)
            {
                //endAngle=(MathHelper.ToRadians(startAngle * -1));
                endAngle = startAngle * -1;
            }

            else if (verticalSurfaceReflection == true)
            {
                if (startAngle <= 0 && startAngle >= -180)
                {
                    endAngle = -180 + (startAngle * -1);
                }
                else if (startAngle > 0 && startAngle <= 180)
                {
                    endAngle = 180 - startAngle;
                }

            }
            return endAngle;
        }



        void rotateTankToVertical(Sprite3 tempSprite)
        {
            if (tempSprite.getDisplayAngleDegrees() != 0 || tempSprite.getDisplayAngleDegrees() != 180)
            {
                if (tempSprite.getDisplayAngleDegrees() > 0)
                {
                    tempSprite.setDisplayAngleDegrees(Convert.ToInt32(tempSprite.getDisplayAngleDegrees()) - tankRotateSpeed);
                }

                else if (tempSprite.getDisplayAngleDegrees() < 0)
                {
                    tempSprite.setDisplayAngleDegrees(Convert.ToInt32(tempSprite.getDisplayAngleDegrees()) + tankRotateSpeed);
                }
            }
        }

        void rotateTankToHorizontal(Sprite3 tempSprite)
        {

            if (tempSprite.getDisplayAngleDegrees() != 90 || tempSprite.getDisplayAngleDegrees() != -90)
            {
                if (tempSprite.getDisplayAngleDegrees() > 90)
                {
                    tempSprite.setDisplayAngleDegrees(Convert.ToInt32(tempSprite.getDisplayAngleDegrees()) - tankRotateSpeed);
                }

                else if (tempSprite.getDisplayAngleDegrees() < 90)
                {
                    tempSprite.setDisplayAngleDegrees(Convert.ToInt32(tempSprite.getDisplayAngleDegrees()) + tankRotateSpeed);
                }
            }
        }


        static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        private void CreateBigExplosion(float x, float y)
        {
            float scale = 0.6f;
            int xoffset = -50;
            int yoffset = -60;
            bigBoom.Play();

            Sprite3 s = new Sprite3(true, texExplosion, x + xoffset, y + yoffset);
            s.setXframes(5);
            s.setYframes(3);
            s.setWidthHeight(480 / 5, 288 / 3);

            Vector2[] anim = new Vector2[21];
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            anim[3].X = 3; anim[3].Y = 0;
            anim[4].X = 4; anim[4].Y = 0;
            anim[5].X = 0; anim[5].Y = 1;
            anim[6].X = 1; anim[6].Y = 1;
            anim[7].X = 2; anim[7].Y = 1;
            anim[8].X = 3; anim[8].Y = 1;
            anim[9].X = 3; anim[9].Y = 1;
            anim[10].X = 0; anim[10].Y = 2;
            anim[11].X = 1; anim[11].Y = 2;
            anim[12].X = 2; anim[12].Y = 2;
            anim[13].X = 3; anim[13].Y = 2;
            anim[14].X = 4; anim[14].Y = 2;
            s.setAnimationSequence(anim, 0, 11, 10);
            s.setAnimFinished(2); // make it inactive and invisible
            s.animationStart();

            expolsionList.addSpriteReuse(s); // add the sprite

        }


        private void CreateSmallExplosion(float x, float y)
        {
            float scale = 0.6f;
            int xoffset = 190;
            int yoffset = 50;
            Vector2 HSOffset = new Vector2(0, 100);
            shellExplosion.Play();


            Sprite3 s = new Sprite3(true, texSmallExplosion, x + xoffset, y + yoffset);

            s.setXframes(7);
            s.setYframes(3);
            s.setWidthHeight(448 / 7, 192 / 3);
            // s.setHSoffset(HSOffset);
            s.setBBandHSFractionOfTexCentered(0.2f);



            Vector2[] anim = new Vector2[21];
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            anim[3].X = 3; anim[3].Y = 0;
            anim[4].X = 4; anim[4].Y = 0;
            anim[5].X = 5; anim[5].Y = 0;
            anim[6].X = 6; anim[6].Y = 0;
            anim[7].X = 0; anim[7].Y = 1;
            anim[8].X = 1; anim[8].Y = 1;
            anim[9].X = 2; anim[9].Y = 1;
            anim[10].X = 3; anim[10].Y = 1;
            anim[11].X = 4; anim[11].Y = 1;
            anim[12].X = 5; anim[12].Y = 1;
            anim[13].X = 6; anim[13].Y = 1;
            anim[14].X = 0; anim[14].Y = 2;
            anim[15].X = 1; anim[15].Y = 2;
            anim[16].X = 2; anim[16].Y = 2;
            anim[17].X = 3; anim[17].Y = 2;
            anim[18].X = 4; anim[18].Y = 2;
            anim[19].X = 5; anim[19].Y = 2;
            anim[20].X = 6; anim[20].Y = 2;


            s.setAnimationSequence(anim, 0, 20, 2);
            s.setAnimFinished(2); // make it inactive and invisible
            s.animationStart();

            expolsionList.addSpriteReuse(s); // add the sprite

        }

        private void bounceSound()
        {
            if (bounceRandomSound == 1) bounceLow.Play();
            else if (bounceRandomSound == 2) bounceMid.Play();
            else if (bounceRandomSound == 3) bounceHigh.Play();
        }


        private void moveEnemyLine(EnemyTank ET) // This makes the enemy line of sight move around the map. 
        {

            if (tempPos.X <= playArea.X && tempPos.Y > playArea.Y) //Moving up
            {

                tempPos.X = playArea.X;
                tempPos.Y -= lineRotateSpeed;

                ET.EnemyLineEnd = tempPos;

                ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(ET.Line1Angle, true));

                ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 1000);
                ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);

                ET.EnemyLineStart3 = ET.EnemyLineEnd2;

                if (ET.EnemyLineEnd2.Y >= playArea.Height || ET.EnemyLineEnd2.Y <= playArea.Y)
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                }
                else
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                }



                ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 1000);
                ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);



            }
            else if (tempPos.X >= playArea.Width && tempPos.Y < playArea.Height) // moving down
            {
                tempPos.X = playArea.Width + playAreaHorizontalOffset;
                tempPos.Y += lineRotateSpeed;
                ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, true));
                ET.EnemyLineEnd = tempPos;
                ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 1000);
                ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                //tempPos = enemyLineEnd;
                ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                if (ET.EnemyLineEnd2.Y >= playArea.Height || ET.EnemyLineEnd2.Y <= playArea.Y)
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                }
                else
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                }


                ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 1000);
                ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);

            }
            else if ((tempPos.Y <= playArea.Y && tempPos.X < playArea.Width)) // moving right
            {

                tempPos.Y = playArea.Y;
                tempPos.X += lineRotateSpeed;

                ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(ET.Line1Angle, false));
                ET.EnemyLineEnd = tempPos;
                ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 1000);
                ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);
                //tempPos = enemyLineEnd;
                ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                if (ET.EnemyLineEnd2.X >= playArea.Width || ET.EnemyLineEnd2.X <= playArea.X)
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                }
                else
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                }
                ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 1000);
                ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);


            }
            else if (tempPos.Y >= playArea.Height && tempPos.X > playArea.X) //moving left
            {
                tempPos.Y = playArea.Bottom;
                tempPos.X -= lineRotateSpeed;

                //enemyLineIntersect = Util.intersect2D(ref x, ref y, enemyLineStart.X, enemyLineStart.Y, tempPos.X, tempPos.Y, wall.X + wall.Width, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);//3
                //if (enemyLineIntersect == 0)
                //{
                //    enemyLineCollision = new Vector2((float)x, (float)y);
                //    enemyLineEnd = enemyLineCollision;
                //    //enemyLineEnd.Y -= 2;
                //    line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)line2Angle, true));
                //}




                //else
                //{
                ET.EnemyLineEnd = tempPos;
                ET.Line2Angle = MathHelper.ToRadians(calcRefletionAngleDegrees((float)ET.Line1Angle, false));

                //}


                ET.EnemyLineEnd2 = moveVectorByAngleSpeed(ET.EnemyLineStart2, ET.Line2Angle, 850);
                ET.EnemyLineEnd2 = keepLineInPlayArea(ET.EnemyLineStart2, ET.EnemyLineEnd2);

                ET.EnemyLineStart3 = ET.EnemyLineEnd2;
                if (ET.EnemyLineEnd2.X >= playArea.Width || ET.EnemyLineEnd2.X <= playArea.X)
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), true));
                }
                else
                {
                    ET.Line3Angle = MathHelper.ToRadians(calcRefletionAngleDegrees(MathHelper.ToDegrees(ET.Line2Angle), false));
                }
                ET.EnemyLineEnd3 = moveVectorByAngleSpeed(ET.EnemyLineStart3, ET.Line3Angle, 1000);
                ET.EnemyLineEnd3 = keepLineInPlayArea(ET.EnemyLineStart3, ET.EnemyLineEnd3);


            }
        }






    }

}



