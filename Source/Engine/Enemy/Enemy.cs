using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace App1.Source.Engine.Enemy
{
    public class Enemy
    {
        public Vector2 Position { get; set; }
        public float Speed { get; set; } = 60f;
        public bool IsAlive { get; set; } = true;
        public bool IsRemoved { get; set; } = false;
        public App1.Source.Engine.Player.Player Player { get; set; }
        public List<Vector2> Waypoints { get; set; } = new List<Vector2>();
        public int EnemyId { get; set; } = -1;

        private IEnemyState _currentState;
        private Dictionary<StateType, IEnemyState> _states = new Dictionary<StateType, IEnemyState>();
        private StateType _currentStateType;
        private Texture2D _texture;
        private Vector2 _origin;

        private EnemyFrames _enemyFrames;
        private Rectangle[] _currentFrames;
        private int _currentFrameIndex;
        private float _animationTimer;
        private float _animationSpeed = 0.1f;
        private float _scale;

        public Enemy(Texture2D texture, Vector2 startPosition, App1.Source.Engine.Player.Player player, float scale = 2f, int enemyId = -1)
        {
            _texture = texture;
            Position = startPosition;
            Player = player;
            _scale = scale;
            EnemyId = enemyId;

            _enemyFrames = new EnemyFrames();
            _currentFrames = _enemyFrames.GetTornadoUAFrames();
            _currentFrameIndex = 0;
            _origin = new Vector2(_currentFrames[0].Width / 2f, _currentFrames[0].Height / 2f);

            _states[StateType.Patrol] = new PatrolState();
            _states[StateType.Alert] = new AlertState();
            _states[StateType.Dead] = new DeathState();

            ChangeState(StateType.Patrol);
        }

        public void SetWaypoints(params Vector2[] waypoints)
        {
            Waypoints.Clear();
            Waypoints.AddRange(waypoints);
        }

        public void ChangeState(StateType newStateType)
        {
            StateType oldStateType = _currentStateType;
            
            if (_currentState != null)
                _currentState.Exit(this);

            _currentStateType = newStateType;
            _currentState = _states[newStateType];
            _currentState.Enter(this);
            
            // Print state transition to console
            StateDisplay.PrintStateTransition(oldStateType, newStateType, EnemyId);
        }

        public void ForceAlertFrames()
        {
            _currentFrames = _enemyFrames.GetTornadoAFrames();
            _currentFrameIndex = 0;
            _origin = new Vector2(_currentFrames[0].Width / 2f, _currentFrames[0].Height / 2f);
        }

        public void TakeDamage()
        {
            if (IsAlive)
            {
                ChangeState(StateType.Dead);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_currentState != null)
                _currentState.Update(this, gameTime);

            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= _animationSpeed)
            {
                _animationTimer = 0f;
                _currentFrameIndex = (_currentFrameIndex + 1) % _currentFrames.Length;
                _origin = new Vector2(_currentFrames[_currentFrameIndex].Width / 2f, _currentFrames[_currentFrameIndex].Height / 2f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = IsAlive ? Color.White : Color.Gray;
            spriteBatch.Draw(
                _texture,
                Position,
                _currentFrames[_currentFrameIndex],
                drawColor,
                0f,
                _origin,
                _scale,
                SpriteEffects.None,
                0f
            );
        }

        public Rectangle BoundingBox
        {
            get
            {
                int width = (int)(_currentFrames[_currentFrameIndex].Width * _scale);
                int height = (int)(_currentFrames[_currentFrameIndex].Height * _scale);
                return new Rectangle(
                    (int)(Position.X - width / 2f),
                    (int)(Position.Y - height / 2f),
                    width,
                    height
                );
            }
        }

        public StateType GetCurrentState() => _currentStateType;
    }
}