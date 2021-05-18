using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class Obstacle : GameObject
    {
        public enum RelativePosition
        {
            left,
            right,
            top,
            bottom,
            topRightCorner,
            topLeftCorner,
            bottomLeftCorner,
            bottomRightCorner,
            single,
            topSingle,
            bottomSingle,
            leftSingle,
            rightSingle,
            verticalTunnel,
            horizontalTunnel,
            middle
        }

        private Rectangle hitBox;
        private int heightShortener = 10;
        private SpriteAnimation currentAnimation;
        private double rotation;
        private Vector2 origin;

        public int xIndex;
        public int yIndex;

        public Rectangle HitBox()
        {
            return hitBox;
        }
        public Obstacle(Vector2 position, RelativePosition relativePosition, int xIndex, int yIndex) : base(position)
        {
            this.origin = new Vector2(8, 8);
            this.xIndex = xIndex;
            this.yIndex = yIndex;
            currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(5, 0), new Vector2(5, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
            UpdateHitbox();
        }

        public void UpdateOrientation(RelativePosition relativePosition)
        {
            switch (relativePosition)
            {
                case RelativePosition.left:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(2, 0), new Vector2(2, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI;
                    break;
                case RelativePosition.right:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(2, 0), new Vector2(2, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
                case RelativePosition.top:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(2, 0), new Vector2(2, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI * 0.5;
                    break;
                case RelativePosition.bottom:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(2, 0), new Vector2(2, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI * 1.5;
                    break;
                case RelativePosition.topRightCorner:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(1, 0), new Vector2(1, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI*1.5;
                    break;
                case RelativePosition.topLeftCorner:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(1, 0), new Vector2(1, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI;
                    break;
                case RelativePosition.bottomLeftCorner:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(1, 0), new Vector2(1, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI*0.5;
                    break;
                case RelativePosition.bottomRightCorner:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(1, 0), new Vector2(1, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
                case RelativePosition.single:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(0, 0), new Vector2(0, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
                case RelativePosition.topSingle:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(3, 0), new Vector2(3, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI;
                    break;
                case RelativePosition.leftSingle:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(3, 0), new Vector2(3, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI * 1.5;
                    break;
                case RelativePosition.rightSingle:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(3, 0), new Vector2(3, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI * 0.5;
                    break;
                case RelativePosition.bottomSingle:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(3, 0), new Vector2(3, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
                case RelativePosition.verticalTunnel:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(4, 0), new Vector2(4, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
                case RelativePosition.horizontalTunnel:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(4, 0), new Vector2(4, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = Math.PI * 0.5;
                    break;
                case RelativePosition.middle:
                    currentAnimation = new SpriteAnimation(AssetManager.BoxSpriteSheet, new Vector2(5, 0), new Vector2(5, 0),
                        new Vector2(17, 16), new Vector2(51, 16));
                    rotation = 0;
                    break;
            }
        }

        public void MoveObstacle(Vector2 newPosition)
        {
            position = newPosition;
            UpdateHitbox();
        }

        private void UpdateHitbox()
        {
            hitBox = new Rectangle((int)Position.X, (int)Position.Y + heightShortener, (int)currentAnimation.FrameSize.X,
                (int)currentAnimation.FrameSize.Y - heightShortener);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, Position, (float)rotation, origin, 4);
        }
    }
}
