public class ToggleableLight : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject[] allLights;

	public bool isAlarm;

	public void SetLights(bool b)
	{
		global::UnityEngine.GameObject[] array = allLights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(isAlarm ? b : (!b));
		}
	}
}
