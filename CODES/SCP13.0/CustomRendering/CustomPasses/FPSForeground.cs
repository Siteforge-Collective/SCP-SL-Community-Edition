using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[Serializable]
public class FPSForeground : CustomPass
{
    private string ShaderName { get; } = "Hidden/Renderers/ForegroundDepthClear";

    private static ShaderTagId[] _litForwardTags;
    private static ShaderTagId[] _depthTags;

    public LayerMask foregroundMask;
    public CustomPass.RenderQueueType renderQueueType;
    public Camera foregroundCamera;

    public bool depthPass;
    public bool clearDepth;
    public bool overrideBlendState;
    public bool overrideDepthState;
    public bool writeDepth = true;
    public CompareFunction compareFunction = CompareFunction.Less;

    private Material depthClearMaterial;
    private RenderStateBlock block;

    protected override void AggregateCullingParameters(ref ScriptableCullingParameters cullingParameters, HDCamera hdCamera)
    {
        cullingParameters.cullingMask |= (uint)this.foregroundMask.value;
    }

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        Shader shader = Shader.Find(this.ShaderName);
        if (shader != null)
        {
            this.depthClearMaterial = new Material(shader);
        }
        else
        {
            Debug.LogError("Unable to find shader '" + this.ShaderName + "'. FPS Foreground is unable to load.");
        }

        this.block = default(RenderStateBlock);

        if (this.overrideBlendState)
        {
            this.block.mask |= RenderStateMask.Blend;
            this.block.blendState = new BlendState(false, false)
            {
                blendState0 = new RenderTargetBlendState(ColorWriteMask.All, BlendMode.One, BlendMode.Zero, BlendMode.One, BlendMode.Zero, BlendOp.Add, BlendOp.Add)
            };
        }

        if (this.overrideDepthState)
        {
            this.block.mask |= RenderStateMask.Depth;
            this.block.depthState = new DepthState(this.writeDepth, this.compareFunction);
        }

        _litForwardTags = new ShaderTagId[]
        {
            HDShaderPassNames.s_ForwardOnlyName,
            HDShaderPassNames.s_ForwardName,
            HDShaderPassNames.s_SRPDefaultUnlitName
        };
        _depthTags = new ShaderTagId[]
        {
            HDShaderPassNames.s_DepthForwardOnlyName,
            HDShaderPassNames.s_DepthOnlyName
        };
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if (ctx.hdCamera.camera.cameraType == CameraType.SceneView || foregroundCamera == null)
        {
            return;
        }

        if (this.clearDepth || this.depthPass)
        {
            CoreUtils.SetKeyword(ctx.cmd, "WRITE_NORMAL_BUFFER", true);
        }

        if (this.clearDepth)
        {
            this.RenderFromCamera(ctx, this.foregroundCamera, ctx.cameraNormalBuffer, ctx.cameraDepthBuffer, ClearFlag.None, this.foregroundMask, true, this.renderQueueType, this.block, this.depthClearMaterial);
        }

        if (this.depthPass)
        {
            this.RenderFromCamera(ctx, this.foregroundCamera, ctx.cameraNormalBuffer, ctx.cameraDepthBuffer, ClearFlag.None, this.foregroundMask, true, this.renderQueueType, this.block, null);
        }

        if (this.clearDepth || this.depthPass)
        {
            CoreUtils.SetKeyword(ctx.cmd, "WRITE_NORMAL_BUFFER", false);
        }

        if (!this.clearDepth && !this.depthPass)
        {
            base.SetRenderTargetAuto(ctx.cmd);
            this.RenderFromCamera(ctx, this.foregroundCamera, ctx.cameraColorBuffer, ctx.cameraDepthBuffer, ClearFlag.None, this.foregroundMask, false, this.renderQueueType, this.block, null);
        }
    }

    public void RenderFromCamera(in CustomPassContext ctx, Camera view, RTHandle targetColor, RTHandle targetDepth, ClearFlag clearFlag, LayerMask layerMask, bool depthTags, CustomPass.RenderQueueType renderQueueFilter, RenderStateBlock overrideRenderState, Material overrideMaterial = null)
    {
        if (targetColor != null && targetDepth != null)
            CoreUtils.SetRenderTarget(ctx.cmd, targetColor, targetDepth, clearFlag, Color.black);
        else if (targetColor != null)
            CoreUtils.SetRenderTarget(ctx.cmd, targetColor, clearFlag, Color.black);
        else if (targetDepth != null)
            CoreUtils.SetRenderTarget(ctx.cmd, targetDepth, clearFlag, Color.black);

        RenderQueueRange queueRange = RenderQueueRange.all;
        if (renderQueueFilter == RenderQueueType.AllOpaque) queueRange = RenderQueueRange.opaque;
        else if (renderQueueFilter == RenderQueueType.AllTransparent) queueRange = RenderQueueRange.transparent;

        FilteringSettings filteringSettings = new FilteringSettings(queueRange, layerMask);

        var tags = depthTags ? _depthTags : _litForwardTags;
        SortingSettings sortingSettings = new SortingSettings(view)
        {
            criteria = SortingCriteria.CommonOpaque | SortingCriteria.CanvasOrder
        };

        DrawingSettings drawingSettings = new DrawingSettings(tags[0], sortingSettings)
        {
            overrideMaterial = overrideMaterial,
            mainLightIndex = -1
        };
        for (int i = 1; i < tags.Length; i++)
            drawingSettings.SetShaderPassName(i, tags[i]);

        ctx.renderContext.DrawRenderers(ctx.cullingResults, ref drawingSettings, ref filteringSettings, ref overrideRenderState);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(this.depthClearMaterial);
    }
}