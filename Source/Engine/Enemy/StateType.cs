using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App1.Source.Engine.Enemy
{
    public enum StateType
    {
        Patrol,
        Alert,
        Dead
    }

    public static class StateDisplay
    {
        public static string GetStateText(StateType state)
        {
            return state switch
            {
                StateType.Patrol => "State: PATROL - Searching for player...",
                StateType.Alert => "State: ALERT - Chasing player!",
                StateType.Dead => "State: DEAD - Player Captured!",
                _ => "State: Unknown"
            };
        }

        public static void PrintStateToConsole(StateType state, int enemyId = -1)
        {
            string prefix = enemyId >= 0 ? $"[Enemy {enemyId}] " : "";
            string message = state switch
            {
                StateType.Patrol => $"{prefix}PATROL - Searching for player...",
                StateType.Alert => $"{prefix}ALERT - Chasing player!",
                StateType.Dead => $"{prefix}DEAD - Player Captured!",
                _ => $"{prefix}Unknown State"
            };
            Console.WriteLine(message);
        }

        public static void PrintStateTransition(StateType fromState, StateType toState, int enemyId = -1)
        {
            if (fromState == toState)
                return;
                
            string prefix = enemyId >= 0 ? $"[Enemy {enemyId}] " : "";
            Console.WriteLine($"{prefix}State changed: {fromState} -> {toState}");
            
            if (toState == StateType.Dead)
            {
                Console.WriteLine($"{prefix}*** PLAYER CAPTURED! ***");
            }
        }

        public static Color GetStateColor(StateType state)
        {
            return state switch
            {
                StateType.Patrol => Color.Green,
                StateType.Alert => Color.Orange,
                StateType.Dead => Color.Red,
                _ => Color.White
            };
        }

        public static void DrawStateText(SpriteBatch spriteBatch, SpriteFont font, StateType state, Vector2 position)
        {
            string text = GetStateText(state);
            Color color = GetStateColor(state);
            spriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawStateAboveEnemy(SpriteBatch spriteBatch, SpriteFont font, StateType state, Vector2 enemyPosition)
        {
            string text = GetStateText(state);
            Color color = GetStateColor(state);
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPos = new Vector2(enemyPosition.X - textSize.X / 2, enemyPosition.Y - 50);
            spriteBatch.DrawString(font, text, textPos, color);
        }
    }


    public interface IEnemyState
    {
        void Enter(Enemy enemy);
        void Update(Enemy enemy, GameTime gameTime);
        void Exit(Enemy enemy);
    }
}

