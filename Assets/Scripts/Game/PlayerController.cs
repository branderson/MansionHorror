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
        [SerializeField] private float _maxSanity = 100f;
        [SerializeField] private float _sanityDeteriorateRate = 2f;

        // Checkpoints
        private Vector2 _currentCheckpoint;

        // Lenses
        private Dictionary<Lens, LensController> _lenses;
        private Lens _activeLens = Lens.NoLens;
        private LensController _activeLensController;

        // Sanity
        private float _currentSanity;

        // Components
         private Rigidbody2D _rigidbody;
        //private Collider2D[] _colliders;
        private List<Collider2D> _enemyColliders;

        public Lens ActiveLens
        {
            get { return _activeLens; }
        }

        private void Awake()
        {
            _lenses = new Dictionary<Lens, LensController>();
            _currentSanity = _maxSanity;
            _currentCheckpoint = new Vector2(0f, 0f);
            AcquireLens(Lens.NoLens);
            _rigidbody = GetComponent<Rigidbody2D>();
            _enemyColliders = new List<Collider2D>();
        }

        private void Update()
        {
            HandleMovement();
            HandleLensControl();
            HandleSanity();
        }

        private void HandleMovement()
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector2 move = new Vector2(hor, ver) * _moveSpeed;

            _rigidbody.velocity = move;
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
            if (Input.GetButtonDown("NoLens"))
            {
                SetLens(Lens.NoLens);
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
        /// Handle player sanity
        /// </summary>
        private void HandleSanity()
        {
            if (_currentSanity <= 0)
            {
                TurnInsane();
            }
            else if (_activeLens == Lens.NoLens)
            {
                _currentSanity -= (_sanityDeteriorateRate * (_lenses.Count - 1)) * Time.deltaTime;
            }
        }

        /// <summary>
        /// Handle when the player turns insane
        /// </summary>
        private void TurnInsane()
        {
            //Play animation for the player going insane

            //TO-DO

            //Reset the player
            ResetPlayer();
        }


        /// <summary>
        /// Reset the player after being killed
        /// </summary>
        private void ResetPlayer()
        {
            _currentSanity = _maxSanity;
            transform.SetPosition2D(_currentCheckpoint);
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
        public void AcquireLens(Lens lens)
        {
            LensController controller = LensManager.Instance.InstantiateLens(lens);
            if (controller == null) return;

            // Add the lens to the player
            _lenses[lens] = controller;
            controller.transform.SetParent(_lensTransform, false);
        }


        public float Sanity
        {
            get
            {
                return _currentSanity;
            }
            //Set percent of max. 1 = 100%, 0.5 = 50%
            set
            {
                _currentSanity = Mathf.Max(_maxSanity, value * _maxSanity);
                if(_currentSanity <= 0)
                {
                    TurnInsane();
                }
            }
        }

        /// <summary>
        /// Increasethe player sanity by percent. 1 = 100%, 0.5 = 50%
        /// </summary>
        /// <param name="newSanity">
        /// Percent sanity to increase in decimal form
        /// </param>
        public void IncreaseSanityByPercent(float newSanity)
        {
            _currentSanity += Mathf.Max(_maxSanity, newSanity * _maxSanity);
        }

        /// <summary>
        /// Restore the player sanity to full
        /// </summary>
        public void RestoreSanityToFull()
        {
            _currentSanity = _maxSanity;
        }

        /// <summary>
        /// Hit the player and drain a percent of sanity
        /// </summary>
        /// <param name="sanityDamage">
        /// Percent of sanity to drain. 0.5 = 50%
        /// </param>
        public void Hit(float sanityDamage)
        {
            _currentSanity -= sanityDamage * _maxSanity;
            if (_currentSanity <= 0)
            {
                TurnInsane();
            }
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