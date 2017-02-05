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
    public class LensController : CustomMonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private string _shaderName;
        BlurShaderTest _shader;
        AudioSource _audio;

        private void Start()
        {
            GameObject camera = GameObject.Find("Camera");

            foreach (BlurShaderTest shader in camera.GetComponents<BlurShaderTest>())
            {
                if (shader.EffectMaterial.name == _shaderName)
                {
                    _shader = shader;
                }
            }
            _audio = camera.GetComponent<AudioSource>();
        }

        public void Activate()
        {
            _shader.enabled = true;
            _audio.clip = _clip;
        }

        public void Deactivate()
        {
            _shader.enabled = false;
            if (_audio.clip == _clip)
            {
                _audio.clip = null;
            }
        }
    }
}