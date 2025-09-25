using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine;

public enum GameStateType
{
    StartScreen,
    Playing,
    PausedMenu,
    PresentsMenu
}

public class GameState
{
    public GameStateType CurrentState { get; private set; }
    private KeyboardState previousKeyboardState;
    private KeyboardState currentKeyboardState;
    
    private MenuScreen menuScreen;
    private PresentsMenu presentsMenu;
    private AnimatedLetters animatedLetters;
    
    public GameState()
    {
        CurrentState = GameStateType.StartScreen;
        previousKeyboardState = Keyboard.GetState();
        currentKeyboardState = Keyboard.GetState();
    }
    
    public void LoadContent()
    {
        menuScreen = new MenuScreen();
        presentsMenu = new PresentsMenu();
        animatedLetters = new AnimatedLetters();
        
        menuScreen.LoadContent();
        menuScreen.LoadContent();
        animatedLetters.LoadContent();
    }
    
    public void Update(GameTime gameTime)
    {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        
        bool mDown = currentKeyboardState.IsKeyDown(Keys.M) && 
                           !previousKeyboardState.IsKeyDown(Keys.M);
        bool spaceDown = currentKeyboardState.IsKeyDown(Keys.Space) && 
                               !previousKeyboardState.IsKeyDown(Keys.Space);
        
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
                else if (spaceDown)
                {
                    CurrentState = GameStateType.PresentsMenu;
                }
                break;
                
            case GameStateType.PausedMenu:
                if (mDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
                
            case GameStateType.PresentsMenu:
                if (spaceDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
        }
        
        menuScreen.Update(gameTime);
        presentsMenu.Update(gameTime);
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
                
            case GameStateType.PresentsMenu:
                DrawPresentsMenu(spriteBatch);
                break;
        }
    }
    
    private void DrawStartScreen(SpriteBatch spriteBatch)
    {
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
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.7f);
        }
        
        menuScreen.Draw(spriteBatch);
        
        animatedLetters.Draw(spriteBatch);
    }
    
    private void DrawPresentsMenu(SpriteBatch spriteBatch)
    {
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.7f);
        }
        
        presentsMenu.Draw(spriteBatch);
        
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