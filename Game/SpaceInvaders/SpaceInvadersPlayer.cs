using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SpaceInvadersPlayer : Actor
    {
        public Key leftkey, rightkey;
        private float borderLeft;
        private float borderRight;
        private float fireTimer = 2;
        public SpaceInvadersPlayer(float x, float y, MicroGame microGame, float leftFrame, float rightFrame) : base(x, y, microGame)
        {
            //player bitmap
            this.microGame = microGame;
            bitmap = new Bitmap("SpaceInvaders/player-SI.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 11, 8, 1, 1, 1, 3);
            //borders
            borderLeft = leftFrame;
            borderRight = rightFrame;
        }
        public override void Function()
        {
            Shoot();
            movementPlayer();
            fireTimer += deltaTime;
        }
        public void movementPlayer()
        {
            //player movement
            if (GAME_ENGINE.GetKey(rightkey) && position.X < borderRight - spriteStruct.width * 3)
            {
                position.X += deltaTime * 130;
            }
            if (GAME_ENGINE.GetKey(leftkey) && position.X > borderLeft)
            {
                position.X -= deltaTime * 130;
            }
        }
        public void Shoot()
        {
            //player shoot
            if (GAME_ENGINE.GetKeyDown(Key.Space) && fireTimer >= 0.7f)
            {
                new SpaceInvadersProjectile((position.X + spriteStruct.width + 4), position.Y, microGame);
                fireTimer = 0;
            }
        }
    }
}
