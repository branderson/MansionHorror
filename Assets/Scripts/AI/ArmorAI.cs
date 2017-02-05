using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;

namespace Assets.AI
{
    public class ArmorAI : CustomMonoBehaviour
    {
        [SerializeField] protected float _damage = 0.2f;
        [SerializeField] protected float _attackRange = 1;
        [SerializeField] protected float _cooldown = 1f;
        [SerializeField] protected float _speed = 1f;
        [SerializeField] protected float _chargeSpeed = 4f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        [SerializeField] protected float _sightRange = 4f;
        [SerializeField] protected float _searchTimeMax = 4f;
        [SerializeField] private int _maxCollisions = 90;
        private GameObject _player;
        private PlayerController _playerController;
        private EnemyController _controller;
        private Vector3 _chargePosition;
        private Vector3 _testPosition;
        private float _searchTime = 0;
        private bool _alert = false;
        private bool _chargeReached = false;
        private int _collisions = 0;
        private BoxCollider2D _collider;
        private bool _swinging = false;
        private const float SWING_ANIM_TIME = 0.75f;
        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<EnemyController>();
            _collider = GetComponent<BoxCollider2D>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if(_collisions > _maxCollisions)
            {
                _collider.isTrigger = true;
                _collisions = 0;
            }
            if(_controller.IsCharacterWithinAttackRange(_player, _sightRange) && !_alert)
            {
                _controller.StopPatrol();
                _alert = true;
                _searchTime = _searchTimeMax;
                _chargePosition = _player.transform.position;
                _chargePosition.z = transform.position.z;
                RaycastHit2D hit = Physics2D.Raycast(_chargePosition, (_chargePosition - transform.position).normalized);
                if (hit.collider != null)
                {
                    _chargePosition = hit.collider.transform.position;
                }
            }
            else if (_alert)
            {
                if (_swinging)
                    return;
                if (!_chargeReached)
                {
                    if(_controller.MoveTowardsPosition(_chargePosition, _chargeSpeed))
                    {
                        _chargeReached = true;
                        _testPosition.x = _chargePosition.x + Random.Range(-2f, 2f);
                        _testPosition.y = _chargePosition.y + Random.Range(-2f, 2f);
                    }
                    Vector2 enemyDirection = (_rigidBody.velocity).normalized;
                    Vector2 characterDirection = (_player.transform.position - transform.position).normalized;
                    if (_controller.IsCharacterWithinAttackRange(_player, _attackRange) && (Vector3.Angle(enemyDirection,characterDirection) < 30 || Vector3.Angle(enemyDirection, characterDirection) > -30))
                    {
                        Vector2 direction = (_player.transform.position - transform.position);
                        direction.Normalize();
                        _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
                        _chargeReached = true;
                        _testPosition.x = _chargePosition.x + Random.Range(-2f, 2f);
                        _testPosition.y = _chargePosition.y + Random.Range(-2f, 2f);
                    }
                }
                else if(_controller.IsCharacterWithinAttackRange(_player, _sightRange))
                {
                    _searchTime = _searchTimeMax;
                    _controller.MoveTowardsCharacter(_player, _speed);
                    _testPosition = _player.transform.position;
                    _testPosition.z = 0;
                    if (_controller.IsCharacterWithinAttackRange(_player, _attackRange, 0.3f * _attackRange))
                    {
                        if (!IsInvoking("Swing"))
                        {
                            Invoke("Swing", SWING_ANIM_TIME);
                        }
                        _swinging = true;
                        Debug.Log("Attacked");             
                    }
                }
                else
                {
                    if(_controller.MoveTowardsPosition(_testPosition, _speed))
                    {
                        _testPosition.x = _chargePosition.x + Random.Range(-2f, 2f);
                        _testPosition.y = _chargePosition.y + Random.Range(-2f, 2f);
                    }
                    _searchTime -= Time.deltaTime;
                    if (_searchTime <= 0)
                    {
                        _searchTime = 0;
                        _alert = false;
                        _chargeReached = false;
                    }
                }
            }
        }

        private void Swing()
        {
            if (_controller.IsCharacterWithinAttackRange(_player, _attackRange))
            {
                Vector2 direction = (_player.transform.position - transform.position);
                direction.Normalize();
                _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
                Debug.Log("Swung");
            }
            if (!IsInvoking("DoneSwing"))
            {
                Invoke("DoneSwing", 0.5f);
            }
        }
        
        private void DoneSwing()
        {
            _swinging = false;
        }

        public void Alert(Vector3 AlertPosition)
        {
        }

        void OnCollisionStay2D(Collision2D collisionInfo)
        {
            _collisions++;
        }

        void OnCollisionEnter2D(Collision2D collisionInfo)
        {
            if (!collisionInfo.gameObject.CompareTag("Player") && !_chargeReached)
            {
                _chargeReached = true;
                _testPosition.x = _chargePosition.x + Random.Range(-2f, 2f);
                _testPosition.y = _chargePosition.y + Random.Range(-2f, 2f);
            }
        }
    }
}