using System.Numerics;
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
    Rectangle[] playerFramesS, playerFramesA, PlayerFramesW, PlayerFramesD;
    int currentFrame;
    int direction = 1;
    float timer;
    float switchTime = 0.1f; // Faster animation speed
    bool isMoving = false;

    public Player(string texturePath, Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        
        // Load texture using Globals content manager
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
        
        playerFramesS = new Rectangle[6];
        playerFramesS[0] = new Rectangle(5, 87, 22, 34);
        playerFramesS[1] = new Rectangle(37, 85, 22, 35);
        playerFramesS[2] = new Rectangle(70, 82, 22, 39); 
        playerFramesS[3] = new Rectangle(105, 86, 21, 34);
        playerFramesS[4] = new Rectangle(137, 85, 22, 34);
        playerFramesS[5] = new Rectangle(173, 85, 19, 35);
        playerFramesA = new Rectangle[7];
        playerFramesA[0] = new Rectangle(208, 85, 26, 34);
        playerFramesA[1] = new Rectangle(239, 85, 32, 34);
        playerFramesA[2] = new Rectangle(274, 87, 32, 32);
        playerFramesA[3] = new Rectangle(310, 84, 22, 35);
        playerFramesA[4] = new Rectangle(347, 81, 25, 39);
        playerFramesA[5] = new Rectangle(373, 84, 37, 34);
        playerFramesA[6] = new Rectangle(413, 84, 30, 33);
        PlayerFramesW = new Rectangle[7];
        PlayerFramesW[0] = new Rectangle(5, 127, 21, 36);
        PlayerFramesW[1] = new Rectangle(38, 124, 21, 39);
        PlayerFramesW[2] = new Rectangle(68, 128, 21, 35);
        PlayerFramesW[3] = new Rectangle(95, 130, 24, 36);
        PlayerFramesW[4] = new Rectangle(125, 130, 19, 33);
        PlayerFramesW[5] = new Rectangle(149, 129, 23, 34);
        PlayerFramesW[6] = new Rectangle(178, 129, 21, 34);
        PlayerFramesD = new Rectangle[7];
        PlayerFramesD[0] = new Rectangle(208, 127, 25, 34);
        PlayerFramesD[1] = new Rectangle(238, 127, 32, 34);
        PlayerFramesD[2] = new Rectangle(274, 129, 32, 32);
        PlayerFramesD[3] = new Rectangle(316, 126, 22, 35);
        PlayerFramesD[4] = new Rectangle(343, 123, 22, 39);
        PlayerFramesD[5] = new Rectangle(373, 126, 37, 34);
        PlayerFramesD[6] = new Rectangle(412, 126, 32, 33);
        currentFrame = 0;
        timer = 0f;
    }

    public void Move(Vector2 movement)
    {
        position += movement;
        isMoving = movement != Vector2.Zero;
    }

    public void SetDirection(string dir)
    {
        int newDirection = dir switch
        {
            "W" => 0, // Up
            "S" => 1, // Down  
            "A" => 2, // Left
            "D" => 3, // Right
            _ => direction
        };
        
        // Reset frame when direction changes to prevent out of bounds
        if (newDirection != direction)
        {
            currentFrame = 0;
            direction = newDirection;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (isMoving)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timer >= switchTime)
            {
                currentFrame++;
                timer = 0f;
                
                Rectangle[] currentFrames = direction switch
                {
                    0 => PlayerFramesW,
                    1 => playerFramesS,
                    2 => playerFramesA,
                    3 => PlayerFramesD,
                    _ => playerFramesS
                };
                
                if (currentFrame >= currentFrames.Length)
                    currentFrame = 0;
            }
        }
        else
        {
            currentFrame = 0; 
        }
    }

    public void Draw()
    {
        if (texture == null) return;

        Rectangle[] currentFrames = direction switch
        {
            0 => PlayerFramesW,
            1 => playerFramesS,
            2 => playerFramesA,
            3 => PlayerFramesD,
            _ => playerFramesS
        };

        // Add bounds checking to prevent crashes
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