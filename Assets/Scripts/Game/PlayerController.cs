using System;
using Assets.Utility;
using Assets.Utility.Static;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Lenses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Game
{
    public class PlayerController : CustomMonoBehaviour
    {
        [SerializeField] private Sprite FacingSprite;
        [SerializeField] private Sprite AwaySprite;
        [SerializeField] private Transform _lensTransform;
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _maxSanity = 100f;
        [SerializeField] private float _sanityDeteriorateRate = 2f;
        [SerializeField] protected float _knockbackSpeed = 40;

        [SerializeField] private bool _unlockAllLenses = false;

        // Checkpoints
        private Vector3 _currentCheckpoint;

        // Lenses
        private Dictionary<Lens, LensController> _lenses;
        private Lens _activeLens = Lens.NoLens;
        private LensController _activeLensController;

        // Sanity
        private float _currentSanity;

        // Knockback
        private bool _knockback;
        private Vector2 _knockbackDirection;

        // Components
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        //private Collider2D[] _colliders;
        private List<Collider2D> _enemyColliders;

        // UI Management
        private GameObject _sanityBar;

        public Lens ActiveLens
        {
            get { return _activeLens; }
        }

        public void LoadScene(string scene, Vector2 position)
        {
            _currentCheckpoint = position;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        private void Awake()
        {
            _lenses = new Dictionary<Lens, LensController>();
            _currentSanity = _maxSanity;
            AcquireLens(Lens.NoLens);
            _activeLens = Lens.NoLens;
            _activeLensController = _lenses[0];
            _activeLensController.gameObject.SetActive(true);
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _enemyColliders = new List<Collider2D>();
            _currentCheckpoint = transform.position;
            SceneManager.sceneLoaded += SceneLoaded;
            if (_unlockAllLenses)
            {
                AcquireLens(Lens.Lens1);
                AcquireLens(Lens.Lens2);
                AcquireLens(Lens.Lens3);
                AcquireLens(Lens.Lens4);
            }
        }

        private void Start() {
            _sanityBar = GameObject.Find("SanityBar");
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            transform.position = _currentCheckpoint;
        }

        private void Update()
        {
            HandleMovement();
            HandleLensControl();
            if(Input.GetButtonDown("Interact"))
            {
                Interact();
            }
            _sanityBar.SendMessage("UpdateSanityBar", _currentSanity);
            HandleSanity();
        }

        private void HandleMovement()
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            if (ver > 0)
            {
                _renderer.sprite = AwaySprite;
            }
            else if (ver < 0)
            {
                _renderer.sprite = FacingSprite;
            }
            Vector2 move = new Vector2(hor, ver) * _moveSpeed;
            if (_knockback)
            {
                move = _knockbackDirection * _knockbackSpeed;
                _knockback = false;
            }

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
            if (Input.GetButtonDown("Lens4"))
            {
                SetLens(Lens.Lens4);
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
                    _activeLensController.Deactivate();
                }
                if (_activeLensController != controller)
                {
                    _activeLens = lens;
                    _activeLensController = controller;
                    _activeLensController.gameObject.SetActive(true);
                    _activeLensController.Activate();
                    EventManager.Instance.TriggerEvent("Activate " + lens);
                }
                else
                {
                    _activeLens = Lens.NoLens;
                    _activeLensController = _lenses[0];
                    _activeLensController.gameObject.SetActive(true);
                    _activeLensController.Activate();
                    
                }
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
                if (_currentSanity <= 0)
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
        public void Hit(float sanityDamage, Vector2 knockbackDirection)
        {
            _currentSanity -= sanityDamage * _maxSanity;
            _knockback = true;
            _knockbackDirection = knockbackDirection;

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
                collider.gameObject.SendMessage("onInteract",this,SendMessageOptions.DontRequireReceiver);
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