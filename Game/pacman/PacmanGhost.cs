using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    abstract public class PacmanGhost : Actor
    {
        protected float lastAnimate;
        protected int movespeed = 250;
        protected float leftBorder;
        protected float rightBorder;
        protected float topBorder;
        protected float bottomBorder;
        public PacmanGhost(float X, float Y, MicroGame microGame, Vector2f framemin, Vector2f framemax) : base(X, Y, microGame)
        {

            this.microGame = microGame;
            lastAnimate = mainClass.globalTimePassed;
            leftBorder = framemin.X;
            rightBorder = framemax.X;
            topBorder = framemin.Y;
            bottomBorder = framemax.Y;
            microGame.actorList.Add(this);
        }
        public override void Function()
        {

        }

    }
}
