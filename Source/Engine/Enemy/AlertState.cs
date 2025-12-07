using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace App1.Source.Engine.Enemy
{
    public class AlertState : IEnemyState
    {
        private float _chaseSpeed = 1.5f;
        private float _stoppingDistance = 10f;
        private float _losePlayerDistance = 250f;
        private float _attackDistance = 20f;
        
        private Pathfinding _pathfinder;
        private List<Vector2> _chasePath;
        private int _chasePathIndex;
        private float _pathRecalculateTimer;
        private const float PathRecalculateInterval = 0.5f;
        
        private const int GridWidth = 50;
        private const int GridHeight = 50;
        private const int CellSize = 32;

        public void Enter(Enemy enemy)
        {
            _chasePath = null;
            _chasePathIndex = 0;
            _pathRecalculateTimer = 0f;
            
            Vector2 gridOffset = new Vector2(
                enemy.Position.X - (GridWidth * CellSize / 2f),
                enemy.Position.Y - (GridHeight * CellSize / 2f)
            );
            _pathfinder = new Pathfinding(GridWidth, GridHeight, CellSize, gridOffset);
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (!enemy.IsAlive)
            {
                enemy.ChangeState(StateType.Dead);
                return;
            }

            if (enemy.Player == null)
            {
                enemy.ChangeState(StateType.Patrol);
                return;
            }

            float distanceToPlayer = Vector2.Distance(enemy.Position, enemy.Player.Position);

            if (distanceToPlayer > _losePlayerDistance)
            {
                enemy.ChangeState(StateType.Patrol);
                return;
            }

            if (distanceToPlayer <= _attackDistance)
            {
                return;
            }

            _pathRecalculateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_chasePath == null || _chasePathIndex >= _chasePath.Count || _pathRecalculateTimer >= PathRecalculateInterval)
            {
                _chasePath = _pathfinder.FindPath(enemy.Position, enemy.Player.Position);
                _chasePathIndex = 0;
                _pathRecalculateTimer = 0f;
            }

            if (_chasePath != null && _chasePath.Count > 0 && _chasePathIndex < _chasePath.Count)
            {
                Vector2 targetPoint = _chasePath[_chasePathIndex];
                Vector2 direction = targetPoint - enemy.Position;
                float distance = direction.Length();

                if (distance > _stoppingDistance)
                {
                    direction.Normalize();
                    enemy.Position += direction * enemy.Speed * _chaseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _chasePathIndex++;
                }
            }
            else
            {
                AlertMovement.MoveTowardsTarget(enemy, gameTime, _stoppingDistance);
            }
        }

        public void Exit(Enemy enemy)
        {
            _chasePath = null;
            _pathfinder = null;
        }
    }
}
