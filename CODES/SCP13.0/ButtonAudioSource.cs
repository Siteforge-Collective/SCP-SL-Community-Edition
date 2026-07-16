using System.Diagnostics;
using UnityEngine;

public class ButtonAudioSource : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clickClip;

    [SerializeField]
    private AudioClip _hoverClip;

    private AudioSource _audioSource;

    private static bool _singletonSet;

    private static ButtonAudioSource _singleton;

    private static readonly Stopwatch LastPlay;

    private const float MinCooldown = 0.06f;

    static ButtonAudioSource()
    {
        LastPlay = new Stopwatch();
        LastPlay.Start();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _singleton = this;
        _singletonSet = true;
    }

    private void OnDestroy()
    {
        if (_singleton == this)
        {
            _singleton = null;
            _singletonSet = false;
        }
    }

    public static void PlayHover()
    {
        if (!_singletonSet || _singleton == null || _singleton._hoverClip == null)
            return;

        Play(_singleton._hoverClip);
    }

    public static void PlayClick()
    {
        if (!_singletonSet || _singleton == null || _singleton._clickClip == null)
            return;

        Play(_singleton._clickClip);
    }

    private static void Play(AudioClip clip)
    {
        if (clip == null || _singleton == null || _singleton._audioSource == null)
            return;

        if (LastPlay.Elapsed.TotalSeconds >= MinCooldown)
        {
            _singleton._audioSource.PlayOneShot(clip);
            LastPlay.Restart();
        }
    }
}