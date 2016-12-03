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


    // Components
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = false;
    }

    private void onInteract(PlayerController controller)
    {
        if(_collider.isTrigger)
        {
            _collider.isTrigger = false;
        }
        else
        {
            _collider.isTrigger = true;
        }
    }
}
