using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using App1.Source.Engine.Collision;
using System;

namespace App1.Source.Engine.Enemy;

public class Enemy
{
    private Texture2D texture;
    private Vector2 position;
    private Vector2 size;
    private float scale = 1f;
    private bool hasCollided = false;

    // Animation properties
    private EnemyFrames enemyFrames;
    private Rectangle[] currentFrames;
    private int currentFrameIndex = 0;
    private float animationTimer = 0f;
    private float frameDuration = 0.15f;
    private bool isStartupComplete = false;
    private bool hasStartedAnimation = false;

    // Attack state
    private bool isAttacking = false;
    private float attackAnimationTimer = 0f;
    private int attackFrameIndex = 0;

    // Movement properties
    private Vector2 velocity;
    private float moveSpeed;
    private Vector2 startPosition;
    private float patrolDistance;
    private bool movingRight = true;
    private bool isStationary;

    // Chase properties
    private const float chaseActivationDistance = 25f;
    private bool isChasing = false;
    private float chaseSpeed;

    public Enemy(Texture2D texture, Vector2 position, float scale = 1f, float moveSpeed = 30f, float patrolDistance = 150f, bool stationary = false)
    {
        this.texture = texture;
        this.position = position;
        this.scale = scale;
        this.moveSpeed = moveSpeed;
        this.chaseSpeed = moveSpeed * 1.5f; // Chase speed is faster than patrol
        this.patrolDistance = patrolDistance;
        this.startPosition = position;
        this.isStationary = stationary;
        this.velocity = new Vector2(moveSpeed, 0);

        // Initialize frames from EnemyFrames
        this.enemyFrames = new EnemyFrames();
        this.currentFrames = enemyFrames.GetTornadoUAFrames();
        this.size = new Vector2(currentFrames[0].Width, currentFrames[0].Height);
    }

    public void Update(GameTime gameTime, Vector2 playerPosition)
    {
        if (!IsActive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update attack animation if attacking (but still allow movement)
        if (isAttacking)
        {
            attackAnimationTimer += deltaTime;
            if (attackAnimationTimer >= frameDuration)
            {
                attackFrameIndex++;
                if (attackFrameIndex >= 8) // Loop through all 8 attack frames
                {
                    attackFrameIndex = 0;
                }
                attackAnimationTimer = 0f;
            }
            // Continue to movement logic even while attacking
        }

        // Check distance to player (only check if not already attacking)
        float distanceToPlayer = Vector2.Distance(CircleCenter, playerPosition);

        // Update chase state based on distance (only if not in attack mode)
        if (!isAttacking)
        {
            if (distanceToPlayer <= chaseActivationDistance)
            {
                isChasing = true;
            }
            else if (distanceToPlayer > chaseActivationDistance * 2) // Stop chasing if player gets far enough
            {
                isChasing = false;
            }
        }

        // Update animation based on startup/patrol phase (only if not attacking)
        if (!isAttacking)
        {
            if (!hasStartedAnimation)
            {
                // Stay on frame 0 until first update
                hasStartedAnimation = true;
            }
            else
            {
                animationTimer += deltaTime;
                if (animationTimer >= frameDuration)
                {
                    if (!isStartupComplete)
                    {
                        // Startup phase: frames 0-3
                        currentFrameIndex++;
                        if (currentFrameIndex > 3)
                        {
                            isStartupComplete = true;
                            currentFrameIndex = 4; // Start patrol phase
                        }
                    }
                    else
                    {
                        // Patrol phase: frames 4-7
                        currentFrameIndex++;
                        if (currentFrameIndex > 7)
                        {
                            currentFrameIndex = 4;
                        }
                    }
                    animationTimer = 0f;
                }
            }
        }

        if (isStationary && !isChasing) return;

        // Chase or patrol movement
        if (isChasing)
        {
            // Chase the player
            Vector2 direction = playerPosition - CircleCenter;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                position += direction * chaseSpeed * deltaTime;
            }
        }
        else if (isStartupComplete)
        {
            // Horizontal patrol movement (only after startup)
            if (movingRight)
            {
                position.X += moveSpeed * deltaTime;
                if (position.X >= startPosition.X + patrolDistance)
                {
                    movingRight = false;
                }
            }
            else
            {
                position.X -= moveSpeed * deltaTime;
                if (position.X <= startPosition.X - patrolDistance)
                {
                    movingRight = true;
                }
            }
        }
    }

    public void CheckCollisionWithPlayer(Rectangle playerBoundingBox, Vector2 playerCircleCenter, float playerCircleRadius)
    {
        if (!IsActive) return;

        // Check rectangle collision
        bool rectangleCollision = Collision2D.CheckRectangleCollision(BoundingBox, playerBoundingBox);

        // Check circle collision
        bool circleCollision = Collision2D.CheckCircleCollision(CircleCenter, CircleRadius, playerCircleCenter, playerCircleRadius);

        // If collision detected and haven't printed message yet
        if ((rectangleCollision || circleCollision) && !hasCollided)
        {
            Console.WriteLine("enemy encountered");
            hasCollided = true;
        }

        // Reset flag if no longer colliding
        if (!rectangleCollision && !circleCollision && hasCollided)
        {
            hasCollided = false;
        }
    }

    public void ActivateAttackState()
    {
        isAttacking = true;
        attackFrameIndex = 0;
        attackAnimationTimer = 0f;
        // Switch to attack frames
        currentFrames = enemyFrames.GetTornadoAFrames();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;

        Rectangle currentFrame;
        if (isAttacking)
        {
            currentFrame = currentFrames[attackFrameIndex];
        }
        else
        {
            currentFrame = currentFrames[currentFrameIndex];
        }

        spriteBatch.Draw(
            texture,
            new Rectangle((int)position.X, (int)position.Y, (int)(currentFrame.Width * scale), (int)(currentFrame.Height * scale)),
            currentFrame,
            Color.White,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            0f
        );
    }

    public bool IsAttacking => isAttacking;

    // Collision properties
    public bool IsActive { get; set; } = true;

    public Rectangle BoundingBox
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)scaledSize.X,
                (int)scaledSize.Y
            );
        }
    }

    public Vector2 CircleCenter
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return new Vector2(
                position.X + scaledSize.X / 2,
                position.Y + scaledSize.Y / 2
            );
        }
    }

    public float CircleRadius
    {
        get
        {
            Vector2 scaledSize = size * scale;
            return System.Math.Min(scaledSize.X, scaledSize.Y) / 2;
        }
    }

    public Vector2 Position => position;
}