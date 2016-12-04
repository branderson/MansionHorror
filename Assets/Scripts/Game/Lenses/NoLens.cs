using UnityEngine;

namespace Assets.Game.Lenses
{
    public class NoLens : LensController
    {
        GameObject _camera;
        BlurShaderTest _shader;

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