public class ConnInfoCloser : ConnInfoButton
{
	public global::UnityEngine.GameObject[] objToClose;

	public override void UseButton()
	{
		global::UnityEngine.GameObject[] array = objToClose;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		base.UseButton();
	}
}
