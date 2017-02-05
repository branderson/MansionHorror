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
        public GameObject gargoyal;
        public float spawnRange;
        public float spawnCooldown;
        private float lastSpawn;
        private bool canSpawn;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _controller = GetComponent<BustController>();
            canSpawn = true;
        }

        private void Update()
        {
            if ((_controller.IsCharacterWithinAttackRange(_player, _sirenRange)) && canSpawn)
            {

                Vector2 spawn2D = Random.insideUnitCircle.normalized;
                Vector3 spawn3D = new Vector3(spawn2D.x, spawn2D.y, 0);
                spawn3D = spawn3D * spawnRange;
                Vector3 spawnPoint = _player.transform.position + spawn3D;
                Instantiate(gargoyal, spawnPoint, new Quaternion(0, 0, 0, 0));
                lastSpawn = Time.time;
                canSpawn = false;
            }
            if(Time.time-lastSpawn>spawnCooldown)
            {
                canSpawn = true;
            }
        }
    }
}