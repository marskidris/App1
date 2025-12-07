using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Enemy
{
    public class DeathState : IEnemyState
    {
        private float deathDuration = 2.0f;
        private float timer = 0f;

        public void Enter(Enemy enemy)
        {
            enemy.ForceAlertFrames();
            enemy.IsAlive = false;
            timer = 0f;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= deathDuration)
            {
                enemy.IsRemoved = true;
            }
        }

        public void Exit(Enemy enemy)
        {
        }
    }
}
