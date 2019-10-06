using UnityEngine;

public interface IGrabbable
{
    void Grabbed(Transform attatchPoint);
    void Released(Vector3 force, float throwDuration);
}
