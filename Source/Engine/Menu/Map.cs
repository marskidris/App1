using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace App1.Source.Engine;

public class Map
{
    private bool isActive;
    private Texture2D mapTexture;
    
    public Map()
    {
        isActive = false;
    }
    
    public void LoadContent()
    {
        if (Globals.content != null)
        {
            try
            {
                mapTexture = Globals.content.Load<Texture2D>("2D/toejam-and-earl-fixed-world-level-01-unmarked-genesis-map");
            }
            catch
            {
                mapTexture = null;
            }
        }
    }
    
    public void Update(GameTime gameTime)
    {
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!isActive) return;
        
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;
        
        if (mapTexture != null)
        {
            Rectangle destinationRect = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(mapTexture, destinationRect, Color.White);
        }
        else
        {
            Texture2D pixel = CreatePixelTexture();
            if (pixel != null)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 0, screenWidth, screenHeight), Color.DarkBlue * 0.8f);
            }
        }
    }
    
    public void Activate()
    {
        isActive = true;
        Console.WriteLine("Map Open");
    }
    
    public void Deactivate()
    {
        isActive = false;
        Console.WriteLine("Map Closed");
    }
    
    private Texture2D CreatePixelTexture()
    {
        try
        {
            Texture2D pixel = new Texture2D(Globals.spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            return pixel;
        }
        catch
        {
            return null;
        }
    }
}