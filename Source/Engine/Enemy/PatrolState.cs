using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace App1.Source.Engine.Enemy
{
    public class PatrolState : IEnemyState
    {
        private int currentWaypointIndex = 0;
        private float detectionRange = 225f;
        private float stoppingDistance = 5f;
        
        private Pathfinding pathfinder;
        private List<Vector2> currentPath;
        private int currentPathIndex = 0;
        
        private const int GRID_WIDTH = 50;
        private const int GRID_HEIGHT = 50;
        private const int CELL_SIZE = 32;

        public void Enter(App1.Source.Engine.Enemy.Enemy enemy)
        {
            currentWaypointIndex = 0;
            currentPath = null;
            currentPathIndex = 0;
            
            Vector2 gridOffset = CalculateGridOffset(enemy);
            pathfinder = new Pathfinding(GRID_WIDTH, GRID_HEIGHT, CELL_SIZE, gridOffset);
        }

        public void Update(App1.Source.Engine.Enemy.Enemy enemy, GameTime gameTime)
        {
            if (!enemy.IsAlive)
            {
                enemy.ChangeState(StateType.Dead);
                return;
            }

            if (PatrolDetection.DetectTarget(enemy, detectionRange))
            {
                enemy.ChangeState(StateType.Alert);
                return;
            }

            PatrolMovement.PatrolWithPathfinding(
                enemy,
                gameTime,
                pathfinder,
                ref currentWaypointIndex,
                ref currentPath,
                ref currentPathIndex,
                stoppingDistance
            );
        }

        public void Exit(App1.Source.Engine.Enemy.Enemy enemy)
        {
            currentPath = null;
            pathfinder = null;
        }
        
        private Vector2 CalculateGridOffset(App1.Source.Engine.Enemy.Enemy enemy)
        {
            if (enemy.Waypoints == null || enemy.Waypoints.Count == 0)
            {
                return new Vector2(
                    enemy.Position.X - (GRID_WIDTH * CELL_SIZE / 2f),
                    enemy.Position.Y - (GRID_HEIGHT * CELL_SIZE / 2f)
                );
            }
            
            float minX = enemy.Position.X;
            float maxX = enemy.Position.X;
            float minY = enemy.Position.Y;
            float maxY = enemy.Position.Y;
            
            foreach (var wp in enemy.Waypoints)
            {
                if (wp.X < minX) minX = wp.X;
                if (wp.X > maxX) maxX = wp.X;
                if (wp.Y < minY) minY = wp.Y;
                if (wp.Y > maxY) maxY = wp.Y;
            }
            
            float centerX = (minX + maxX) / 2f;
            float centerY = (minY + maxY) / 2f;
            
            return new Vector2(
                centerX - (GRID_WIDTH * CELL_SIZE / 2f),
                centerY - (GRID_HEIGHT * CELL_SIZE / 2f)
            );
        }
    }
}
