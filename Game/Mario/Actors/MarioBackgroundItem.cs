using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioBackgroundItem : Actor
    {
        private MarioGame game;
        private bool isFloor;

        public MarioBackgroundItem(float x, float y, MarioGame microGame, string filePath, bool isFloor = false) : base(x, y, microGame)
        {
            this.microGame = microGame;
            game = microGame;
            game.sceneList.Add(this);
            bitmap = new Bitmap(filePath);
            spriteStruct = new SpriteStruct(bitmap, 0, 0, (int)bitmap.GetWidth(), (int)bitmap.GetHeight(), 1, 1, 0, microGame.gameScale);
            SetHitbox(spriteStruct);
            this.isFloor = isFloor;
        }

        public override void Function()
        {
            if (position.X + hitBox.Width < microGame.frameMin.X)
            {
                if (isFloor)
                {
                    position.X += 13 * game.tileSize;
                } 
                else
                {
                    position.X = game.frameMax.X;
                }
            }
        }
    }
}
