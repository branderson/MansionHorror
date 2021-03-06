using Assets.Utility;
using UnityEngine;

namespace Assets.Game
{
    public class DualStateController : CustomMonoBehaviour
    {
        [SerializeField] private string ActiveLens;
        [SerializeField] private float _transitionTime = 0f;
        [SerializeField] private Transform _dormant;
        [SerializeField] private Transform _active;
        [SerializeField] private bool _debugOn = false;

        private bool _deactivating = false;
        private float _deactivateTime = 0f;
        public bool ai_enabled = false;
        private bool _isActive = false;

        private void Awake()
        {
            _dormant.gameObject.SetActive(!_debugOn);
            _active.gameObject.SetActive(_debugOn);
        }

        private void Start()
        {
            EventManager.Instance.StartListening("Activate " + ActiveLens, Activate);
            EventManager.Instance.StartListening("Deactivate " + ActiveLens, TriggerDeactivate);
        }

        private void Update()
        {
            if (_deactivating)
            {
                _deactivateTime -= Time.deltaTime;
                if (_deactivateTime <= 0)
                {
                    _deactivating = false;
                    Deactivate();
                }
            }
            if (_isActive)
            {
                transform.position += _active.localPosition;
                _active.localPosition = new Vector3();
            }
            else
            {
                transform.position += _dormant.localPosition;
                _dormant.localPosition = new Vector3();
            }
        }

        private void Activate()
        {
            _dormant.gameObject.SetActive(false);
            _active.gameObject.SetActive(true);
            _isActive = true;
        }

        private void TriggerDeactivate()
        {
            _deactivating = true;
            _deactivateTime = _transitionTime;
        }

        private void Deactivate()
        {
            _dormant.gameObject.SetActive(true);
            _active.gameObject.SetActive(false);
            _isActive = false;
        }

        private void OnDestroy()
        {
            if (!EventManager.Destroyed)
            {
                EventManager.Instance.StopListening("Activate " + ActiveLens, Activate);
                EventManager.Instance.StopListening("Deactivate " + ActiveLens, TriggerDeactivate);
            }
        }
    }
}
