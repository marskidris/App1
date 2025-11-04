using App1.Source;
using App1.Source.Engine;
using App1.Source.Engine.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App1;

public class Main : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    
    Player player;
    PlayerMovement playerMovement;
    GameState gameState;
    Animated2D testAnimation;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        Globals.graphics = graphics;
        // graphics.IsFullScreen = true;
        graphics.PreferredBackBufferWidth = 1600;
        graphics.PreferredBackBufferHeight = 900;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.content = this.Content;
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load and play background music
        AudioState.Instance.LoadContent(Content);
        AudioState.Instance.PlayBackgroundMusic();

        gameState = new GameState();
        gameState.LoadContent();

        // TODO: use this.Content to load your game content here
        player = new Player("2D/Earl_Transparent", new Vector2(100, 100), new Vector2(150, 150));
        playerMovement = new PlayerMovement(player);
        
        testAnimation = new Animated2D(
            Globals.content.Load<Texture2D>("2D/Earl_Transparent"), 
            new Vector3(400, 200, 0), // Using Vector3 - Z=0 for default layer depth
            new Vector3(100, 100, 0)  // Using Vector3 - Z component not used for size
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update audio state for sound instance management
        AudioState.Instance.Update();

        gameState.Update(gameTime);

        if (gameState.IsPlaying())
        {
            playerMovement.Update(gameTime);
            testAnimation.SetPlayerPosition(new Vector3(player.Position.X, player.Position.Y, 0));
            testAnimation.Update(gameTime); // Update test animation
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        gameState.Draw(Globals.spriteBatch);
        
        if (gameState.ShowGameContent())
        {
            player.Draw();
            testAnimation.Draw(Globals.spriteBatch); // Draw test animation
        }
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}
