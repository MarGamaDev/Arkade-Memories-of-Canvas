using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MenuButton : GameObject
    {
        public Vector2f position;
        public Rectanglef hitBox;
        private Arkade arkade;
        public Bitmap bitmap;
        public Bitmap bitmapSelected;
        public bool isSelected;
        public float scale;
        private Arkade.ButtonFunction purpose;

        public MenuButton(float x, float y, float scale, Arkade.ButtonFunction purpose, Arkade arkade)
        {
            this.arkade = arkade;
            this.scale = scale;
            this.purpose = purpose;
            position = new Vector2f(x, y);
            bitmap = new Bitmap("Menus/lege_button.png");
            bitmapSelected = new Bitmap("Menus/lege_button_selected.png");
            hitBox = new Rectanglef(x, y, bitmap.GetWidth() * scale, bitmap.GetHeight() * scale);
        }

        public override void Update()
        {
            //makes the button light up when you hover over it, and makes it do a thing when you click it
            isSelected = false;
            Vector2f mouse = new Vector2f(GAME_ENGINE.GetMousePosition().X, GAME_ENGINE.GetMousePosition().Y);
            if (mouse.X > hitBox.X && mouse.X < hitBox.X + hitBox.Width)
            {
                if (mouse.Y > hitBox.Y && mouse.Y < hitBox.Y + hitBox.Height)
                {
                    isSelected = true;
                    if (GAME_ENGINE.GetMouseButtonDown(0))
                    {
                        arkade.ClickMenuButton(purpose);
                    }
                }
            }
        }

        public override void Dispose()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
            if (bitmapSelected != null)
            {
                bitmapSelected.Dispose();
                bitmapSelected = null;
            }
            base.Dispose();
        }
    }
}
