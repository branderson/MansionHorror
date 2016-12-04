using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour {
    private GameObject _bar;
    private RectTransform _rect;

    private void Awake() {

    }

    // Changes the size of the sanity bar to match the player's current sanity
    public void UpdateSanityBar(float _sanity) {
        Vector3 _newScale = new Vector3(_sanity / 100f, 1, 1);
        _rect.localScale = _newScale;
    }

    // Use this for initialization
    void Start () {
        _bar = GameObject.Find("Image");
        _rect = _bar.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
