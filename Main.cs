using App1.Source;
using App1.Source.Engine;
using App1.Source.Engine.Audio;
using App1.Source.Engine.Collision;
using App1.Source.Engine.Enemy;
using App1.Source.Engine.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace App1;

public class Main : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    
    Player player;
    PlayerMovement playerMovement;
    GameState gameState;

    // Multiple enemies and items using Animated2D coordinates
    List<Enemy> enemies;
    List<Item> items;

    // Player hit state
    private bool playerHitByTornado = false;
    private float restartCountdown = 30f;
    private SpriteFont countdownFont;

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

        // Try to load font for countdown (may not exist)
        try
        {
            countdownFont = Content.Load<SpriteFont>("Fonts");
        }
        catch
        {
            // Font not available, will skip countdown text
            countdownFont = null;
        }

        // Player setup
        player = new Player("2D/Earl_Transparent", new Vector2(100, 100), new Vector2(150, 150));
        playerMovement = new PlayerMovement(player);
        
        // Initialize enemy and item lists
        enemies = new List<Enemy>();
        items = new List<Item>();
        
        // Load textures
        Texture2D tornadoTexture = Globals.content.Load<Texture2D>("2D/Tornado");
        Texture2D itemsTexture = Globals.content.Load<Texture2D>("2D/items_scenery_tranparent");

        // Create enemies using ONLY Animated2D's coordinates (400, 200)
        // Enemy 1: At exact Animated2D position
        enemies.Add(new Enemy(
            tornadoTexture,
            new Vector2(400, 200),
            scale: 4f,
            moveSpeed: 50f,
            patrolDistance: 100f
        ));

        // Enemy 2: 200 pixels up from base position
        enemies.Add(new Enemy(
            tornadoTexture,
            new Vector2(400, 200 - 200),  // (400, 0)
            scale: 4f,
            moveSpeed: 60f,
            patrolDistance: 150f
        ));

        // Enemy 3: 300 pixels down from base position
        enemies.Add(new Enemy(
            tornadoTexture,
            new Vector2(400, 200 + 300),  // (400, 500)
            scale: 4f,
            moveSpeed: 40f,
            patrolDistance: 120f
        ));
        
        // Create items using PresentItems' relative positions from Animated2D (400, 200)
        Rectangle presentFrame = new Rectangle(137, 10, 15, 14);
        
        // Item 1: 150 pixels to the right (same as PresentItems position 0)
        items.Add(new Item(
            itemsTexture,
            new Vector2(400 + 150, 200),  // (550, 200)
            new Vector2(15, 14),
            presentFrame
        ));
        
        // Item 2: 150 pixels to the left (same as PresentItems position 1)
        items.Add(new Item(
            itemsTexture,
            new Vector2(400 - 150, 200),  // (250, 200)
            new Vector2(15, 14),
            presentFrame
        ));
        
        // Item 3: 150 pixels down (same as PresentItems position 2)
        items.Add(new Item(
            itemsTexture,
            new Vector2(400, 200 + 150),  // (400, 350)
            new Vector2(15, 14),
            presentFrame
        ));
        
        // Item 4: 150 pixels up
        items.Add(new Item(
            itemsTexture,
            new Vector2(400, 200 - 150),  // (400, 50)
            new Vector2(15, 14),
            presentFrame
        ));
        
        // Item 5: Diagonal top-right
        items.Add(new Item(
            itemsTexture,
            new Vector2(400 + 150, 200 - 150),  // (550, 50)
            new Vector2(15, 14),
            presentFrame
        ));
        
        // Item 6: Diagonal bottom-left
        items.Add(new Item(
            itemsTexture,
            new Vector2(400 - 150, 200 + 150),  // (250, 350)
            new Vector2(15, 14),
            presentFrame
        ));
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
            // Update countdown if player was hit
            if (playerHitByTornado)
            {
                restartCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (restartCountdown <= 0)
                {
                    // Restart the game
                    LoadContent();
                    playerHitByTornado = false;
                    restartCountdown = 30f;
                }
            }

            // Update player movement only if not hit
            if (!playerHitByTornado)
            {
                playerMovement.Update(gameTime);
            }

            // Update all enemies from list (they continue moving even if player is hit)
            foreach (var enemy in enemies)
            {
                if (enemy.IsActive)
                {
                    enemy.Update(gameTime, player.CircleCenter);

                    // Only check collision if player hasn't been hit yet
                    if (player.IsActive && !playerHitByTornado)
                    {
                        enemy.CheckCollisionWithPlayer(player.BoundingBox, player.CircleCenter, player.CircleRadius);

                        bool rectCollision = Collision2D.CheckRectangleCollision(
                            player.BoundingBox,
                            enemy.BoundingBox
                        );

                        bool circCollision = Collision2D.CheckCircleCollision(
                            player.CircleCenter,
                            player.CircleRadius,
                            enemy.CircleCenter,
                            enemy.CircleRadius
                        );

                        if (rectCollision || circCollision)
                        {
                            // Player hit by tornado - activate attack state
                            playerHitByTornado = true;
                            player.IsActive = false;
                            enemy.ActivateAttackState();
                            AudioState.Instance.PlayBoogeymanSound();
                        }
                    }
                }
            }
            
            // Update all items from list
            foreach (var item in items)
            {
                if (item.IsActive)
                {
                    item.CheckCollisionWithPlayer(player.BoundingBox, player.CircleCenter, player.CircleRadius);
                    
                    if (player.IsActive)
                    {
                        bool rectCollision = Collision2D.CheckRectangleCollision(
                            player.BoundingBox,
                            item.BoundingBox
                        );
                        
                        bool circCollision = Collision2D.CheckCircleCollision(
                            player.CircleCenter,
                            player.CircleRadius,
                            item.CircleCenter,
                            item.CircleRadius
                        );
                        
                        if (rectCollision || circCollision)
                        {
                            item.IsActive = false;
                            AudioState.Instance.PlayMoneySoundWithVolumeAdjustment();
                        }
                    }
                }
            }
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
            // Draw items first (background layer)
            foreach (var item in items)
            {
                if (item.IsActive)
                    item.Draw(Globals.spriteBatch);
            }

            // Draw enemies from list
            foreach (var enemy in enemies)
            {
                if (enemy.IsActive)
                    enemy.Draw(Globals.spriteBatch);
            }

            // Draw player on top (only if not hit by tornado)
            if (player.IsActive && !playerHitByTornado)
                player.Draw();

            // Draw countdown if player was hit
            if (playerHitByTornado && countdownFont != null)
            {
                string countdownText = $"Restarting in: {(int)Math.Ceiling(restartCountdown)}";
                Vector2 textSize = countdownFont.MeasureString(countdownText);
                Vector2 textPosition = new Vector2(
                    (graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    (graphics.PreferredBackBufferHeight - textSize.Y) / 2
                );
                Globals.spriteBatch.DrawString(countdownFont, countdownText, textPosition, Color.Red);
            }
        }
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}
