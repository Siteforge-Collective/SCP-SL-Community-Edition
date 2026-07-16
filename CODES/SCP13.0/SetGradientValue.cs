using UnityEngine;
using UnityEngine.UI;

public class SetGradientValue : MonoBehaviour
{
    private readonly int _startId = Shader.PropertyToID("_Start");

    public float _startValue = 1f;

    private void Start()
    {
        Image component = GetComponent<Image>();
        component.material = new Material(component.material);
        component.material.SetFloat(_startId, _startValue);
    }
}
