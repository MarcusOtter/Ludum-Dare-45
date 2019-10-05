using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private CameraMovement _cameraMovement;
    private Rigidbody _rigibody;

    private Vector3 _movementVector;

    private void Awake()
    {
        _rigibody = GetComponent<Rigidbody>();   
    }

    private void Start()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void Update()
    {
        if (IsMoving)
        {
            transform.forward = _cameraMovement.LookDirection;
        }

        // move to input manager
        float velocityX = Input.GetAxisRaw("Horizontal");
        float velocityZ = Input.GetAxisRaw("Vertical");

        _movementVector = new Vector3(velocityX, 0, velocityZ).normalized * _movementSpeed;
        _movementVector.y = _rigibody.velocity.y;
    }
    
    private void FixedUpdate()
    {
        var localMovementVector = transform.TransformDirection(_movementVector);
        _rigibody.velocity = localMovementVector;
    }

    private bool IsMoving => _rigibody.velocity.magnitude > 0.01f;
}
