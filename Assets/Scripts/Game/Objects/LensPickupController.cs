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


public class LensPickupController : CustomMonoBehaviour
{
    [SerializeField]
    private Lens _lens;

    // Components
    private Rigidbody2D _rigidbody;

    private void onInteract(PlayerController controller)
    {
        controller.AcquireLens(_lens);
        Destroy(this.gameObject);
    }
}
