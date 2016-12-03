using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentLens : MonoBehaviour {
    [SerializeField]
    Sprite[] _lens_images;
    private Image _lens_image;
    private int _current_lens;


	// Use this for initialization
	void Start () {
        _lens_image = GetComponent<Image>();
	}

    public void DisplayLens(int idx) {
        _current_lens = idx;
        _lens_image.sprite = _lens_images[idx];
    }

    public void CycleLensLeft() {
        _current_lens--;
        if (_current_lens < 0) {
            DisplayLens(_lens_images.Length - 1);
        } else {
            DisplayLens(_current_lens);
        }
    }

    public void CycleLensRight() {
        _current_lens++;
        if (_current_lens >= _lens_images.Length) {
            DisplayLens(0);
        } else {
            DisplayLens(_current_lens);
        }
    }
}
