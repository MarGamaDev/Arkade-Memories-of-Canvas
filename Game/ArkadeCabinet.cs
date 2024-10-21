using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ArkadeCabinet : GameObject
    {
        public Vector2f position;
        public Arkade.ArkadeMicroGame myGame;
        public Bitmap bitmap;
        private Arkade arkade;
        private ArkadePlayer player;
        public Rectanglef hitBox;
        public int spriteIndex;
        public float scale;

        public ArkadeCabinet(float x, float y, Arkade.ArkadeMicroGame myGame, string cabinet, Arkade arkade, ArkadePlayer player, float scale = 1)
        {
            this.arkade = arkade;
            this.myGame = myGame;
            this.player = player;
            this.scale = scale;
            position = new Vector2f(x, y);
            bitmap = new Bitmap(cabinet);
            hitBox = new Rectanglef(x, y, bitmap.GetWidth() / 2 * scale, bitmap.GetHeight() * scale);
        }

        public override void Update()
        {
            if (arkade.mainGameState == Arkade.MainGameState.Playing)
            {
                Collision();
            }
        }

        //makes the cabinet light up if there's collision, and starts the right game when you press E
        public void Collision()
        {
            spriteIndex = 0;
            if (hitBox.X < player.hitBox.X + player.hitBox.Width && hitBox.X + hitBox.Width > player.hitBox.X)
            {
                if (hitBox.Y < player.hitBox.Y + player.hitBox.Height && hitBox.Y + hitBox.Height > player.hitBox.Y)
                {
                    spriteIndex = 1;
                    if (GAME_ENGINE.GetKeyDown(Key.E))
                    {
                        switch (myGame)
                        {
                            case Arkade.ArkadeMicroGame.Mario:
                                new MarioGame(arkade);
                                break;
                            case Arkade.ArkadeMicroGame.Pacman:
                                new Pacman(arkade);
                                break;
                            case Arkade.ArkadeMicroGame.Snake:
                                new Snake(arkade);
                                break;
                            case Arkade.ArkadeMicroGame.SpaceInvaders:
                                new SpaceInvaders(arkade);
                                break;
                            default:
                                break;
                        }
                        arkade.mainGameState = Arkade.MainGameState.MicroGamePlaying;
                    }
                }
            }
        }

        public override void Dispose()
        {
            bitmap.Dispose();
            base.Dispose();
        }
    }
}
