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
    
    World world;
    Animated2D animated;

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
        // world = new World();
        animated = new Animated2D(Globals.content.Load<Texture2D>("2D/Earl_Transparent"), new Vector2(100, 100), new Vector2(150, 150));
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        // world.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
       
        // world.Draw();
        animated.Update(gameTime);
        animated.Draw(Globals.spriteBatch);
        
        
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
    
}

