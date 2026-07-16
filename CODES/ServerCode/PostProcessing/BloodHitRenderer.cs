namespace PostProcessing
{
	public sealed class BloodHitRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.BloodHit>
	{
		private global::UnityEngine.Shader shader;

		private global::UnityEngine.Texture2D Texture2;

		private readonly int _timeXId = global::UnityEngine.Shader.PropertyToID("_TimeX");

		private readonly int _lightReflectId = global::UnityEngine.Shader.PropertyToID("_LightReflect");

		private readonly int _hitLeftId = global::UnityEngine.Shader.PropertyToID("_HitLeft");

		private readonly int _hitUpId = global::UnityEngine.Shader.PropertyToID("_HitUp");

		private readonly int _hitRightId = global::UnityEngine.Shader.PropertyToID("_HitRight");

		private readonly int _hitDownId = global::UnityEngine.Shader.PropertyToID("_HitDown");

		private readonly int _bloodHitLeftId = global::UnityEngine.Shader.PropertyToID("_BloodHitLeft");

		private readonly int _bloodHitUpId = global::UnityEngine.Shader.PropertyToID("_BloodHitUp");

		private readonly int _bloodHitRightId = global::UnityEngine.Shader.PropertyToID("_BloodHitRight");

		private readonly int _bloodHitDownId = global::UnityEngine.Shader.PropertyToID("_BloodHitDown");

		private readonly int _hitFullId = global::UnityEngine.Shader.PropertyToID("_HitFull");

		private readonly int _bloodHitFull1Id = global::UnityEngine.Shader.PropertyToID("_BloodHitFull1");

		private readonly int _bloodHitFull2Id = global::UnityEngine.Shader.PropertyToID("_BloodHitFull2");

		private readonly int _bloodHitFull3Id13 = global::UnityEngine.Shader.PropertyToID("_BloodHitFull3");

		private readonly int _mainTex2Id = global::UnityEngine.Shader.PropertyToID("_MainTex2");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/BloodHit");
			Texture2 = global::UnityEngine.Resources.Load("BloodHitTexture") as global::UnityEngine.Texture2D;
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(_lightReflectId, base.settings.LightReflect);
			propertySheet.properties.SetFloat(_hitLeftId, base.settings.Hit_Left);
			propertySheet.properties.SetFloat(_hitUpId, base.settings.Hit_Up);
			propertySheet.properties.SetFloat(_hitRightId, base.settings.Hit_Right);
			propertySheet.properties.SetFloat(_hitDownId, base.settings.Hit_Down);
			propertySheet.properties.SetFloat(_bloodHitLeftId, base.settings.Blood_Hit_Left);
			propertySheet.properties.SetFloat(_bloodHitUpId, base.settings.Blood_Hit_Up);
			propertySheet.properties.SetFloat(_bloodHitRightId, base.settings.Blood_Hit_Right);
			propertySheet.properties.SetFloat(_bloodHitDownId, base.settings.Blood_Hit_Down);
			propertySheet.properties.SetFloat(_hitFullId, base.settings.Hit_Full);
			propertySheet.properties.SetFloat(_bloodHitFull1Id, base.settings.Blood_Hit_Full_1);
			propertySheet.properties.SetFloat(_bloodHitFull2Id, base.settings.Blood_Hit_Full_2);
			propertySheet.properties.SetFloat(_bloodHitFull3Id13, base.settings.Blood_Hit_Full_3);
			propertySheet.properties.SetTexture(_mainTex2Id, Texture2);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
