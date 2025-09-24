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
        
        bool mDown = currentKeyboardState.IsKeyDown(Keys.M) && 
                           !previousKeyboardState.IsKeyDown(Keys.M);
        
        switch (CurrentState)
        {
            case GameStateType.StartScreen:
                if (mDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
                
            case GameStateType.Playing:
                if (mDown)
                {
                    CurrentState = GameStateType.PausedMenu;
                }
                break;
                
            case GameStateType.PausedMenu:
                if (mDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
        }
        
        // Update animated 
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