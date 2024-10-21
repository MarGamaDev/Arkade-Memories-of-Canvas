using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Pacman : MicroGame
    {
        readonly PacmanPlayer pacmanplayer;
        private readonly List<Actor> ghostList = new List<Actor>();
        private float returnTimer = 3;
        private float startTimer = 2;
        public Pacman(Arkade mainclass) : base(mainclass)
        {
            Cabinet = new Bitmap("Pacman/gameOverlayPacMan-final.png");
            pacmanplayer = new PacmanPlayer(frameMin.X + 268, frameMin.Y + 400, this, frameMin, frameMax);
            actorList.Add(pacmanplayer);
            ghostList.Add(new Inky(frameMin.X + 50, frameMin.Y, this, frameMin, frameMax));
            ghostList.Add(new Blinky(frameMin.X + 150, frameMin.Y, this, frameMin, frameMax));
            ghostList.Add(new Pinky(frameMin.X + 250, frameMin.Y, this, frameMin, frameMax));
            ghostList.Add(new Clyde(frameMin.X + 350, frameMin.Y, this, frameMin, frameMax));
        }
        public override void Function()
        {
            //collision check for the player and the ghosts
            foreach (Actor ghost in ghostList)
            {
                if (CheckCollision(pacmanplayer, ghost))
                {
                    gameState = GameState.GameOver;
                }
            }
            if (Timer <= 0 && gameState == GameState.Playing)
            {
                gameState = GameState.Won;
            }
            if (gameState == GameState.Won || gameState == GameState.GameOver)
            {
                returnTimer -= deltaTime;
                if (returnTimer <= 0)
                {
                    this.Dispose();
                }
            }
            if (startTimer >= 0)
            {
                startTimer -= deltaTime;
            }
        }
        public override void Draw()
        {
            //drawing the win or lose screen
            if (gameState == GameState.GameOver)
            {
                GAME_ENGINE.SetColor(Color.Blue);
                GAME_ENGINE.SetScale(5,5);
                GAME_ENGINE.DrawString("gameover", frameMin.X + 140, frameMin.Y + 100, 100, 100);
            }
            if (gameState == GameState.Won)
            {
                GAME_ENGINE.SetColor(Color.Blue);
                GAME_ENGINE.SetScale(5,5);
                GAME_ENGINE.DrawString("congratulations", frameMin.X + 75, frameMin.Y + 100, 100, 100);
            }
            if (gameState == GameState.Playing && startTimer >= 0)
            {
                GAME_ENGINE.SetColor(Color.Blue);
                GAME_ENGINE.SetScale(5, 5);
                GAME_ENGINE.DrawString("survive", frameMin.X + 175, frameMin.Y + 100, 100, 100);
            }
        }
    }
}
