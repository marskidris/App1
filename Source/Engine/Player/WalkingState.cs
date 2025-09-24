using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace App1.Source.Engine
{
    public class WalkingState : MovementState
    {
        private Player _player;
        private float _speed = 150f;

        public WalkingState(Player player)
        {
            _player = player;
        }

        public void HandleInput(KeyboardState currentKeyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
            // No complex timing logic needed - just check for R key press
        }

        public void UpdateMovement(Vector2 velocity, GameTime gameTime)
        {
            _player.SetRunning(false);
            
            if (velocity.Y < 0)
                _player.SetDirection(0);
            else if (velocity.Y > 0)
                _player.SetDirection(1);
            else if (velocity.X < 0)
                _player.SetDirection(2);
            else if (velocity.X > 0)
                _player.SetDirection(3);
        }

        public float GetSpeed() => _speed;

        public MovementState GetNextState()
        {
            // Check if R key was just pressed to switch to running
            return this;
        }
    }
}
