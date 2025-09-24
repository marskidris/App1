using App1.Source;
using App1.Source.Engine;
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
    Animated2D testAnimation; // Added for testing playerFramesA frames

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        Globals.graphics = graphics;
        // graphics.IsFullScreen = true;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
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

        // Initialize game state system
        gameState = new GameState();
        gameState.LoadContent();

        // TODO: use this.Content to load your game content here
        player = new Player("2D/Earl_Transparent", new Vector2(100, 100), new Vector2(150, 150));
        playerMovement = new PlayerMovement(player);
        
        // testAnimation = new Animated2D(
        //     Globals.content.Load<Texture2D>("2D/Earl_Transparent"), 
        //     new Vector2(400, 200), 
        //     new Vector2(100, 100)
        // );
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        gameState.Update(gameTime);

        if (gameState.IsPlaying())
        {
            playerMovement.Update(gameTime);
            // testAnimation.Update(gameTime); // Update test animation
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        // Draw game state screens (start screen, pause screen)
        gameState.Draw(Globals.spriteBatch);
        
        // Only draw game content when playing
        if (gameState.ShowGameContent())
        {
            player.Draw();
            // testAnimation.Draw(Globals.spriteBatch); // Draw test animation
        }
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}
