using System;
using Assets.Utility;
using Assets.Utility.Static;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Lenses;
using UnityEngine;

namespace Assets.Game
{
    public class PlayerController : CustomMonoBehaviour
    {
        [SerializeField] private Transform _lensTransform;
        [SerializeField] private float _moveSpeed = 1f;

        // Lenses
        private Dictionary<Lens, LensController> _lenses;
        private Lens _activeLens;
        private LensController _activeLensController;

        // Components
        private Rigidbody2D _rigidbody;
        //private Collider2D[] _colliders;
        private List<Collider2D> _enemyColliders;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            //_colliders = GetComponentsInChildren<Collider2D>();
        }

        private void Update()
        {
            HandleMovement();
            HandleLensControl();
        }

        private void HandleMovement()
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector2 move = new Vector2(hor, ver) * _moveSpeed * Time.deltaTime;

            transform.Translate(move);
        }

        private void HandleLensControl()
        {
            if (Input.GetButtonDown("CycleLensRight"))
            {
                CycleLens(1);
            }
            else if (Input.GetButtonDown("CycleLensLeft"))
            {
                CycleLens(-1);
            }
            if (Input.GetButtonDown("Lens1"))
            {
                SetLens(Lens.Lens1);
            }
            if (Input.GetButtonDown("Lens2"))
            {
                SetLens(Lens.Lens2);
            }
            if (Input.GetButtonDown("Lens3"))
            {
                SetLens(Lens.Lens3);
            }
        }

        /// <summary>
        /// Set the active lens to the given lens if the player has it
        /// </summary>
        /// <param name="lens">
        /// Lens to set the active lens to
        /// </param>
        private void SetLens(Lens lens)
        {
            LensController controller;
            if (_lenses.TryGetValue(lens, out controller))
            {
                if (_activeLensController)
                {
                    _activeLensController.gameObject.SetActive(false);
                    EventManager.Instance.TriggerEvent("Deactivate " + _activeLens);
                }
                _activeLens = lens;
                _activeLensController = controller;
                _activeLensController.gameObject.SetActive(true);
                EventManager.Instance.TriggerEvent("Activate " + lens);
            }
        }

        /// <summary>
        /// Cycle the player's lens by the given amount
        /// </summary>
        /// <param name="amount">
        /// Amount forward or backward to cycle the player's lens
        /// </param>
        private void CycleLens(int amount)
        {
            int index = MathExtensions.Mod((int)_activeLens + amount, _lenses.Count);
            SetLens(_lenses.Keys.ToList()[index]);
        }

        /// <summary>
        /// Instantiate the given lens and add it to the player
        /// </summary>
        /// <param name="lens">
        /// Type of lens to instantiate
        /// </param>
        public void AquireLens(Lens lens)
        {
            LensController controller = LensManager.Instance.InstantiateLens(lens);
            if (controller == null) return;

            // Add the lens to the player
            _lenses[lens] = controller;
            controller.transform.SetParent(_lensTransform, false);
        }

        /// <summary>
        /// Interact
        /// </summary>
        /// <param name="lens">
        /// Type of lens to instantiate
        /// </param>
        public void Interact()
        {
            foreach( Collider2D collider in _enemyColliders)
            {
                collider.gameObject.SendMessage("OnInteract",this);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _enemyColliders.Add(collision.collider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _enemyColliders.Remove(collision.collider);
        }
    }
}