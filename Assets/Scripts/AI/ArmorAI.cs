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
        [SerializeField] protected float _speed = 3f;
        [SerializeField] protected float _activeTime = 10f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        [SerializeField] protected float _sightRange = 4f;
        [SerializeField] protected float _searchTimeMax = 4f;
        [SerializeField] private int _maxCollisions = 90;
        private GameObject _player;
        private PlayerController _playerController;
        private EnemyController _controller;
        private bool _searching = false;
        private Vector3 _alertPosition;
        private Vector3 _testPosition;
        private float _searchTime = 0;
        private bool _ariveAtAlert = false;
        private int _collisions = 0;
        private BoxCollider2D _collider;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<EnemyController>();
            _collider = GetComponent<BoxCollider2D>();
            _controller.StartPatrol();
        }

        private void Update()
        {
            if(_collisions > _maxCollisions)
            {
                _collider.isTrigger = true;
                _collisions = 0;
            }
            if(_controller.IsCharacterWithinAttackRange(_player, _sightRange))
            {
                _controller.StopPatrol();
                _searching = true;
                _ariveAtAlert = false;
                _searchTime = _searchTimeMax;
                Vector3 playerPosition = _player.transform.position;
                playerPosition.z = 0;
                _alertPosition = playerPosition;
                if(_controller.MoveTowardsCharacter(_player, _attackRange, _speed))
                {
                    Vector2 direction = (playerPosition - transform.position).normalized;
                    _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
                }
            }
            else if (_searching)
            {
                if (!_ariveAtAlert)
                {
                    if(_controller.MoveTowardsPosition(_alertPosition, _speed))
                    {
                        _ariveAtAlert = true;
                        _testPosition.x = _alertPosition.x + Random.Range(-2f, 2f);
                        _testPosition.y = _alertPosition.y + Random.Range(-2f, 2f);
                    }
                }
                else
                {
                    if(_controller.MoveTowardsPosition(_testPosition, _speed))
                    {
                        _testPosition.x = _alertPosition.x + Random.Range(-2f, 2f);
                        _testPosition.y = _alertPosition.y + Random.Range(-2f, 2f);
                    }
                    _searchTime -= Time.deltaTime;
                    if (_searchTime <= 0)
                    {
                        _searchTime = 0;
                        _searching = false;
                        _controller.ReturnToPatrol(_controller.CurrentPatrolTarget());
                    }
                }
            }
        }

        public void Alert(Vector3 AlertPosition)
        {
            _alertPosition = AlertPosition;
            _searching = true;
            _ariveAtAlert = false;
            _searchTime = _searchTimeMax;
        }

        void OnCollisionStay2D(Collision2D collisionInfo)
        {
            _collisions++;
        }
    }
}