using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sheep : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _detectionRange;
    [SerializeField] [Range(0f, 0.2f)] private float _breakAmount = 0.05f;

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
        Vector3 currentVelocity = _rigidbody.velocity;
        Vector3 playerDirection = _playerTransform.position - transform.position;

        // Start breaking if the sheep stops detecting the player
        if (playerDirection.magnitude > _detectionRange)
        {
            if (currentVelocity.sqrMagnitude < 0.05f)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            float velocityFactor = 1 - _breakAmount;
            _rigidbody.velocity = new Vector3(currentVelocity.x * velocityFactor,
                                              currentVelocity.y,
                                              currentVelocity.z * velocityFactor);
            return;
        }

        Vector3 movementDirection = playerDirection.normalized * -_movementSpeed;
        movementDirection.y = currentVelocity.y;
        _rigidbody.velocity = movementDirection;
    }
}
