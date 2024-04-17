using Silk.NET.Maths;
using System;
using TheAdventure;

namespace TheAdventure.Models
{
    public class PetObject : RenderableGameObject
    {
        private int _pixelsPerSecond = 192;
        private string _currentAnimation = "IdleDown";

        public PetObject(SpriteSheet spriteSheet, int x, int y) : base(spriteSheet, (x, y))
        {
            SpriteSheet.ActivateAnimation(_currentAnimation);
         
        }

        public void Move(double up, double down, double left, double right, double time)
        {
            // Calculate the movement in each direction based on the player's movement
            var moveX = _pixelsPerSecond * (right - left) * time;
            var moveY = _pixelsPerSecond * (down - up) * time;

            // Call the UpdateNpcPosition method with the calculated movement
            UpdateNpcPosition(moveX, moveY, time);
        }

        public void UpdateNpcPosition(double moveX, double moveY, double time)
        {
            var x = Position.X + (int)moveX;
            var y = Position.Y + (int)moveY;

            if (x > Position.X && _currentAnimation != "MoveRight")
            {
                _currentAnimation = "MoveRight";
                SpriteSheet.ActivateAnimation(_currentAnimation);
            }
            if (x < Position.X && _currentAnimation != "MoveLeft")
            {
                _currentAnimation = "MoveLeft";
                SpriteSheet.ActivateAnimation(_currentAnimation);
            }

            Position = (x, y);
        }
    }
}
