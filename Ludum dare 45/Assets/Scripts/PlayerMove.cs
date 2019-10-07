using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    internal float CurrentVelocity => _rigidbody.velocity.sqrMagnitude;

    [SerializeField] private float _movementSpeed;
    [SerializeField] [Range(0f, 1f)] private float _rotationSpeed = 0.2f;

    private CameraMovement _cameraMovement;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private Vector3 _movementVector;

    private PauseState _currentPauseState;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();   
    }

    private void Start()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
        _playerInput = FindObjectOfType<PlayerInput>();
        LevelManager.Instance.OnPauseChanged += HandlePauseChanged;
    }

    private void HandlePauseChanged(PauseState state)
    {
        _currentPauseState = state;
        _rigidbody.isKinematic = _currentPauseState != PauseState.NotPaused;
    }

    private void Update()
    {
        if (_currentPauseState != PauseState.NotPaused) { return; }

        if (IsMoving)
        {
            Vector3 forwardRotation = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardRotation), _rotationSpeed);
        }

        _movementVector = new Vector3(_playerInput.HorizontalAxis, 0, _playerInput.VerticalAxis).normalized * _movementSpeed;
        _movementVector.y = _rigidbody.velocity.y;
    }
    
    private void FixedUpdate()
    {
        if (_currentPauseState != PauseState.NotPaused) { return; }

        var localMovementVector = _cameraMovement.transform.TransformDirection(_movementVector);
        _rigidbody.velocity = localMovementVector;
    }

    private bool IsMoving => _rigidbody.velocity.magnitude > 0.01f;

    private void OnDisable()
    {
        LevelManager.Instance.OnPauseChanged -= HandlePauseChanged;
    }
}
