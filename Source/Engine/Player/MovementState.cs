using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace App1.Source.Engine
{
    public interface MovementState
    {
        void HandleInput(KeyboardState currentKeyboard, KeyboardState previousKeyboard, GameTime gameTime);
        void UpdateMovement(Vector2 velocity, GameTime gameTime);
        float GetSpeed();
        MovementState GetNextState();
    }
}
