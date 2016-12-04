using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;

namespace Assets.AI
{
    public class LampAI : CustomMonoBehaviour
    {
        [SerializeField] protected float _damage = 0.2f;
        [SerializeField] protected float _attackRange = 1;
        [SerializeField] protected float _cooldown = 1f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        private GameObject _player;
        private PlayerController _playerController;
        private EnemyController _controller;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (_controller.IsCharacterWithinAttackRange(_player, _attackRange))
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
            }
        }
    }
}