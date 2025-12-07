using App1.Source.Engine.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine.Menu;

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
    
    private TitleScreen titleScreen;
    private Menu pauseMenu;
    private AnimatedLetters animatedLetters;
    public bool SuppressPauseOverlay { get; set; } = false;
    private bool gameplayPaused = false;
    
    public bool QuitRequested { get; private set; } = false;
    public bool ReturnToTitleRequested { get; private set; } = false;
    public string SelectedCharacter { get; private set; } = null;
    public string SelectedCharacter2 { get; private set; } = null;
    public bool TwoPlayerMode { get; private set; } = false;
    private bool skipNextKeyboardUpdate = false;

    public void PauseGameplay()
    {
        gameplayPaused = true;
    }

    public void ResumeGameplay()
    {
        gameplayPaused = false;
    }
    
    public void ClearReturnToTitleRequest()
    {
        ReturnToTitleRequested = false;
    }
    
    public void ForcePlaying()
    {
        CurrentState = GameStateType.Playing;
    }
    
    public GameState()
    {
        CurrentState = GameStateType.StartScreen;
        previousKeyboardState = Keyboard.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    public void Pause()
    {
        if (CurrentState == GameStateType.Playing)
            CurrentState = GameStateType.PausedMenu;
    }

    public void Resume()
    {
        if (CurrentState == GameStateType.PausedMenu)
            CurrentState = GameStateType.Playing;
    }
    
    public void LoadContent()
    {
        titleScreen = new TitleScreen();
        pauseMenu = new Menu();
        animatedLetters = new AnimatedLetters();
        
        titleScreen.LoadContent();
        pauseMenu.LoadContent();
        animatedLetters.LoadContent();
    }
    
    public void Update(GameTime gameTime)
    {
        if (skipNextKeyboardUpdate)
        {
            skipNextKeyboardUpdate = false;
            previousKeyboardState = Keyboard.GetState();
            currentKeyboardState = previousKeyboardState;
        }
        else
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
        
        if (CurrentState == GameStateType.StartScreen)
        {
            titleScreen.Update(gameTime);
            
            if (titleScreen.QuitRequested)
            {
                QuitRequested = true;
            }
            
            if (titleScreen.GameStartRequested)
            {
                CurrentState = GameStateType.Playing;
                SelectedCharacter = titleScreen.SelectedCharacter;
                SelectedCharacter2 = titleScreen.SelectedCharacter2;
                TwoPlayerMode = titleScreen.TwoPlayerMode;
                skipNextKeyboardUpdate = true;
                return;
            }
        }
        else if (CurrentState == GameStateType.PausedMenu)
        {
            pauseMenu.Update(gameTime);
            
            if (pauseMenu.ResumeRequested)
            {
                pauseMenu.Deactivate();
                CurrentState = GameStateType.Playing;
                skipNextKeyboardUpdate = true;
                return;
            }
            
            if (pauseMenu.ReturnToTitleRequested)
            {
                pauseMenu.Deactivate();
                titleScreen.Reset();
                SelectedCharacter = null;
                SelectedCharacter2 = null;
                TwoPlayerMode = false;
                ReturnToTitleRequested = true;
                CurrentState = GameStateType.StartScreen;
            }
            
            if (pauseMenu.QuitRequested)
            {
                QuitRequested = true;
            }
        }
        else if (CurrentState == GameStateType.Playing)
        {
            bool spacePressed = currentKeyboardState.IsKeyDown(Keys.Space) && 
                               !previousKeyboardState.IsKeyDown(Keys.Space);
            
            if (spacePressed)
            {
                pauseMenu.Activate();
                CurrentState = GameStateType.PausedMenu;
            }
        }
        
        animatedLetters.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        switch (CurrentState)
        {
            case GameStateType.StartScreen:
                if (!SuppressPauseOverlay)
                    DrawStartScreen(spriteBatch);
                break;
                
            case GameStateType.Playing:
                // Game content will be drawn by Main class
                break;
                
            case GameStateType.PausedMenu:
                // Only draw the full-screen pause menu if not suppressed (e.g., when showing map overlay)
                if (!SuppressPauseOverlay)
                    DrawPauseMenu(spriteBatch);
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
        
        titleScreen.Draw(spriteBatch);
        
        animatedLetters.Draw(spriteBatch);
    }
    
    private void DrawPauseMenu(SpriteBatch spriteBatch)
    {
        pauseMenu.Draw(spriteBatch);
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
        return CurrentState == GameStateType.Playing && !gameplayPaused;
    }
    
    public bool ShowGameContent()
    {
        return CurrentState == GameStateType.Playing && !gameplayPaused;
    }
    
    public bool IsInMenu()
    {
        return CurrentState == GameStateType.StartScreen || CurrentState == GameStateType.PausedMenu;
    }
}