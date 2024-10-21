using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    //a struct used to set your hitbox and animate something
    public struct SpriteStruct
    {
        public Bitmap bitmap;
        public int startY;
        public int startX;
        public Vector2f index;
        public int width;
        public int height;
        public int amountX;
        public int amountY;
        public float animationSpeed;
        public int scale;

        public SpriteStruct(Bitmap bitmap, int startX, int startY, int width, int height, int amountX, int amountY, float animationSpeed, int scale = 1)
        {
            this.bitmap = bitmap;
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.amountX = amountX;
            this.amountY = amountY;
            this.animationSpeed = animationSpeed;

            this.index = new Vector2f(startX, startY);
            this.scale = scale;
        }
    }
}
