// filepath: c:\Users\Shadow\Desktop\MonoGame\MonotTest2\App1\App1\Source\Engine\Enemy\Pathfinding.cs
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace App1.Source.Engine.Enemy
{
    
    public class Pathfinding
    {
        private int gridWidth;
        private int gridHeight;
        private int cellSize;
        private bool[,] walkableGrid; // true = walkable, false = obstacle
        private Vector2 gridOffset;

        public Pathfinding(int width, int height, int cellSize, Vector2 offset = default)
        {
            this.gridWidth = width;
            this.gridHeight = height;
            this.cellSize = cellSize;
            this.gridOffset = offset;
            walkableGrid = new bool[width, height];

            // Initialize all cells as walkable by default
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    walkableGrid[x, y] = true;
                }
            }
        }

    
        public void SetBlocked(int gridX, int gridY)
        {
            if (IsValidCell(gridX, gridY))
                walkableGrid[gridX, gridY] = false;
        }

       
        public void SetWalkable(int gridX, int gridY)
        {
            if (IsValidCell(gridX, gridY))
                walkableGrid[gridX, gridY] = true;
        }

        
        public void SetBlockedArea(Rectangle worldRect)
        {
            int startX = (int)((worldRect.X - gridOffset.X) / cellSize);
            int startY = (int)((worldRect.Y - gridOffset.Y) / cellSize);
            int endX = (int)((worldRect.Right - gridOffset.X) / cellSize);
            int endY = (int)((worldRect.Bottom - gridOffset.Y) / cellSize);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    SetBlocked(x, y);
                }
            }
        }

        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
        }

        /// <summary>
        /// Convert world position to grid coordinates
        /// </summary>
        public Point WorldToGrid(Vector2 worldPos)
        {
            int x = (int)((worldPos.X - gridOffset.X) / cellSize);
            int y = (int)((worldPos.Y - gridOffset.Y) / cellSize);
            return new Point(
                MathHelper.Clamp(x, 0, gridWidth - 1),
                MathHelper.Clamp(y, 0, gridHeight - 1)
            );
        }

      
        public Vector2 GridToWorld(Point gridPos)
        {
            return new Vector2(
                gridPos.X * cellSize + cellSize / 2f + gridOffset.X,
                gridPos.Y * cellSize + cellSize / 2f + gridOffset.Y
            );
        }

       
        public List<Vector2> FindPath(Vector2 startWorld, Vector2 goalWorld)
        {
            Point start = WorldToGrid(startWorld);
            Point goal = WorldToGrid(goalWorld);

            // If goal is blocked or same as start, return direct path
            if (!IsValidCell(goal.X, goal.Y) || !walkableGrid[goal.X, goal.Y])
            {
                return new List<Vector2> { goalWorld };
            }

            if (start == goal)
            {
                return new List<Vector2> { goalWorld };
            }

            var openSet = new PriorityQueue<PathNode>();
            var closedSet = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, float>();
            var fScore = new Dictionary<Point, float>();

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);
            openSet.Enqueue(new PathNode(start, fScore[start]));

            // 8-directional movement (including diagonals)
            Point[] neighbors = new Point[]
            {
                new Point(0, -1),  // Up
                new Point(0, 1),   // Down
                new Point(-1, 0),  // Left
                new Point(1, 0),   // Right
                new Point(-1, -1), // Up-Left
                new Point(1, -1),  // Up-Right
                new Point(-1, 1),  // Down-Left
                new Point(1, 1)    // Down-Right
            };

            int iterations = 0;
            int maxIterations = gridWidth * gridHeight; // Prevent infinite loops

            while (openSet.Count > 0 && iterations < maxIterations)
            {
                iterations++;
                PathNode current = openSet.Dequeue();

                if (current.Position == goal)
                {
                    return ReconstructPath(cameFrom, current.Position, goalWorld);
                }

                closedSet.Add(current.Position);

                foreach (var dir in neighbors)
                {
                    Point neighbor = new Point(current.Position.X + dir.X, current.Position.Y + dir.Y);

                    if (!IsValidCell(neighbor.X, neighbor.Y) || 
                        !walkableGrid[neighbor.X, neighbor.Y] || 
                        closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // Diagonal movement costs more (sqrt(2) ≈ 1.41)
                    float moveCost = (dir.X != 0 && dir.Y != 0) ? 1.414f : 1f;
                    float tentativeG = gScore.GetValueOrDefault(current.Position, float.MaxValue) + moveCost;

                    if (tentativeG < gScore.GetValueOrDefault(neighbor, float.MaxValue))
                    {
                        cameFrom[neighbor] = current.Position;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);
                        openSet.Enqueue(new PathNode(neighbor, fScore[neighbor]));
                    }
                }
            }

            // No path found - return direct path to goal
            return new List<Vector2> { goalWorld };
        }

        private float Heuristic(Point a, Point b)
        {
            // Using Euclidean distance for smoother paths
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        private List<Vector2> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current, Vector2 finalGoal)
        {
            var path = new List<Vector2>();
            
            // Build path in reverse
            var points = new List<Point> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                points.Add(current);
            }

            // Reverse to get path from start to goal
            points.Reverse();

            // Skip first point (current position) and convert to world coords
            for (int i = 1; i < points.Count; i++)
            {
                path.Add(GridToWorld(points[i]));
            }

            // Ensure the exact goal position is the last point
            if (path.Count > 0)
            {
                path[path.Count - 1] = finalGoal;
            }
            else
            {
                path.Add(finalGoal);
            }

            return path;
        }

   
        private class PriorityQueue<T> where T : IComparable<T>
        {
            private List<T> data = new List<T>();

            public int Count => data.Count;

            public void Enqueue(T item)
            {
                data.Add(item);
                int ci = data.Count - 1;
                while (ci > 0)
                {
                    int pi = (ci - 1) / 2;
                    if (data[ci].CompareTo(data[pi]) >= 0) break;
                    (data[ci], data[pi]) = (data[pi], data[ci]);
                    ci = pi;
                }
            }

            public T Dequeue()
            {
                int li = data.Count - 1;
                T frontItem = data[0];
                data[0] = data[li];
                data.RemoveAt(li);

                li--;
                int pi = 0;
                while (true)
                {
                    int ci = pi * 2 + 1;
                    if (ci > li) break;
                    int rc = ci + 1;
                    if (rc <= li && data[rc].CompareTo(data[ci]) < 0)
                        ci = rc;
                    if (data[pi].CompareTo(data[ci]) <= 0) break;
                    (data[pi], data[ci]) = (data[ci], data[pi]);
                    pi = ci;
                }
                return frontItem;
            }
        }

        private struct PathNode : IComparable<PathNode>
        {
            public Point Position;
            public float FScore;

            public PathNode(Point pos, float fScore)
            {
                Position = pos;
                FScore = fScore;
            }

            public int CompareTo(PathNode other)
            {
                return FScore.CompareTo(other.FScore);
            }
        }
    }
}


