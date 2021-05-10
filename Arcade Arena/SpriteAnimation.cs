using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class SpriteAnimation
    {
        private Texture2D texture;
        private Vector2 start;      // What index in the spritesheet the animation will start usually at 0,0 unless the spritesheet contains multiple animations
        private Vector2 end;        // What index in the spritesheet the current animation ends at
        private Vector2 frameSize;  // How big the frames are in X and Y
        private Vector2 dimensions; // How many rows and columns there are in the sprite sheet
        private SpriteEffects spriteFX;
        private float msSinceLastFrame = 0;
        private float msBetweenFrames;
        public double speedAlteration = 1;

        private int xIndex, yIndex;

        public SpriteAnimation(Texture2D texture, Vector2 start, Vector2 end, Vector2 frameSize, Vector2 dimensions, float msBetweenFrames = 50)
        {
            this.texture = texture;
            this.start = start;
            this.end = end;
            this.frameSize = frameSize;
            this.dimensions = dimensions;

            this.msBetweenFrames = msBetweenFrames;
            xIndex = (int)start.X;
            yIndex = (int)start.Y;

            Loop = 0;
        }

        public Rectangle Source => new Rectangle((int)(start.X*frameSize.X + (frameSize.X * (xIndex-(int)start.X))), 
            (int)(start.Y * (frameSize.Y * (yIndex-(int)start.Y+1))), (int)frameSize.X, (int)frameSize.Y);

        public int XIndex { get { return xIndex; } set { xIndex = value; } }
        public int YIndex { get { return yIndex; } set { yIndex = value; } }

        public Vector2 FrameSize { get { return frameSize; } set { frameSize = value; } }

        public Texture2D Texture { get { return texture; } set { texture = value; } }

        public int Loop { get; private set; }

        public void Update()
        {
            if (msSinceLastFrame >= msBetweenFrames)
            {
                msSinceLastFrame = 0;
                if (xIndex >= dimensions.X || xIndex >= end.X)
                {
                    if (yIndex >= end.Y && xIndex >= end.X)
                    {
                        xIndex = (int)start.X;
                        yIndex = (int)start.Y;
                    }
                    else if (xIndex >= end.X)
                    {
                        xIndex = 0;
                        yIndex++;
                    }

                    if (yIndex >= dimensions.Y )
                    {
                        yIndex = (int)start.Y;
                    }
                }
                else
                {
                    xIndex++;
                }
                Loop++;
            }
            else
            {
                msSinceLastFrame += (float)Game1.elapsedGameTimeMilliseconds * (float)speedAlteration;
            }
           
            if(ProjectileCharacter.orbiterRotation >= 1.53269 || ProjectileCharacter.orbiterRotation <= -1.547545)
            {
                    spriteFX = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteFX = SpriteEffects.None;
            }
        }

        public void StartAnimation()
        {
            XIndex = (int)start.X;
            yIndex = (int)start.Y;
            Loop = 0;
            msSinceLastFrame = 0;

            //Denna metoden kallas varje frame, vilket i sin tur gör så att man aldrig kommer att kunna gå igenom sin walk animation
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(texture, position, Source, Color.White, rotation, origin, scale, spriteFX, 1.0f);
        }
    }
}
