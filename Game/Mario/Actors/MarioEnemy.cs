using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class MarioEnemy : Actor
    {
        protected float lastAnimate;
        public bool isAlive = true;
        protected MarioGame game;

        public MarioEnemy(float x, float y, MarioGame microGame) : base(x, y, microGame)
        {
            this.microGame = microGame;
            game = microGame;
            bitmap = new Bitmap("Mario/Enemies.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 16, 16, 7, 2, 0.2f, microGame.gameScale);
            SetHitbox(spriteStruct);
            microGame.actorList.Add(this);
            lastAnimate = mainClass.globalTimePassed;
        }

        public override void Function()
        {
            if (isAlive)
            {
                position.X -= speed * deltaTime;
            }

            if (game.gameState == MicroGame.GameState.Playing)
            {
                position.X -= game.levelSpeed;
            }

            if (position.X + hitBox.Width < microGame.frameMin.X)
            {
                this.Dispose();
            }
        }

        public override void Animate()
        {
            if (isAlive)
            {
                if (mainClass.globalTimePassed >= lastAnimate + spriteStruct.animationSpeed)
                {
                    if (spriteStruct.index.X == 1)
                    {
                        spriteStruct.index.X = 0;
                    }
                    else
                    {
                        spriteStruct.index.X++;
                    }
                    lastAnimate = mainClass.globalTimePassed;
                }
            }
            else
            {
                spriteStruct.index.X = 2;
            }
        }
    }
}
