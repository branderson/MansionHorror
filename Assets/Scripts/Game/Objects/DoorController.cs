using System;
using Assets.Utility;
using Assets.Utility.Static;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Lenses;
using UnityEngine.Events;
using UnityEngine;
using Assets.Game;
using UnityEngine.UI;



public class DoorController : CustomMonoBehaviour {

    [SerializeField]
    private SpriteRenderer _doorRenderer;
    [SerializeField]
    private Sprite _openDoorSprite;
    [SerializeField]
    private Sprite _closedDoorSprite;

    // Components
    private Rigidbody2D _rigidbody;
    private bool status = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_colliders = GetComponentsInChildren<Collider2D>();
    }

    private void doorInteraction(PlayerController controller)
    {
        if(status)
        {
            _doorRenderer.sprite = _openDoorSprite;
            _rigidbody.isKinematic = true;
        }
        else
        {
            _doorRenderer.sprite = _closedDoorSprite;
            _rigidbody.isKinematic = false;
        }
    }
}
