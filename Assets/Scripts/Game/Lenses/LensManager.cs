using Assets.Utility;
using UnityEngine;

namespace Assets.Game.Lenses
{
    public class LensManager : Singleton<LensManager>
    {
        [SerializeField] private GameObject _noLensPrefab;
        [SerializeField] private GameObject _lens1Prefab;
        [SerializeField] private GameObject _lens2Prefab;
        [SerializeField] private GameObject _lens3Prefab;
        [SerializeField] private GameObject _lens4Prefab;

        protected LensManager() { }

        /// <summary>
        /// Instantiate a lens corresponding to the given enum value, returning the instance
        /// </summary>
        /// <param name="lens">
        /// Enum value representing lens type to instantiate
        /// </param>
        /// <returns>
        /// LensController reference to instantiated lens
        /// </returns>
        public LensController InstantiateLens(Lens lens)
        {
            GameObject instance = null;
            switch (lens)
            {
                case Lens.NoLens:
                    instance = Instantiate(_noLensPrefab);
                    break;
                case Lens.Lens1:
                    instance = Instantiate(_lens1Prefab);
                    break;
                case Lens.Lens2:
                    instance = Instantiate(_lens2Prefab);
                    break;
                case Lens.Lens3:
                    instance = Instantiate(_lens3Prefab);
                    break;
                case Lens.Lens4:
                    instance = Instantiate(_lens4Prefab);
                    break;
            }

            // Check if instance is null, getting the component if it's not
            return instance ? instance.GetComponent<LensController>() : null;
        }
    }
}