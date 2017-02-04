using System.Collections.Generic;
using Assets.Utility;
using TiledLoader;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Game.Rooms
{
    [ExecuteInEditMode]
    public class LayerImporter : CustomMonoBehaviour
    {
        [SerializeField] private string _roomBoundaryLayerName = "RoomBoundaries";
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private string _fogProperty = "Room";

        private Dictionary<string, RoomController> _compMap;

        private void HandleLayerProperties()
        {
            if (name == _roomBoundaryLayerName)
            {
                // Set up rooms
                _compMap = new Dictionary<string, RoomController>();
                foreach (Transform child in transform)
                {
                    TiledLoaderProperties cProps = child.GetComponent<TiledLoaderProperties>();
                    if (cProps == null)
                    {
                        DestroyImmediate(child.gameObject);
                        continue;
                    }
                    string fieldResult;
                    if (cProps.TryGetString(_fogProperty, out fieldResult))
                    {
                        if (!_compMap.ContainsKey(fieldResult)) // No room component has been found yet
                        {
#if UNITY_EDITOR
                            GameObject roomInstance = PrefabUtility.InstantiatePrefab(_roomPrefab) as GameObject;
#else
                            GameObject roomInstance = Instantiate(_roomPrefab);
#endif
                            roomInstance.transform.parent = transform;
                            RoomController roomController = roomInstance.GetComponent<RoomController>();
                            _compMap[fieldResult] = roomController;
                        }
                        _compMap[fieldResult].AddFog(child, cProps);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }
    }
}