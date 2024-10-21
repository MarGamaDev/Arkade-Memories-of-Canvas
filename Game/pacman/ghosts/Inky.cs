using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Inky : PacmanGhost
    {
        private int moveDirection;
        readonly Random random = new Random();
        private bool inked = false;
        private float inkCoolDown = 1;
        public Inky(float X, float Y, MicroGame microGame, Vector2f framemin, Vector2f framemax) : base(X, Y, microGame, framemin, framemax)
        {
            bitmap = new Bitmap("Pacman/inky.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 14, 14, 2, 4, 0.3f, 2);
            SetHitbox(spriteStruct);
            movespeed = 150;
        }
        public override void Function()
        {
            if (microGame.gameState == MicroGame.GameState.Playing)
            {
                //code for randomly changing direction
                inkCoolDown -= deltaTime;
                int randomdirection = random.Next(0, 200);
                if (randomdirection == 0)
                {
                    moveDirection = 0;
                }
                if (randomdirection == 1)
                {
                    moveDirection = 1;
                }
                if (randomdirection == 2)
                {
                    moveDirection = 2;
                }
                if (randomdirection == 3)
                {
                    moveDirection = 3;
                }
                if (randomdirection < 25 && inkCoolDown <= 0)
                {
                    inked = true;
                    inkCoolDown = 1;
                }
                if (randomdirection > 175 && inkCoolDown <= 0)
                {
                    inked = false;
                    inkCoolDown = 1;
                }
                switch (moveDirection)
                {
                    //selecting the right sprites
                    case 0:
                        if (position.X < rightBorder - spriteStruct.width * 2)
                        {
                            position.X += movespeed * deltaTime;
                            if (inked)
                            {
                                spriteStruct.index.Y = 4;
                            }
                            else
                            {
                                spriteStruct.index.Y = 0;
                            }
                        }
                        else
                        {
                            moveDirection = 1;
                        }
                        break;
                    case 1:
                        if (position.X > leftBorder)
                        {
                            position.X -= movespeed * deltaTime;
                            if (inked)
                            {
                                spriteStruct.index.Y = 4;
                            }
                            else
                            {
                                spriteStruct.index.Y = 3;
                            }
                        }
                        else
                        {
                            moveDirection = 0;
                        }
                        break;
                    case 2:
                        if (position.Y > topBorder)
                        {
                            position.Y -= movespeed * deltaTime;
                            if (inked)
                            {
                                spriteStruct.index.Y = 4;
                            }
                            else
                            {
                                spriteStruct.index.Y = 2;
                            }
                        }
                        else
                        {
                            moveDirection = 3;
                        }
                        break;
                    case 3:
                        if (position.Y < bottomBorder - spriteStruct.height * 2)
                        {
                            position.Y += movespeed * deltaTime;
                            if (inked)
                            {
                                spriteStruct.index.Y = 4;
                            }
                            else
                            {
                                spriteStruct.index.Y = 1;
                            }
                        }
                        else
                        {
                            moveDirection = 2;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (microGame.gameState == MicroGame.GameState.GameOver)
            {
                spriteStruct.index.Y = 0;
            }
        }
        public override void Animate()
        {
            //animating inky
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
    }
}
