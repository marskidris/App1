using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Enemy
{
    public static class AlertMovement
    {
        public static void MoveTowardsTarget(App1.Source.Engine.Enemy.Enemy enemy, GameTime gameTime, float stoppingDistance = 10f)
        {
            if (enemy.Player == null)
                return;

            Vector2 direction = enemy.Player.Position - enemy.Position;
            float distance = direction.Length();

            if (distance > stoppingDistance)
            {
                direction.Normalize();
                enemy.Position += direction * enemy.Speed * 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
