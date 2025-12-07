using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace App1.Source.Engine.Player
{
    public class Player
    {
        Texture2D texture;
        Vector2 position;
        Vector2 size;
        ICharacterFrames characterFrames;
        CharacterType characterType;
        int currentFrame;
        int direction = 1;
        float timer;
        float switchTime = 0.1f;
        bool isMoving = false;
        bool isRunning = false;
        bool isSneaking = false;
        // Player health
        private PlayerHealth health;

        public Player(string texturePath, Vector2 position, Vector2 size, CharacterType characterType = CharacterType.Earl)
        {
            this.position = position;
            this.size = size;
            this.characterType = characterType;
            health = new PlayerHealth(100);
            
            try
            {
                texture = Globals.content.Load<Texture2D>(texturePath);
            }
            catch (ContentLoadException)
            {
                string pngPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", texturePath + ".png");
                if (File.Exists(pngPath))
                {
                    using var fs = File.OpenRead(pngPath);
                    texture = Texture2D.FromStream(Globals.spriteBatch.GraphicsDevice, fs);
                }
                else
                {
                    throw;
                }
            }
            
            characterFrames = CharacterFramesFactory.Create(characterType);
            currentFrame = 0;
            timer = 0f;
        }

        public Player(string texturePath, Vector2 position, Vector2 size, string characterName)
            : this(texturePath, position, size, ParseCharacterType(characterName))
        {
        }

        private static CharacterType ParseCharacterType(string characterName)
        {
            return characterName?.ToLower() switch
            {
                "toejam" => CharacterType.ToeJam,
                _ => CharacterType.Earl
            };
        }

        public void Move(Vector2 movement)
        {
            position += movement;
            isMoving = movement != Vector2.Zero;
        }

        public void SetDirection(string dir)
        {
            int newDirection = dir switch
            {
                "W" => 0,
                "S" => 1,
                "A" => 2,
                "D" => 3,
                _ => direction
            };
            
            if (newDirection != direction)
            {
                currentFrame = 0;
                direction = newDirection;
            }
        }

        public void SetDirection(int dir)
        {
            if (dir < 0 || dir > 3) return;
            if (dir != direction)
            {
                currentFrame = 0;
                direction = dir;
            }
        }

        public void SetMovementState(bool running, bool sneaking)
        {
            if (isRunning != running || isSneaking != sneaking)
            {
                currentFrame = 0;
                timer = 0f;
            }
            isRunning = running;
            isSneaking = sneaking;
        }

        public void Update(GameTime gameTime)
        {
            if (isMoving)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (timer >= switchTime)
                {
                    currentFrame++;
                    timer = 0f;
                    
                    Rectangle[] currentFrames = characterFrames.GetFrames(isRunning, isSneaking, direction);
                    
                    if (currentFrame >= currentFrames.Length)
                        currentFrame = 0;
                }
            }
            else
            {
                currentFrame = 0; 
            }
        }

        public void Draw()
        {
            if (texture == null) return;

            Rectangle[] currentFrames = characterFrames.GetFrames(isRunning, isSneaking, direction);
            
            if (currentFrame >= currentFrames.Length)
                currentFrame = 0;

            Globals.spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                currentFrames[currentFrame],
                Color.White,
                0.0f,
                new Vector2(0, 0),
                SpriteEffects.None,
                0.0f
            );
        }

        public Vector2 Position => position;

        public Vector2 Size
        {
            get => size;
            set => size = value;
        }

        public CharacterType CurrentCharacterType => characterType;

        public void ResetAnimation()
        {
            currentFrame = 0;
            timer = 0f;
        }

        public void SetAnimationSpeed(float speed)
        {
            switchTime = speed;
        }

        public float GetAnimationSpeed()
        {
            return switchTime;
        }

        public PlayerHealth Health => health;
    }
}