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

public class SpecialDoorController : CustomMonoBehaviour {

    [SerializeField]
    private Lens _lens;


    // Components
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void onInteraction(PlayerController controller)
    {
        if (controller.ActiveLens == _lens)
        {
            _rigidbody.isKinematic = true;
        }
        else
        {
            _rigidbody.isKinematic = false;
        }   
    }

}
