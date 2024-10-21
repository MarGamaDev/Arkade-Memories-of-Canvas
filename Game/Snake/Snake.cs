using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Snake : MicroGame
    {
        public float ticsPast = 0;
        public Vector2 snakeHeadDirection;
        public Vector2 snakeHeadPosition;
        public int gridCellSize = 32;
        public Vector2 snakeGridSize = new Vector2(17, 15);
        public Vector2 gridPixelOffset = new Vector2(1, 0);
        public Vector2 snakeGridPosition;
        public int snakeBodyLength = 0;
        public float snakeTicAfterTime = 0.2f;
        public Vector2 applePosition = new Vector2(2, 4);
        public List<Vector2> lastSnakeHeadPositions = new List<Vector2>();
        public float localTimePast = 0;

        public Snake(Arkade arkade) : base(arkade)
        {
            snakeHeadDirection = new Vector2(0, 0);
            snakeHeadPosition = new Vector2(5, 5);

            Cabinet = new Bitmap("snake_kabinet.png");
            backgroundColor = Color.Gray;
            drawTimer = false;

            snakeGridPosition = new Vector2((int)frameMin.X + gridPixelOffset.X, (int)frameMin.Y + gridPixelOffset.Y);
        }

        public override void Function()
        {
            // update time
            localTimePast += deltaTime;

            // update snake direction
            if (GAME_ENGINE.GetKeyDown(Key.Up) || GAME_ENGINE.GetKeyDown(Key.W) && snakeHeadDirection.Y != 1)
                snakeHeadDirection = new Vector2(0, -1);
            if (GAME_ENGINE.GetKeyDown(Key.Down) || GAME_ENGINE.GetKeyDown(Key.S) && snakeHeadDirection.Y != -1)
                snakeHeadDirection = new Vector2(0, 1);
            if (GAME_ENGINE.GetKeyDown(Key.Left) || GAME_ENGINE.GetKeyDown(Key.A) && snakeHeadDirection.X != 1)
                snakeHeadDirection = new Vector2(-1, 0);
            if (GAME_ENGINE.GetKeyDown(Key.Right) || GAME_ENGINE.GetKeyDown(Key.D) && snakeHeadDirection.X != -1)
                snakeHeadDirection = new Vector2(1, 0);

            // every tic
            if (localTimePast > ticsPast * snakeTicAfterTime)
            {
                ticsPast += 1;

                lastSnakeHeadPositions.Add(snakeHeadPosition);

                // move head
                snakeHeadPosition += snakeHeadDirection;

                //check if apple is eaten
                if (snakeHeadPosition.X == applePosition.X && snakeHeadPosition.Y == applePosition.Y)
                {
                    snakeBodyLength += 1;

                    Random rnd = new Random();
                    int x = rnd.Next(snakeGridSize.X);
                    int y = rnd.Next(snakeGridSize.Y);
                    Vector2 newApplePos = new Vector2(x, y);
                    applePosition = newApplePos;
                }
            }

            // check for head and world border collisions
            if (snakeHeadPosition.X < 0 || snakeHeadPosition.X > snakeGridSize.X || snakeHeadPosition.Y < 0 || snakeHeadPosition.Y > snakeGridSize.Y)
            {
                GameOver();
            }

            // check for head and body collisions
            for (int i = 0; i < snakeBodyLength; i++)
            {
                Vector2 bodypart = lastSnakeHeadPositions[lastSnakeHeadPositions.Count - 1 - i];

                if (snakeHeadPosition.X == bodypart.X && snakeHeadPosition.Y == bodypart.Y)
                    GameOver();
            }

            // end game on time
            if (Timer <= 0)
                GameEnd();
        }

        public override void Draw()
        {
            base.Draw();

            // draw timer
            GAME_ENGINE.SetScale(2, 2);
            GAME_ENGINE.SetColor(Color.Cyan);
            GAME_ENGINE.DrawString(((int)Timer).ToString(), frameMin.X + 4, frameMin.Y, 20, 20);
            GAME_ENGINE.SetColor(Color.Cyan);
            GAME_ENGINE.SetScale(1, 1);


            // draw all grid cells
            for (int x = 0; x < snakeGridSize.X; x++)
            {
                for (int y = 0; y < snakeGridSize.Y; y++)
                {
                    DrawCell(new Vector2(x, y), Color.White, false);
                }
            }

            // draw all body parts
            for (int i = 0; i < snakeBodyLength; i++)
            {
                DrawCell(lastSnakeHeadPositions[lastSnakeHeadPositions.Count - 1 - i], Color.Green, true);
            }

            // draw apple
            DrawCell(applePosition, Color.Red, true);

            // draw head
            DrawCell(snakeHeadPosition, Color.Green, true);
        }

        public void GameOver()
        {
            snakeHeadDirection = Vector2.zero;
            GameEnd();
        }

        public override void GameEnd()
        {
            base.GameEnd();
        }

        public void DrawCell(Vector2 coord, Color color, bool fill)
        {
            GAME_ENGINE.SetColor(color);

            if (fill)
                GAME_ENGINE.FillRectangle(snakeGridPosition.X + coord.X * gridCellSize, snakeGridPosition.Y + coord.Y * gridCellSize, gridCellSize, gridCellSize);
            else
                GAME_ENGINE.DrawRectangle(snakeGridPosition.X + coord.X * gridCellSize, snakeGridPosition.Y + coord.Y * gridCellSize, gridCellSize, gridCellSize);

            GAME_ENGINE.SetColor(Color.White);
        }
    }
}
