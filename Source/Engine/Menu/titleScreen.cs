using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace App1.Source.Engine.Menu;

public enum MenuState
{
    ShowingTitle,
    MainMenu,
    CharacterSelect,
    CharacterConfirm
}

public class TitleScreen
{
    private Texture2D jamOutTexture;
    private Texture2D fontTexture;
    private LetterFrames letterFrames;
    private Vector2 sourcePosition;
    private Vector2 sourceSize;
    private float scrollOffset;
    private float scrollSpeed = 500f;
    
    private const string TitleText = "ToeJam and Earl";
    private const int LetterScale = 12;
    private const int LetterSpacing = 8;
    
    // Menu state
    private MenuState currentState = MenuState.ShowingTitle;
    private float titleDisplayTimer = 0f;
    private const float TitleDisplayDuration = 3f;
    
    // Blinking
    private float blinkTimer = 0f;
    private const float BlinkInterval = 0.5f;
    private bool showText = true;
    
    // Main menu selection (0 = Play, 1 = Quit)
    private int mainMenuSelection = 0;
    private readonly string[] mainMenuOptions = { "Play", "Quit" };
    
    // Character select (0 = Earl, 1 = ToeJam)
    private int characterSelection = 0;
    private readonly string[] characterOptions = { "Earl", "ToeJam" };
    
    // Character confirmation
    private float confirmTimer = 0f;
    private const float ConfirmDisplayDuration = 2f;
    
    // Input handling
    private KeyboardState previousKeyboardState;
    
    // Selected character result (null until selected)
    public string SelectedCharacter { get; private set; } = null;
    public bool QuitRequested { get; private set; } = false;
    public bool GameStartRequested { get; private set; } = false;
    
    public TitleScreen()
    {
        sourcePosition = new Vector2(646, 227);
        sourceSize = new Vector2(320, 224);
        scrollOffset = 0f;
        letterFrames = new LetterFrames();
        previousKeyboardState = Keyboard.GetState();
    }
    
    public void Reset()
    {
        currentState = MenuState.ShowingTitle;
        titleDisplayTimer = 0f;
        blinkTimer = 0f;
        showText = true;
        mainMenuSelection = 0;
        characterSelection = 0;
        confirmTimer = 0f;
        SelectedCharacter = null;
        QuitRequested = false;
        GameStartRequested = false;
        previousKeyboardState = Keyboard.GetState();
    }
    
    public void LoadContent()
    {
        jamOutTexture = Globals.content.Load<Texture2D>("2D/Jam_Out");
        fontTexture = Globals.content.Load<Texture2D>("2D/Fonts");
    }
    
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Scroll background
        scrollOffset += scrollSpeed * deltaTime;
        if (scrollOffset >= sourceSize.Y)
        {
            scrollOffset -= sourceSize.Y;
        }
        
        // Update blink timer
        blinkTimer += deltaTime;
        if (blinkTimer >= BlinkInterval)
        {
            blinkTimer = 0f;
            showText = !showText;
        }
        
        KeyboardState currentKeyboardState = Keyboard.GetState();
        
        switch (currentState)
        {
            case MenuState.ShowingTitle:
                titleDisplayTimer += deltaTime;
                if (titleDisplayTimer >= TitleDisplayDuration)
                {
                    currentState = MenuState.MainMenu;
                }
                break;
                
            case MenuState.MainMenu:
                // Navigate with Up/Down arrows
                if (IsKeyPressed(Keys.Up, currentKeyboardState) || IsKeyPressed(Keys.W, currentKeyboardState))
                {
                    mainMenuSelection = (mainMenuSelection - 1 + mainMenuOptions.Length) % mainMenuOptions.Length;
                }
                if (IsKeyPressed(Keys.Down, currentKeyboardState) || IsKeyPressed(Keys.S, currentKeyboardState))
                {
                    mainMenuSelection = (mainMenuSelection + 1) % mainMenuOptions.Length;
                }
                // Select with Enter or Space
                if (IsKeyPressed(Keys.Enter, currentKeyboardState) || IsKeyPressed(Keys.Space, currentKeyboardState))
                {
                    if (mainMenuSelection == 0) // Play
                    {
                        currentState = MenuState.CharacterSelect;
                    }
                    else if (mainMenuSelection == 1) // Quit
                    {
                        QuitRequested = true;
                    }
                }
                break;
                
            case MenuState.CharacterSelect:
                if (IsKeyPressed(Keys.Up, currentKeyboardState) || IsKeyPressed(Keys.Left, currentKeyboardState) ||
                    IsKeyPressed(Keys.W, currentKeyboardState) || IsKeyPressed(Keys.A, currentKeyboardState))
                {
                    characterSelection = (characterSelection - 1 + characterOptions.Length) % characterOptions.Length;
                }
                if (IsKeyPressed(Keys.Down, currentKeyboardState) || IsKeyPressed(Keys.Right, currentKeyboardState) ||
                    IsKeyPressed(Keys.S, currentKeyboardState) || IsKeyPressed(Keys.D, currentKeyboardState))
                {
                    characterSelection = (characterSelection + 1) % characterOptions.Length;
                }
                if (IsKeyPressed(Keys.Enter, currentKeyboardState))
                {
                    SelectedCharacter = characterOptions[characterSelection];
                    confirmTimer = 0f;
                    currentState = MenuState.CharacterConfirm;
                }
                // Go back with Escape
                if (IsKeyPressed(Keys.Escape, currentKeyboardState))
                {
                    currentState = MenuState.MainMenu;
                }
                break;
                
            case MenuState.CharacterConfirm:
                confirmTimer += deltaTime;
                if (confirmTimer >= ConfirmDisplayDuration)
                {
                    GameStartRequested = true;
                }
                break;
        }
        
        previousKeyboardState = currentKeyboardState;
    }
    
    private bool IsKeyPressed(Keys key, KeyboardState currentState)
    {
        return currentState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (jamOutTexture == null) return;
        
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;
        
        // Draw scrolling background
        DrawScrollingBackground(spriteBatch, screenWidth, screenHeight);
        
        DrawTitle(spriteBatch, screenWidth, screenHeight);
        
        if (currentState == MenuState.MainMenu)
        {
            DrawMainMenu(spriteBatch, screenWidth, screenHeight);
        }
        else if (currentState == MenuState.CharacterSelect)
        {
            DrawCharacterSelect(spriteBatch, screenWidth, screenHeight);
        }
        else if (currentState == MenuState.CharacterConfirm)
        {
            DrawCharacterConfirm(spriteBatch, screenWidth, screenHeight);
        }
    }
    
    private void DrawScrollingBackground(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        int srcX = (int)sourcePosition.X;
        int srcY = (int)sourcePosition.Y;
        int srcWidth = (int)sourceSize.X;
        int srcHeight = (int)sourceSize.Y;
        int yOffset = (int)scrollOffset;
        
        int firstHeight = srcHeight - yOffset;
        if (firstHeight > 0)
        {
            Rectangle sourceRect1 = new Rectangle(srcX, srcY + yOffset, srcWidth, firstHeight);
            float ratio = (float)firstHeight / srcHeight;
            int destHeight1 = (int)(screenHeight * ratio);
            Rectangle destRect1 = new Rectangle(0, 0, screenWidth, destHeight1);
            spriteBatch.Draw(jamOutTexture, destRect1, sourceRect1, Color.White);
            
            if (yOffset > 0)
            {
                int secondHeight = yOffset;
                Rectangle sourceRect2 = new Rectangle(srcX, srcY, srcWidth, secondHeight);
                int destY2 = destHeight1;
                int destHeight2 = screenHeight - destHeight1;
                Rectangle destRect2 = new Rectangle(0, destY2, screenWidth, destHeight2);
                spriteBatch.Draw(jamOutTexture, destRect2, sourceRect2, Color.White);
            }
        }
    }
    
    private void DrawTitle(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        int totalWidth = CalculateTextWidth(TitleText);
        int avgLetterHeight = 7 * LetterScale;
        int startX = (screenWidth - totalWidth) / 2;
        int startY = (screenHeight - avgLetterHeight) / 2 - 100; // Move up to make room for menu
        
        DrawText(spriteBatch, TitleText, startX, startY);
    }
    
    private void DrawMainMenu(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        int menuScale = 8;
        int menuSpacing = 6;
        int optionSpacing = 60;
        int startY = screenHeight / 2 + 20;
        
        for (int i = 0; i < mainMenuOptions.Length; i++)
        {
            string option = mainMenuOptions[i];
            string displayText = (i == mainMenuSelection) ? "> " + option : "  " + option;
            
            int textWidth = CalculateTextWidth(displayText, menuScale, menuSpacing);
            int startX = (screenWidth - textWidth) / 2;
            int y = startY + i * optionSpacing;
            
            // Blink selected option
            if (i == mainMenuSelection && !showText)
                continue;
            
            DrawText(spriteBatch, displayText, startX, y, menuScale, menuSpacing);
        }
    }
    
    private void DrawCharacterSelect(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        // Draw "Select Character" header
        int headerScale = 6;
        int headerSpacing = 4;
        string headerText = "Select Character";
        int headerWidth = CalculateTextWidth(headerText, headerScale, headerSpacing);
        int headerX = (screenWidth - headerWidth) / 2;
        int headerY = screenHeight / 2 - 20;
        DrawText(spriteBatch, headerText, headerX, headerY, headerScale, headerSpacing);
        
        // Draw character options
        int menuScale = 8;
        int menuSpacing = 6;
        int optionSpacing = 60;
        int startY = screenHeight / 2 + 40;
        
        for (int i = 0; i < characterOptions.Length; i++)
        {
            string option = characterOptions[i];
            string displayText = (i == characterSelection) ? "> " + option : "  " + option;
            
            int textWidth = CalculateTextWidth(displayText, menuScale, menuSpacing);
            int startX = (screenWidth - textWidth) / 2;
            int y = startY + i * optionSpacing;
            
            // Blink selected option
            if (i == characterSelection && !showText)
                continue;
            
            DrawText(spriteBatch, displayText, startX, y, menuScale, menuSpacing);
        }
    }
    
    private void DrawCharacterConfirm(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        // Display "[Character] Selected" message
        string confirmText = SelectedCharacter + " Selected";
        int confirmScale = 10;
        int confirmSpacing = 6;
        int textWidth = CalculateTextWidth(confirmText, confirmScale, confirmSpacing);
        int avgLetterHeight = 7 * confirmScale;
        int startX = (screenWidth - textWidth) / 2;
        int startY = (screenHeight - avgLetterHeight) / 2;
        
        // Blink the confirmation text
        if (showText)
        {
            DrawText(spriteBatch, confirmText, startX, startY, confirmScale, confirmSpacing);
        }
    }
    
    private int CalculateTextWidth(string text, int scale = LetterScale, int spacing = LetterSpacing)
    {
        int totalWidth = 0;
        foreach (char c in text)
        {
            if (c == ' ')
            {
                totalWidth += 5 * scale;
            }
            else if (c == '>')
            {
                totalWidth += 6 * scale + spacing;
            }
            else
            {
                Rectangle frame = letterFrames.GetFrame(c);
                if (frame != Rectangle.Empty)
                {
                    totalWidth += frame.Width * scale + spacing;
                }
            }
        }
        if (totalWidth > 0) totalWidth -= spacing;
        return totalWidth;
    }
    
    private void DrawText(SpriteBatch spriteBatch, string text, int startX, int startY, int scale = LetterScale, int spacing = LetterSpacing)
    {
        int currentX = startX;
        
        foreach (char c in text)
        {
            if (c == ' ')
            {
                currentX += 5 * scale;
            }
            else if (c == '>')
            {
                // Draw a simple arrow using letters or skip if not in font
                // For now, use a placeholder approach - draw a small triangle shape
                int arrowWidth = 6 * scale;
                int arrowHeight = 7 * scale;
                // We'll just advance the position - the ">" won't render if not in font
                Rectangle arrowFrame = letterFrames.GetFrame('>');
                if (arrowFrame == Rectangle.Empty)
                {
                    // Draw a simple ">" manually - skip for now, just show space
                    currentX += arrowWidth + spacing;
                }
                else
                {
                    int destWidth = arrowFrame.Width * scale;
                    int destHeight = arrowFrame.Height * scale;
                    Rectangle destRect = new Rectangle(currentX, startY, destWidth, destHeight);
                    spriteBatch.Draw(fontTexture, destRect, arrowFrame, Color.White);
                    currentX += destWidth + spacing;
                }
            }
            else
            {
                Rectangle sourceRect = letterFrames.GetFrame(c);
                if (sourceRect != Rectangle.Empty)
                {
                    int destWidth = sourceRect.Width * scale;
                    int destHeight = sourceRect.Height * scale;
                    Rectangle destRect = new Rectangle(currentX, startY, destWidth, destHeight);
                    spriteBatch.Draw(fontTexture, destRect, sourceRect, Color.White);
                    currentX += destWidth + spacing;
                }
            }
        }
    }
}
