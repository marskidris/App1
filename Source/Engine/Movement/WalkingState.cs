using App1.Source.Engine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace App1.Source.Engine.Movement
{
    public class WalkingState : MovementState
    {
        private App1.Source.Engine.Player.Player _player;
        private float _speed = 150f;

        public WalkingState(App1.Source.Engine.Player.Player player)
        {
            _player = player;
        }

        public void HandleInput(KeyboardState currentKeyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
        }

        public void UpdateMovement(Vector2 velocity, GameTime gameTime)
        {
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
            return this;
        }
    }
}
