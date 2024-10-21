using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MarioGame : MicroGame
    {
        public enum Enemy
        {
            Goomba,
            Spiny,
            Turtle
        }
        public struct SpawnListItem
        {
            public Enemy enemy;
            public float time;
            public SpawnListItem(Enemy enemy, float time)
            {
                this.enemy = enemy;
                this.time = time;
            }
        }
        private List<SpawnListItem> spawnList = new List<SpawnListItem>();
        private MarioEnemySpawner spawner;
        private MarioPlayer mario;
        private MarioPipe pipe;
        public float tileSize;
        public float levelSpeed;
        public List<MarioBackgroundItem> sceneList = new List<MarioBackgroundItem>();
        private float gameEndTimer = 3;

        public MarioGame(Arkade arkade) : base(arkade)
        {
            gameScale = 3;
            tileSize = (16 * gameScale);
            Cabinet = new Bitmap("Mario/MarioCabinet.png");
            backgroundColor = new Color(146, 144, 255);

            mario = new MarioPlayer(frameMin.X + 30, frameMax.Y - tileSize * 2, this);
            pipe = new MarioPipe(frameMax.X, frameMax.Y - tileSize * 5, this);
            new MarioBackgroundItem(frameMin.X - 30, frameMax.Y - tileSize * 3 - 9, this, "Mario/mountain.png");
            new MarioBackgroundItem(frameMin.X + 500, frameMax.Y - tileSize * 2, this, "Mario/bush.png");
            new MarioBackgroundItem(frameMin.X + 300, frameMax.Y - tileSize * 8, this, "Mario/cloud.png");
            new MarioBackgroundItem(frameMin.X + 620, frameMax.Y - tileSize * 6, this, "Mario/cloud.png");
            FloorGen();
            InitSpawner();
        }

        public override void Function()
        {
            CollisionManager();
            GameOver();
            levelSpeed = 200 * deltaTime;
        }

        public override void Draw()
        {
            foreach (MarioBackgroundItem item in sceneList)
            {
                if (item.bitmap != null)
                {
                    GAME_ENGINE.SetScale(gameScale, gameScale);
                    GAME_ENGINE.DrawBitmap(item.spriteStruct.bitmap, item.position);
                }
                if (gameState != GameState.GameOver && gameState != GameState.Paused)
                {
                    item.position.X -= levelSpeed;
                }
            }
        }

        public void FloorGen()
        {
            //generates the floor
            for (int i = 0; i < Math.Round(frameWidth / (16 * gameScale)) + 2; i++)
            {
                new MarioBackgroundItem(frameMin.X + tileSize * (i), frameMax.Y - tileSize, this, "Mario/brick.png", true);
            }
        }

        public void InitSpawner()
        {
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 0));

            spawnList.Add(new SpawnListItem(Enemy.Goomba, 2));

            spawnList.Add(new SpawnListItem(Enemy.Turtle, 2.8f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 3.5f));

            spawnList.Add(new SpawnListItem(Enemy.Goomba, 5f));

            spawnList.Add(new SpawnListItem(Enemy.Spiny, 7));
            spawnList.Add(new SpawnListItem(Enemy.Spiny, 7.9f));
            spawnList.Add(new SpawnListItem(Enemy.Spiny, 8.8f));

            spawnList.Add(new SpawnListItem(Enemy.Goomba, 11));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 12));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 13));

            spawnList.Add(new SpawnListItem(Enemy.Turtle, 13.6f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 13.8f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 14f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 14.2f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 14.4f));
            spawnList.Add(new SpawnListItem(Enemy.Spiny, 14.6f));

            spawnList.Add(new SpawnListItem(Enemy.Goomba, 17));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 17.2f));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 18.6f));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 18.8f));
            spawnList.Add(new SpawnListItem(Enemy.Spiny, 20.2f));
            spawnList.Add(new SpawnListItem(Enemy.Spiny, 20.4f));

            spawnList.Add(new SpawnListItem(Enemy.Turtle, 22f));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 22.2f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 22.8f));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 23f));
            spawnList.Add(new SpawnListItem(Enemy.Turtle, 23.6f));
            spawnList.Add(new SpawnListItem(Enemy.Goomba, 23.8f));

            spawner = new MarioEnemySpawner(spawnList, this);
        }

        public void GameOver()
        {
            if (Timer <= 3 && gameState != GameState.GameOver)
            {
                pipe.position.X -= levelSpeed;
                gameState = GameState.Won;
            }

            if (gameState != GameState.Playing)
            {
                if (gameEndTimer <= 0)
                {
                    this.Dispose();
                }
                gameEndTimer -= deltaTime;
            }
        }

        //compares hitboxes of all actors with mario
        public void CollisionManager()
        {
            foreach (Actor actor in actorList)
            {
                if (mario.state != MarioPlayer.State.dying)
                {
                    if (CheckCollision(actor, mario))
                    {
                        switch (actor)
                        {
                            case MarioTurtle turtle:
                                TopCollision(turtle);
                                break;
                            case MarioGoomba goomba:
                                TopCollision(goomba);
                                break;
                            case MarioSpiny Spiny:
                                mario.Die();
                                break;
                            case MarioPipe p:
                                if (mario.piped == false)
                                {
                                    mario.position.Y -= 3;
                                    mario.piped = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        //makes mario jump if he lands ontop of the enemy
        public void TopCollision(MarioEnemy enemy)
        {
            if (enemy.isAlive)
            {
                if (mario.hitBox.Y + mario.hitBox.Height < enemy.hitBox.Y + (enemy.hitBox.Height * 0.4))
                {
                    enemy.isAlive = false;
                    mario.velocity = mario.speed;
                    mario.state = MarioPlayer.State.jumping;
                }
                else
                {
                    mario.Die();
                }
            }
        }

        public override void Dispose()
        {
            spawner.Dispose();
            foreach (MarioBackgroundItem item in sceneList)
            {
                item.Dispose();
            }
            base.Dispose();
        }
    }
}
