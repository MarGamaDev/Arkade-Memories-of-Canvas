using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class PacmanPlayer : Actor
    {
        protected float lastAnimate;
        private int moveDirection;
        private readonly int movespeed = 200;
        private readonly float leftBorder;
        private readonly float rightBorder;
        private readonly float topBorder;
        private readonly float bottomBorder;
        public PacmanPlayer(float X, float Y, MicroGame microGame, Vector2f framemin, Vector2f framemax) : base(X,Y,microGame)
        {
            this.microGame = microGame;
            bitmap = new Bitmap("Pacman/pacman.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 13, 13, 3, 4, 0.1f, 2);
            lastAnimate = mainClass.globalTimePassed;
            leftBorder = framemin.X;
            rightBorder = framemax.X;
            topBorder = framemin.Y;
            bottomBorder = framemax.Y;
            SetHitbox(spriteStruct);
        }
        public override void Function()
        {
            if (microGame.gameState == MicroGame.GameState.Playing)
            {
                PacmanMovement();
            }
        }
        //all the movement code for the player
        public void PacmanMovement()
        {
            if (GAME_ENGINE.GetKeyDown(Key.D))
            {
                spriteStruct.index.Y = 0;
                moveDirection = 0;
            }
            if (GAME_ENGINE.GetKeyDown(Key.A))
            {
                spriteStruct.index.Y = 1;
                moveDirection = 1;
            }   
            if (GAME_ENGINE.GetKeyDown(Key.W))
            {
                spriteStruct.index.Y = 2;
                moveDirection = 2;
            }
            if (GAME_ENGINE.GetKeyDown(Key.S))
            {
                spriteStruct.index.Y = 3;
                moveDirection = 3;
            }
            switch (moveDirection)
            {
                case 0:
                    if (position.X < rightBorder - spriteStruct.width * 2)
                    {
                        position.X += movespeed * deltaTime;
                    }
                    else
                    {
                        microGame.gameState = MicroGame.GameState.GameOver;
                    }
                    break;
                case 1:
                    if (position.X > leftBorder)
                    {
                        position.X -= movespeed * deltaTime;
                    }
                    else
                    {
                        microGame.gameState = MicroGame.GameState.GameOver;
                    }
                    break;
                case 2:
                    if (position.Y > topBorder)
                    {
                        position.Y -= movespeed * deltaTime;
                    }
                    else
                    {
                        microGame.gameState = MicroGame.GameState.GameOver;
                    }
                    break;
                case 3:
                    if (position.Y < bottomBorder - spriteStruct.height * 2)
                    {
                        position.Y += movespeed * deltaTime;
                    }
                    else
                    {
                        microGame.gameState = MicroGame.GameState.GameOver;
                    }
                    break;
                default:
                    break;
            }

        }
        //animating pacmanplayer
        public override void Animate()
        {
            if (microGame.gameState == MicroGame.GameState.Playing)
            {
                if (mainClass.globalTimePassed >= lastAnimate + spriteStruct.animationSpeed)
                {
                    if (spriteStruct.index.X == 2)
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
}
