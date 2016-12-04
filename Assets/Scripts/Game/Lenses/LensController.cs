using System;
using Assets.Utility;
using UnityEngine;

namespace Assets.Game.Lenses
{
    public enum Lens
    {
        NoLens,
        Lens1,
        Lens2, 
        Lens3,
        Lens4,
    }

    /// <summary>
    /// Abstract class providing a common interface and functionality across all lenses,
    /// so that they can be managed by the player
    /// </summary>
    public abstract class LensController : CustomMonoBehaviour
    {
        public abstract void Activate();

        public abstract void Deactivate();
    }
}