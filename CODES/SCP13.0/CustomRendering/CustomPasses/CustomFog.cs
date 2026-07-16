using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CustomFog : CustomPass
{
    private readonly int _fogColorId = Shader.PropertyToID("_CustomFogColor");
    private readonly int _distanceParamsId = Shader.PropertyToID("_DistanceParams");

    [ColorUsage(false, true)]
    public Color FogColor = Color.black;

    [Range(0f, 1f)]
    public float FadeIntensity = 1f;

    public float EndDistance = 200f;

    public float StartDistance = 1f;           

    [Range(0f, 1f)]
    public float CoverSkybox = 1f;

    private Material _fogMaterial;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        Shader shader = Shader.Find("Hidden/Renderers/CustomFog");
        _fogMaterial = CoreUtils.CreateEngineMaterial(shader);
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if (_fogMaterial == null)
            return;

        base.SetRenderTargetAuto(ctx.cmd);

        _fogMaterial.SetColor(_fogColorId, FogColor);
        _fogMaterial.SetVector(_distanceParamsId, new Vector4(StartDistance, EndDistance, FadeIntensity, CoverSkybox));

        CoreUtils.DrawFullScreen(ctx.cmd, _fogMaterial, null, 0);
    }

    public override IEnumerable<Material> RegisterMaterialForInspector()
    {
        if (_fogMaterial != null)
        {
            yield return _fogMaterial;
        }
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_fogMaterial);
    }
}