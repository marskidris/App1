using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace App1.Source.Engine;

public enum CharacterType
{
    Earl,
    Toejam
}

public class PresentsMenu
{
    private Texture2D HUDTexture;
    private Vector2 EarlHUDPosition;
    private Vector2 ToejamHUDPosition;
    private Vector2 HUDSize;
    private CharacterType currentCharacter;
    private bool isFirstActivation = true;
    
    public PresentsMenu()
    {
        EarlHUDPosition = new Vector2(336, 220);
        ToejamHUDPosition = new Vector2(336, 8);
        HUDSize = new Vector2(320, 108);
        
        currentCharacter = CharacterType.Earl;
    }
    
    public void LoadContent()
    {
        HUDTexture = Globals.content.Load<Texture2D>("2D/HUD_Display");
    }
    
    public void Activate()
    {
        if (isFirstActivation)
        {
            Console.WriteLine("ToeJam's Menu Open");
            isFirstActivation = false;
        }
        else
        {
            Console.WriteLine("ToeJam's Menu Open");
        }
    }
    
    public void Update(GameTime gameTime)
    {
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;

        Rectangle destinationRect = new Rectangle(0, screenHeight / 2, screenWidth, screenHeight / 2);
        Vector2 sourcePosition = GetCurrentCharacterHUDPosition();
        
        Rectangle sourceRect = new Rectangle(
            (int)sourcePosition.X, 
            (int)sourcePosition.Y, 
            (int)HUDSize.X, 
            (int)HUDSize.Y
        );

        spriteBatch.Draw(
            HUDTexture,
            destinationRect,
            sourceRect,
            Color.White
        );
    }
    
    private Vector2 GetCurrentCharacterHUDPosition()
    {
        return currentCharacter switch
        {
            CharacterType.Earl => EarlHUDPosition,
            CharacterType.Toejam => ToejamHUDPosition,
            _ => EarlHUDPosition
        };
    }
}
