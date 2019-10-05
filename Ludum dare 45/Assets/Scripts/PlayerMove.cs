using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    internal float CurrentVelocity => _rigibody.velocity.sqrMagnitude;

    [SerializeField] private float _movementSpeed;
    [SerializeField] [Range(0f, 1f)] private float _rotationSpeed = 0.2f;

    private CameraMovement _cameraMovement;
    private PlayerInput _playerInput;
    private Rigidbody _rigibody;

    private Vector3 _movementVector;

    private void Awake()
    {
        _rigibody = GetComponent<Rigidbody>();   
    }

    private void Start()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (IsMoving)
        {
            Vector3 forwardRotation = new Vector3(_rigibody.velocity.x, 0, _rigibody.velocity.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardRotation), _rotationSpeed);
        }

        _movementVector = new Vector3(_playerInput.HorizontalAxis, 0, _playerInput.VerticalAxis).normalized * _movementSpeed;
        _movementVector.y = _rigibody.velocity.y;
    }
    
    private void FixedUpdate()
    {
        var localMovementVector = _cameraMovement.transform.TransformDirection(_movementVector);
        _rigibody.velocity = localMovementVector;
    }

    private bool IsMoving => _rigibody.velocity.magnitude > 0.01f;
}
