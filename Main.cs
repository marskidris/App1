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

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
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

        // TODO: use this.Content to load your game content here
        player = new Player("2D/Earl_Transparent", new Vector2(100, 100), new Vector2(150, 150));
        playerMovement = new PlayerMovement(player);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        playerMovement.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
       
        player.Draw();
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
    
}

