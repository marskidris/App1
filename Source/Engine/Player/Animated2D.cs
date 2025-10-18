using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace App1.Source.Engine;

public class Animated2D
{
    Texture2D texture;
    Texture2D itemTexture;
    Vector3 position;
    Vector3 size;
    float scale = 1f;
    float minScale = 0.5f;
    float maxScale = 3f;
    float scaleStep = 0.1f;
    float rotation = 0f;
    float rotationSpeed = MathHelper.PiOver4;
    Rectangle[] frames;
    Rectangle itemFrame;
    Vector3[] itemPositions;
    bool[] itemsCollected;
    int currentFrame;
    int direction = 1;
    float timer;
    float switchTime = 1.5f;
    
    Vector3 facingDirection;
    Vector3 playerPosition;
    float moveSpeed = 50f;
    float pickupDistance = 100f;
    float patrolDistance = 100f;
    Vector3 startPosition;
    bool movingForward = true;
    
    KeyboardState previousKeyboardState;

    public Animated2D(Texture2D texture, Vector3 position, Vector3 size)
    {
        this.texture = texture;
        this.position = position;
        this.size = size;
        
        frames = new Rectangle[4];
        frames[0] = new Rectangle(134, 215, 40, 25);
        frames[1] = new Rectangle(176, 204, 24, 36);
        frames[2] = new Rectangle(204, 206, 31, 34);
        frames[3] = new Rectangle(236, 210, 32, 30);

        itemFrame = new Rectangle(137, 10, 15, 14);
        
        itemPositions = new Vector3[3];
        itemPositions[0] = new Vector3(position.X + 150, position.Y, 0);
        itemPositions[1] = new Vector3(position.X - 150, position.Y, 0);
        itemPositions[2] = new Vector3(position.X, position.Y + 150, 0);
        
        itemsCollected = new bool[3];
        
        currentFrame = 0;
        timer = 0f;
        previousKeyboardState = Keyboard.GetState();
        
        facingDirection = new Vector3(1, 0, 0);
        playerPosition = new Vector3(100, 100, 0);
        startPosition = position;
        
        itemTexture = Globals.content.Load<Texture2D>("2D/items_scenery_tranparent");
    }

    public void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer >= switchTime)
        {
            currentFrame += direction;
            if (currentFrame == frames.Length - 1 || currentFrame == 0)
                direction *= -1;
            timer = 0f;
        }
        
        KeyboardState currentKeyboardState = Keyboard.GetState();
        
        if (currentKeyboardState.IsKeyDown(Keys.OemPlus) && !previousKeyboardState.IsKeyDown(Keys.OemPlus))
        {
            scale += scaleStep;
            if (scale > maxScale)
                scale = maxScale;
        }
        
        if (currentKeyboardState.IsKeyDown(Keys.OemMinus) && !previousKeyboardState.IsKeyDown(Keys.OemMinus))
        {
            scale -= scaleStep;
            if (scale < minScale)
                scale = minScale;
        }
        
        if (currentKeyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
        {
            rotation += rotationSpeed;
            if (rotation >= MathHelper.TwoPi)
                rotation -= MathHelper.TwoPi;
        }
        
        if (currentKeyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
        {
            rotation -= rotationSpeed;
            if (rotation < 0)
                rotation += MathHelper.TwoPi;
        }
        
        HandlePatrolMovement(gameTime);
        PerformMathOperations();
        
        previousKeyboardState = currentKeyboardState;
    }

    private void HandlePatrolMovement(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (movingForward)
        {
            position.X += moveSpeed * deltaTime;
            facingDirection = new Vector3(1, 0, 0);
            
            if (position.X >= startPosition.X + patrolDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            position.X -= moveSpeed * deltaTime;
            facingDirection = new Vector3(-1, 0, 0);
            
            if (position.X <= startPosition.X - patrolDistance)
            {
                movingForward = true;
            }
        }
    }

    private void PerformMathOperations()
    {
        Vector3 toPlayer = playerPosition - position;
        
        if (toPlayer != Vector3.Zero)
        {
            Vector3 toPlayerNormalized = Vector3.Normalize(toPlayer);
            
            float dotProduct = Vector3.Dot(facingDirection, toPlayerNormalized);
            bool isFacingPlayer = dotProduct > 0.5f;
            
            Vector3 crossProduct = Vector3.Cross(facingDirection, toPlayerNormalized);
            bool isPlayerOnRight = crossProduct.Z < 0;
            bool isPlayerOnLeft = crossProduct.Z > 0;
            
            float distanceToPlayer = Vector3.Distance(position, playerPosition);
        }
        
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (!itemsCollected[i])
            {
                float distanceToItem = Vector3.Distance(playerPosition, itemPositions[i]);
                if (distanceToItem < pickupDistance)
                {
                    itemsCollected[i] = true;
                    Console.WriteLine($"Item {i + 1} Collected");
                }
            }
        }
    }

    public void SetPlayerPosition(Vector3 playerPos)
    {
        playerPosition = playerPos;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 position2D = new Vector2(position.X, position.Y);
        Vector2 size2D = new Vector2(size.X, size.Y);
        Vector2 scaledSize = size2D * scale;
        
        Vector2 origin = new Vector2(frames[currentFrame].Width / 2f, frames[currentFrame].Height / 2f);
        
        Vector2 adjustedPosition = position2D + (size2D * scale / 2f);
        
        spriteBatch.Draw(
            texture, 
            adjustedPosition, 
            frames[currentFrame], 
            Color.White, 
            rotation,
            origin,
            scaledSize / new Vector2(frames[currentFrame].Width, frames[currentFrame].Height), 
            SpriteEffects.None, 
            position.Z);
        
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (!itemsCollected[i])
            {
                Vector2 itemPosition2D = new Vector2(itemPositions[i].X, itemPositions[i].Y);
                Vector2 itemOrigin = new Vector2(itemFrame.Width / 2f, itemFrame.Height / 2f);
                
                spriteBatch.Draw(
                    itemTexture,
                    itemPosition2D,
                    itemFrame,
                    Color.White,
                    0f,
                    itemOrigin,
                    2f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}