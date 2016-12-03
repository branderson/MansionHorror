using Assets.Utility;
using UnityEngine;
using Assets.Game.Enemies;
using Assets.Game;

namespace Assets.AI
{
    public class ChairAI : CustomMonoBehaviour
    {
        public enum Attack { Left, Right, Wait};
        public enum Activeness { Low, Average, High };

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
        private GameObject _player;
        private PlayerController _playerController;
        private ChairController _controller;
        private Attack _attackStatus;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<ChairController>();
        }

        private void Update()
        {
            if (_controller.MoveTowardsCharacter(_player, _attackRange, Random.Range(_speedMin, _speedMax + 1), _attackStatus))
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
            }
            if (!IsInvoking("NextMove"))
            {
                Invoke("NextMove", Random.Range(_switchTimeMin, _switchTimeMax));
            }
        }

        private void NextMove()
        {
            int nextMove = Random.Range(0,10);
            if (_activeness == Activeness.Low)
            {
                switch (nextMove)
                {
                    case 0:
                        _attackStatus = Attack.Left;
                        break;
                    case 1:
                        _attackStatus = Attack.Right;
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        _attackStatus = Attack.Wait;
                        break;
                }
            }
            else if (_activeness == Activeness.Average)
            {
                switch (nextMove)
                {
                    case 0:
                    case 1:
                    case 2:
                        _attackStatus = Attack.Left;
                        break;
                    case 3:
                    case 4:
                    case 5:
                        _attackStatus = Attack.Right;
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        _attackStatus = Attack.Wait;
                        break;
                }
            }
            else if (_activeness == Activeness.High)
            {
                switch (nextMove)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        _attackStatus = Attack.Left;
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        _attackStatus = Attack.Right;
                        break;
                    case 9:
                        _attackStatus = Attack.Wait;
                        break;
                }
            }
        }
    }
}