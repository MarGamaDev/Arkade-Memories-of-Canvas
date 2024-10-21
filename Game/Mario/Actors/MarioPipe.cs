using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioPipe : Actor
    {
        private MarioGame game;
        public MarioPipe(float x, float y, MarioGame game) : base(x, y, game)
        {
            microGame = game;
            this.game = game;
            bitmap = new Bitmap("Mario/pipe.png");
            spriteStruct = new SpriteStruct(bitmap, 0, 0, 64, 64, 1, 1, 0, game.gameScale);
            SetHitbox(spriteStruct);
            game.actorList.Add(this);
        }

        public override void Function()
        {

        }
    }
}
