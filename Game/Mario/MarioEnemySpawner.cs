using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioEnemySpawner : GameObject
    {
        private List<MarioGame.SpawnListItem> spawnList;
        private MarioGame game;
        private float timer;

        public MarioEnemySpawner(List<MarioGame.SpawnListItem> spawnList, MarioGame game)
        {
            this.spawnList = spawnList;
            this.game = game;
        }

        //spawns an enemy when the it gets to their time
        public override void Update()
        {
            if (game.gameState != MicroGame.GameState.Paused)
            {
                for(int i = 0; i < spawnList.Count; i++)
                {
                    if (timer > spawnList[i].time)
                    {
                        switch (spawnList[i].enemy)
                        {
                            case MarioGame.Enemy.Goomba:
                                Console.WriteLine("spawning Goomba");
                                new MarioGoomba(game.frameMax.X, game.frameMax.Y - game.tileSize * 2, game);
                                break;
                            case MarioGame.Enemy.Spiny:
                                Console.WriteLine("spawning Spiny");
                                new MarioSpiny(game.frameMax.X, game.frameMax.Y - game.tileSize * 2, game);
                                break;
                            case MarioGame.Enemy.Turtle:
                                Console.WriteLine("spawning Turtle");
                                new MarioTurtle(game.frameMax.X, game.frameMax.Y - game.tileSize * 2, game);
                                break;
                            default:
                                break;
                        }
                        spawnList.Remove(spawnList[i]);
                    }
                }
                timer += game.deltaTime;
            }
        }
    }
}
