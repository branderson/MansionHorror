using Assets.Utility;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Game
{
    public class EnemyController : CustomMonoBehaviour
    {
        [SerializeField] protected List<Vector2> _patrolPoints;
        [SerializeField] protected float _patrolSpeed = 1;
        protected int _currentPatrolPoint = 0;
        protected bool _patrolling = false;
        protected bool _move = false;
        protected bool _onCooldown = false;
        protected Vector2 _startPosition;
        protected Vector2 _destination;
        protected Vector2 _returnPosition;
        protected CameraController Camera;
        protected float _moveSpeed = 0f;
        protected bool _return = false;
        protected bool _swayPatrolUp = false;
        protected bool _swayPatrolDown = true;

        public virtual void Awake()
        {
            _startPosition = transform.position;
            _destination = _startPosition;
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
            _onCooldown = false;
        }

        /// <summary>
        /// Move towards Character position without regards to attack range
        /// </summary>
        /// <param name="Player">Player</param>
        /// <param name="Speed">Speed to move</param>
        /// <returns></returns>
        public virtual bool MoveTowardsCharacter(GameObject Player, float Speed)
        {
            return MoveTowardsCharacter(Player, 1, Speed);
        }


        /// <summary>
        /// Move towards the character and return true when within attack range and stop
        /// </summary>
        /// <param name="Player">Player</param>
        /// <param name="AttackRange">Attack range</param>
        /// <param name="Speed">Speed</param>
        /// <returns></returns>
        public virtual bool MoveTowardsCharacter(GameObject Player, float AttackRange, float Speed)
        {
            if(IsCharacterWithinAttackRange(Player, AttackRange))
            {
                return true;
            }
            Vector3 Direction = Player.transform.position - transform.position;
            Direction.z = 0;
            Direction.Normalize();
            Direction *= AttackRange;
            return MoveTowardsPosition(Player.transform.position - Direction, Speed);
        }

        /// <summary>
        /// Move towards the 2D position at the given speed.
        /// </summary>
        /// <param name="Position">2D position trying to go to</param>
        /// <param name="Speed">Speed to reach target</param>
        /// <returns>Reached target</returns>
        public virtual bool MoveTowardsPosition(Vector2 Position, float Speed)
        {
            Vector3 NewPosition = new Vector3(Position.x,Position.y,0f);
            if (transform.position == NewPosition)
            {
                return true;
            }
            bool left, down, newLeft, newDown;
            NewPosition -= transform.position;
            Vector3 Direction = NewPosition.normalized;
            left = Direction.x < 0 ? true : false;
            down = Direction.y < 0 ? true : false;
            Direction *= Speed * Time.deltaTime;
            newLeft = Position.x - (transform.position.x + Direction.x) < 0 ? true : false;
            newDown = Position.y - (transform.position.y + Direction.y) < 0 ? true : false;
            //If we overshoot, go directly to target
            if (((left ^ newLeft) || Direction.x == 0) && ((down ^ newDown) || Direction.y == 0) || (transform.position.x + Direction.x == Position.x && transform.position.y + Direction.y == Position.y) )
            {
                Direction.x = Position.x - transform.position.x;
                Direction.y = Position.y - transform.position.y;
                transform.Translate(Direction);
                return true;
            }
            transform.Translate(Direction);
            return false;
        }

        /// <summary>
        /// Switch between patrolling and not patrolling
        /// </summary>
        public virtual void StopPatrol()
        {
            _patrolling = false;
            _swayPatrolUp = false;
            _swayPatrolDown = false;
        }

        public virtual void StartPatrol()
        {
            _patrolling = true;
        }

        public virtual void StartSwayPatrol()
        {
            _patrolling = true;
            if (_currentPatrolPoint == 0)
            {
                _swayPatrolUp = true;
                _swayPatrolDown = false;
            }
            else
            {
                _swayPatrolDown = true;
                _swayPatrolUp = false;
            }
        }

        public virtual void ReturnToStartPosition()
        {
            ReturnToStartPosition(_patrolSpeed);
        }

        public virtual void ReturnToStartPosition(float Speed)
        {
            _move = true;
            _destination = _startPosition;
            _moveSpeed = Speed;
        }

        public virtual void ReturnToPatrol()
        {
            ReturnToPatrol(_patrolPoints[_currentPatrolPoint], _patrolSpeed);
        }

        public virtual void ReturnToPatrol(Vector2 Location)
        {
            ReturnToPatrol(Location, _patrolSpeed);
        }

        public virtual void ReturnToPatrol(Vector2 Location, float Speed)
        {
            _return = true;
            MoveToLocation(Location, Speed);
        }

        public virtual void MoveToLocation(Vector2 Location)
        {
            MoveToLocation(Location, _patrolSpeed);
        }

        public virtual void MoveToLocation(Vector2 Location, float Speed)
        {
            _move = true;
            _destination = Location;
            _moveSpeed = Speed;
        }

        public Vector2 ReturnLocation
        {
            get
            {
                return _startPosition;
            }
            set
            {
                _startPosition = value;
            }
        }

        protected virtual void Update() {
            // Movement
            if (_patrolling)
            {
                if (_patrolPoints.Count != 0)
                {
                    if (MoveTowardsPosition(_patrolPoints[_currentPatrolPoint], _patrolSpeed))
                    {
                        if (_swayPatrolUp || _swayPatrolDown)
                        {
                            _currentPatrolPoint = _swayPatrolUp ? _currentPatrolPoint + 1 : _currentPatrolPoint - 1;
                            if(_currentPatrolPoint >= _patrolPoints.Count)
                            {
                                _currentPatrolPoint--;
                                _swayPatrolUp = false;
                                _swayPatrolDown = true;
                            }
                            else if (_currentPatrolPoint < 0)
                            {
                                _currentPatrolPoint++;
                                _swayPatrolUp = true;
                                _swayPatrolDown = false;
                            }
                        }
                        else
                        {
                            _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Capacity;
                        }
                    }
                }
            }
            else if (_move)
            {
                if(MoveTowardsPosition(_destination, _moveSpeed))
                {
                    if (_return)
                    {
                        StartPatrol();
                    }
                    _return = false;
                    _move = false;
                }
            }
        }

        /// <summary>
        /// Check whether the character is within range
        /// </summary>
        /// <param name="Player"></param>
        /// <returns>True if character in range</returns>
        public virtual bool IsCharacterWithinRange(GameObject Player)
        {
            return IsCharacterWithinAttackRange(Player, 0f, 0f);
        }

        /// <summary>
        /// Check whether the character is within range plus extra
        /// </summary>
        /// <param name="Position">Extra range to check</param>
        /// <param name="Offset">Extra offset</param>
        /// <returns></returns>
        public virtual bool IsCharacterWithinRange(GameObject Player, float Offset)
        {
            return IsCharacterWithinAttackRange(Player, 0f, Offset);
        }

        /// <summary>
        /// Check whether the character is within attack range
        /// </summary>
        /// <param name="Position">Extra range to check</param>
        /// <param name="AttackRange">Attack Range</param>
        /// <returns></returns>
        public virtual bool IsCharacterWithinAttackRange(GameObject Player, float AttackRange)
        {
            return IsCharacterWithinAttackRange(Player, AttackRange, 0);
        }

        /// <summary>
        /// Check whether the character is within attack range plus extra
        /// </summary>
        /// <param name="Position">Extra range to check</param>
        /// <param name="AttackRange">Attack Range</param>
        /// <param name="Offset">Extra offset</param>
        /// <returns></returns>
        public virtual bool IsCharacterWithinAttackRange(GameObject Player, float AttackRange, float Offset)
        {
            return (Player.transform.position - transform.position).sqrMagnitude <= ((AttackRange - Offset) * (AttackRange - Offset));
        }

        /// <summary>
        /// Take away Damage% sanity from player
        /// </summary>
        /// <param name="Player">Player controller</param>
        /// <param name="Cooldown">Time until next attack</param>
        /// <param name="Damage">Percent sanity to take from player</param>
        public virtual void Attack(PlayerController Player, float Cooldown, float Damage)
        {
            Attack(Player, Cooldown, Damage, new Vector2(0, 0), 0, 0);
        }


        /// <summary>
        /// Take away Damage% sanity from player and causes camera to shake
        /// </summary>
        /// <param name="Player">Player controller</param>
        /// <param name="Cooldown">Time until next attack</param>
        /// <param name="Damage">Percent sanity to take from player</param>
        /// <param name="ShakeDirection"> Direction the camera shakes in</param>
        /// <param name="ShakeDuratioin">How long the camera will shake</param>
        /// <param name="ShakeIntensity">How intense the camera shakes</param>
        public virtual void Attack(PlayerController Player, float Cooldown, float Damage, Vector2 ShakeDirection, float ShakeDuratioin, float ShakeIntensity)
        {
            if (!_onCooldown)
            {
                //Attack animation
                Player.Hit(Damage);
                if(Camera == null)
                {
                    Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
                }
                Camera.Shake(ShakeDuratioin, ShakeDirection, ShakeIntensity);
                _onCooldown = true;
                Invoke("RefreshAttack", Cooldown);
                Debug.Log("Attacked");
            }
        }

        /// <summary>
        /// Attack cooldown refresh
        /// </summary>
        public virtual void RefreshAttack()
        {
            _onCooldown = false;
        }

        public virtual bool CanAttack()
        {
            return !_onCooldown;
        }

        public virtual Vector2 CurrentPatrolTarget()
        {
            if(_patrolPoints.Count == 0)
            {
                return new Vector2(0, 0);
            }
            return _patrolPoints[_currentPatrolPoint];
        }
    }
}