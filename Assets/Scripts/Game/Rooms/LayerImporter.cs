using Assets.Utility;
using UnityEngine;

namespace Assets.Game.Rooms
{
    [ExecuteInEditMode]
    public class LayerImporter : CustomMonoBehaviour
    {
        [SerializeField] private string _roomBoundaryLayerName = "RoomBoundaries";

        private void HandleLayerProperties()
        {
            if (name == _roomBoundaryLayerName)
            {
                // Set up rooms
                gameObject.AddComponent<LayerManager>();
            }
        }
    }
}