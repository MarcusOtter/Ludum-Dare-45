using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sheep : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _detectionRange;

    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    private void Update()
    {
        Vector3 playerDirection = _playerTransform.position - transform.position;

        if (playerDirection.magnitude > _detectionRange)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        Vector3 movementDirection = playerDirection.normalized * -_movementSpeed;
        movementDirection.y = _rigidbody.velocity.y;
        _rigidbody.velocity = movementDirection;
    }
}
