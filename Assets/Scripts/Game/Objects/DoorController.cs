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

//    [SerializeField]
//private SpriteRenderer _doorRenderer;
 //   [SerializeField]
//    private Sprite _openDoorSprite;
//    [SerializeField]
 //   private Sprite _closedDoorSprite;

    // Components
    private Collider2D _collider;
    private bool status = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = false;
    }

    private void onInteract(PlayerController controller)
    {
        if(_collider.isTrigger)
        {
            //       _doorRenderer.sprite = _openDoorSprite;
            _collider.isTrigger = false;
           
        }
        else
        {
            //         _doorRenderer.sprite = _closedDoorSprite;
            _collider.isTrigger = true;
        }
    }
}
