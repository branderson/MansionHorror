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
        [SerializeField] protected float _cooldown = 2f;
        [SerializeField] protected float _shakeDuration = 0.25f;
        [SerializeField] protected float _shakeIntensity = 1.5f;
        [SerializeField] protected float _radius = 4f;
        public float offsetFactor;
        public GameObject proj;
        private GameObject _player;
        private PlayerController _playerController;
        private EnemyController _controller;
        private Vector2 _center;
        private bool canFire;
        private float lastFire;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<EnemyController>();
            lastFire = 0;
        }

        private void Update()
        {
            if(Time.time-lastFire>_cooldown)
            {
                canFire = true;
            }
        }

        public void Attack()
        {
            if (_player == null)
            {
                _player = GameObject.FindWithTag("Player");
            }
            if(_playerController == null)
            {
                _playerController = _player.GetComponent<PlayerController>();
            }
            if(_controller == null)
            {
                _controller = GetComponent<EnemyController>();
            }
            if (canFire) //temp code until a timing component is added
            {
                LampProjectileAI projAI = Instantiate(proj, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 0)).GetComponent<LampProjectileAI>();
                projAI.dir = (_player.transform.position - transform.position).normalized;
                canFire = false;
                lastFire = Time.time;
            }
            
            
            //Vector2 direction = (_player.transform.position - transform.position).normalized;
            // _controller.Attack(_playerController, _cooldown, _damage, direction, _shakeDuration, _shakeIntensity);
        }
    }
}