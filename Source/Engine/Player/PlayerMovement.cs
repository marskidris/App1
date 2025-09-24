using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine
{
    public class PlayerMovement
    {
        private Player _player;
        private KeyboardState _previousKeyboardState;
        private float _speed = 150f;

        public PlayerMovement(Player player)
        {
            _player = player;
            _previousKeyboardState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Vector2 velocity = Vector2.Zero;
            
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y = -_speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = _speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -_speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = _speed;
            }

            if (velocity.X != 0 && velocity.Y != 0)
            {
                velocity.Normalize();
                velocity *= _speed;
            }

            if (velocity.Y < 0)
            {
                _player.SetDirection("W");
            }
            else if (velocity.Y > 0)
            {
                _player.SetDirection("S");
            }
            else if (velocity.X < 0)
            {
                _player.SetDirection("A");
            }
            else if (velocity.X > 0)
            {
                _player.SetDirection("D");
            }

            _player.Move(velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            
            _player.Update(gameTime);
            
            _previousKeyboardState = currentKeyboardState;
        }

        public void Update()
        {
            Update(new GameTime());
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }

        public float GetSpeed()
        {
            return _speed;
        }
    }
}
