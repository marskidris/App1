using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace App1.Source.Engine.Enemy
{
    
    public static class PatrolMovement
    {
        
        public static void StepAlongWaypoints(App1.Source.Engine.Enemy.Enemy enemy, GameTime gameTime, ref int currentWaypointIndex, float stoppingDistance = 5f)
        {
            if (enemy.Waypoints == null || enemy.Waypoints.Count == 0)
                return;

            Vector2 targetWaypoint = enemy.Waypoints[currentWaypointIndex];
            Vector2 direction = targetWaypoint - enemy.Position;
            float distance = direction.Length();

            if (distance > stoppingDistance)
            {
                direction.Normalize();
                enemy.Position += direction * enemy.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % enemy.Waypoints.Count;
            }
        }

       
        public static bool StepAlongPath(App1.Source.Engine.Enemy.Enemy enemy, GameTime gameTime, List<Vector2> path, ref int pathIndex, float stoppingDistance = 5f)
        {
            if (path == null || path.Count == 0 || pathIndex >= path.Count)
                return true; // Path completed

            Vector2 targetPoint = path[pathIndex];
            Vector2 direction = targetPoint - enemy.Position;
            float distance = direction.Length();

            if (distance > stoppingDistance)
            {
                direction.Normalize();
                enemy.Position += direction * enemy.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                return false;
            }
            else
            {
                pathIndex++;
                return pathIndex >= path.Count; // Return true if path completed
            }
        }

      
        public static void PatrolWithPathfinding(
            App1.Source.Engine.Enemy.Enemy enemy,
            GameTime gameTime,
            Pathfinding pathfinder,
            ref int currentWaypointIndex,
            ref List<Vector2> currentPath,
            ref int currentPathIndex,
            float stoppingDistance = 5f)
        {
            if (enemy.Waypoints == null || enemy.Waypoints.Count == 0)
                return;

            // Calculate new path if needed
            if (currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
            {
                Vector2 targetWaypoint = enemy.Waypoints[currentWaypointIndex];
                currentPath = pathfinder.FindPath(enemy.Position, targetWaypoint);
                currentPathIndex = 0;
            }

            // Follow the current path
            bool pathCompleted = StepAlongPath(enemy, gameTime, currentPath, ref currentPathIndex, stoppingDistance);

            // If path completed, move to next waypoint
            if (pathCompleted)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % enemy.Waypoints.Count;
                currentPath = null;
                currentPathIndex = 0;
            }
        }
    }
}
