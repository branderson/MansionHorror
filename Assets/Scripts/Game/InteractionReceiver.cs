using System;
using Assets.Utility;
using Assets.Utility.Static;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Lenses;
using UnityEngine.Events;
using UnityEngine;
using Assets.Game;


public class InteractionReceiver : CustomMonoBehaviour {

    [System.Serializable]
    public class PlayerEvent : UnityEvent<PlayerController> { }


    [SerializeField]
    private PlayerEvent _event;

    private void OnInteract(PlayerController controller)
    {
        _event.Invoke(controller);
    }
}
