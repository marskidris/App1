using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using App1.Source.Engine;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public class Player
{
    Texture2D texture;
    Vector2 position;
    Vector2 size;
    Earl earl;
    int currentFrame;
    int direction = 1;
    float timer;
    float switchTime = 0.1f;
    bool isMoving = false;
    Rectangle[] currentFrames;

    private const int DIR_UP = 0;
    private const int DIR_DOWN = 1; 
    private const int DIR_LEFT = 2;
    private const int DIR_RIGHT = 3;

    public Player(string texturePath, Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        earl = new Earl();
        
        try
        {
            texture = Globals.content.Load<Texture2D>(texturePath);
        }
        catch (ContentLoadException)
        {
            string pngPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", texturePath + ".png");
            if (File.Exists(pngPath))
            {
                using var fs = File.OpenRead(pngPath);
                texture = Texture2D.FromStream(Globals.spriteBatch.GraphicsDevice, fs);
            }
            else
            {
                throw;
            }
        }
        
        currentFrame = 0;
        timer = 0f;
        currentFrames = earl.GetFrames(false, false, direction);
    }

    public void Move(Vector2 movement)
    {
        position += movement;
        isMoving = movement.Length() > 0;
    }

    public void SetDirection(int dir)
    {
        if (dir != direction)
        {
            currentFrame = 0;
            direction = dir;
        }
    }

    public void ResetAnimation()
    {
        currentFrame = 0;
        timer = 0f;
    }

    public void Update(GameTime gameTime, MovementState movementState)
    {
        Rectangle[] newFrames;
        switch (movementState)
        {
            case SneakState when isMoving:
                newFrames = earl.GetFrames(false, true, direction);
                break;
            case RunningState when isMoving:
                newFrames = earl.GetFrames(true, false, direction);
                break;
            default:
                newFrames = earl.GetFrames(false, false, direction);
                break;
        }
        if (currentFrames != newFrames)
        {
            currentFrames = newFrames;
            currentFrame = 0;
            timer = 0f;
        }
        if (isMoving)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= switchTime)
            {
                currentFrame++;
                timer = 0f;
                if (currentFrame >= currentFrames.Length)
                    currentFrame = 0;
            }
        }
        else
        {
            currentFrame = 0;
            timer = 0f;
        }
    }

    public void Draw()
    {
        if (texture == null) return;

        Globals.spriteBatch.Draw(
            texture,
            new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
            currentFrames[currentFrame],
            Color.White,
            0.0f,
            new Vector2(0, 0),
            SpriteEffects.None,
            0.0f
        );
    }


    public Vector2 Position => position;
}