namespace PostProcessing
{
	public sealed class ArtefactRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Artefact>
	{
		private global::UnityEngine.Shader shader;

		private readonly int _fadeId = global::UnityEngine.Shader.PropertyToID("_Fade");

		private readonly int _colorisationId = global::UnityEngine.Shader.PropertyToID("_Colorisation");

		private readonly int _parasiteId = global::UnityEngine.Shader.PropertyToID("_Parasite");

		private readonly int _noiseId = global::UnityEngine.Shader.PropertyToID("_Noise");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Artefact");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(_fadeId, base.settings.Fade);
			propertySheet.properties.SetFloat(_colorisationId, base.settings.Colorization);
			propertySheet.properties.SetFloat(_parasiteId, base.settings.Parasite);
			propertySheet.properties.SetFloat(_noiseId, base.settings.Noise);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
