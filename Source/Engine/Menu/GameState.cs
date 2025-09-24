using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine;

public enum GameStateType
{
    StartScreen,
    Playing,
    PausedMenu
}

public class GameState
{
    public GameStateType CurrentState { get; private set; }
    private KeyboardState previousKeyboardState;
    private KeyboardState currentKeyboardState;
    
    // Menu screen components
    private MenuScreen menuScreen;
    private AnimatedLetters animatedLetters;
    
    public GameState()
    {
        CurrentState = GameStateType.StartScreen;
        previousKeyboardState = Keyboard.GetState();
        currentKeyboardState = Keyboard.GetState();
    }
    
    public void LoadContent()
    {
        // Initialize menu screen and animated letters
        menuScreen = new MenuScreen();
        animatedLetters = new AnimatedLetters();
        
        menuScreen.LoadContent();
        animatedLetters.LoadContent();
    }
    
    public void Update(GameTime gameTime)
    {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        
        // Handle spacebar input for state transitions
        bool spacePressed = currentKeyboardState.IsKeyDown(Keys.Space) && 
                           !previousKeyboardState.IsKeyDown(Keys.Space);
        
        switch (CurrentState)
        {
            case GameStateType.StartScreen:
                if (spacePressed)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
                
            case GameStateType.Playing:
                if (spacePressed)
                {
                    CurrentState = GameStateType.PausedMenu;
                }
                break;
                
            case GameStateType.PausedMenu:
                if (spacePressed)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
        }
        
        // Update animated components
        menuScreen.Update(gameTime);
        animatedLetters.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        switch (CurrentState)
        {
            case GameStateType.StartScreen:
                DrawStartScreen(spriteBatch);
                break;
                
            case GameStateType.Playing:
                // Game content will be drawn by Main class
                break;
                
            case GameStateType.PausedMenu:
                DrawPauseMenu(spriteBatch);
                break;
        }
    }
    
    private void DrawStartScreen(SpriteBatch spriteBatch)
    {
        // Clear with dark background
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.8f);
        }
        
        // Draw animated menu screen elements
        menuScreen.Draw(spriteBatch);
        
        // Draw animated letters on top
        animatedLetters.Draw(spriteBatch);
    }
    
    private void DrawPauseMenu(SpriteBatch spriteBatch)
    {
        // Draw semi-transparent overlay
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.7f);
        }
        
        // Draw animated menu screen elements
        menuScreen.Draw(spriteBatch);
        
        // Draw animated letters on top
        animatedLetters.Draw(spriteBatch);
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
    
    public bool IsPlaying()
    {
        return CurrentState == GameStateType.Playing;
    }
    
    public bool ShowGameContent()
    {
        return CurrentState == GameStateType.Playing;
    }
    
    public bool IsInMenu()
    {
        return CurrentState == GameStateType.StartScreen || CurrentState == GameStateType.PausedMenu;
    }
}