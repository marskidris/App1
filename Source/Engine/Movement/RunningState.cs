using App1.Source.Engine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine.Movement
{
    public class RunningState : MovementState
    {
        private App1.Source.Engine.Player.Player _player;
        private float _runSpeed = 300f;

        public RunningState(App1.Source.Engine.Player.Player player)
        {
            _player = player;
        }

        public void HandleInput(KeyboardState currentKeyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
        }

        public void UpdateMovement(Vector2 velocity, GameTime gameTime)
        {
            bool isCurrentlyMoving = velocity.Length() > 0;
            
            if (isCurrentlyMoving)
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
        }

        public float GetSpeed() => _runSpeed;

        public MovementState GetNextState()
        {
            return this;
        }
    }
}
