using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioPlayer : Actor
    {
        public enum State
        {
            running,
            jumping,
            dying
        }

        public bool piped = false;
        public State state;
        private float lastAnimate;
        public float velocity;
        private readonly float gravity = 1700f;

        public MarioPlayer(float x, float y, MicroGame microGame) : base (x, y, microGame)
        {
            this.microGame = microGame;
            bitmap = new Bitmap("Mario/mario.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 16, 16, 7, 2, 0.08f, microGame.gameScale);
            SetHitbox(spriteStruct);
            microGame.actorList.Add(this);
            state = State.running;
            lastAnimate = mainClass.globalTimePassed;
            speed = 800;
            velocity = speed;
        }

        public override void Function()
        {
            switch (state)
            {
                case State.running:
                    if (GAME_ENGINE.GetKey(Key.Space))
                    {
                        state = State.jumping;
                    }
                    break;
                case State.jumping:
                    Jump();
                    break;
                case State.dying:
                    Jump();
                    if (position.Y > microGame.frameMax.Y)
                    {
                        this.Dispose();
                    }
                    break;
                default:
                    break;
            }
        }

        //shows the right part of the bitmap based on mario's state
        public override void Animate()
        {
            switch (state)
            {
                case State.running:
                    if (mainClass.globalTimePassed >= lastAnimate + spriteStruct.animationSpeed)
                    {
                        if (spriteStruct.index.X >= 3)
                        {
                            spriteStruct.index.X = 1;
                        }
                        else
                        {
                            spriteStruct.index.X++;
                        }
                        lastAnimate = mainClass.globalTimePassed;
                    }
                    break;
                case State.jumping:
                    spriteStruct.index.X = 5;
                    break;
                case State.dying:
                    spriteStruct.index.X = 6;
                    break;
                default:
                    break;
            }
        }

        //jumps in an arc using gravity and velocity
        public void Jump()
        {
            position.Y -= velocity * deltaTime;
            velocity -= gravity * deltaTime;
            if (state == State.jumping && position.Y + hitBox.Height > microGame.frameMax.Y - hitBox.Height)
            {
                state = State.running;
                position.Y = microGame.frameMax.Y - hitBox.Height * 2;
                velocity = speed;
            }
        }

        public void Die()
        {
            velocity = speed;
            state = State.dying;
            microGame.gameState = MicroGame.GameState.GameOver;
        }
    }
}
