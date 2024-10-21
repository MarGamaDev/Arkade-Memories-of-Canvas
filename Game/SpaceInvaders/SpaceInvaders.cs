using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SpaceInvaders : MicroGame
    {
        SpaceInvadersPlayer player;
        SpaceInvadersEnemy enemy;
        SpaceInvadersProjectile projectile;
        public float enemyY = 150;
        public float enemyY2 = 200;
        public float bossY = 50;
        public bool leftMovement;
        public bool rightMovement;
        public List<SpaceInvadersEnemy> enemyList = new List<SpaceInvadersEnemy>();
        public float endTimer = 3;
        
        public SpaceInvaders(Arkade arkade) : base (arkade)
        {
            //Creates the enemies
            Cabinet = new Bitmap("SpaceInvaders/gameOverlaySpaceInvaders-final.png");
            leftMovement = false;
            rightMovement = false;
            player = new SpaceInvadersPlayer(frameMin.X + 268, frameMin.Y + 400, this, frameMin.X, frameMax.X);
            actorList.Add(player);
            float Swarm = frameWidth - 100;
            for (int i = 0; i < 5; i++)
            {
                int eType = i;
                switch (eType)
                {
                    case 3:
                        eType = 1;
                        break;
                    case 4:
                        eType = 0;
                        break;
                    default:
                        break;
                }
                new SpaceInvadersEnemy(frameMin.X + 50 + (i * Swarm / 5), enemyY, this, eType);
                new SpaceInvadersEnemy(frameMin.X + 50 + (i * Swarm / 5), enemyY2, this, eType);
            }
            SpaceInvadersEnemy Boss = new SpaceInvadersEnemy(frameMin.X + 200, frameMin.Y + bossY, this, 3);
            //Player movement
            player.leftkey = Key.A;
            player.rightkey = Key.D;   
        }
        public override void Function()
        {
            //Adds enemies to actorlist
            enemyList.Clear();
            foreach (Actor actor in actorList)
            {
                switch (actor)
                {
                    case SpaceInvadersEnemy enemy:
                        enemyList.Add(enemy);
                        break;
                    default:
                        break;
                }
            }
            //ends game when times runs out
            if (Timer <= 0)
            {
                this.Dispose();
            }
            //ends game when you win
            if (gameState == MicroGame.GameState.Won)
            {
                if (endTimer <= 0)
                {
                    this.Dispose();
                }
                endTimer -= deltaTime;
            }
        }

        public override void Draw()
        {
            // win conditions
            if (gameState == MicroGame.GameState.Won && enemyList.Count != 0)
            {
                backgroundColor = Color.White;
                mainClass.DrawText(frameMin.X + 225, frameMin.Y + 200, 1, "you win");
            }
            if (gameState == MicroGame.GameState.Won && enemyList.Count == 0)
            {
                backgroundColor = Color.White;
                mainClass.DrawText(frameMin.X + 130, frameMin.Y + 200, 1, "you completed this level");
            }
        }
        //enemy movement
        public Boolean GetLeftMovement()
        {
            return leftMovement;
        }
             
    }
}
