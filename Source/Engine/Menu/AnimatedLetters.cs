using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace App1.Source.Engine;

public class AnimatedLetters
{
    private struct Letter
    {
        public char Character;
        public Vector2 Position;
        public Vector2 TargetPosition;
        public Vector2 Velocity;
        public Color Color;
        public float Scale;
        public float Rotation;
        public float RotationSpeed;
    }
    
    private Letter[] letters;
    private SpriteFont font;
    private Random random;
    private float timer;
    private float repositionTime = 3.0f; // Change positions every 3 seconds
    private string gameTitle = "TOEJAM & EARL";
    
    public AnimatedLetters()
    {
        random = new Random();
        timer = 0f;
        InitializeLetters();
    }
    
    private void InitializeLetters()
    {
        letters = new Letter[gameTitle.Length];
        
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i] = new Letter
            {
                Character = gameTitle[i],
                Position = GetRandomPosition(),
                TargetPosition = GetRandomPosition(),
                Velocity = Vector2.Zero,
                Color = GetRandomColor(),
                Scale = (float)(random.NextDouble() * 0.5 + 0.5), // Scale between 0.5 and 1.0
                Rotation = 0f,
                RotationSpeed = (float)(random.NextDouble() * 2.0 - 1.0) // Random rotation speed
            };
        }
    }
    
    public void LoadContent()
    {
        try
        {
            font = Globals.content.Load<SpriteFont>("Fonts");
        }
        catch
        {
            // Font will be null, we'll handle this in Draw method
            font = null;
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        return new Vector2(
            random.Next(50, viewport.Width - 50),
            random.Next(50, viewport.Height - 50)
        );
    }
    
    private Color GetRandomColor()
    {
        Color[] colors = {
            Color.Yellow,
            Color.Cyan,
            Color.Magenta,
            Color.Orange,
            Color.LimeGreen,
            Color.Pink,
            Color.LightBlue,
            Color.White
        };
        
        return colors[random.Next(colors.Length)];
    }
    
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timer += deltaTime;
        
        // Update each letter's animation
        for (int i = 0; i < letters.Length; i++)
        {
            // Update rotation
            letters[i].Rotation += letters[i].RotationSpeed * deltaTime;
            
            // Move towards target position
            Vector2 direction = letters[i].TargetPosition - letters[i].Position;
            float distance = direction.Length();
            
            if (distance > 5f)
            {
                direction.Normalize();
                letters[i].Velocity = Vector2.Lerp(letters[i].Velocity, direction * 100f, deltaTime * 2f);
                letters[i].Position += letters[i].Velocity * deltaTime;
            }
            else
            {
                letters[i].Velocity *= 0.9f; // Slow down when near target
            }
            
            // Pulsing scale effect
            float pulseEffect = (float)System.Math.Sin(timer * 2 + i) * 0.1f;
            letters[i].Scale = System.Math.Max(0.3f, 0.8f + pulseEffect);
        }
        
        // Change target positions periodically
        if (timer >= repositionTime)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].TargetPosition = GetRandomPosition();
                letters[i].Color = GetRandomColor();
            }
            timer = 0f;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (font == null) return;
        
        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i].Character == ' ') continue; // Skip spaces
            
            string character = letters[i].Character.ToString();
            Vector2 origin = font.MeasureString(character) * 0.5f;

            spriteBatch.DrawString(
                font,
                character,
                letters[i].Position,
                letters[i].Color,
                letters[i].Rotation,
                origin,
                letters[i].Scale,
                SpriteEffects.None,
                0.9f // High layer depth so letters appear on top
            );
        }
    }
}

