using System.Collections.Generic;
using TiledLoader;
using UnityEngine;

namespace Assets.Game.Rooms
{
    public class LayerManager : MonoBehaviour
    {
//        [SerializeField] private GameObject _roomPrefab;
//        [SerializeField] private string _fogProperty = "Room";
////        [SerializeField] private float _translucentAmount = 0.5f;
////        [SerializeField] private string _toggleableTag = "RoomDisable";
//
////        struct ColliderInfo // TODO: Make this a component on the fog objects
////        {
////            public List<GameObject> collisions;
////        };
//
//        private Dictionary<string, RoomController> _compMap;
//        private RoomController _curRoom = null;
//
//        private void Start () {
//            _compMap = new Dictionary<string, RoomController>();
//            foreach (Transform child in transform)
//            {
//                TiledLoaderProperties cProps = child.GetComponent<TiledLoaderProperties>();
//                string fieldResult;
//                if (cProps.TryGetString(_fogProperty, out fieldResult))
//                {
//                    if(!_compMap.ContainsKey(fieldResult)) // No room component has been found yet
//                    {
//                        GameObject roomInstance = Instantiate(_roomPrefab);
//                        roomInstance.transform.parent = transform.parent;
//                        RoomController roomController = roomInstance.AddComponent<RoomController>();
//                        _compMap.Add(fieldResult, roomController);
//                    }
//                    _compMap[fieldResult].AddCollider(child, cProps);
//                }
//                Destroy(child.gameObject);
//            }
//        }
//
//        private void NotifyCollision(string GroupName)
//        {
//            if (!_compMap.ContainsKey(GroupName))
//            {
//                Debug.LogWarning("Fog manager notified of invalid group collision: " + GroupName);
//            }
//            List<Transform> group = _compMap[GroupName];
//            if (group != _curRoom)
//            {
//                SetTransparent(group);
//                SetTranslucent(_curRoom);
//                _curRoom = group;
//            }
//        }
//
//        void SetTransparent(List<Transform> group)
//        {
//            foreach (Transform t in group)
//            {
//                t.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.0f);
//                ToggleCollisions(t.gameObject, true);
//            }
//        }
//
//        void SetTranslucent(List<Transform> group)
//        {
//            if (group == null)
//                return;
//
//            foreach (Transform t in group)
//            {
//                t.GetComponent<Renderer>().material.color = new Color(1, 1, 1, _translucentAmount);
//                ToggleCollisions(t.gameObject, false);
//            }
//        }
//
//        void ToggleCollisions(GameObject g, bool activate)
//        {
//            ColliderInfo c = g.GetComponent<ColliderInfo>();
//            foreach (GameObject t in c.collisions)
//            {
//                if (t.tag == _toggleableTag)
//                {
//                    DualStateController controller = g.GetComponent<DualStateController>();
//                    if(controller == null)
//                    {
//                        Debug.LogWarning("Invalid entity tagged toggleable!");
//                        return;
//                    }
//                    controller.ai_enabled = activate;
//                }
//            }
//        }
    }
}
