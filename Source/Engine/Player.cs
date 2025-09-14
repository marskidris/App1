using System.Numerics;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public struct PlayerFrame
{
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rect;

    public PlayerFrame(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
        Rect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }
}

public class Player
{
    public PlayerFrame[] Frames;
    public int CurrentFrame;

    public Player(PlayerFrame[] frames)
    {
        Frames = frames;
        CurrentFrame = 0;
    }

    public void Move(Vector2 direction)
    {
        Frames[CurrentFrame].Position += direction;
        Frames[CurrentFrame].Rect = new Rectangle(
            (int)Frames[CurrentFrame].Position.X,
            (int)Frames[CurrentFrame].Position.Y,
            (int)Frames[CurrentFrame].Size.X,
            (int)Frames[CurrentFrame].Size.Y
        );
    }

    public void Update(Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
    {
        Vector2 move = Vector2.Zero;
        if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W)) move.Y -= 1;
        if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S)) move.Y += 1;
        if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)) move.X -= 1;
        if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) move.X += 1;
        if (move != Vector2.Zero)
            Move(move);
    }
}