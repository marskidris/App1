// csharp
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace App1.Source.Engine.Player;

public class IdleEarl
{
    public Vector2 pos, dims;
    public Texture2D myModel;
    public Rectangle sourceRect;
    

    public IdleEarl(string PATH, Vector2 POS, Vector2 DIMS)
    {
        pos = POS;
        dims = DIMS;

        try
        {
            myModel = Globals.content.Load<Texture2D>(PATH);
        }
        catch (ContentLoadException)
        {
            string pngPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", PATH + ".png");
            if (File.Exists(pngPath))
            {
                using var fs = File.OpenRead(pngPath);
                myModel = Texture2D.FromStream(Globals.spriteBatch.GraphicsDevice, fs);
            }
            else
            {
                throw;
            }
        }
    }

    public virtual void Update()
    {
    }

    public virtual void Draw()
    {
        if (myModel == null) return;

        Globals.spriteBatch.Draw(
            myModel,
            new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y),
            sourceRect, // Use the source rectangle
            Color.White,
            0.0f,
            new Vector2(0, 0), // Use (0,0) for top-left origin
            SpriteEffects.None,
            0.0f
        );
    }
}