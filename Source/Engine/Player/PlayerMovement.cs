using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine
{
    public class PlayerMovement
    {
        private Player _player;
        private KeyboardState _previousKeyboardState;
        private MovementState _currentState;
        
        private Vector2 _velocity = Vector2.Zero;

        public PlayerMovement(Player player)
        {
            _player = player;
            _previousKeyboardState = Keyboard.GetState();
            _currentState = new WalkingState(player);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            
            bool rKeyPressed = currentKeyboardState.IsKeyDown(Keys.R) && !_previousKeyboardState.IsKeyDown(Keys.R);
            bool eKeyPressed = currentKeyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E);

            if (eKeyPressed)
            {
                if (_currentState is SneakState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation(); // Reset animation when exiting sneak
                }
                else
                {
                    _currentState = new SneakState(_player);
                    _player.ResetAnimation(); // Reset animation when entering sneak
                }
            }
            else if (rKeyPressed)
            {
                if (_currentState is WalkingState)
                {
                    _currentState = new RunningState(_player);
                    _player.ResetAnimation(); // Reset animation when switching to running
                }
                else if (_currentState is RunningState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation(); // Reset animation when switching to walking
                }
                else if (_currentState is SneakState)
                {
                    _currentState = new RunningState(_player);
                    _player.ResetAnimation(); // Reset animation when switching from sneak to running
                }
            }
            
            _currentState.HandleInput(currentKeyboardState, _previousKeyboardState, gameTime);
            
            float currentSpeed = _currentState.GetSpeed();
            _velocity.X = 0f;
            _velocity.Y = 0f;
            
            if (currentKeyboardState.IsKeyDown(Keys.W))
                _velocity.Y = -currentSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.S))
                _velocity.Y = currentSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.A))
                _velocity.X = -currentSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.D))
                _velocity.X = currentSpeed;

            if (_velocity.X != 0f && _velocity.Y != 0f)
            {
                _velocity.Normalize();
                _velocity.X *= currentSpeed;
                _velocity.Y *= currentSpeed;
            }

            _currentState.UpdateMovement(_velocity, gameTime);
            
            _currentState = _currentState.GetNextState();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _player.Move(new Vector2(_velocity.X * deltaTime, _velocity.Y * deltaTime));
            _player.Update(gameTime, _currentState);
            
            _previousKeyboardState = currentKeyboardState;
        }
    }
}
