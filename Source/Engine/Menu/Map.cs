using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace App1.Source.Engine.Menu;

public class Map
{
    private bool isActive;
    private Texture2D mapTexture;
    private Texture2D hudTexture;
    
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
            try
            {
                hudTexture = Globals.content.Load<Texture2D>("2D/HUD_Display");
            }
            catch
            {
                hudTexture = null;
            }
        }
    }
    
    public void Update(GameTime gameTime)
    {
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!isActive) return;
        
        // Draw HUD frame at the requested HUD position/size so it shows even if map texture is missing
        Rectangle destinationRect = new Rectangle(336, 220, 320, 107);
        Rectangle hudSource = new Rectangle(336, 220, 320, 108);

        if (hudTexture != null)
        {
            spriteBatch.Draw(hudTexture, destinationRect, hudSource, Color.White);
        }

        // Draw map texture on top of the HUD frame if available
        if (mapTexture != null)
        {
            spriteBatch.Draw(mapTexture, destinationRect, Color.White);
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