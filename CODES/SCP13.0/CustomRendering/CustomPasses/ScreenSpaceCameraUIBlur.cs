using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

internal class ScreenSpaceCameraUIBlur : CustomPass
{
    public float blurRadius = 10f;
    public LayerMask uiLayer = 32;
    [Range(0f, 1f)]
    public float darkenAmount = 0.2f;

    private RTHandle downSampleBuffer;
    private Material darkenMaterial;
    private readonly int _intensityId = Shader.PropertyToID("_DarknessIntensity");

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        if (base.injectionPoint != CustomPassInjectionPoint.AfterPostProcess)
        {
            Debug.LogWarning("Custom Pass UI Blur isn't using the after post process injection point. Your post processes will be applied to the UI");
        }

        Shader shader = Shader.Find("Hidden/Renderers/Darken");
        if (shader != null)
            this.darkenMaterial = CoreUtils.CreateEngineMaterial(shader);

        this.downSampleBuffer = RTHandles.Alloc(
            scaleFactor: Vector2.one * 0.5f,
            slices: TextureXR.slices,
            depthBufferBits: DepthBits.None,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            filterMode: FilterMode.Point,
            wrapMode: TextureWrapMode.Repeat,
            dimension: TextureXR.dimension,
            enableRandomWrite: false,
            useMipMap: false,
            autoGenerateMips: true,
            isShadowMap: false,
            anisoLevel: 1,
            mipMapBias: 0f,
            msaaSamples: MSAASamples.None,
            bindTextureMS: false,
            useDynamicScale: true,
            memoryless: RenderTextureMemoryless.None,
            name: "DownSampleBuffer"
        );
    }

    protected override void AggregateCullingParameters(ref ScriptableCullingParameters cullingParameters, HDCamera hdCamera)
    {
        cullingParameters.cullingMask |= (uint)this.uiLayer.value;
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if (ctx.hdCamera.camera.cameraType == CameraType.SceneView || darkenMaterial == null)
            return;

        this.darkenMaterial.SetFloat(this._intensityId, this.darkenAmount);
        ctx.cmd.Blit(ctx.cameraColorBuffer, ctx.cameraColorBuffer, this.darkenMaterial);

        CustomPassUtils.GaussianBlur(ctx, ctx.cameraColorBuffer, ctx.cameraColorBuffer, this.downSampleBuffer, 9, this.blurRadius, 0, 0, true);

        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ctx.cameraDepthBuffer, ClearFlag.Depth);

        var sortingSettings = new SortingSettings(ctx.hdCamera.camera) { criteria = SortingCriteria.CommonTransparent };
        var drawingSettings = new DrawingSettings(HDShaderPassNames.s_ForwardName, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.transparent, uiLayer);

        ctx.renderContext.DrawRenderers(ctx.cullingResults, ref drawingSettings, ref filteringSettings);
    }

    protected override void Cleanup()
    {
        downSampleBuffer?.Release();

        CoreUtils.Destroy(darkenMaterial);
    }
}