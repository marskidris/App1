using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine.Menu;

public enum PauseMenuSelection
{
    Resume,
    ReturnToTitle,
    Quit
}

public class Menu
{
    private Texture2D jamOutTexture;
    private Texture2D fontTexture;
    private LetterFrames letterFrames;
    private Vector2 sourcePosition1;
    private Vector2 sourcePosition2;
    private Vector2 sourceSize;
    private float scrollOffset;
    private float scrollSpeed = 500f;
    private bool useSecondFrame;
    private float frameTimer;
    private const float FrameSwitchInterval = 0.1f;
    
    private float blinkTimer;
    private const float BlinkInterval = 0.5f;
    private bool showText = true;
    
    private int menuSelection;
    private readonly string[] menuOptions = { "Resume", "Return to Title", "Quit" };
    
    private KeyboardState previousKeyboardState;
    
    public bool ResumeRequested { get; private set; }
    public bool ReturnToTitleRequested { get; private set; }
    public bool QuitRequested { get; private set; }
    public bool IsActive { get; private set; }
    
    public Menu()
    {
        sourcePosition1 = new Vector2(2, 1131);
        sourcePosition2 = new Vector2(324, 1131);
        sourceSize = new Vector2(320, 224);
        scrollOffset = 0f;
        letterFrames = new LetterFrames();
        previousKeyboardState = Keyboard.GetState();
        menuSelection = 0;
        useSecondFrame = false;
        frameTimer = 0f;
    }
    
    public void LoadContent()
    {
        jamOutTexture = Globals.content.Load<Texture2D>("2D/Jam_Out");
        fontTexture = Globals.content.Load<Texture2D>("2D/Fonts");
    }
    
    public void Activate()
    {
        IsActive = true;
        ResumeRequested = false;
        ReturnToTitleRequested = false;
        QuitRequested = false;
        menuSelection = 0;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        scrollOffset += scrollSpeed * deltaTime;
        if (scrollOffset >= sourceSize.Y)
        {
            scrollOffset -= sourceSize.Y;
        }
        
        frameTimer += deltaTime;
        if (frameTimer >= FrameSwitchInterval)
        {
            frameTimer = 0f;
            useSecondFrame = !useSecondFrame;
        }
        
        blinkTimer += deltaTime;
        if (blinkTimer >= BlinkInterval)
        {
            blinkTimer = 0f;
            showText = !showText;
        }
        
        KeyboardState currentKeyboardState = Keyboard.GetState();
        
        if (IsKeyPressed(Keys.Up, currentKeyboardState) || IsKeyPressed(Keys.W, currentKeyboardState))
        {
            menuSelection = (menuSelection - 1 + menuOptions.Length) % menuOptions.Length;
        }
        if (IsKeyPressed(Keys.Down, currentKeyboardState) || IsKeyPressed(Keys.S, currentKeyboardState))
        {
            menuSelection = (menuSelection + 1) % menuOptions.Length;
        }
        
        if (IsKeyPressed(Keys.Enter, currentKeyboardState))
        {
            switch (menuSelection)
            {
                case 0:
                    ResumeRequested = true;
                    break;
                case 1:
                    ReturnToTitleRequested = true;
                    break;
                case 2:
                    QuitRequested = true;
                    break;
            }
        }
        
        if (IsKeyPressed(Keys.Space, currentKeyboardState) || IsKeyPressed(Keys.Escape, currentKeyboardState))
        {
            ResumeRequested = true;
        }
        
        previousKeyboardState = currentKeyboardState;
    }
    
    private bool IsKeyPressed(Keys key, KeyboardState currentState)
    {
        return currentState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive || jamOutTexture == null) return;
        
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;
        
        DrawScrollingBackground(spriteBatch, screenWidth, screenHeight);
        DrawMenuTitle(spriteBatch, screenWidth, screenHeight);
        DrawMenuOptions(spriteBatch, screenWidth, screenHeight);
    }
    
    private void DrawScrollingBackground(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        Vector2 currentSourcePosition = useSecondFrame ? sourcePosition2 : sourcePosition1;
        int srcX = (int)currentSourcePosition.X;
        int srcY = (int)currentSourcePosition.Y;
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
    
    private void DrawMenuTitle(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        string titleText = "Paused";
        int titleScale = 12;
        int titleSpacing = 8;
        int totalWidth = CalculateTextWidth(titleText, titleScale, titleSpacing);
        int avgLetterHeight = 7 * titleScale;
        int startX = (screenWidth - totalWidth) / 2;
        int startY = (screenHeight - avgLetterHeight) / 2 - 100;
        
        DrawText(spriteBatch, titleText, startX, startY, titleScale, titleSpacing);
    }
    
    private void DrawMenuOptions(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (fontTexture == null || letterFrames == null) return;
        
        int menuScale = 8;
        int menuSpacing = 6;
        int optionSpacing = 60;
        int startY = screenHeight / 2 + 20;
        
        for (int i = 0; i < menuOptions.Length; i++)
        {
            string option = menuOptions[i];
            string displayText = (i == menuSelection) ? "> " + option : "  " + option;
            
            int textWidth = CalculateTextWidth(displayText, menuScale, menuSpacing);
            int startX = (screenWidth - textWidth) / 2;
            int y = startY + i * optionSpacing;
            
            if (i == menuSelection && !showText)
                continue;
            
            DrawText(spriteBatch, displayText, startX, y, menuScale, menuSpacing);
        }
    }
    
    private int CalculateTextWidth(string text, int scale, int spacing)
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
    
    private void DrawText(SpriteBatch spriteBatch, string text, int startX, int startY, int scale, int spacing)
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
                Rectangle arrowFrame = letterFrames.GetFrame('>');
                if (arrowFrame == Rectangle.Empty)
                {
                    currentX += 6 * scale + spacing;
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