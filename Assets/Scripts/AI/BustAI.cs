using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;
using System.Collections.Generic;

namespace Assets.AI
{
    public class BustAI : CustomMonoBehaviour
    {
        [SerializeField]
        protected List<ArmorAI> _armors = new List<ArmorAI>();
        [SerializeField] protected float _damage = 0.2f;
        [SerializeField] protected float _sirenRange = 3;
        [SerializeField] protected float _cooldown = 5f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        private GameObject _player;
        private PlayerController _playerController;
        private BustController _controller;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<BustController>();
        }

        private void Update()
        {
            if (_controller.IsCharacterWithinAttackRange(_player, _sirenRange))
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity, _armors);
            }
        }
    }
}