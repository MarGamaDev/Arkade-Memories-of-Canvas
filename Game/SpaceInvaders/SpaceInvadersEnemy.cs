using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SpaceInvadersEnemy : Actor
    {
        private bool isBoss = false;
        private bool leftMovement = true;
        private float lastTime;
        private SpaceInvaders spaceInvaders;
        private bool bossLeftMovement = false;
        private bool bossLife = false;
        public enum EnemyState
        {
            alive,
            exploding
        }
        public EnemyState enemyState;
        private Bitmap explosion;
        public SpaceInvadersEnemy(float x, float y, SpaceInvaders spaceInvaders, int enemyType) : base(x, y, spaceInvaders)
        {
            //enemy bitmaps
            this.microGame = spaceInvaders;
            this.spaceInvaders = spaceInvaders;
            spaceInvaders.actorList.Add(this);
            lastTime = mainClass.globalTimePassed;
            speed = 50;
            switch (enemyType)
            {
                case 0:
                    bitmap = new Bitmap("SpaceInvaders/enemy1Animation-11x8.png");
                    spriteStruct = new SpriteStruct(bitmap, 0, 0, 11, 8, 2, 1, 0.5f, 3); //1
                    break;
                case 1:
                    bitmap = new Bitmap("SpaceInvaders/enemy2Animation-8x8.png");
                    spriteStruct = new SpriteStruct(bitmap, 0, 0, 8, 8, 2, 1, 0.5f, 3); //2
                    break;
                case 2:
                    bitmap = new Bitmap("SpaceInvaders/enemy3Animation-12x8.png");
                    spriteStruct = new SpriteStruct(bitmap, 0, 0, 12, 8, 2, 1, 0.5f, 3); //3
                    break;
                case 3:
                    bitmap = new Bitmap("SpaceInvaders/enemyBoss-SI2.png");
                    spriteStruct = new SpriteStruct(bitmap, 0, 0, 16, 7, 1, 1, 1, 3); //boss
                    isBoss = true;
                    speed = 165;
                    bossLife = true;
                    break;
                default:
                    this.Dispose();
                    break;
            }
            SetHitbox(spriteStruct);
            explosion = new Bitmap("Spaceinvaders/enemyExplosion-SI.png");

        }
        public override void Function()
        {
            //disposes the enemy
            if (enemyState == EnemyState.exploding && mainClass.globalTimePassed >= lastTime + spriteStruct.animationSpeed)
            {
                this.Dispose();
            }
            enemyMovement();
        }
        public override void Animate()
        {
            //animates the enemies
            if (enemyState == EnemyState.alive && mainClass.globalTimePassed >= lastTime + spriteStruct.animationSpeed)
            {
                if (spriteStruct.index.X == spriteStruct.amountX - 1)
                {
                    spriteStruct.index.X = 0;
                }
                else
                {
                    spriteStruct.index.X += 1;
                }
                lastTime = mainClass.globalTimePassed;
            }
        }
        public void ExplodeEnemy()
        {
            //changes enemy bitmap when exploding
            enemyState = EnemyState.exploding;
            spriteStruct = new SpriteStruct(explosion, 0, 0, 13, 9, 1, 1, 0.5f, 3);
            lastTime = mainClass.globalTimePassed;
        }
        public override void Dispose()
        {
            //win conditions
            if (explosion != null)
            {
                explosion.Dispose();
                explosion = null;
                if (spriteStruct.bitmap != null)
                {
                    spriteStruct.bitmap.Dispose();
                    spriteStruct.bitmap = null;
                    if (isBoss)
                    {
                        Console.WriteLine(bossLife.ToString());
                        microGame.gameState = MicroGame.GameState.Won;
                    }
                }
            }
            base.Dispose();
        }
        public void enemyMovement()
        {
            //enemy movement
            if (enemyState == EnemyState.alive) 
            {
                switch (isBoss)
                {
                    case true:
                        if (bossLeftMovement == true)
                        {
                            position.X -= deltaTime * speed;
                            if (position.X <= microGame.frameMin.X + 10)
                            {
                                bossLeftMovement = false;
                            }
                        }
                        if (bossLeftMovement == false)
                        {
                            position.X += deltaTime * speed;
                            if (position.X + hitBox.Width >= microGame.frameMax.X - 10)
                            {
                                bossLeftMovement = true;
                            }
                        }
                        break;
                    case false:
                        if (spaceInvaders.leftMovement == true)
                        {
                            position.X -= deltaTime * speed;
                            if (position.X <= microGame.frameMin.X + 10)
                            {
                                spaceInvaders.leftMovement = false;
                            }
                        }
                        if (spaceInvaders.leftMovement == false)
                        {
                            position.X += deltaTime * speed;
                            if (position.X + hitBox.Width >= microGame.frameMax.X - 10)
                            {
                                spaceInvaders.leftMovement = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
