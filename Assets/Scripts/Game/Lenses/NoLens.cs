using UnityEngine;

namespace Assets.Game.Lenses
{
    public class NoLens : LensController
    {
        GameObject _camera;
        BlurShaderTest _shader;
        AudioSource _audio;

        private void Start()
        {
            _camera = GameObject.Find("Camera");

            foreach (BlurShaderTest shader in _camera.GetComponents<BlurShaderTest>())
            {
                if (shader.EffectMaterial.name == "Standard Blur")
                {
                    _shader = shader;
                }
            }
            foreach (AudioSource audio in _camera.GetComponents<AudioSource>())
            {
                if (audio.clip == null)
                {
                    Debug.Log("Audio clip is null");
                    continue;
                }
                if (audio.clip.name == "CrazyGlasses_NormalLoop") ;
                {
                    _audio = audio;
                }
            }
        }

        public override void Activate()
        {
            _shader.enabled = true;
            _audio.enabled = true;
        }
        public override void Deactivate()
        {
            _shader.enabled = false;
            _audio.enabled = false;
        }
    }
}