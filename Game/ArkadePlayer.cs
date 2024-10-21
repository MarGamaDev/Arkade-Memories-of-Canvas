using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ArkadePlayer : GameObject
    {
        private Arkade arkade;
        float moveSpeed = 175;
        public Bitmap player_sprites;
        public float floatingOffset = 0;
        float timePast = 0;
        public Vector2f position;
        public Rectanglef hitBox;
        private float deltaTime;
        public int spriteIndex;
        bool up_hold;
        bool down_hold;
        bool left_hold;
        bool right_hold;

        public ArkadePlayer(float X, float Y, Arkade arkade)
        {
            this.arkade = arkade;
            position = new Vector2f(X, Y);
            player_sprites = new Bitmap("player_sprites.png");
        }

        public override void Update()
        {
            if (arkade.mainGameState == Arkade.MainGameState.Playing)
            {
                hitBox = new Rectanglef(position.X, position.Y , 32, 32);
                deltaTime = GAME_ENGINE.GetDeltaTime();
                timePast += deltaTime;
                floatingOffset = (float)Math.Sin(timePast * 5) * 5;
                Movement();
            }
        }

        public void Movement()
        {
            // input
            bool up_hold = GAME_ENGINE.GetKey(Key.Up) || GAME_ENGINE.GetKey(Key.W);
            bool down_hold = GAME_ENGINE.GetKey(Key.Down) || GAME_ENGINE.GetKey(Key.S);
            bool left_hold = GAME_ENGINE.GetKey(Key.Left) || GAME_ENGINE.GetKey(Key.A);
            bool right_hold = GAME_ENGINE.GetKey(Key.Right) || GAME_ENGINE.GetKey(Key.D);

            // move
            if (up_hold)
            {
                position.Y -= moveSpeed * deltaTime;
                spriteIndex = 0;
            }
            if (left_hold)
            {
                position.X -= moveSpeed * deltaTime;
                spriteIndex = 3;
            }
            if (right_hold)
            {
                position.X += moveSpeed * deltaTime;
                spriteIndex = 2;
            }
            if (down_hold)
            {
                position.Y += moveSpeed * deltaTime;
                spriteIndex = 1;
            }
        }

        public override void Dispose()
        {
            player_sprites.Dispose();
            base.Dispose();
        }
    }
}
