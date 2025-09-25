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
    private bool isMenuOpen;
    private KeyboardState previousKeyboardState;
    
    public PresentsMenu()
    {
        EarlHUDPosition = new Vector2(336, 220);
        ToejamHUDPosition = new Vector2(336, 8);
        HUDSize = new Vector2(320, 108);
        // Toejam second is 116.
        
        currentCharacter = CharacterType.Earl;
        isMenuOpen = false;
        previousKeyboardState = Keyboard.GetState();
    }
    
    public void LoadContent()
    {
        try
        {
            HUDTexture = Globals.content.Load<Texture2D>("2D/HUD_Display");
            if (HUDTexture == null)
            {
                Console.WriteLine("Warning: HUD_Display texture failed to load - texture is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading HUD_Display texture: {ex.Message}");
            HUDTexture = null;
        }
    }
    
    public void Update(GameTime gameTime)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();
        
        bool spacePressed = currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space);
        
        if (spacePressed)
        {
            isMenuOpen = !isMenuOpen;
            string menuStatus = isMenuOpen ? "Presents open" : "Presents closed";
            Console.WriteLine(menuStatus);
        }
        
        previousKeyboardState = currentKeyboardState;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!isMenuOpen || HUDTexture == null) return;
        
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;

        Rectangle destinationRect = new Rectangle(0, 0, screenWidth, screenHeight);
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
    
    public bool IsMenuOpen => isMenuOpen;
    
    public void SetCharacter(CharacterType character)
    {
        currentCharacter = character;
    }
    
    public CharacterType GetCurrentCharacter()
    {
        return currentCharacter;
    }
}
