using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine
{
    public class Arkade : AbstractGame
    {
        public enum ArkadeMicroGame
        {
            Mario,
            Pacman,
            Snake,
            SpaceInvaders
        }
        public enum MainGameState
        {
            SplashScreen,
            StartMenu,
            Playing,
            Paused,
            MicroGamePlaying,
            Credits
        }
        public enum ButtonFunction
        {
            Lobby,
            Resume,
            Quit,
            Credits
        }
        public MainGameState mainGameState;
        public MainGameState lastMainGameState;
        public MicroGame currentMicroGame;
        public float deltaTime;
        public float globalTimePassed = 0.0f;

        public Vector2 floorCoord = new Vector2(0, 0);
        public Vector2 leftWallCoord = new Vector2(1, 0);
        public Vector2 rightWallCoord = new Vector2(2, 0);
        public Vector2 topWallCoord = new Vector2(3, 0);
        public Vector2 bottomWallCoord = new Vector2(4, 0);
        public Vector2 topRightCornerCoord = new Vector2(5, 0);
        public Vector2 topLeftCornerCoord = new Vector2(6, 0);
        public Vector2 bottomRightCornerCoord = new Vector2(7, 0);
        public Vector2 bottomLeftCornerCoord = new Vector2(8, 0);

        public Vector2 arcadeSize = new Vector2(20, 11);
        public Vector2 arcadePosition = new Vector2(0, 0);
        public Color backgroundColor = new Color(66, 57, 58);

        public Bitmap tileSetBitmap;
        public Bitmap cursorBitmap;
        public Bitmap fontBitmap;
        public Bitmap splashscreenBitmap;
        public Bitmap menuBitmap;
        public Bitmap titleBitmap;
        public Bitmap pauseBitmap;
        public Bitmap brokenCabBitmap;
        public Bitmap creditBitmap;
        public float splashTimer = 2;

        public ArkadePlayer player;
        public ArkadeCabinet marioCabinet;
        public ArkadeCabinet pacmanCabinet;
        public ArkadeCabinet snakeCabinet;
        public ArkadeCabinet spaceInvadersCabinet;
        private List<ArkadeCabinet> cabList= new List<ArkadeCabinet>();

        private MenuButton lobby;
        private MenuButton resume;
        private MenuButton quit;
        private MenuButton credits;
        private List<MenuButton> mainMenuButtons = new List<MenuButton>();
        private List<MenuButton> pauseMenuButtons = new List<MenuButton>();

        public override void GameStart()
        {
            mainGameState = MainGameState.SplashScreen;
            player = new ArkadePlayer(arcadePosition.X + 610, arcadePosition.Y + 550, this);
            tileSetBitmap = new Bitmap("testArcadeTileset.png");
            cursorBitmap = new Bitmap("Menus/cursor.png");
            fontBitmap = new Bitmap("Menus/font_bitmap_10x16.png");
            splashscreenBitmap = new Bitmap("Menus/splash_screen.png");
            menuBitmap = new Bitmap("Menus/menu_backdrop.png");
            titleBitmap = new Bitmap("Menus/GameTitle.png");
            pauseBitmap = new Bitmap("Menus/pause.png");
            brokenCabBitmap = new Bitmap("arcadekasten_textures/broken_arcadekast.png");
            creditBitmap = new Bitmap("credits-spritesheet.png");
            InitCabinets();
        }

        public override void GameEnd()
        {
            marioCabinet.Dispose();
            pacmanCabinet.Dispose();
            snakeCabinet.Dispose();
            spaceInvadersCabinet.Dispose();
            player.Dispose();
            DisposeMenuButtons(mainMenuButtons);
            DisposeMenuButtons(pauseMenuButtons);

            splashscreenBitmap.Dispose();
            fontBitmap.Dispose();
            cursorBitmap.Dispose();
            tileSetBitmap.Dispose();
            menuBitmap.Dispose();
            titleBitmap.Dispose();
            pauseBitmap.Dispose();
            brokenCabBitmap.Dispose();
            creditBitmap.Dispose();
        }
        
        public override void Update()
        {
            deltaTime = GAME_ENGINE.GetDeltaTime();

            //toggles pause using escape
            if (GAME_ENGINE.GetKeyDown(Key.Escape))
            {
                if (mainGameState != MainGameState.SplashScreen && mainGameState != MainGameState.StartMenu)
                {
                    switch (mainGameState)
                    {
                        case MainGameState.Paused:
                            mainGameState = lastMainGameState;
                            DisposeMenuButtons(pauseMenuButtons);
                            break;
                        default:
                            lastMainGameState = mainGameState;
                            MakePauseMenu();
                            mainGameState = MainGameState.Paused;
                            break;
                    }
                    if (currentMicroGame != null)
                    {
                        currentMicroGame.PauseGame();
                    }
                }
            }

            switch (mainGameState)
            {
                case MainGameState.SplashScreen:
                    if (splashTimer < 0)
                    {
                        mainGameState = MainGameState.StartMenu;
                        MakeMainMenu();
                    }
                    splashTimer -= deltaTime;
                    break;
                case MainGameState.Playing:
                    globalTimePassed += deltaTime;
                    TrapPlayer();
                    break;
                case MainGameState.MicroGamePlaying:
                    globalTimePassed += deltaTime;  
                    break;
                default:
                    break;
            }

        }

        public override void Paint()
        {
            GAME_ENGINE.SetScale(1, 1);
            Cursor.Hide();

            switch (mainGameState)
            {
                case MainGameState.SplashScreen:
                    DrawSplashScreen();
                    break;
                case MainGameState.StartMenu:
                    DrawMenu();
                    DrawCursor();
                    break;
                case MainGameState.Playing:
                    DrawRoom();
                    DrawCabinets();
                    DrawPlayer();
                    break;
                case MainGameState.Paused:
                    DrawPause();
                    DrawCursor();
                    break;
                case MainGameState.Credits:
                    DrawCredits();
                    break;
                default:
                    break;
            }

            GAME_ENGINE.SetScale(1, 1);
        }

        //lobby stuff
        public void InitCabinets()
        {
            float width = (GAME_ENGINE.GetScreenWidth() - 300) / 7;
            marioCabinet = new ArkadeCabinet(width * 1 + 150 - 16 * 3, 180 * 0 + 100, ArkadeMicroGame.Mario, "arcadekasten_textures/mario_arcadekast.png", this, player, 3);
            pacmanCabinet = new ArkadeCabinet(width * 3 + 150 - 16 *3, 180 * 2 + 100, ArkadeMicroGame.Pacman, "arcadekasten_textures/pacman_arcadekast.png", this, player, 3);
            snakeCabinet = new ArkadeCabinet(width * 4 + 150 - 16 * 3, 180 * 1 + 100, ArkadeMicroGame.Snake, "arcadekasten_textures/snake_arcadekast.png", this, player, 3);
            spaceInvadersCabinet = new ArkadeCabinet(width * 6 + 150 - 16 * 3, 180 * 0 + 100, ArkadeMicroGame.SpaceInvaders, "arcadekasten_textures/spaceinvaders_arcadekast.png", this, player, 3);
            cabList.Add(marioCabinet);
            cabList.Add(pacmanCabinet);
            cabList.Add(snakeCabinet);
            cabList.Add(spaceInvadersCabinet);
        }
        
        public void DrawCabinets()
        {
            float width = (GAME_ENGINE.GetScreenWidth() - 300) / 7;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    DrawBrokenCab(width * i + 150 - 16 * 3, 180 * j + 100, 0, 3);
                }
            }
            foreach (ArkadeCabinet cab in cabList)
            {
                GAME_ENGINE.SetScale(cab.scale, cab.scale);
                GAME_ENGINE.DrawBitmap(cab.bitmap, cab.position.X, cab.position.Y, cab.spriteIndex * 32, 0, 32, 32);
            }
            //test screen
            DrawBrokenCab(width * 1 + 150 - 16 * 3, 180 * 2 + 100, 3, 3);
            DrawBrokenCab(width * 4 + 150 - 16 * 3, 180 * 2 + 100, 3, 3);
            //spiderweb
            DrawBrokenCab(width * 2 + 150 - 16 * 3, 180 * 1 + 100, 2, 3);
            DrawBrokenCab(width * 6 + 150 - 16 * 3, 180 * 2 + 100, 2, 3);
            DrawBrokenCab(width * 0 + 150 - 16 * 3, 180 * 0 + 100, 2, 3);
            //broken
            DrawBrokenCab(width * 7 + 150 - 16 * 3, 180 * 0 + 100, 1, 3);
            DrawBrokenCab(width * 5 + 150 - 16 * 3, 180 * 1 + 100, 1, 3);
            DrawBrokenCab(width * 3 + 150 - 16 * 3, 180 * 0 + 100, 1, 3);


            GAME_ENGINE.ResetScale();
        }

        public void DrawBrokenCab(float x, float y, int index, float scale)
        {
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(brokenCabBitmap, x, y, 32 * index, 0, 32, 32);
            GAME_ENGINE.ResetScale();
        }

        public void DrawRoom()
        {
            // draw arcade
            GAME_ENGINE.SetScale(2, 2);

            GAME_ENGINE.SetColor(backgroundColor);
            GAME_ENGINE.FillRectangle(0, 0, GAME_ENGINE.GetScreenWidth(), GAME_ENGINE.GetScreenHeight());
            GAME_ENGINE.SetColor(Color.White);

            // draw floor
            for (int x = 0; x < arcadeSize.X; x++)
            {
                for (int y = 0; y < arcadeSize.Y; y++)
                {
                    // draw floor
                    DrawTile(floorCoord, new Vector2(x, y));

                    // draw walls
                    if (x == 0)
                        DrawTile(leftWallCoord, new Vector2(x, y));
                    if (x == arcadeSize.X - 1)
                        DrawTile(rightWallCoord, new Vector2(x, y));
                    if (y == 0)
                        DrawTile(topWallCoord, new Vector2(x, y));
                    if (y == arcadeSize.Y - 1)
                        DrawTile(bottomWallCoord, new Vector2(x, y));

                    // draw corners
                    if (x == 0 && y == 0)
                        DrawTile(topLeftCornerCoord, new Vector2(x, y));
                    if (x == 0 && y == arcadeSize.Y - 1)
                        DrawTile(bottomLeftCornerCoord, new Vector2(x, y));
                    if (x == arcadeSize.X - 1 && y == 0)
                        DrawTile(topRightCornerCoord, new Vector2(x, y));
                    if (x == arcadeSize.X - 1 && y == arcadeSize.Y - 1)
                        DrawTile(bottomRightCornerCoord, new Vector2(x, y));
                }
            }
        }

        public void DrawTile(Vector2 tileCoord, Vector2 cellCoord)
        {
            GAME_ENGINE.DrawBitmap(tileSetBitmap, cellCoord.X * 32 * 2 + arcadePosition.X, cellCoord.Y * 32 * 2 + arcadePosition.Y, tileCoord.X * 32, tileCoord.Y * 32, 32, 32);
        }

        //player stuff
        public void DrawPlayer()
        {
            GAME_ENGINE.SetScale(2, 2);
            GAME_ENGINE.DrawBitmap(player.player_sprites, player.position.X, player.position.Y + player.floatingOffset, 32 * player.spriteIndex, 0, 32, 32);
            GAME_ENGINE.ResetScale();
        }
        
        public void TrapPlayer()
        {
            // lock player in arcade :<
            if (player.position.X < arcadePosition.X + 32 * 1.5f)
                player.position.X += 1;
            if (player.position.Y < arcadePosition.Y + 32 * 1.5f)
                player.position.Y += 1;
            if (player.position.X > arcadePosition.X + (arcadeSize.X * 32 * 2) - 110)
                player.position.X -= 1;
            if (player.position.Y > arcadePosition.Y + (arcadeSize.Y * 32 * 2) - 128)
                player.position.Y -= 1;
        }

        //main menu stuff
        public void MakeMainMenu()
        {
            float scale = 4;
            lobby = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 400, scale, ButtonFunction.Lobby, this);
            credits = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 500, scale, ButtonFunction.Credits, this);
            quit = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 600, scale, ButtonFunction.Quit, this);
            mainMenuButtons.Add(lobby);
            mainMenuButtons.Add(credits);
            mainMenuButtons.Add(quit);
        }

        public void DrawMenu()
        {
            float scale = GAME_ENGINE.GetScreenHeight() / menuBitmap.GetHeight();
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(menuBitmap, GAME_ENGINE.GetScreenWidth() / 2 - menuBitmap.GetWidth() * scale / 2, 0);
            scale = 5;
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(titleBitmap, GAME_ENGINE.GetScreenWidth() / 2 - titleBitmap.GetWidth() * scale / 2, 30);
            DrawMenuButtons(mainMenuButtons);
            DrawText(460, 408, 3.8f, "play game");
            DrawText(510, 508, 3.8f, "credits");
            DrawText(560, 608, 3.8f, "quit");
            GAME_ENGINE.SetScale(1, 1);
        }

        //pause menu stuff
        public void MakePauseMenu()
        {
            float scale = 4;
            lobby = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 400, scale, ButtonFunction.Lobby, this);
            resume = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 500, scale, ButtonFunction.Resume, this);
            quit = new MenuButton(GAME_ENGINE.GetScreenWidth() / 2 - 100 * scale, 600, scale, ButtonFunction.Quit, this);
            pauseMenuButtons.Add(lobby);
            pauseMenuButtons.Add(resume);
            pauseMenuButtons.Add(quit);
        }

        public void DrawPause()
        {
            float scale = GAME_ENGINE.GetScreenHeight() / menuBitmap.GetHeight();
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(menuBitmap, GAME_ENGINE.GetScreenWidth() / 2 - menuBitmap.GetWidth() * scale / 2, 0);
            scale = 7;
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(pauseBitmap, GAME_ENGINE.GetScreenWidth() / 2 - titleBitmap.GetWidth() * scale / 2, 80);
            DrawMenuButtons(pauseMenuButtons);
            DrawText(530, 408, 3.8f, "lobby");
            DrawText(510, 508, 3.8f, "resume");
            DrawText(560, 608, 3.8f, "quit");
            GAME_ENGINE.SetScale(1, 1);
        }
        
        public void DrawMenuButtons(List<MenuButton> buttons)
        {
            foreach (MenuButton button in buttons)
            {
                if (button.bitmap != null)
                {
                    GAME_ENGINE.SetScale(button.scale, button.scale);
                    switch (button.isSelected)
                    {
                        case true:
                            GAME_ENGINE.DrawBitmap(button.bitmapSelected, button.position);
                            break;
                        case false:
                            GAME_ENGINE.DrawBitmap(button.bitmap, button.position);
                            break;
                    }
                }
            }
            GAME_ENGINE.SetScale(1, 1);
        }
        
        //gets called by buttons when you click them
        public void ClickMenuButton(ButtonFunction purpose)
        {
            if (mainGameState == MainGameState.StartMenu)
            {
                DisposeMenuButtons(mainMenuButtons);
            }
            switch (purpose)
            {
                case ButtonFunction.Lobby:
                    if (currentMicroGame != null)
                    {
                        currentMicroGame.Dispose();
                    }
                    mainGameState = MainGameState.Playing;
                    break;
                case ButtonFunction.Resume:
                    mainGameState = lastMainGameState;
                    if (currentMicroGame != null)
                    {
                        currentMicroGame.PauseGame();
                    }
                    DisposeMenuButtons(pauseMenuButtons);
                    break;
                case ButtonFunction.Quit:
                    Application.Exit();
                    break;
                case ButtonFunction.Credits:
                    mainGameState = MainGameState.Credits;
                    break;
                default:
                    Console.WriteLine("faulty button");
                    break;
            }
        }
        
        public void DisposeMenuButtons(List<MenuButton> buttons)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Dispose();
            }
            buttons.Clear();
        }
        
        public void DrawCursor()
        {
            GAME_ENGINE.SetScale(4, 4);
            GAME_ENGINE.DrawBitmap(cursorBitmap, new Vector2f(GAME_ENGINE.GetMousePosition().X, GAME_ENGINE.GetMousePosition().Y));
            GAME_ENGINE.SetScale(1, 1);
        }

        //draws a string at coordinates
        public void DrawText(float x, float y, float scale, string text)
        {
            GAME_ENGINE.SetScale(scale, scale);
            char[] characters = text.ToCharArray();
            int offset = 97;
            float letterWidth = fontBitmap.GetWidth() / 26;
            float letterHeight = fontBitmap.GetHeight();
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] - offset < 26 && characters[i] - offset >= 0)
                {
                    GAME_ENGINE.DrawBitmap(fontBitmap, x + i * ((letterWidth * scale) + 1), y, (characters[i] - offset) * letterWidth, 0, letterWidth, letterHeight);
                }
            }
            GAME_ENGINE.SetScale(1, 1);
        }

        //draws for splashscreen and credits
        public void DrawSplashScreen()
        {
            GAME_ENGINE.DrawBitmap(splashscreenBitmap, new Vector2f(0, 0));
        }

        public void DrawCredits()
        {
            float scale = GAME_ENGINE.GetScreenHeight() / menuBitmap.GetHeight();
            GAME_ENGINE.SetScale(scale, scale);
            GAME_ENGINE.DrawBitmap(menuBitmap, GAME_ENGINE.GetScreenWidth() / 2 - menuBitmap.GetWidth() * scale / 2, 0);
            DrawText(500, 50, 3.0f, "credits");
            GAME_ENGINE.SetScale(5,5);
            GAME_ENGINE.DrawBitmap(creditBitmap, 270, 140, 16, 0, 16, 16);
            GAME_ENGINE.DrawBitmap(creditBitmap, 875, 280, 0, 0, 16, 16);
            GAME_ENGINE.DrawBitmap(creditBitmap, 270, 440, 32, 0, 16, 16);
            GAME_ENGINE.DrawBitmap(creditBitmap, 875, 580, 48, 0 , 16, 16);
            DrawText(375, 150, 3.0f, "mar gama broeken");
            DrawText(425, 300, 3.0f, "bart verschuur");
            DrawText(375, 450, 3.0f, "floris de ruiter");
            DrawText(425, 600, 3.0f, "sjoerd wouters");
            GAME_ENGINE.ResetScale();
        }
    }
}
