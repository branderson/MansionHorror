using Assets.Utility;
using UnityEngine;

namespace Assets.Game
{
    public class CameraController : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _follow;
        [SerializeField] private float _bufferDistanceSquared = 3f;
        [SerializeField] private float _speed = 1f;

        private Vector3 _velocity;
        private Vector3 _goalPos;

        private bool _shaking = false;
        private float _shakeDuration = 0f;
        private Vector2 _shakeDirection;
        private float _shakeIntensity = 1f;

        private void Awake()
        {
            _goalPos = transform.position;
            _shakeDirection = new Vector2(0, 0);
        }

        private void Update()
        {
            if (_shaking)
            {
                Vector3 move = (Mathf.Sin(_shakeDuration * 2 * 15) * _shakeDirection * _shakeIntensity) / 10f;
                transform.Translate(move);

                _shakeDuration -= Time.deltaTime;
                if (_shakeDuration <= 0)
                {
                    _shakeDuration = 0;
                    _shaking = false;
                }
            }
            float moveDistance = SquaredDistance2(_follow.transform.position) - _bufferDistanceSquared;
            if (moveDistance > 0)
            {
                Vector2 moveDirection = Direction2(_follow.transform.position);
                _goalPos = new Vector3(transform.position.x + moveDistance * moveDirection.x, 
                    transform.position.y + moveDistance * moveDirection.y, 
                    transform.position.z);
            }
            transform.position = Vector3.SmoothDamp(transform.position, _goalPos, ref _velocity, _speed);
        }

        public void Shake(float Duration, Vector2 Direction, float Intensity)
        {
            _shaking = true;
            _shakeDuration = Duration;
            _shakeDirection = Direction;
            _shakeIntensity = Intensity;
        }
    }
}