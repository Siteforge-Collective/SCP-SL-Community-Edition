using Decals;
using Mirror;
using UnityEngine;

public class BloodDrawer : NetworkBehaviour
{
    private static Collider[] cachedColliders = new Collider[3];

    public void DrawBlood(RaycastHit hit, float randomMin = 0.75f, float randomMax = 1.25f)
    {
        TestOverlapBlood(hit.point);
        if (DecalPoolManager.Singleton.TrySpawnDecal(DecalPoolType.Blood, out Decal decal))
        {
            decal.AttachToSurface(hit.collider, hit.point, hit.normal);
            float scale = Random.Range(randomMin, randomMax);
            decal.transform.localScale = new Vector3(scale, scale, scale);
            decal.SetRandomRotation();
            decal.transform.parent = hit.collider.transform;
        }
    }

    private void TestOverlapBlood(Vector3 position)
    {
        int num = Physics.OverlapSphereNonAlloc(position, 0.5f, cachedColliders, LayerMask.GetMask("Decals"));
        BloodController oldest = null;
        float maxAge = 0f;
        for (int i = 0; i < num; i++)
        {
            Collider collider = cachedColliders[i];
            if (collider != null)
            {
                BloodController component = collider.GetComponent<BloodController>();
                if (component != null)
                {
                    if (component.BloodAgeTimer > maxAge)
                    {
                        maxAge = component.BloodAgeTimer;
                        oldest = component;
                    }
                }
            }
        }
        if (oldest != null)
        {
            oldest.BloodAgeTimer = 0f;
            oldest.enabled = true;
        }
    }

    public void PlaceUnderneath(Vector3 pos, float amountMultiplier = 1f)
    {
        pos.y += 0.1f;
        if (Physics.Raycast(pos, -Vector3.up, out RaycastHit hit, 1f, LayerMask.GetMask("Default", "Glass", "Door")))
        {
            pos = hit.point;
            pos.y += 0.01f;
            TestOverlapBlood(pos);
            if (DecalPoolManager.Singleton.TrySpawnDecal(DecalPoolType.Blood, out Decal decal))
            {
                decal.transform.position = pos;
                decal.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                decal.transform.Rotate(0f, Random.Range(0, 360), 0f);
                float scale = Random.Range(0.75f, 1.25f) * amountMultiplier;
                decal.transform.localScale = new Vector3(scale, 0.01f, scale);
                decal.transform.SetParent(hit.collider.transform, true);
            }
        }
    }
}