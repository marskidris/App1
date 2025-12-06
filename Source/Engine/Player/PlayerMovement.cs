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

        public PlayerMovement(Player player)
        {
            _player = player;
            _previousKeyboardState = Keyboard.GetState();
            _currentState = new WalkingState(player);
            Console.WriteLine("Walking State is Active");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Vector2 velocity = Vector2.Zero;
            
            bool rKeyPressed = currentKeyboardState.IsKeyDown(Keys.R) && !_previousKeyboardState.IsKeyDown(Keys.R);
            bool eKeyPressed = currentKeyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E);

            if (eKeyPressed)
            {
                if (_currentState is SneakState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.1f);
                    _player.SetMovementState(false, false);
                    Console.WriteLine("Walking State is Active");
                }
                else
                {
                    _currentState = new SneakState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.2f);
                    _player.SetMovementState(false, true);
                    Console.WriteLine("Sneak State is Active");
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
                    Console.WriteLine("Running State is Active");
                }
                else if (_currentState is RunningState)
                {
                    _currentState = new WalkingState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.1f);
                    _player.SetMovementState(false, false);
                    Console.WriteLine("Walking State is Active");
                }
                else if (_currentState is SneakState)
                {
                    _currentState = new RunningState(_player);
                    _player.ResetAnimation();
                    _player.SetAnimationSpeed(0.05f);
                    _player.SetMovementState(true, false);
                    Console.WriteLine("Running State is Active");
                }
            }
            
            _currentState.HandleInput(currentKeyboardState, _previousKeyboardState, gameTime);
            
            float currentSpeed = _currentState.GetSpeed();
            
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y = -currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -currentSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
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
