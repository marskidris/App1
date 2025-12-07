using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using App1.Source.Engine.Movement;

namespace App1.Source.Engine.Player
{
    public class PlayerMovement
    {
        private Player _player;
        private KeyboardState _previousKeyboardState;
        private MovementState _currentState;
        
        private Keys _upKey;
        private Keys _downKey;
        private Keys _leftKey;
        private Keys _rightKey;
        private Keys _runKey;
        private Keys _sneakKey;

        public PlayerMovement(Player player)
            : this(player, Keys.W, Keys.S, Keys.A, Keys.D, Keys.R, Keys.E)
        {
        }

        public PlayerMovement(Player player, Keys upKey, Keys downKey, Keys leftKey, Keys rightKey, Keys runKey, Keys sneakKey)
        {
            _player = player;
            _previousKeyboardState = Keyboard.GetState();
            _currentState = new WalkingState(player);
            _upKey = upKey;
            _downKey = downKey;
            _leftKey = leftKey;
            _rightKey = rightKey;
            _runKey = runKey;
            _sneakKey = sneakKey;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Vector2 velocity = Vector2.Zero;
            
            bool rKeyPressed = currentKeyboardState.IsKeyDown(_runKey) && !_previousKeyboardState.IsKeyDown(_runKey);
            bool eKeyPressed = currentKeyboardState.IsKeyDown(_sneakKey) && !_previousKeyboardState.IsKeyDown(_sneakKey);

            if (eKeyPressed)
            {
                if (_currentState is SneakState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.1f);
                    _player.SetMovementState(false, false);
                }
                else
                {
                    _currentState = new SneakState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.2f);
                    _player.SetMovementState(false, true);
                }
            }
            else if (rKeyPressed)
            {
                if (_currentState is WalkingState)
                {
                    _currentState = new RunningState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.05f);
                    _player.SetMovementState(true, false);
                }
                else if (_currentState is RunningState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.1f);
                    _player.SetMovementState(false, false);
                }
                else if (_currentState is SneakState)
                {
                    _currentState = new RunningState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.05f);
                    _player.SetMovementState(true, false);
                }
            }
            
            _currentState.HandleInput(currentKeyboardState, _previousKeyboardState, gameTime);
            
            float currentSpeed = _currentState.GetSpeed();
            
            if (currentKeyboardState.IsKeyDown(_upKey))
            {
                velocity.Y = -currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(_downKey))
            {
                velocity.Y = currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(_leftKey))
            {
                velocity.X = -currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(_rightKey))
            {
                velocity.X = currentSpeed;
            }

            if (velocity.X != 0 && velocity.Y != 0)
            {
                velocity.Normalize();
                velocity *= currentSpeed;
            }

            _currentState.UpdateMovement(velocity, gameTime);

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
        }

        public float GetSpeed()
        {
            return _currentState.GetSpeed();
        }
    }
}
