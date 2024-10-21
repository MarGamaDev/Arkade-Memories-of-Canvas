using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Hides the basic setup from the XYZ class.
    /// </summary>
    public class AbstractGame : GameObject
    {
        /// <summary>
        /// Sets up several default values for the gameview.
        /// </summary>
        public override void GameInitialize()
        {
            // Set the required values
            GAME_ENGINE.SetTitle("Arkade: Memories of Canvas");
            GAME_ENGINE.SetIcon("logo.ico");
            GAME_ENGINE.SetScale(1, 1);

            // Set the optional values
            GAME_ENGINE.SetScreenWidth(1280);
            GAME_ENGINE.SetScreenHeight(720);
            GAME_ENGINE.SetBackgroundColor(0, 0, 0); //Appelblauwzeegroen
            //GAME_ENGINE.SetBackgroundColor(49, 77, 121); //The Unity background color
        }
    }
}
