﻿﻿﻿using App1.Source;
using App1.Source.Engine;
using App1.Source.Engine.Menu;
using App1.Source.Engine.Player;
using App1.Source.Engine.Enemy;
using App1.Source.Engine.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace App1;

public class Main : Game
{
    GraphicsDeviceManager graphics;
    
    Player player;
    PlayerMovement playerMovement;
    GameState gameState;

    List<Enemy> tornados;
    List<Item> presents;
    Texture2D tornadoTexture;
    Texture2D itemTexture;

    bool playerCaught = false;
    float restartTimer = 0f;
    const float RestartDelay = 10f;
    SpriteFont font;
    Enemy catchingTornado;
    Time gameTimer;

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

        font = Content.Load<SpriteFont>("GameFont");

        // Initialize game state system
        gameState = new GameState();
        gameState.LoadContent();

        InitializeGame();
    }

    private void InitializeGame()
    {
        player = new Player("2D/Earl_Transparent", new Vector2(640, 360), new Vector2(150, 150));
        playerMovement = new PlayerMovement(player);

        tornadoTexture = Content.Load<Texture2D>("2D/Tornado");
        itemTexture = Content.Load<Texture2D>("2D/items_scenery_tranparent");

        Vector2[] presentPositions = new Vector2[]
        {
            new Vector2(300, 200),
            new Vector2(900, 200),
            new Vector2(300, 500),
            new Vector2(900, 500)
        };

        presents = new List<Item>();
        tornados = new List<Enemy>();

        Rectangle presentFrame = new Rectangle(137, 10, 15, 14);

        for (int i = 0; i < 4; i++)
        {
            presents.Add(new Item(
                itemTexture,
                presentPositions[i],
                new Vector2(15, 14),
                presentFrame,
                4f
            ));

            Vector2 tornadoPos = presentPositions[i] + new Vector2(80, 0);
            Enemy tornado = new Enemy(tornadoTexture, tornadoPos, player, 4f, i + 1);

            Vector2 p1 = presentPositions[i] + new Vector2(200, 0);
            Vector2 p2 = presentPositions[i] + new Vector2(-100, 0);
            Vector2 p3 = presentPositions[i] + new Vector2(-100, 150);
            Vector2 p4 = presentPositions[i] + new Vector2(200, 150);
            tornado.SetWaypoints(p1, p2, p3, p4);

            tornados.Add(tornado);
        }

        playerCaught = false;
        restartTimer = 0f;
        catchingTornado = null;
        gameTimer = new Time();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        gameState.Update(gameTime);

        if (gameState.IsPlaying())
        {
            // Update game timer (prints to console every second)
            gameTimer.Update(gameTime);
            
            if (playerCaught)
            {
                restartTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (catchingTornado != null)
                {
                    catchingTornado.Update(gameTime);
                }
                if (restartTimer >= RestartDelay)
                {
                    InitializeGame();
                }
                return;
            }

            playerMovement.Update(gameTime);

            Rectangle playerBounds = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y,
                (int)player.Size.X,
                (int)player.Size.Y
            );

            foreach (var present in presents)
            {
                if (present.IsActive && present.BoundingBox.Intersects(playerBounds))
                {
                    present.IsActive = false;
                }
            }

            // Check if player presses Space to eliminate nearby enemies (requirement 1c)
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                foreach (var tornado in tornados)
                {
                    if (!tornado.IsRemoved && tornado.IsAlive)
                    {
                        float distance = Vector2.Distance(player.Position, tornado.Position);
                        if (distance < 100f) // Attack range
                        {
                            tornado.TakeDamage();
                            System.Console.WriteLine($"[Enemy {tornado.EnemyId}] Eliminated by player!");
                        }
                    }
                }
            }

            foreach (var tornado in tornados)
            {
                if (!tornado.IsRemoved)
                {
                    tornado.Update(gameTime);

                    if (tornado.BoundingBox.Intersects(playerBounds))
                    {
                        playerCaught = true;
                        catchingTornado = tornado;
                        tornado.TakeDamage(); // Trigger Dead state
                        break;
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
        
        // Draw game state screens (start screen, pause screen)
        gameState.Draw(Globals.spriteBatch);
        
        // Only draw game content when playing
        if (gameState.ShowGameContent())
        {
            foreach (var present in presents)
            {
                present.Draw(Globals.spriteBatch);
            }

            foreach (var tornado in tornados)
            {
                if (!tornado.IsRemoved)
                    tornado.Draw(Globals.spriteBatch);
            }


            if (!playerCaught)
            {
                player.Draw();
            }

            if (playerCaught)
            {
                if (catchingTornado != null)
                {
                    catchingTornado.Draw(Globals.spriteBatch);
                }

                // Display "Player Captured!" message
                string capturedText = "PLAYER CAPTURED!";
                Vector2 capturedSize = font.MeasureString(capturedText);
                Vector2 capturedPos = new Vector2(
                    (graphics.PreferredBackBufferWidth - capturedSize.X) / 2,
                    60
                );
                Globals.spriteBatch.DrawString(font, capturedText, capturedPos, Color.Red);

                int secondsLeft = (int)(RestartDelay - restartTimer) + 1;
                string countdownText = $"Restarting in {secondsLeft}...";
                Vector2 textSize = font.MeasureString(countdownText);
                Vector2 textPos = new Vector2(
                    (graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    100
                );
                Globals.spriteBatch.DrawString(font, countdownText, textPos, Color.Red);
            }
        }
        
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}
