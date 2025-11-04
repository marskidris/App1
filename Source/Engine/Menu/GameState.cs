using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using App1.Source.Engine.Audio;

namespace App1.Source.Engine;

public enum GameStateType
{
    StartScreen,
    Playing,
    PausedMenu,
    PresentsMenu,
    MapMenu
}

public class GameState
{
    public GameStateType CurrentState { get; private set; }
    private GameStateType previousState;
    private KeyboardState previousKeyboardState;
    private KeyboardState currentKeyboardState;
    
    private MenuScreen menuScreen;
    private PresentsMenu _presentsMenu;
    private AnimatedLetters animatedLetters;
    private Map mapMenu;
    private Time gameTime;
    
    public GameState()
    {
        CurrentState = GameStateType.StartScreen;
        previousState = GameStateType.StartScreen;
        previousKeyboardState = Keyboard.GetState();
        currentKeyboardState = Keyboard.GetState();
        gameTime = new Time();
    }
    
    public void LoadContent()
    {
        menuScreen = new MenuScreen();
        _presentsMenu = new PresentsMenu();
        animatedLetters = new AnimatedLetters();
        mapMenu = new Map();
        
        menuScreen.LoadContent();
        _presentsMenu.LoadContent();
        animatedLetters.LoadContent();
        mapMenu.LoadContent();
        
        menuScreen.Activate();
    }
    
    public void Update(GameTime gameTime)
    {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        
        bool mDown = currentKeyboardState.IsKeyDown(Keys.M) && 
                           !previousKeyboardState.IsKeyDown(Keys.M);
        bool spaceDown = currentKeyboardState.IsKeyDown(Keys.Space) && 
                               !previousKeyboardState.IsKeyDown(Keys.Space);
        bool nDown = currentKeyboardState.IsKeyDown(Keys.N) && 
                           !previousKeyboardState.IsKeyDown(Keys.N);
        
        previousState = CurrentState;
        
        switch (CurrentState)
        {
            case GameStateType.StartScreen:
                if (mDown)
                {
                    CurrentState = GameStateType.Playing;
                    this.gameTime.Reset();
                }
                break;
                
            case GameStateType.Playing:
                this.gameTime.Update(gameTime);
                if (mDown)
                {
                    CurrentState = GameStateType.PausedMenu;
                }
                else if (spaceDown)
                {
                    CurrentState = GameStateType.PresentsMenu;
                }
                else if (nDown)
                {
                    CurrentState = GameStateType.MapMenu;
                    mapMenu.Activate();
                }
                break;
                
            case GameStateType.PausedMenu:
                if (mDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
                
            case GameStateType.PresentsMenu:
                this.gameTime.Update(gameTime);
                if (spaceDown)
                {
                    CurrentState = GameStateType.Playing;
                }
                break;
                
            case GameStateType.MapMenu:
                if (nDown)
                {
                    CurrentState = GameStateType.Playing;
                    mapMenu.Deactivate();
                }
                break;
        }
        
        if (previousState != CurrentState && IsInMenuState(CurrentState))
        {
            menuScreen.Activate();
        }
        
        if (previousState != CurrentState && CurrentState == GameStateType.PresentsMenu)
        {
            _presentsMenu.Activate();
        }
        
        menuScreen.Update(gameTime);
        _presentsMenu.Update(gameTime);
        animatedLetters.Update(gameTime);
        mapMenu.Update(gameTime);
        
        // Handle music playback based on game state
        HandleMusicPlayback();
    }
    
    private bool IsInMenuState(GameStateType state)
    {
        return state == GameStateType.StartScreen || state == GameStateType.PausedMenu;
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
                
            case GameStateType.MapMenu:
                DrawMapMenu(spriteBatch);
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
        if (Globals.content != null)
        {
            
        }
        
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.3f);
        }
        
        _presentsMenu.Draw(spriteBatch);
        
        animatedLetters.Draw(spriteBatch);
    }
    
    private void DrawMapMenu(SpriteBatch spriteBatch)
    {
        var viewport = Globals.spriteBatch.GraphicsDevice.Viewport;
        Texture2D pixel = CreatePixelTexture();
        if (pixel != null)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), 
                Color.Black * 0.5f);
        }
        
        mapMenu.Draw(spriteBatch);
        
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
        return CurrentState == GameStateType.Playing || CurrentState == GameStateType.PresentsMenu;
    }
    
    public bool IsInMenu()
    {
        return CurrentState == GameStateType.StartScreen || CurrentState == GameStateType.PausedMenu;
    }
    
    public MenuScreen GetMenuScreen()
    {
        return menuScreen;
    }
    
    private void HandleMusicPlayback()
    {
        // Pause or resume music based on the game state
        if (CurrentState == GameStateType.PausedMenu)
        {
            AudioState.Instance.PauseMusic();
        }
        else if (previousState == GameStateType.PausedMenu && CurrentState == GameStateType.Playing)
        {
            AudioState.Instance.ResumeMusic();
        }
    }
}