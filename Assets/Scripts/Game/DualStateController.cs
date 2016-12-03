using Assets.Utility;
using UnityEngine;

namespace Assets.Game
{
    public class DualStateController : CustomMonoBehaviour
    {
        [SerializeField] private string ActiveLens;
        [SerializeField] private Transform _dormant;
        [SerializeField] private Transform _active;
        [SerializeField] private bool _debugOn = false;

        public bool ai_enabled = false;


        private void Awake()
        {
            _dormant.gameObject.SetActive(!_debugOn);
            _active.gameObject.SetActive(_debugOn);
        }

        private void Start()
        {
            EventManager.Instance.StartListening("Activate " + ActiveLens, Activate);
            EventManager.Instance.StartListening("Deactivate " + ActiveLens, Deactivate);
        }

        private void Activate()
        {
            _dormant.gameObject.SetActive(false);
            _active.gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            _dormant.gameObject.SetActive(true);
            _active.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventManager.Instance.StopListening("Activate " + ActiveLens, Activate);
            EventManager.Instance.StopListening("Deactivate " + ActiveLens, Deactivate);
        }
    }
}
