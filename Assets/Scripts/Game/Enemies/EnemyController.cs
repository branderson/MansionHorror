using Assets.Utility;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Game
{
    public class EnemyController : CustomMonoBehaviour
    {
        [SerializeField] protected List<Vector2> _patrolPoints;
        [SerializeField] protected float _sanityDamage = 0.2f;
        [SerializeField] protected float _attackRange = 2;
        [SerializeField] protected float _patrolSpeed = 2;
        protected int _currentPatrolPoint = 0;
        protected bool _patrolling = false;
        protected bool _move = false;
        protected Vector2 _startPosition;
        protected Vector2 _destination;
        protected Vector2 _returnPosition;
        
        public void Awake()
        {
            _startPosition = transform.position;
            _destination = _startPosition;
        }


        /// <summary>
        /// Move towards character until in attack range
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="Speed"></param>
        /// <returns></returns>
        public bool MoveTowardsCharacter(GameObject Player, float Speed)
        {
            Vector3 Direction = Player.transform.position - transform.position;
            Direction.z = 0;
            Direction.Normalize();
            Direction *= _attackRange;
            return MoveTowardsPosition(Player.transform.position - Direction, Speed);
        }

        /// <summary>
        /// Move towards the 2D position at the given speed.
        /// </summary>
        /// <param name="Position">2D position trying to go to</param>
        /// <param name="Speed">Speed to reach target</param>
        /// <returns>Reached target</returns>
        public bool MoveTowardsPosition(Vector2 Position, float Speed)
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
        public void SwitchPatrol()
        {
            _patrolling = !_patrolling;
        }

        public void ReturnToStartPosition()
        {
            _move = true;
            _destination = _startPosition;
        }

        public void MoveToLocation(Vector2 Location)
        {
            _move = true;
            _destination = Location;
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

        protected void Update() {
            if (_patrolling)
            {
                if (_patrolPoints.Capacity != 0)
                {
                    if (MoveTowardsPosition(_patrolPoints[_currentPatrolPoint], _patrolSpeed))
                    {
                        _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Capacity;
                    }
                }
            }
            else if (_move)
            {
                if(MoveTowardsPosition(_destination, _patrolSpeed))
                {
                    _move = false;
                }
            }
        }

        /// <summary>
        /// Check whether the character is within range
        /// </summary>
        /// <param name="Player"></param>
        /// <returns>True if character in range</returns>
        public bool IsCharacterWithinRange(GameObject Player)
        {
            return (Player.transform.position - transform.position).magnitude > _attackRange;
        }

        public void Attack()
        {
            //Attack animation
        }
    }
}