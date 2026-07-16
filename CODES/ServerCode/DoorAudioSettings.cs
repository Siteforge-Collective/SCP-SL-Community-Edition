[global::UnityEngine.CreateAssetMenu(fileName = "New Door Audio Settings", menuName = "ScriptableObject/Doors/DoorAudioSettings")]
public class DoorAudioSettings : global::UnityEngine.ScriptableObject
{
	private static readonly global::System.Random Rng = Misc.CreateRandom();

	public global::UnityEngine.AudioClip[] DoorOpeningSound;

	public global::UnityEngine.AudioClip[] DoorClosingSound;

	public global::UnityEngine.AudioClip BeepSound;

	public global::UnityEngine.AudioClip RandomOpeningSound => DoorOpeningSound[Rng.Next(DoorOpeningSound.Length)];

	public global::UnityEngine.AudioClip RandomClosingSound => DoorClosingSound[Rng.Next(DoorClosingSound.Length)];
}
