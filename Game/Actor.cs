using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class Actor : GameObject
    {
        // general actor variables
        public Vector2f position;
        public int bitmapDrawScale = 1;
        public Rectanglef hitBox;
        public float speed;
        protected MicroGame microGame;
        protected float deltaTime;
        public Arkade mainClass; // a refference to the main class to acces variables like globalTimePast

        // bitmap variables
        public Color color = new Color(255,255,255);
        public SpriteStruct spriteStruct;
        public Bitmap bitmap;
        public Vector2 bitmapSelection = new Vector2(0, 0);
        public Vector2 bitmapSpriteSize;
        public float timeBitmapAnimationStarted; // this variable is important for animation timing


        public Actor(float x, float y, MicroGame microGame)
        {
            position = new Vector2f(x,y);
            this.microGame = microGame;
            mainClass = microGame.mainClass;
            timeBitmapAnimationStarted = mainClass.globalTimePassed;
        }

        public override void Update()
        {
            if (microGame.gameState != MicroGame.GameState.Paused)
            {
                deltaTime = GAME_ENGINE.GetDeltaTime();
                hitBox = new Rectanglef(position.X, position.Y, hitBox.Width, hitBox.Height);
                if (spriteStruct.bitmap != null)
                {
                    Animate();
                }
                Function();
            }
        }

        public override void Dispose()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
                if (spriteStruct.bitmap != null)
                {
                    spriteStruct.bitmap.Dispose();
                    spriteStruct.bitmap = null;
                }
            }
            else
            {
                Console.WriteLine("disposed unlisted actor");
            }
            microGame.actorList.Remove(this);
            base.Dispose();
        }

        //editable update function that's less error-prone
        public abstract void Function();

        //editable function to animate through sprites if you have a spritestruct
        public virtual void Animate()
        {

        }

        public float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public Vector2f Lerp(Vector2f firstVector, Vector2f secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2f(retX, retY);
        }

        public float Distance(Vector2f firstVector, Vector2f secondVector)
        {
            Vector2f deltaPos = new Vector2f(secondVector.X - firstVector.X, secondVector.Y - firstVector.Y);
            float distance = (float)Math.Sqrt((double)(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y));
            return distance;
        }

        //lerps at a constant speed for moving vectors
        public Vector2f LerpDistance(Vector2f firstVector, Vector2f secondVector, float by)
        {
            float t = 1 / Distance(firstVector, secondVector) * speed * deltaTime;
            float retX = Lerp(firstVector.X, secondVector.X, t);
            float retY = Lerp(firstVector.Y, secondVector.Y, t);
            return new Vector2f(retX, retY);
        }

        //sets your hitbox to spritesize
        public void SetHitbox(SpriteStruct sprite)
        {
            hitBox = new Rectanglef(position.X, position.Y, sprite.width * sprite.scale, sprite.height * sprite.scale);
        }
    }
}
