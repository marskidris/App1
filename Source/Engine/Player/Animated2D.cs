using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App1.Source.Engine;

public class Animated2D
{
    Texture2D texture;
    Vector2 position;
    Vector2 size;
    Rectangle[] frames;
    int currentFrame;
    int direction = 1;
    float timer;
    float switchTime = 1.5f;

    public Animated2D(Texture2D texture, Vector2 position, Vector2 size)
    {
        this.texture = texture;
        this.position = position;
        this.size = size;
        
        frames = new Rectangle[3];
        frames[0] = new Rectangle(9, 13, 23, 33);
        frames[1] = new Rectangle(37, 16, 24, 30);
        frames[2] = new Rectangle(65, 13, 23, 33);
        
        currentFrame = 0;
        timer = 0f;
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
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, frames[currentFrame], Color.White, 0f, Vector2.Zero, size / new Vector2(frames[currentFrame].Width, frames[currentFrame].Height), SpriteEffects.None, 0f);
    }
}