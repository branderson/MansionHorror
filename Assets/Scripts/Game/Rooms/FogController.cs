using System.Collections.Generic;
using Assets.Utility;
using UnityEngine;

namespace Assets.Game.Rooms
{
    [ExecuteInEditMode]
    public class FogController : CustomMonoBehaviour
    {
        [SerializeField] private float _translucentAmount = 0.5f;
        [SerializeField] private string _toggleableTag = "RoomDisable";

        public bool ContainsPlayer = false;
        private List<GameObject> _toggleObjects;
         
        private BoxCollider2D _collider;
        private SpriteRenderer _sprite;

        public void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetSize(float width, float height)
        {
//            _collider.offset = new Vector2(width/2, height/2);
//            _collider.size = new Vector2(width, height);
            transform.localScale = new Vector3(width, height, 1);
            
//            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
        }

        public void EnableObjects()
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
            foreach (GameObject obj in _toggleObjects)
            {
                obj.SetActive(true);
            }
        }

        public void DisableObjects()
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _translucentAmount);
            foreach (GameObject obj in _toggleObjects)
            {
                obj.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                ContainsPlayer = true;
            }
            else if (collider.gameObject.tag == _toggleableTag)
            {
                _toggleObjects.Add(collider.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                ContainsPlayer = false;
            }
            else if (collider.gameObject.tag == _toggleableTag)
            {
                _toggleObjects.Remove(collider.gameObject);
            }
        }
    }
}
