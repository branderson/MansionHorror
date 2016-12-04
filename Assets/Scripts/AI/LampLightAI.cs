using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;
using System.Collections.Generic;

namespace Assets.AI
{
    public class LampLightAI : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _lamp;
        [SerializeField] protected float _speed = 0.5f;
        [SerializeField] protected float _lampRange = 5;
        private PlayerController _playerController;
        private EnemyController _controller;
        private LampAI _lampAI;
        private bool _playerEntered;

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
            _controller.StartPatrol();
            if(_lamp == null)
            {
                _lamp = FindObjectOfType<LampAI>().gameObject;
            }
            _lampAI = _lamp.GetComponentInChildren<LampAI>();
        }

        private void Update()
        {
            if (_playerEntered)
            {
                _controller.MoveTowardsCharacter(_playerController.gameObject, _speed);
                if ((_playerController.transform.position - transform.position).sqrMagnitude > (_lampRange * _lampRange))
                {
                    _playerEntered = false;
                }
            }   
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            GameObject actor = other.gameObject;
            actor = actor.transform.parent.gameObject;
            if(actor.tag == "Player")
            {
                _playerEntered = true;
                if (_playerController == null)
                {
                    _playerController = actor.GetComponent<PlayerController>();
                }
                if(_lampAI == null)
                {
                    _lampAI = _lamp.GetComponent<LampAI>();
                }
                _lampAI.Attack(_playerController);
            }
        }
    }
}