﻿﻿using App1.Source;
using App1.Source.Engine;
using App1.Source.Engine.Menu;
using App1.Source.Engine.Player;
using App1.Source.Engine.Enemy;
using App1.Source.Engine.Items;
using App1.Source.Engine.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace App1;

public class Main : Game
{
    GraphicsDeviceManager graphics;
    
    Player player;
    PlayerMovement playerMovement;
    GameState gameState;
    Map map;
    Camera camera;
    private bool isMapActive = false;
    private Microsoft.Xna.Framework.Input.KeyboardState _previousKeyboardState;

    List<Enemy> tornados;
    List<Item> presents;
    Texture2D tornadoTexture;
    Texture2D itemTexture;

    SpriteFont font;
    Time gameTimer;
    private Texture2D HUDTexture;
    private bool needsGameInit = true;
    private float invincibilityTimer = 0f;
    private const float InvincibilityDuration = 3f;
    private bool playerCaptured = false;
    private float capturedTimer = 0f;
    private const float CapturedDuration = 5f;
    private Enemy capturingTornado = null;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        Globals.graphics = graphics;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _previousKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.content = this.Content;
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

        font = Content.Load<SpriteFont>("GameFont");

        AudioState.Instance.LoadContent(Content);
        AudioState.Instance.PlayBackgroundMusic();

        HUDTexture = Content.Load<Texture2D>("2D/HUD_Display");

        map = new Map();
        map.LoadContent();
         
         gameState = new GameState();
         gameState.LoadContent();
     }

    private void InitializeGame()
    {
        string selectedCharacter = gameState?.SelectedCharacter ?? "Earl";
        string texturePath = CharacterFramesFactory.GetTexturePath(selectedCharacter);
        
        player = new Player(texturePath, new Vector2(0, 0), new Vector2(150, 150), selectedCharacter);
        playerMovement = new PlayerMovement(player);
        camera = new Camera(GraphicsDevice.Viewport);

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

        gameTimer = new Time();
    }

    protected override void Update(GameTime gameTime)
    {
        var currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            currentKeyboardState.IsKeyDown(Keys.Escape))
            Exit();
        
        if (gameState != null && gameState.QuitRequested)
        {
            Exit();
        }
        
        if (gameState != null && gameState.ReturnToTitleRequested)
        {
            needsGameInit = true;
            playerCaptured = false;
            capturedTimer = 0f;
            capturingTornado = null;
            invincibilityTimer = 0f;
            isMapActive = false;
            map?.Deactivate();
            gameState.ClearReturnToTitleRequest();
        }
 
        bool mPressed = currentKeyboardState.IsKeyDown(Keys.M) && !_previousKeyboardState.IsKeyDown(Keys.M);
        if (mPressed)
        {
            if (!isMapActive)
            {
                gameState?.ForcePlaying();
                 if (gameState != null) gameState.SuppressPauseOverlay = true;
                gameState?.PauseGameplay();
                map.Activate();
                isMapActive = true;
            }
            else
            {
                map.Deactivate();
                gameState?.ResumeGameplay();
                if (gameState != null) gameState.SuppressPauseOverlay = false;
                isMapActive = false;
            }
         }
        
        gameState.Update(gameTime);

        if (gameState.IsPlaying() && needsGameInit)
        {
            InitializeGame();
            needsGameInit = false;
        }

        if (gameState.IsPlaying())
        {
            gameTimer.Update(gameTime);

            playerMovement.Update(gameTime);
            
            camera.Follow(player.Position);

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
                    AudioState.Instance.PlayMoneySoundWithVolumeAdjustment();
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                foreach (var tornado in tornados)
                {
                    if (!tornado.IsRemoved && tornado.IsAlive)
                    {
                        float distance = Vector2.Distance(player.Position, tornado.Position);
                        if (distance < 100f)
                        {
                            tornado.TakeDamage();
                            System.Console.WriteLine($"[Enemy {tornado.EnemyId}] Eliminated by player!");
                        }
                    }
                }
            }

            if (invincibilityTimer > 0)
            {
                invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (playerCaptured)
            {
                capturedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (capturingTornado != null && !capturingTornado.IsRemoved)
                {
                    capturingTornado.Update(gameTime);
                }
                
                if (capturedTimer >= CapturedDuration)
                {
                    int damage = player.Health.MaxHealth / 2;
                    player.Health.TakeDamage(damage);
                    playerCaptured = false;
                    capturedTimer = 0f;
                    if (capturingTornado != null)
                    {
                        capturingTornado.IsRemoved = true;
                    }
                    capturingTornado = null;
                    invincibilityTimer = InvincibilityDuration;
                }
            }
            else
            {
                foreach (var tornado in tornados)
                {
                    if (!tornado.IsRemoved)
                    {
                        tornado.Update(gameTime);

                        if (invincibilityTimer <= 0 && tornado.BoundingBox.Intersects(playerBounds))
                        {
                            playerCaptured = true;
                            capturedTimer = 0f;
                            capturingTornado = tornado;
                            capturingTornado.ForceAlertFrames();
                            break;
                        }
                    }
                }
            }
        }
        
        base.Update(gameTime);
        _previousKeyboardState = currentKeyboardState;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        if (isMapActive && map != null)
        {
            map.Draw(Globals.spriteBatch);
            Globals.spriteBatch.End();
            base.Draw(gameTime);
            return;
        }

        gameState.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();
        
        if (gameState.ShowGameContent())
        {
            Matrix? transformMatrix = camera?.GetTransformMatrix();
            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transformMatrix);

            foreach (var present in presents)
            {
                present.Draw(Globals.spriteBatch);
            }

            foreach (var tornado in tornados)
            {
                if (!tornado.IsRemoved)
                    tornado.Draw(Globals.spriteBatch);
            }

            bool showPlayer = true;
            if (playerCaptured)
            {
                showPlayer = false;
            }
            else if (invincibilityTimer > 0)
            {
                showPlayer = ((int)(invincibilityTimer * 10) % 2) == 0;
            }
            
            if (showPlayer)
            {
                player.Draw();
            }

            Globals.spriteBatch.End();

            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (player != null && HUDTexture != null)
            {
                Rectangle healthSource = new Rectangle(56, 160, 32, 3);
                int destX = 10;
                int destY = 50;
                int destWidth = 160;
                int destHeight = 12;

                Globals.spriteBatch.DrawString(font, "Health", new Vector2(destX, 10), Color.White);

                Rectangle destRectBg = new Rectangle(destX, destY, destWidth, destHeight);
                Globals.spriteBatch.Draw(HUDTexture, destRectBg, healthSource, Color.DarkGray);

                float healthPercent = 1f;
                if (player.Health != null && player.Health.MaxHealth > 0)
                {
                    healthPercent = (float)player.Health.CurrentHealth / (float)player.Health.MaxHealth;
                    if (healthPercent < 0f) healthPercent = 0f;
                    if (healthPercent > 1f) healthPercent = 1f;
                }

                int filledWidth = (int)(destWidth * healthPercent);
                if (filledWidth > 0)
                {
                    Rectangle destRectFill = new Rectangle(destX, destY, filledWidth, destHeight);
                    Globals.spriteBatch.Draw(HUDTexture, destRectFill, healthSource, Color.White);
                }
            }
            
            if (map != null)
            {
                map.Draw(Globals.spriteBatch);
            }

            Globals.spriteBatch.End();
        }

        base.Draw(gameTime);
    }
}
