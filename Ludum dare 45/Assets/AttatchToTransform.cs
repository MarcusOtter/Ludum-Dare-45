using UnityEngine;

public class AttatchToTransform : MonoBehaviour
{
    [SerializeField] private Transform _transformToAttachTo;

    private void Update()
    {
        transform.position = _transformToAttachTo.position;
    }
}
