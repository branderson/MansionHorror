using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ShaderTest : MonoBehaviour {

    public Material EffectMaterial;

    void Awake()
    {
        //EffectMaterial = GetComponent<Material>();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }

}
