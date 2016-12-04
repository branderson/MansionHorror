using UnityEngine;

namespace Assets.Game.Lenses
{
    public class Lens3 : LensController
    {
        GameObject _camera;
        ShaderTest _shader;
        AudioSource _audio;

        private void Start()
        {
            _camera = GameObject.Find("Camera");

            foreach (ShaderTest shader in _camera.GetComponents<ShaderTest>())
            {
                if (shader.EffectMaterial.name == "Displace")
                {
                    _shader = shader;
                }
            }
            foreach (AudioSource audio in _camera.GetComponents<AudioSource>())
            {
                if (audio.clip.name == "CrazyGlasses_GlassesKaleidoscopeLoop") 
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