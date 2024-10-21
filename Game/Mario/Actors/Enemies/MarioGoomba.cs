using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioGoomba : MarioEnemy
    {
        public MarioGoomba(float x, float y, MarioGame microGame) : base(x, y, microGame)
        {
            speed = 100;
        }
    }
}
