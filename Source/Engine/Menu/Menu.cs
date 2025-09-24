using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App1.Source.Engine;

public class MenuScreen
{
    private Texture2D jamOutTexture;
    private Vector2[] jamOutPositions;
    private Vector2 jamOutSize;
    private float animationTimer;
    private float flipSpeed = 1.0f; 
    private int currentPositionIndex;
    
    public MenuScreen()
    {
        jamOutPositions = new Vector2[2];
        jamOutPositions[0] = new Vector2(646, 227);
        jamOutPositions[1] = new Vector2(968, 227);
        jamOutSize = new Vector2(320, 224);
        
        animationTimer = 0f;
        currentPositionIndex = 0;
    }
    
    public void LoadContent()
    {
        jamOutTexture = Globals.content.Load<Texture2D>("2D/Jam_Out");
    }
    
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        animationTimer += deltaTime;
        
        if (animationTimer >= flipSpeed)
        {
            currentPositionIndex = (currentPositionIndex + 1) % jamOutPositions.Length;
            animationTimer = 0f;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        int screenWidth = Globals.graphics.PreferredBackBufferWidth;
        int screenHeight = Globals.graphics.PreferredBackBufferHeight;

        Rectangle destinationRect = new Rectangle(0, 0, screenWidth, screenHeight);
        Rectangle sourceRect = new Rectangle(
            (int)jamOutPositions[currentPositionIndex].X, 
            (int)jamOutPositions[currentPositionIndex].Y, 
            (int)jamOutSize.X, 
            (int)jamOutSize.Y
        );

        spriteBatch.Draw(
            jamOutTexture,
            destinationRect,
            sourceRect,
            Color.White
        );
    }
}
