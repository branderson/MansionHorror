using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;

namespace Assets.AI
{
    public class ChairAI : CustomMonoBehaviour
    {
        public enum Attack { Move, Wait};
        public enum Activeness { Low, Average, High };

        private const int MAX_BLOCK = 2;
        private const int MAX_COLLISIONS = 50;

        [SerializeField] private float _speedMax = 4;
        [SerializeField] private float _speedMin = 1;
        [SerializeField] protected float _damage = 0.2f;
        [SerializeField] protected float _attackRange = 1;
        [SerializeField] protected float _switchTimeMin = .3f;
        [SerializeField] protected float _switchTimeMax = .8f;
        [SerializeField] protected float _cooldown = 1f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        [SerializeField] protected Activeness _activeness = Activeness.Average;
        [SerializeField] protected float _awareRange = 7;

        private GameObject _player;
        private PlayerController _playerController;
        private ChairController _controller;
        private Attack _attackStatus;
        private Vector2 _target;
        private int moveThreshold;
        private int _blockRight;
        private int _blockLeft;
        private int _blockUp;
        private int _blockDown;
        private int _collisions;


        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<ChairController>();
            _attackStatus = Attack.Wait;
            _blockRight = _blockLeft = _blockUp = _blockDown = _collisions = 0;
            if (_activeness == Activeness.Low)
                moveThreshold = 6;
            else if (_activeness == Activeness.Average)
                moveThreshold = 4;
            else
                moveThreshold = 2;
        }

        private void Update()
        {
            if (_attackStatus != Attack.Wait)
            {
                if (_controller.MoveTowardsPosition(_target, Random.Range(_speedMin, _speedMax + 1)) || _collisions > MAX_COLLISIONS)
                {
                    if (!IsInvoking("NextMove"))
                    {
                        _attackStatus = Attack.Wait;
                        Invoke("NextMove", Random.Range(_switchTimeMin, _switchTimeMax));
                    }
                }
                if (_controller.IsCharacterWithinAttackRange(_player,_attackRange) && _controller.CanAttack())
                {
                    Vector2 direction = (_player.transform.position - transform.position);
                    direction.Normalize();
                    _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
                }
            }
            else if (!IsInvoking("NextMove"))
            {
                Invoke("NextMove", Random.Range(_switchTimeMin, _switchTimeMax));
            }
        }

        private void NextMove()
        {
            _collisions = 0;
            int nextMove = Random.Range(0,10);

            if (nextMove > moveThreshold)
            {
                _attackStatus = Attack.Move;
                if (_controller.IsCharacterWithinAttackRange(_player, _attackRange, _awareRange) && _blockLeft != MAX_BLOCK && _blockRight != MAX_BLOCK && _blockUp != MAX_BLOCK && _blockDown!= MAX_BLOCK)
                {
                    _target = _player.transform.position;
                }
                else
                {
                    _target = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
                    if (_blockLeft != 0 || _blockRight != 0)
                    {
                        if (_blockRight != 0 && _blockLeft != 0)
                        {
                            _target.x = Mathf.Abs(_target.y * 0.1f);
                            _target.x *= (_blockRight > _blockLeft) ? -1 : 1;
                        }
                        else if ((_blockLeft != 0 && _target.x < 0) || (_blockRight != 0 && _target.x > 0))
                            _target.x *= -1;
                    }
                    if (_blockDown != 0 || _blockUp != 0)
                    {
                        if (_blockUp != 0 && _blockDown != 0)
                        {
                            _target.y = Mathf.Abs(_target.x * 0.1f);
                            _target.y *= (_blockUp > _blockDown) ? -1 : 1;
                        }
                        else if ((_blockDown != 0 && _target.y < 0) || (_blockUp != 0 && _target.y > 0))
                            _target.y *= -1;
                    }
                    _target.Normalize();
                    _target *= _speedMin;
                    _target.x += _controller.transform.position.x;
                    _target.y += _controller.transform.position.y;
                }
                _blockLeft -= (_blockLeft != 0) ? 1 : 0;
                _blockRight -= (_blockRight != 0) ? 1 : 0;
                _blockUp -= (_blockUp != 0) ? 1 : 0;
                _blockDown -= (_blockDown != 0) ? 1 : 0;
            }
            else
                _attackStatus = Attack.Wait;           
        }

        void OnCollisionStay2D(Collision2D collisionInfo)
        {
            _collisions++;
            Vector2 blockingObject, currentPosition;
            blockingObject = collisionInfo.transform.position;
            currentPosition = _controller.transform.position;
            if (currentPosition.x < blockingObject.x)
            {
                if (currentPosition.y < blockingObject.y)
                {
                    if (blockingObject.x - currentPosition.x > blockingObject.y - currentPosition.y)
                    {
                        _blockRight = (_blockRight == 0) ? MAX_BLOCK : _blockRight;
                    }
                    else
                    {
                        _blockUp = (_blockUp == 0) ? MAX_BLOCK : _blockUp;
                    }
                }
                else
                {
                    if (blockingObject.x - currentPosition.x > currentPosition.y - blockingObject.y)
                    {
                        _blockRight = (_blockRight == 0) ? MAX_BLOCK : _blockRight;
                    }
                    else
                    {
                        _blockDown = (_blockDown == 0) ? MAX_BLOCK : _blockDown;
                    }
                }
            }
            else
            {
                if (currentPosition.y < blockingObject.y)
                {
                    if (currentPosition.x - blockingObject.x > blockingObject.y - currentPosition.y)
                    {
                        _blockLeft = (_blockLeft == 0) ? MAX_BLOCK : _blockLeft;
                    }
                    else
                    {
                        _blockUp = (_blockUp == 0) ? MAX_BLOCK : _blockUp;
                    }
                }
                else
                {
                    if (currentPosition.x - blockingObject.x > currentPosition.y - blockingObject.y)
                    {
                        _blockLeft = (_blockLeft == 0) ? MAX_BLOCK : _blockLeft;
                    }
                    else
                    {
                        _blockDown = (_blockDown== 0) ? MAX_BLOCK : _blockDown;
                    }
                }
            }
        }
    }
}