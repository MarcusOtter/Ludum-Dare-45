using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerMove))]
public class PlayerGraphics : MonoBehaviour
{
    [SerializeField] private string _velocityParameterName = "Velocity";
    private int _velocityParameterHash;

    private Animator _animator;
    private PlayerMove _playerMove;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();

        _velocityParameterHash = Animator.StringToHash(_velocityParameterName);
    }

    private void Update()
    {
        _animator.SetFloat(_velocityParameterHash, _playerMove.CurrentVelocity);
    }
}
