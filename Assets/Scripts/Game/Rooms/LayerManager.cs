using System.Collections.Generic;
using TiledLoader;
using UnityEngine;

namespace Assets.Game.Rooms
{
    public class LayerManager : MonoBehaviour {

        [SerializeField] private string _fogProperty = "Room";
        [SerializeField] private float _translucentAmount = 0.5f;
        [SerializeField] private string _toggleableTag = "RoomDisable";

        struct ColliderInfo // TODO: Make this a component on the fog objects
        {
            public List<GameObject> collisions;
        };

        private Dictionary<string, List<Transform>> _compMap;
        private List<Transform> _curGroup = null;

        private void Start () {
            _compMap = new Dictionary<string, List<Transform>>();
            foreach (Transform child in transform)
            {
                TiledLoaderProperties cProps = child.GetComponent<TiledLoaderProperties>();
                string fieldResult;
                if (cProps.TryGetString(_fogProperty, out fieldResult))
                {
                    if(!_compMap.ContainsKey(fieldResult)) // No room component has been found yet
                    {
                        _compMap.Add(fieldResult, new List<Transform>());
                    }
                    _compMap[fieldResult].Add(child);
                }
            }
        }

        private void NotifyCollision(string GroupName)
        {
            if (!_compMap.ContainsKey(GroupName))
            {
                Debug.LogWarning("Fog manager notified of invalid group collision: " + GroupName);
            }
            List<Transform> group = _compMap[GroupName];
            if (group != _curGroup)
            {
                SetTransparent(group);
                SetTranslucent(_curGroup);
                _curGroup = group;
            }
        }

        void SetTransparent(List<Transform> group)
        {
            foreach (Transform t in group)
            {
                t.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.0f);
                ToggleCollisions(t.gameObject, true);
            }
        }

        void SetTranslucent(List<Transform> group)
        {
            if (group == null)
                return;

            foreach (Transform t in group)
            {
                t.GetComponent<Renderer>().material.color = new Color(1, 1, 1, _translucentAmount);
                ToggleCollisions(t.gameObject, false);
            }
        }

        void ToggleCollisions(GameObject g, bool activate)
        {
            ColliderInfo c = g.GetComponent<ColliderInfo>();
            foreach (GameObject t in c.collisions)
            {
                if (t.tag == _toggleableTag)
                {
                    DualStateController controller = g.GetComponent<DualStateController>();
                    if(controller == null)
                    {
                        Debug.LogWarning("Invalid entity tagged toggleable!");
                        return;
                    }
                    controller.ai_enabled = activate;
                }
            }
        }
    }
}
