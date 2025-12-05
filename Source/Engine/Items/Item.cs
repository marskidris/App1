using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using App1.Source.Engine.Collision;
using System;

namespace App1.Source.Engine.Items;

public class Item
{
    private Texture2D texture;
    private Vector2 position;
    private Vector2 size;
    private float scale = 2f;
    private Rectangle sourceRectangle;
    private bool hasCollided = false;
    private float pickupDistance = 80f;

    public Item(Texture2D texture, Vector2 position, Vector2 size, Rectangle sourceRect, float scale = 2f)
    {
        this.texture = texture;
        this.position = position;
        this.size = size;
        this.sourceRectangle = sourceRect;
        this.scale = scale;
    }

    public void CheckCollisionWithPlayer(Rectangle playerBoundingBox, Vector2 playerCircleCenter, float playerCircleRadius)
    {
        if (!IsActive) return;

        // Check rectangle collision
        bool rectangleCollision = Collision2D.CheckRectangleCollision(BoundingBox, playerBoundingBox);

        // Check circle collision
        bool circleCollision = Collision2D.CheckCircleCollision(CircleCenter, CircleRadius, playerCircleCenter, playerCircleRadius);

        // If collision detected
        if (rectangleCollision || circleCollision)
        {
            if (!hasCollided)
            {
                Console.WriteLine("item collected");
                hasCollided = true;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;

        Vector2 origin = new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f);
        Vector2 drawPosition = new Vector2(
            position.X + (size.X * scale / 2f),
            position.Y + (size.Y * scale / 2f)
        );

        spriteBatch.Draw(
            texture,
            drawPosition,
            sourceRectangle,
            Color.White,
            0f,
            origin,
            scale,
            SpriteEffects.None,
            0f
        );
    }

    // Collision properties
    public bool IsActive { get; set; } = true;

    public Rectangle BoundingBox
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)scaledSize.X,
                (int)scaledSize.Y
            );
        }
    }

    public Vector2 CircleCenter
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return new Vector2(
                position.X + scaledSize.X / 2,
                position.Y + scaledSize.Y / 2
            );
        }
    }

    public float CircleRadius
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return System.Math.Min(scaledSize.X, scaledSize.Y) / 2;
        }
    }

    public Vector2 Position => position;
}

