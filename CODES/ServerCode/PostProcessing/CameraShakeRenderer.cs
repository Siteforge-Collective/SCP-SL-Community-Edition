namespace PostProcessing
{
	public sealed class CameraShakeRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.CameraShake>
	{
		private global::UnityEngine.Shader shader;

		private float _verticalJumpTime;

		private readonly int _ScanLineJitter = global::UnityEngine.Shader.PropertyToID("_ScanLineJitter");

		private readonly int _VerticalJump = global::UnityEngine.Shader.PropertyToID("_VerticalJump");

		private readonly int _HorizontalShake = global::UnityEngine.Shader.PropertyToID("_HorizontalShake");

		private readonly int _ColorDrift = global::UnityEngine.Shader.PropertyToID("_ColorDrift");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/CameraShake");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			_verticalJumpTime += global::UnityEngine.Time.deltaTime * (float)base.settings.verticalJump * 11.3f;
			float y = global::UnityEngine.Mathf.Clamp01(1f - (float)base.settings.scanLineJitter * 1.2f);
			float x = 0.002f + global::UnityEngine.Mathf.Pow(base.settings.scanLineJitter, 3f) * 0.05f;
			propertySheet.properties.SetVector(_ScanLineJitter, new global::UnityEngine.Vector2(x, y));
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(base.settings.verticalJump, _verticalJumpTime);
			propertySheet.properties.SetVector(_VerticalJump, vector);
			propertySheet.properties.SetFloat(_HorizontalShake, (float)base.settings.horizontalShake * 0.2f);
			global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2((float)base.settings.colorDrift * 0.04f, global::UnityEngine.Time.time * 606.11f);
			propertySheet.properties.SetVector(_ColorDrift, vector2);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
