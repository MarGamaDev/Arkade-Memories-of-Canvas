using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioSpiny : MarioEnemy
    {
        public MarioSpiny(float x, float y, MarioGame microGame) : base(x, y, microGame)
        {
            spriteStruct.index.Y = 2;
            speed = 40;
        }
    }
}
