using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    class SpriteAnimation
    {
        private Texture2D texture;
        private Vector2 start;      // What index in the spritesheet the animation will start usually at 0,0 unless the spritesheet contains multiple animations
        private Vector2 end;        // What index in the spritesheet the current animation ends at
        private Vector2 frameSize;  // How big the frames are in X and Y
        private Vector2 dimensions; // How many rows and columns there are in the sprite sheet
        private SpriteEffects spriteFX;
        private float msSinceLastFrame;
        private float msBetweenFrames;

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
                msSinceLastFrame += (float)Game1.elapsedGameTimeMilliseconds;
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(texture, position, Source, Color.White, rotation, origin, scale, spriteFX, 1.0f);
        }
    }
}
