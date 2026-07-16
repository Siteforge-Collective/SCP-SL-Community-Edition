using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakingCardSFX : MonoBehaviour
{
    public string[] texts;

    public float waitTime = 1.3f;

    private TextMeshProUGUI text;

    private Text txt;

    private float _timer;

    private int _index;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        txt = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer < waitTime)
            return;

        _timer = 0f;

        // Advance through the list; once past the end fall back to an empty frame (index -1) so the
        // effect blanks out for one step before looping back to the start.
        _index++;

        string current;
        if (texts == null)
            return;

        if (_index >= texts.Length)
        {
            _index = -1;
            current = string.Empty;
        }
        else if (_index < 0)
        {
            current = string.Empty;
        }
        else
        {
            current = texts[_index];
        }

        if (txt != null)
            txt.text = current;
        else if (text != null)
            text.text = current;
    }
}
