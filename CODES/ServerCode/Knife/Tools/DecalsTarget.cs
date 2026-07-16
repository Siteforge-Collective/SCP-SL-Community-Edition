namespace Knife.Tools
{
	public class DecalsTarget : global::UnityEngine.MonoBehaviour
	{
		private global::UnityEngine.Renderer[] myRenderers;

		public global::UnityEngine.Renderer[] Renderers
		{
			get
			{
				if (myRenderers == null)
				{
					Awake();
				}
				return myRenderers;
			}
		}

		private void Awake()
		{
			myRenderers = GetComponentsInChildren<global::UnityEngine.Renderer>();
		}

		private void OnEnable()
		{
			global::Knife.Tools.GPURaycast.AddDecalsTarget(this);
		}

		private void OnDestroy()
		{
			global::Knife.Tools.GPURaycast.RemoveDecalsTarget(this);
		}
	}
}
