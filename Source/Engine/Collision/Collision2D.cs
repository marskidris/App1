using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Collision
{
    public static class Collision2D
    {
        public static bool CheckRectangleCollision(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);
        }

        public static bool CheckCircleCollision(Vector2 center1, float radius1, Vector2 center2, float radius2)
        {
            float distance = Vector2.Distance(center1, center2);
            return distance < (radius1 + radius2);
        }

        public static bool CheckRectangleCircleCollision(Rectangle rect, Vector2 circleCenter, float circleRadius)
        {
            float closestX = MathHelper.Clamp(circleCenter.X, rect.Left, rect.Right);
            float closestY = MathHelper.Clamp(circleCenter.Y, rect.Top, rect.Bottom);

            float distanceX = circleCenter.X - closestX;
            float distanceY = circleCenter.Y - closestY;
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

            return distanceSquared < (circleRadius * circleRadius);
        }

        public static bool CheckPointInRectangle(Vector2 point, Rectangle rect)
        {
            return rect.Contains(point);
        }

        public static bool CheckPointInCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            float distance = Vector2.Distance(point, circleCenter);
            return distance <= circleRadius;
        }
    }
}

