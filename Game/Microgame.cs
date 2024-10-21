using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class MicroGame : GameObject
    {
        public enum GameState
        {
            Starting,
            Playing,
            Paused,
            GameOver,
            Won
        }
        public GameState gameState;
        private GameState lastGameState;
        public float deltaTime;
        public Arkade mainClass;

        private float screenHeight;
        private float screenWidth;
        private int widthOffset = 369;
        private int heightOffset = 126;
        public readonly Vector2f frameMin;
        public readonly Vector2f frameMax;
        public readonly float frameWidth;
        public readonly float frameHeight;
        public int gameScale = 1;
        protected Color backgroundColor = new Color(0,0,0);
        protected float Timer = 30;
        protected bool drawTimer = true;

        public List<Actor> actorList = new List<Actor>();
        protected Bitmap Cabinet;
        public Vector2 timerOffset = new Vector2(5, 2);

        public MicroGame(Arkade mainClass)
        {
            this.mainClass = mainClass;
            mainClass.currentMicroGame = this;
            gameState = GameState.Playing;

            screenHeight = GAME_ENGINE.GetScreenHeight();
            screenWidth = GAME_ENGINE.GetScreenWidth();
            frameMin = new Vector2f(widthOffset, heightOffset - 91);
            frameMax = new Vector2f(screenWidth - widthOffset, screenHeight - heightOffset - 91);
            frameWidth = frameMax.X - frameMin.X;
            frameHeight = frameMax.Y - frameMin.Y;
        }

        public override void Update()
        {
            if (gameState != GameState.Paused)
            {
                deltaTime = GAME_ENGINE.GetDeltaTime();
                if (Timer > 0)
                {
                    Timer -= deltaTime;
                }
                else if (Timer < 0)
                {
                    Timer = 0;
                }
                Function();
            }
        }

        public override void Paint()
        {
            if (gameState != GameState.Paused)
            {
                GAME_ENGINE.SetScale(1, 1);

                //sets background to the colour you want, default black
                GAME_ENGINE.SetColor(backgroundColor);
                GAME_ENGINE.FillRectangle(frameMin.X, frameMin.Y, frameWidth, frameHeight);

                //fillable draw function
                Draw();

                //paints all the actors you added to the actor list
                foreach (Actor actor in actorList)
                {
                    SpriteStruct sprite = actor.spriteStruct;
                    if (actor.bitmap != null)
                    {
                        GAME_ENGINE.SetScale(sprite.scale, sprite.scale);
                        GAME_ENGINE.DrawBitmap(sprite.bitmap, actor.position.X, actor.position.Y, sprite.index.X * sprite.width, sprite.index.Y * sprite.height, sprite.width, sprite.height);
                    } else
                    {
                        GAME_ENGINE.SetColor(actor.color);
                        GAME_ENGINE.FillRectangle(actor.hitBox);
                    }
                    GAME_ENGINE.SetScale(1, 1);
                }

                //paints the cabinet
                if (Cabinet != null)
                {
                    GAME_ENGINE.SetScale(4.3f, 4.3f);
                    GAME_ENGINE.DrawBitmap(Cabinet, 111, -17);
                }

                // draws a timer
                if (drawTimer)
                {
                    GAME_ENGINE.SetScale(1, 1);
                    GAME_ENGINE.SetColor(Color.Red);
                    GAME_ENGINE.FillRectangle(frameMin.X + timerOffset.X, frameMax.Y + timerOffset.Y, Timer * ((frameWidth - 10) / 30), 13);
                }
            }
        }

        //editable update function that's less error-prone
        public abstract void Function();

        //editable draw function that's less error-prone, and draws on the right layer
        public virtual void Draw()
        {

        }

        public override void Dispose()
        {
            int actors = actorList.Count();
            for (int i = 0; i < actors; i++)
            {
                actorList[0].Dispose();
            }
            if (Cabinet != null)
            {
                Cabinet.Dispose();
                Cabinet = null;
            }
            mainClass.mainGameState = Arkade.MainGameState.Playing;
            mainClass.currentMicroGame = null;
            base.Dispose();
        }

        //checks the collission between two actors
        public bool CheckCollision(Actor a, Actor b)
        {
            if (a.hitBox.X < b.hitBox.X + b.hitBox.Width && a.hitBox.X + a.hitBox.Width > b.hitBox.X)
            {
                if (a.hitBox.Y < b.hitBox.Y + b.hitBox.Height && a.hitBox.Y + a.hitBox.Height > b.hitBox.Y)
                {
                    return true;
                }
            }
            return false;
        }

        //pauses the game
        public void PauseGame()
        {
            switch (gameState)
            {
                case GameState.Paused:
                    gameState = lastGameState;
                    break;
                default:
                    lastGameState = gameState;
                    gameState = GameState.Paused;
                    break;
            }
        }
    }
}
