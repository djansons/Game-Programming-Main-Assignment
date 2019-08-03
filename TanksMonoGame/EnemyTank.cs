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
    class EnemyTank : playLevel
    {
        SpriteBatch spriteBatch;
        //Texture2D texEnemyTankBody = null;
        private Texture2D texEnemyTankGun = null;
        public Texture2D TexEnemyTankGun { get; set; }
        private Texture2D texEnemyTankBody = null;
        public Texture2D TexEnemyTankBody { get; set; }

        private Sprite3 enemyTankBody;
        public Sprite3 EnemyTankBody { get; set; }
        private Sprite3 enemyTankGun;
        public Sprite3 EnemyTankGun { get; set; }

        private float enemyTankStartPosX = 600;
        public float EnemyTankStartPosX { get; set; }
        private float enemyTankStartPosY = 100;
        public float EnemyTankStartPosY { get; set; }
        private float line1Angle = 0;
        public float Line1Angle { get; set; }
        private float line2Angle = 0;
        public float Line2Angle { get; set; }
        private float line3Angle = 0;
        public float Line3Angle { get; set; }      

        private Vector2 enemyLineStart = new Vector2(10, 10);
        public Vector2 EnemyLineStart { get; set; }
        private Vector2 enemyLineEnd = new Vector2(300, 10);
        public Vector2 EnemyLineEnd { get; set; }
        private Vector2 enemyLineCollision = new Vector2(0, 0);
        public Vector2 EnemyLineCollision { get; set; }
        private Vector2 enemyLineStart2 = new Vector2(10, 10);
        public Vector2 EnemyLineStart2 { get; set; }
        private Vector2 enemyLineEnd2 = new Vector2(300, 10);
        public Vector2 EnemyLineEnd2 { get; set; }
        private Vector2 enemyLineCollision2 = new Vector2(0, 0);
        public Vector2 EnemyLineCollision2 { get; set; }
        private Vector2 enemyLineStart3 = new Vector2(10, 10);
        public Vector2 EnemyLineStart3 { get; set; }
        private Vector2 enemyLineEnd3 = new Vector2(300, 10);
        public Vector2 EnemyLineEnd3 { get; set; }
        private Vector2 enemyLineCollision3 = new Vector2(0, 0);
        public Vector2 EnemyLineCollision3 { get; set; }


        
        //Enemy Tank Initilization

        public EnemyTank(string texBody, string texGun, float enemyTankStartPosX, float enemyTankStartPosY)
        {
            
            
            texEnemyTankBody = Content.Load<Texture2D>(texBody);
            texEnemyTankGun = Content.Load<Texture2D>(texGun);
            EnemyTankBody = new Sprite3(true, texEnemyTankBody, enemyTankStartPosX, enemyTankStartPosY);
            EnemyTankBody.setWidthHeight(EnemyTankBody.getWidth() / 1.5f, EnemyTankBody.getHeight() / 1.5f);
            EnemyTankBody.setBBandHSFractionOfTexCentered(0.9f);


            EnemyTankGun = new Sprite3(true, texEnemyTankGun, enemyTankStartPosX, enemyTankStartPosY);
            EnemyTankGun.setWidthHeight(EnemyTankGun.getWidth() / 1.3f, EnemyTankGun.getHeight() / 1.3f);
            EnemyTankGun.setBBandHSFractionOfTexCentered(1.0f);
            EnemyTankGun.setDisplayAngleOffsetDegrees(90);

            EnemyLineStart = EnemyTankBody.getPos();
            EnemyLineEnd = new Vector2(0, 400);

            EnemyLineStart2 = EnemyLineEnd;
            EnemyLineEnd2 = new Vector2(100, 100);
        }

    }

}