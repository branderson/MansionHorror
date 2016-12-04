using UnityEngine;

namespace Assets.Game.Lenses
{
    public class Lens3 : LensController
    {
        GameObject _camera;
        ShaderTest _shader;

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
        }

        public override void Activate()
        {
            _shader.enabled = true;
        }
        public override void Deactivate()
        {
            _shader.enabled = false;
        }
    }
}