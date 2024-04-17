using Silk.NET.Maths;
using System;
using TheAdventure;

namespace TheAdventure.Models
{
    public class NpcObject : RenderableGameObject
    {
        private int _pixelsPerSecond = 192;
        private string _currentAnimation = "IdleDown";

        public NpcObject(SpriteSheet spriteSheet, int x, int y) : base(spriteSheet, (x, y))
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

            Position = (x, y);
        }
    }
}
