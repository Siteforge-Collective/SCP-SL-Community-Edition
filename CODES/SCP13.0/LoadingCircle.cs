using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    public int i;

    public int framesToNextRotation = 10;

    public void FixedUpdate()
    {
        i++;
        if (i > framesToNextRotation)
        {
            i = 0;
            base.transform.Rotate(Vector3.forward * -45f, Space.Self);
        }
    }
}
