using Silk.NET.Maths;
using TheAdventure;

namespace TheAdventure.Models;

public class PlayerObject : RenderableGameObject
{
    private int _pixelsPerSecond = 192;

    private string _currentAnimation = "IdleDown";


    public PlayerObject(SpriteSheet spriteSheet, int x, int y) : base(spriteSheet, (x, y))
    {
        SpriteSheet.ActivateAnimation(_currentAnimation);
       
    }

    public void UpdatePlayerPosition(double up, double down, double left, double right, int width, int height,
        double time)
    {
        var pixelsToMove = time * _pixelsPerSecond;

        var x = Position.X + (int)(right * pixelsToMove);
        x -= (int)(left * pixelsToMove);

        var y = Position.Y - (int)(up * pixelsToMove);
        y += (int)(down * pixelsToMove);

        if (x < 10)
        {
            x = 10;
        }

        if (y < 24)
        {
            y = 24;
        }

        if (x > width - 10)
        {
            x = width - 10;
        }

        if (y > height - 6)
        {
            y = height - 6;
        }

        if(x > Position.X && _currentAnimation != "MoveRight"){
            _currentAnimation = "MoveRight";
            SpriteSheet.ActivateAnimation(_currentAnimation);
        }
        if (x < Position.X && _currentAnimation != "MoveLeft"){
            _currentAnimation = "MoveLeft";
            SpriteSheet.ActivateAnimation(_currentAnimation);
        }

        if (y > Position.Y && _currentAnimation != "MoveDown")
        {
            _currentAnimation = "MoveDown";
            SpriteSheet.ActivateAnimation(_currentAnimation);
        }
        else if (y < Position.Y && _currentAnimation != "MoveUp")
        {
            _currentAnimation = "MoveUp";
            SpriteSheet.ActivateAnimation(_currentAnimation);
        }
        Position = (x, y);
    }
}