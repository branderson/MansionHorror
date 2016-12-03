using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiledLoader;
using Assets.Game;

public class LayerManager : MonoBehaviour {

    [SerializeField] private string FogFieldName = "FogGroup";
    [SerializeField] private float TranslucentAmount = 0.5f;
    [SerializeField] private string ToggleableTag = "AI_Toggleable";

    struct ColliderInfo // TODO: Make this a component on the fog objects
    {
        public List<GameObject> collisions;
    };

    private Dictionary<string, List<Transform>> compMap;
    private List<Transform> curGroup = null;
    // Use this for initialization
    void Start () {
        compMap = new Dictionary<string, List<Transform>>();
        foreach (Transform child in transform)
        {
            TiledLoaderProperties cProps = child.GetComponent<TiledLoaderProperties>();
            string fieldResult;
            bool fieldExists = cProps.TryGetString(FogFieldName, out fieldResult);
            if(fieldExists)
            {
                if(!compMap.ContainsKey(fieldResult)) // No room component has been found yet
                {
                    compMap.Add(fieldResult, new List<Transform>());
                }
                compMap[fieldResult].Add(child);
            }
        }
    }

    void NotifyCollision(string GroupName)
    {
        if (!compMap.ContainsKey(GroupName))
        {
            Debug.LogWarning("Fog manager notified of invalid group collision: " + GroupName);
        }
        List<Transform> group = compMap[GroupName];
        if (group != curGroup)
        {
            SetTransparent(group);
            SetTranslucent(curGroup);
            curGroup = group;
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
            t.GetComponent<Renderer>().material.color = new Color(1, 1, 1, TranslucentAmount);
            ToggleCollisions(t.gameObject, false);
        }
    }

    void ToggleCollisions(GameObject g, bool activate)
    {
        ColliderInfo c = g.GetComponent<ColliderInfo>();
        foreach (GameObject t in c.collisions)
        {
            if (t.tag == ToggleableTag)
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

    // Update is called once per frame
    void Update () {
		
	}
}
