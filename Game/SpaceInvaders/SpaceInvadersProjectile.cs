using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class SpaceInvadersProjectile : Actor
    {
        public float projectileSpeed = 250;
        public SpaceInvadersProjectile(float x, float y, MicroGame microGame) : base(x, y, microGame)
        {
            //laser bitmap
            this.microGame = microGame;
            microGame.actorList.Add(this);
            bitmap = new Bitmap("SpaceInvaders/laser-SI.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 0, 7, 1, 1, 0, 3);
            SetHitbox(spriteStruct);
        }
        public override void Function()
        {
            collisionCheck();
            //projectile speed
            position.Y -= deltaTime * projectileSpeed;
        }
        public void collisionCheck()
        {
            //checks collision with enemy
            List<SpaceInvadersEnemy> enemyList = new List<SpaceInvadersEnemy>();
            {
                foreach (Actor actor in microGame.actorList)
                {
                    switch(actor)
                    {
                        case SpaceInvadersEnemy enemy:
                            enemyList.Add(enemy);
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (SpaceInvadersEnemy enemy in enemyList)
            {
                if(microGame.CheckCollision(this, enemy))
                {
                    enemy.ExplodeEnemy();
                    this.Dispose();
                }
            }
        }
    }
}
