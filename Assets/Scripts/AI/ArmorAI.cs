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
        [SerializeField] protected float _speed = 0.4f;
        [SerializeField] protected float _activeTime = 10f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        private GameObject _player;
        private PlayerController _playerController;
        private EnemyController _controller;
        private bool _active = false;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (_active && _controller.MoveTowardsCharacter(_player, _attackRange, _speed))
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
            }
        }

        public void ActivateSuit()
        {
            _active = true;
            Invoke("ShutdownSuit", _activeTime);
        }

        public void ShutdownSuit()
        {
            _active = false;
            _controller.ReturnToStartPosition();
        }
    }
}