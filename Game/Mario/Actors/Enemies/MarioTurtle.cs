using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioTurtle : MarioEnemy
    {
        public MarioTurtle(float x , float y, MarioGame microGame) : base(x, y, microGame)
        {
            spriteStruct.index.Y = 1;
            speed = 40;
        }
    }
}
