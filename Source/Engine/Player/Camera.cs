using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App1.Source.Engine.Player;

public class Camera
{
    private Vector2 position;
    private readonly Viewport viewport;

    public Camera(Viewport viewport)
    {
        this.viewport = viewport;
        position = Vector2.Zero;
    }

    public Vector2 Position
    {
        get => position;
        set => position = value;
    }

    public void Follow(Vector2 targetPosition)
    {
        position = targetPosition;
    }

    public Matrix GetTransformMatrix()
    {
        return Matrix.CreateTranslation(
            -position.X + viewport.Width / 2f,
            -position.Y + viewport.Height / 2f,
            0f);
    }

    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        return screenPosition + position - new Vector2(viewport.Width / 2f, viewport.Height / 2f);
    }

    public Vector2 WorldToScreen(Vector2 worldPosition)
    {
        return worldPosition - position + new Vector2(viewport.Width / 2f, viewport.Height / 2f);
    }
}