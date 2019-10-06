using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {

        }
    }
}
