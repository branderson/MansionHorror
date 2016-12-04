using UnityEngine;

namespace Assets.Game.Lenses
{
    public class Lens4 : LensController
    {
        GameObject _camera;
        ShaderTest _shader;
        AudioSource _audio;

        private void Start()
        {
            _camera = GameObject.Find("Camera");

            foreach (ShaderTest shader in _camera.GetComponents<ShaderTest>())
            {
                if (shader.EffectMaterial.name == "Foggy")
                {
                    _shader = shader;
                }
            }
            foreach (AudioSource audio in _camera.GetComponents<AudioSource>())
            {
                Debug.Log("outside of loop: "+ audio.clip.name);
                if (audio.clip.name == "CrazyGlasses_GlassesSilentHillLoop") 
                {
                    Debug.Log("inside of loop: "+ audio.clip.name);
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