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
    public class RoomController : CustomMonoBehaviour
    {
        private bool _containsPlayer = false;

        private List<FogController> fogs = new List<FogController>();

        public void AddFog(Transform fog, TiledLoaderProperties props)
        {
            float width;
            float height;
            props.TryGetFloat("ScaleX", out width);
            props.TryGetFloat("ScaleY", out height);

            fog.transform.parent = transform;
            FogController fogController = fog.GetComponent<FogController>();
            fogController.SetSize(width, height);
            fogs.Add(fogController);
        }

        private void Update()
        {
            bool nowContainsPlayer = false;
            // Does room contain player
            foreach (FogController fog in fogs)
            {
                if (fog.ContainsPlayer)
                {
                    nowContainsPlayer = true;
                }
            }
            if (nowContainsPlayer && !_containsPlayer)
            {
                // Player entered
                _containsPlayer = true;
                foreach (FogController fog in fogs)
                {
                    fog.DisableObjects();
                }
            }
            else if (!nowContainsPlayer && _containsPlayer)
            {
                // Player left
                _containsPlayer = false;
                foreach (FogController fog in fogs)
                {
                    fog.EnableObjects();
                }
            }
        }
    }
}