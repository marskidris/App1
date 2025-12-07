using System;
using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Enemy
{
    public static class PatrolDetection
    {
        public static bool DetectTarget(Enemy enemy, float detectionRange = 200f)
        {
            if (enemy.Player == null)
            {
                Console.WriteLine($"[Enemy {enemy.EnemyId}] Player is null!");
                return false;
            }

            float distanceToPlayer = Vector2.Distance(enemy.Position, enemy.Player.Position);
            bool detected = distanceToPlayer < detectionRange;
            
            if (detected)
            {
                Console.WriteLine($"[Enemy {enemy.EnemyId}] Player DETECTED! Distance: {distanceToPlayer:F1}, Range: {detectionRange}");
            }
            
            return detected;
        }
    }
}
