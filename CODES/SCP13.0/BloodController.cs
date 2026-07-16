using Decals;
using UnityEngine;

public class BloodController : Decal
{
    public float decayTimeSeconds = 600f;

    public Color startColor;

    public Color endColor;

    [Header("Grid is square")]
    public int gridSize;

    public int numberOfDecals;

    private bool _isDying;

    public float BloodAgeTimer { get; set; }

    protected override void Awake()
    {
        if (_isDying)
        {
            return;
        }
        BloodAgeTimer = 0f;
        int randomIndex = Random.Range(0, numberOfDecals);
        UVOffset = new Vector2((randomIndex % gridSize) / (float)gridSize, (randomIndex / gridSize) / (float)gridSize);
        base.Awake();
    }

    public void DestroyDecal()
    {
        startColor = InstancedColor;
        _isDying = true;
        BloodAgeTimer = 0f;
        decayTimeSeconds = 5f;
        endColor = Color.clear;
        enabled = true;
    }

    private void Start()
    {
        UVTiling = Vector2.one;
    }

    private void Update()
    {
        BloodAgeTimer += Time.deltaTime;
        float t = BloodAgeTimer / decayTimeSeconds;
        InstancedColor = Color.Lerp(startColor, endColor, t);
        if (BloodAgeTimer > decayTimeSeconds)
        {
            ReturnToPool(true);
        }
    }
}