using UnityEngine;

public class SheepDetector : MonoBehaviour
{
    //[SerializeField] private Animator _gateAnimator;
    //[SerializeField] private string _closeGateParameterName;

    //private int _closeGateParameterHash;

    //private void Awake()
    //{
    //    _closeGateParameterHash = Animator.StringToHash(_closeGateParameterName);
    //}

    private void OnTriggerEnter(Collider other)
    {
        var sheep = other.GetComponent<Sheep>();
        if (sheep == null) { return; }

        LevelManager.Instance.RegisterSavedSheep(sheep);
    }
}
