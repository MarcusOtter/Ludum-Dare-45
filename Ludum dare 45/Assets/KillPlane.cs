using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {
            LevelManager.Instance.RegisterSheepDeath(sheep);
            return;
        }

        var player = other.GetComponent<PlayerMove>();
        if (player != null)
        {
            LevelManager.Instance.RegisterPlayerDeath();
        }
    }
}
